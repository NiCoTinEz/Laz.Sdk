# Laz.Sdk — Claude Code project memory

## What this is

Unofficial Lazada Open Platform SDK for .NET 10. Public NuGet package `Laz.Sdk`, MIT licensed, async-only public API, dependency-injection-first.

## Layout

- `/src/Laz.Sdk/` — the library (the only thing that ships to NuGet).
- `/tests/` — placeholder for future test project. Currently empty.
- `/net/` — **legacy .NET Framework 2.0 SDK. Frozen. Slated for removal. Never edit.**
- `/Laz.Sdk.slnx` — slnx solution at repo root.
- `/Directory.Build.props` — shared build + packaging metadata.

## Conventions

- C# 14, `<Nullable>enable</Nullable>`, `<ImplicitUsings>enable</ImplicitUsings>`, `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>`.
- All formatting / parsing uses `CultureInfo.InvariantCulture`. Never rely on `Thread.CurrentThread.CurrentCulture`.
- All wire encoding is UTF-8 (`Encoding.UTF8`). Never `Encoding.Default`.
- All string comparisons inside the SDK use `StringComparer.Ordinal` / `StringComparison.Ordinal` (signature stability across cultures). Reserve `InvariantCultureIgnoreCase` for human-text comparisons only.
- Timestamps sent to Lazada are UTC milliseconds-since-epoch via `DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()`. Caller-supplied `DateTime` overrides are coerced to UTC before formatting.
- HTTP via `IHttpClientFactory` — never `new HttpClient()`.
- No `ILogger` injected yet. Do not add until product decision.
- No new public type without a corresponding README entry and CHANGELOG line.

## Multi-tenant pattern

- Default registration (`AddLazClient(...)`) registers `ILazClient` directly for single-tenant apps.
- Named registration (`AddLazClient("name", ...)`) registers per-name `LazClientOptions` and a per-name `HttpClient` (named `Laz.Sdk:{name}`); resolve via `ILazClientFactory.CreateClient("name")`.
- Each tenant gets its own `AppKey`, `AppSecret`, region URL, timeout, and message-handler chain.

## Commands

```powershell
# build
dotnet build .\Laz.Sdk.slnx -c Release

# pack (produces .nupkg + .snupkg in .\artifacts)
dotnet pack .\src\Laz.Sdk\Laz.Sdk.csproj -c Release -o .\artifacts

# publish to NuGet.org (replace <KEY> with API key)
dotnet nuget push .\artifacts\Laz.Sdk.<ver>.nupkg --api-key <KEY> --source https://api.nuget.org/v3/index.json
```

## Public API stability

Every breaking change to a `public` type requires:

1. A major-version bump in `Laz.Sdk.csproj` (semver).
2. A CHANGELOG entry under the new version.
3. README updates for any renamed / removed surface.

## Do-not-touch

- `/net/**` — legacy SDK, frozen.
- `LICENSE` — legal text, do not paraphrase.
- The reproducibility flags in `Directory.Build.props` (`PublishRepositoryUrl`, `EmbedUntrackedSources`, `ContinuousIntegrationBuild`) — they make the package debuggable.

## Before pushing to NuGet

1. Replace `OWNER` in `Directory.Build.props` with the GitHub account that owns the repo.
2. Set `Authors` to a real author / org name (currently `$(UserName)`, a build-machine placeholder).
3. Bump `<Version>` in `Laz.Sdk.csproj`.
4. Add a CHANGELOG entry for the new version.
5. Tag the commit `vX.Y.Z`.
