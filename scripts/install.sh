#!/usr/bin/env sh
set -e

REPO="lennykean/pmmux"
INSTALL_LIB="/usr/local/lib/pmmux"
INSTALL_BIN="/usr/local/bin"

cleanup() {
    if [ -n "$TMPDIR_CREATED" ] && [ -d "$TMPDIR_CREATED" ]; then
        rm -rf "$TMPDIR_CREATED"
    fi
}
trap cleanup EXIT

die() {
    printf "error: %s\n" "$1" >&2
    exit 1
}

check_tools() {
    if command -v curl >/dev/null 2>&1; then
        FETCH="curl"
    elif command -v wget >/dev/null 2>&1; then
        FETCH="wget"
    else
        die "curl or wget is required"
    fi

    command -v tar >/dev/null 2>&1 || die "tar is required"
}

fetch_url() {
    if [ "$FETCH" = "curl" ]; then
        curl -fsSL "$1"
    else
        wget -qO- "$1"
    fi
}

download_file() {
    if [ "$FETCH" = "curl" ]; then
        curl -fsSL -o "$2" "$1"
    else
        wget -qO "$2" "$1"
    fi
}

detect_platform() {
    OS="$(uname -s)"
    ARCH="$(uname -m)"

    case "$OS" in
        MINGW*|MSYS*|CYGWIN*)
            die "Windows detected. Use install.ps1 or the PowerShell installer instead."
            ;;
        Linux)
            case "$ARCH" in
                x86_64)  RID="linux-x64" ;;
                aarch64) RID="linux-arm64" ;;
                *)       die "unsupported architecture: $ARCH" ;;
            esac
            ;;
        Darwin)
            case "$ARCH" in
                x86_64) RID="osx-x64" ;;
                arm64)  RID="osx-arm64" ;;
                *)      die "unsupported architecture: $ARCH" ;;
            esac
            ;;
        *)
            die "unsupported operating system: $OS"
            ;;
    esac
}

get_latest_version() {
    RELEASE_JSON="$(fetch_url "https://api.github.com/repos/${REPO}/releases/latest")" \
        || die "failed to fetch latest release from GitHub API"

    LATEST_TAG="$(printf "%s" "$RELEASE_JSON" \
        | sed -n 's/.*"tag_name"[[:space:]]*:[[:space:]]*"\([^"]*\)".*/\1/p' \
        | head -1)"

    [ -n "$LATEST_TAG" ] || die "failed to parse latest version from GitHub API response"

    LATEST_VERSION="$(printf "%s" "$LATEST_TAG" | sed 's/^v//')"
}

get_installed_version() {
    INSTALLED_VERSION=""
    if [ -x "${INSTALL_LIB}/pmmux" ]; then
        INSTALLED_VERSION="$("${INSTALL_LIB}/pmmux" --version 2>/dev/null \
            | head -1 | sed 's/^pmmux //')" || true
    fi
}

SUDO=""
ensure_sudo() {
    if [ "$(id -u)" -eq 0 ]; then
        return
    fi
    if command -v sudo >/dev/null 2>&1; then
        SUDO="sudo"
        return
    fi
    die "installation requires root privileges. Re-run with sudo."
}

install_pmmux() {
    WORK="$(mktemp -d)"
    TMPDIR_CREATED="$WORK"

    ARCHIVE="pmmux-${RID}.tar.gz"
    DOWNLOAD_URL="https://github.com/${REPO}/releases/download/${LATEST_TAG}/${ARCHIVE}"

    printf "downloading %s ...\n" "$ARCHIVE"
    download_file "$DOWNLOAD_URL" "${WORK}/${ARCHIVE}" \
        || die "failed to download ${DOWNLOAD_URL}"

    printf "extracting ...\n"
    $SUDO mkdir -p "$INSTALL_LIB"
    $SUDO tar -xzf "${WORK}/${ARCHIVE}" -C "$INSTALL_LIB" \
        || die "failed to extract archive"

    $SUDO chmod +x "${INSTALL_LIB}/pmmux"

    $SUDO mkdir -p "${INSTALL_LIB}/extensions"

    $SUDO mkdir -p "$INSTALL_BIN"
    $SUDO ln -sf "${INSTALL_LIB}/pmmux" "${INSTALL_BIN}/pmmux"

    printf "pmmux %s installed to %s\n" "$LATEST_VERSION" "$INSTALL_LIB"
}

main() {
    check_tools
    detect_platform
    get_latest_version
    get_installed_version

    if [ -n "$INSTALLED_VERSION" ] && [ "$INSTALLED_VERSION" = "$LATEST_VERSION" ]; then
        printf "pmmux is already up to date (%s)\n" "$INSTALLED_VERSION"
        exit 0
    fi

    ensure_sudo
    install_pmmux
}

main
