#!/usr/bin/env sh
set -e

REPO="lennykean/pmmux"
INSTALL_LIB="/usr/local/lib/pmmux"
EXTENSIONS_DIR="${INSTALL_LIB}/extensions"
KNOWN_EXTENSIONS="http tls bittorrent management acme acme-route53 otlp"

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

usage() {
    printf "usage: install-extension.sh <extension-name>\n\n"
    printf "available extensions: %s\n" "$KNOWN_EXTENSIONS"
    exit 1
}

# -- tool detection --

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

# -- validation --

validate_extension() {
    VALID=""
    for ext in $KNOWN_EXTENSIONS; do
        if [ "$ext" = "$1" ]; then
            VALID="1"
            break
        fi
    done
    if [ -z "$VALID" ]; then
        printf "error: unknown extension '%s'\n\n" "$1" >&2
        printf "available extensions: %s\n" "$KNOWN_EXTENSIONS" >&2
        exit 1
    fi
}

# -- pmmux detection --

find_pmmux() {
    if [ -x "${INSTALL_LIB}/pmmux" ]; then
        PMMUX_BIN="${INSTALL_LIB}/pmmux"
    elif command -v pmmux >/dev/null 2>&1; then
        PMMUX_BIN="$(command -v pmmux)"
    else
        die "pmmux is not installed. Run install.sh first."
    fi
}

# -- extension name mapping --

extension_dll_name() {
    case "$1" in
        http)          printf "Pmmux.Extensions.Http.dll" ;;
        tls)           printf "Pmmux.Extensions.Tls.dll" ;;
        bittorrent)    printf "Pmmux.Extensions.BitTorrent.dll" ;;
        management)    printf "Pmmux.Extensions.Management.dll" ;;
        acme)          printf "Pmmux.Extensions.Acme.dll" ;;
        acme-route53)  printf "Pmmux.Extensions.Acme.Route53.dll" ;;
        otlp)          printf "Pmmux.Extensions.Otlp.dll" ;;
    esac
}

# -- version helpers --

get_latest_version() {
    RELEASE_JSON="$(fetch_url "https://api.github.com/repos/${REPO}/releases/latest")" \
        || die "failed to fetch latest release from GitHub API"

    LATEST_TAG="$(printf "%s" "$RELEASE_JSON" \
        | sed -n 's/.*"tag_name"[[:space:]]*:[[:space:]]*"\([^"]*\)".*/\1/p' \
        | head -1)"

    [ -n "$LATEST_TAG" ] || die "failed to parse latest version from GitHub API response"

    LATEST_VERSION="$(printf "%s" "$LATEST_TAG" | sed 's/^v//')"
}

get_installed_extension_version() {
    INSTALLED_EXT_VERSION=""
    DLL_NAME="$(extension_dll_name "$EXT_NAME")"
    VERSION_OUTPUT="$("$PMMUX_BIN" --version 2>/dev/null)" || return 0

    INSTALLED_EXT_VERSION="$(printf "%s" "$VERSION_OUTPUT" \
        | sed -n "s/.*${DLL_NAME}[[:space:]]\{1,\}\(.*\)/\1/p" \
        | head -1)" || true
}

# -- installation --

check_root() {
    if [ "$(id -u)" -ne 0 ]; then
        die "installation requires root privileges. Re-run with sudo."
    fi
}

install_extension() {
    WORK="$(mktemp -d)"
    TMPDIR_CREATED="$WORK"

    ARCHIVE="pmmux-extension-${EXT_NAME}.tar.gz"
    DOWNLOAD_URL="https://github.com/${REPO}/releases/download/${LATEST_TAG}/${ARCHIVE}"

    printf "downloading %s ...\n" "$ARCHIVE"
    download_file "$DOWNLOAD_URL" "${WORK}/${ARCHIVE}" \
        || die "failed to download ${DOWNLOAD_URL}"

    printf "extracting ...\n"
    mkdir -p "$EXTENSIONS_DIR"
    tar -xzf "${WORK}/${ARCHIVE}" -C "$EXTENSIONS_DIR" \
        || die "failed to extract archive"

    printf "extension %s (%s) installed to %s\n" "$EXT_NAME" "$LATEST_VERSION" "$EXTENSIONS_DIR"
}

# -- main --

main() {
    if [ $# -lt 1 ]; then
        usage
    fi

    EXT_NAME="$1"

    check_tools
    validate_extension "$EXT_NAME"
    find_pmmux
    get_latest_version
    get_installed_extension_version

    if [ -n "$INSTALLED_EXT_VERSION" ] && [ "$INSTALLED_EXT_VERSION" = "$LATEST_VERSION" ]; then
        printf "extension %s is already up to date (%s)\n" "$EXT_NAME" "$INSTALLED_EXT_VERSION"
        exit 0
    fi

    check_root
    install_extension
}

main "$@"
