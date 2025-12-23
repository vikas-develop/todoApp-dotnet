# Todo App - .NET

A modern, full-featured Todo application built with .NET 8.0, available as both a **web application** (ASP.NET Core MVC) and a **desktop application** (Avalonia UI).

## 🚀 Features

- ✅ Create, Read, Update, and Delete todos
- ✅ Mark todos as complete/incomplete
- ✅ Beautiful, modern UI with responsive design
- ✅ SQLite database for data persistence
- ✅ Cross-platform desktop support (Windows, Linux, macOS)
- ✅ Clean MVC architecture (Web) and MVVM pattern (Desktop)
- ✅ Real-time status updates with visual indicators

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
│   └── Todo.cs              # Todo data model
├── Services/
│   └── TodoService.cs       # Data service layer
├── ViewModels/              # Desktop app view models
│   ├── MainWindowViewModel.cs
│   ├── TodoViewModel.cs
│   └── ViewModelBase.cs
├── Views/                   # Both web and desktop views
│   ├── MainWindow.axaml     # Desktop UI
│   ├── Todo/                # Web views
│   └── Shared/              # Shared layouts
├── Converters/              # Desktop value converters
├── wwwroot/                 # Web app static files
│   └── css/
│       └── site.css
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

The databases are created automatically on first run in the application directory.

## 🏗️ Architecture

### Web Application
- **Pattern:** MVC (Model-View-Controller)
- **Framework:** ASP.NET Core 8.0
- **UI:** Razor Views with modern CSS

### Desktop Application
- **Pattern:** MVVM (Model-View-ViewModel)
- **Framework:** Avalonia UI 11.0
- **UI:** XAML with Fluent Design

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

- [README.Desktop.md](README.Desktop.md) - Desktop app documentation
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
