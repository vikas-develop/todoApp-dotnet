# Building Windows Executable

## Quick Build Command

To create a Windows executable (.exe) file, run:

```bash
dotnet publish TodoApp.Desktop.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true
```

## Output Location

The executable will be created at:
```
bin/Release/net8.0/win-x64/publish/TodoApp.Desktop.exe
```

## Publishing Options Explained

- `-c Release`: Build in Release mode (optimized, smaller size)
- `-r win-x64`: Target Windows 64-bit platform
- `--self-contained true`: Include .NET runtime (no need to install .NET on target machine)
- `-p:PublishSingleFile=true`: Create a single executable file
- `-p:IncludeNativeLibrariesForSelfExtract=true`: Include native libraries in the exe

## Alternative: Smaller Executable (Requires .NET Runtime)

If you want a smaller file and the target machine has .NET 8.0 installed:

```bash
dotnet publish TodoApp.Desktop.csproj -c Release -r win-x64 --self-contained false
```

## Other Windows Targets

For 32-bit Windows:
```bash
dotnet publish TodoApp.Desktop.csproj -c Release -r win-x86 --self-contained true -p:PublishSingleFile=true
```

For ARM64 Windows:
```bash
dotnet publish TodoApp.Desktop.csproj -c Release -r win-arm64 --self-contained true -p:PublishSingleFile=true
```

## Distribution

1. Copy the entire `publish` folder to your Windows laptop
2. Run `TodoApp.Desktop.exe` directly
3. The database file (`todos_desktop.db`) will be created in the same folder as the exe

## File Size

- Self-contained single file: ~70-100 MB (includes .NET runtime)
- Framework-dependent: ~5-10 MB (requires .NET 8.0 installed)

## Troubleshooting

If the exe doesn't run:
- Make sure you're using `win-x64` for 64-bit Windows
- Try `win-x86` for 32-bit Windows
- Check Windows Defender isn't blocking the file
- Right-click the exe → Properties → Unblock (if blocked)

