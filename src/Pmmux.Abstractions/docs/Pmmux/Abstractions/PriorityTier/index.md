#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions')

## PriorityTier Enum

Routing priority tier for backends\.

```csharp
public enum PriorityTier
```
### Fields

<a name='Pmmux.Abstractions.PriorityTier.Fallback'></a>

`Fallback` 0

Fallback priority \- lowest priority, used only when all higher\-priority backends are unavailable\.

### Remarks
Typically used for error pages, maintenance notices, or blackhole backends\.

<a name='Pmmux.Abstractions.PriorityTier.Standby'></a>

`Standby` 1

Standby priority \- used when normal\-priority backends are unavailable\.

### Remarks
Typically used for backup servers or disaster recovery resources\.

<a name='Pmmux.Abstractions.PriorityTier.Normal'></a>

`Normal` 2

Normal priority \- standard priority for most production backends\.

### Remarks
This is the default priority when not explicitly specified\.

<a name='Pmmux.Abstractions.PriorityTier.Vip'></a>

`Vip` 3

VIP priority \- highest priority, always preferred when healthy\.

### Remarks
Typically used for premium or primary servers that should receive traffic first\.

### Remarks
Priority tiers influence backend selection when multiple backends match a request\.
Some routing strategies \(like `least-requests`\) prefer higher tiers \([Vip](index.md#Pmmux.Abstractions.PriorityTier.Vip 'Pmmux\.Abstractions\.PriorityTier\.Vip')\) over
lower tiers \([Fallback](index.md#Pmmux.Abstractions.PriorityTier.Fallback 'Pmmux\.Abstractions\.PriorityTier\.Fallback')\)\.