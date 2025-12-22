# Distributing Todo App to Windows Users

## Quick Build Command

Run this command to create a Windows executable:

```bash
dotnet publish TodoApp.Desktop.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:EnableCompressionInSingleFile=true
```

## Output Location

The executable will be created at:
```
bin/Release/net8.0/win-x64/publish/TodoApp.Desktop.exe
```

## What to Send to Windows Users

### Option 1: Single Executable (Recommended)
Send only the `.exe` file:
- `TodoApp.Desktop.exe` (~98 MB)

**Pros:**
- Single file, easy to distribute
- No installation needed
- Works on any Windows 10/11 machine

**Cons:**
- Larger file size
- First run extracts files (takes a few seconds)

### Option 2: Entire Publish Folder
Send the entire `publish` folder as a ZIP file:
- Create a ZIP of the `publish` folder
- User extracts and runs `TodoApp.Desktop.exe`

**Pros:**
- Faster startup (no extraction needed)
- Can include additional files if needed

**Cons:**
- Multiple files to manage

## Distribution Methods

### 1. Email
- Attach the `.exe` file or ZIP
- Note: Some email providers block `.exe` files
- Solution: Rename to `.zip` or use cloud storage

### 2. Cloud Storage (Recommended)
Upload to:
- Google Drive
- Dropbox
- OneDrive
- Share the download link

### 3. USB Drive
- Copy the `.exe` file to USB
- User can run directly from USB or copy to their computer

### 4. File Sharing Services
- WeTransfer
- SendAnywhere
- File.io

## Instructions for Windows Users

Include these instructions when sending:

```
1. Download TodoApp.Desktop.exe
2. Right-click the file → Properties
3. If you see "Unblock" checkbox, check it and click OK
4. Double-click TodoApp.Desktop.exe to run
5. The app will create a database file (todos_desktop.db) in the same folder
```

## System Requirements

- Windows 10 (64-bit) or Windows 11
- No .NET installation required (self-contained)
- ~100 MB free disk space

## Creating a ZIP for Distribution

```bash
cd bin/Release/net8.0/win-x64/publish
zip -r TodoApp-Windows.zip TodoApp.Desktop.exe
```

Or on Windows:
```powershell
Compress-Archive -Path TodoApp.Desktop.exe -DestinationPath TodoApp-Windows.zip
```

## Alternative: Smaller Build (Requires .NET 8.0)

If users have .NET 8.0 installed, create a smaller build:

```bash
dotnet publish TodoApp.Desktop.csproj -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true
```

This creates a ~5-10 MB file but requires .NET 8.0 runtime on the target machine.

## Troubleshooting for Users

If the app doesn't run:
1. **Windows Defender blocking**: Add exception or temporarily disable
2. **"Blocked" message**: Right-click → Properties → Unblock
3. **Missing DLL errors**: Use self-contained build (includes everything)
4. **Antivirus false positive**: Common with new executables, add exception

## Current Build Status

✅ Latest build created at: `bin/Release/net8.0/win-x64/publish/`
✅ File size: ~98 MB (self-contained)
✅ Ready to distribute!

