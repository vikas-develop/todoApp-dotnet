# Todo App - .NET

A modern, full-featured Todo application built with .NET 8.0, available as both a **web application** (ASP.NET Core MVC) and a **desktop application** (Avalonia UI).

## 🚀 Features

### Core Functionality
- ✅ Create, Read, Update, and Delete todos
- ✅ Mark todos as complete/incomplete
- ✅ Beautiful, modern UI with responsive design
- ✅ SQLite database for data persistence
- ✅ Cross-platform desktop support (Windows, Linux, macOS)
- ✅ Clean MVC architecture (Web) and MVVM pattern (Desktop)
- ✅ Real-time status updates with visual indicators

### Advanced Features (Desktop)
- 🔍 **Search & Filter**: Search todos by title/description, filter by status (All, Pending, Completed), and filter by category
- 📊 **Sorting**: Sort todos by date, title, status, or priority (High/Medium/Low)
- 🏷️ **Categories/Tags**: Organize todos with color-coded categories
- ⚡ **Priority Levels**: Set High, Medium, or Low priority with visual indicators
- 💾 **Data Export/Import**: Export todos to JSON or CSV, import from files, and create/restore backups
- 🌙 **Dark Mode**: Toggle between light and dark themes with persistent preference
- ⌨️ **Keyboard Shortcuts**: Fast navigation with keyboard shortcuts
- 🎨 **Animations**: Smooth transitions and animations for better UX
- ✅ **Validation**: Input validation with real-time error messages
- 🔔 **Notifications**: Toast notifications for success and error states
- ❓ **Confirmation Dialogs**: Confirm critical actions like deletion

## 📦 Project Structure

This repository contains two applications:

### 1. Web Application (ASP.NET Core MVC)
- **Project File:** `TodoApp.csproj`
- **Database:** `todos.db`
- **Port:** `http://localhost:5000` or `https://localhost:5001`

### 2. Desktop Application (Avalonia UI)
- **Project File:** `TodoApp.Desktop.csproj`
- **Database:** `todos_desktop.db`
- **Platforms:** Windows, Linux, macOS

```
TodoApp/
├── Controllers/              # Web app controllers
│   └── TodoController.cs
├── Data/
│   └── TodoDbContext.cs     # Entity Framework context
├── Models/
│   ├── Todo.cs              # Todo data model
│   ├── Category.cs          # Category model
│   └── PriorityLevel.cs     # Priority enum
├── Services/
│   ├── TodoService.cs       # Data service layer
│   ├── CategoryService.cs   # Category management
│   ├── ExportService.cs     # Data export functionality
│   ├── ImportService.cs     # Data import functionality
│   ├── ThemeService.cs      # Theme management
│   ├── NotificationService.cs # Toast notifications
│   └── ConfirmationService.cs # Confirmation dialogs
├── ViewModels/              # Desktop app view models
│   ├── MainWindowViewModel.cs
│   ├── TodoViewModel.cs
│   ├── ViewModelBase.cs
│   ├── FilterStatus.cs      # Filter enum
│   └── SortOption.cs        # Sort options enum
├── Views/                   # Both web and desktop views
│   ├── MainWindow.axaml     # Desktop UI
│   ├── Todo/                # Web views
│   └── Shared/              # Shared layouts
├── Converters/              # Desktop value converters
├── wwwroot/                 # Web app static files
│   └── css/
│       └── site.css
├── Assets/                  # Application assets
│   ├── appicon.png
│   └── appicon.ico
├── TodoApp.csproj          # Web application
├── TodoApp.Desktop.csproj  # Desktop application
└── Program.cs              # Desktop app entry point
```

## 🛠️ Prerequisites

- .NET 8.0 SDK or later
- For desktop app: No additional requirements (self-contained builds available)

## 🚀 Getting Started

### Web Application

1. **Restore dependencies:**
   ```bash
   dotnet restore TodoApp.csproj
   ```

2. **Run the application:**
   ```bash
   dotnet run --project TodoApp.csproj
   ```

3. **Open your browser:**
   Navigate to `https://localhost:5001` or `http://localhost:5000`

### Desktop Application

1. **Restore dependencies:**
   ```bash
   dotnet restore TodoApp.Desktop.csproj
   ```

2. **Run the application:**
   ```bash
   dotnet run --project TodoApp.Desktop.csproj
   ```

## 📖 Desktop Application Usage

### Keyboard Shortcuts
- **Ctrl+N**: New Todo (focus on Todos view)
- **Ctrl+S**: Save Todo (when editing)
- **Escape**: Cancel Edit
- **Delete**: Delete selected todo
- **Ctrl+D**: Toggle Dark Mode

### Features Guide

#### Search & Filter
- Use the search box to find todos by title or description
- Filter by status: All, Pending, or Completed
- Filter by category to see todos in specific categories
- Sort by: Date, Title, Status, or Priority

#### Categories
- Create color-coded categories to organize your todos
- Assign categories to todos when creating or editing
- Filter todos by category

#### Priority Levels
- Set priority when creating or editing todos: High, Medium, or Low
- Visual indicators show priority with color-coded badges
- Sort todos by priority to see high-priority items first

#### Export/Import
- **Export to JSON**: Export all todos to a JSON file
- **Export to CSV**: Export todos to a CSV file for spreadsheet applications
- **Create Backup**: Create a complete backup including todos and categories
- **Import from JSON/CSV**: Import todos from external files
- **Restore Backup**: Restore from a backup file

#### Dark Mode
- Toggle dark mode from the menu: **Theme → Toggle Dark Mode** or press **Ctrl+D**
- Theme preference is saved and persists across application restarts

## 📦 Building for Distribution

### Windows Executable

To create a Windows executable:

```bash
dotnet publish TodoApp.Desktop.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true
```

The executable will be at: `bin/Release/net8.0/win-x64/publish/TodoApp.Desktop.exe`

See [BUILD_WINDOWS.md](BUILD_WINDOWS.md) and [DISTRIBUTE_WINDOWS.md](DISTRIBUTE_WINDOWS.md) for detailed instructions.

### Other Platforms

**Linux:**
```bash
dotnet publish TodoApp.Desktop.csproj -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true
```

**macOS:**
```bash
dotnet publish TodoApp.Desktop.csproj -c Release -r osx-x64 --self-contained true -p:PublishSingleFile=true
```

## 🗄️ Database

Both applications use SQLite databases:
- **Web App:** `todos.db` (created automatically)
- **Desktop App:** `todos_desktop.db` (created automatically)

The databases are created automatically on first run in the application directory. The desktop app automatically migrates the database schema when new features are added.

## 🏗️ Architecture

### Web Application
- **Pattern:** MVC (Model-View-Controller)
- **Framework:** ASP.NET Core 8.0
- **UI:** Razor Views with modern CSS

### Desktop Application
- **Pattern:** MVVM (Model-View-ViewModel)
- **Framework:** Avalonia UI 11.0
- **UI:** XAML with Fluent Design
- **Services:** Modular service architecture for data, export/import, themes, and notifications

## 🛠️ Technologies Used

### Web Application
- ASP.NET Core 8.0
- Entity Framework Core 8.0
- SQLite
- MVC Pattern
- Modern CSS with CSS Variables

### Desktop Application
- Avalonia UI 11.0
- .NET 8.0
- Entity Framework Core 8.0
- SQLite
- CommunityToolkit.Mvvm
- ReactiveUI

## 📝 Documentation

- [README.Desktop.md](README.Desktop.md) - Desktop app detailed documentation
- [BUILD_WINDOWS.md](BUILD_WINDOWS.md) - Windows build instructions
- [DISTRIBUTE_WINDOWS.md](DISTRIBUTE_WINDOWS.md) - Distribution guide

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## 📄 License

This project is open source and available for personal and commercial use.

## 🔗 Repository

GitHub: [https://github.com/vikas-develop/todoApp-dotnet](https://github.com/vikas-develop/todoApp-dotnet)

---

**Built with ❤️ using .NET 8.0**
