# Windows Installation Guide

## 📦 Installation Instructions

The Todo App Desktop is a **portable application** - no installation required! Simply download and run the executable file.

### Quick Start

1. **Download the executable:**
   - Download `TodoApp.Desktop.exe` from the releases or build folder

2. **Run the application:**
   - Double-click `TodoApp.Desktop.exe` to launch the application
   - The app will start immediately - no installation needed!

3. **First Run:**
   - The application will automatically create a database file (`todos_desktop.db`) in the same folder as the executable
   - A theme configuration file (`theme.config`) will also be created to save your theme preference

### System Requirements

- **Operating System:** Windows 10 or later (64-bit)
- **No additional software required:** The executable includes everything needed to run
- **Disk Space:** ~50 MB for the executable

### Installation Options

#### Option 1: Portable (Recommended)
- Place `TodoApp.Desktop.exe` anywhere on your computer
- Double-click to run
- All data is stored in the same folder as the executable
- Easy to move or delete - just delete the folder

#### Option 2: Create Desktop Shortcut
1. Right-click on `TodoApp.Desktop.exe`
2. Select "Create shortcut"
3. Move the shortcut to your Desktop
4. Double-click the shortcut to launch the app

#### Option 3: Add to Start Menu
1. Copy `TodoApp.Desktop.exe` to a permanent location (e.g., `C:\Program Files\TodoApp\`)
2. Right-click the executable
3. Select "Pin to Start" or "Pin to taskbar"

### Data Storage

The application stores all data locally:
- **Database:** `todos_desktop.db` (created automatically)
- **Theme Config:** `theme.config` (created automatically)
- **Location:** Same folder as the executable

**Important:** If you move or delete the executable, make sure to also move/keep the database file to preserve your todos!

### Troubleshooting

#### The app won't start
- Make sure you're using Windows 10 or later
- Try right-clicking and selecting "Run as administrator"
- Check if your antivirus is blocking the file (it's safe, but may need to be whitelisted)

#### "Windows protected your PC" message
- This is normal for unsigned executables
- Click "More info" then "Run anyway"
- The application is safe to run

#### Can't find my todos after moving the file
- The database file (`todos_desktop.db`) must be in the same folder as the executable
- Make sure you moved both files together

### Uninstallation

Since this is a portable application:
1. Simply delete the folder containing `TodoApp.Desktop.exe`
2. Optionally delete the database file if you want to remove all data
3. Remove any shortcuts you created

### Features

Once installed, you can:
- ✅ Create and manage todos
- ✅ Organize with categories and priorities
- ✅ Search and filter todos
- ✅ Export/import data
- ✅ Use dark mode
- ✅ Use keyboard shortcuts

### Getting Help

If you encounter any issues:
1. Check that you're using Windows 10 or later
2. Ensure you have sufficient disk space
3. Try running as administrator
4. Check the application folder for error logs

---

**Note:** This is a self-contained application that includes the .NET runtime. No additional software installation is required.

