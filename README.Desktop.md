# Todo App - Desktop Version

A modern, cross-platform desktop Todo application built with Avalonia UI and .NET 8.

## Features

- ✅ Create, Read, Update, and Delete todos
- ✅ Mark todos as complete/incomplete
- ✅ Beautiful, modern UI with Fluent Design
- ✅ SQLite database for data persistence
- ✅ MVVM architecture with CommunityToolkit.Mvvm
- ✅ Cross-platform (Windows, Linux, macOS)

## Prerequisites

- .NET 8.0 SDK or later

## Getting Started

1. **Build the application:**
   ```bash
   dotnet build TodoApp.Desktop.csproj
   ```

2. **Run the application:**
   ```bash
   dotnet run --project TodoApp.Desktop.csproj
   ```

## Project Structure

```
TodoApp/
├── ViewModels/
│   ├── MainWindowViewModel.cs    # Main view model with business logic
│   ├── TodoViewModel.cs           # Todo item view model
│   └── ViewModelBase.cs          # Base view model class
├── Views/
│   └── MainWindow.axaml          # Main window UI (XAML)
├── Models/
│   └── Todo.cs                   # Todo data model
├── Services/
│   └── TodoService.cs            # Data service layer
├── Data/
│   └── TodoDbContext.cs         # Entity Framework context
├── Converters/
│   └── *.cs                      # Value converters for bindings
└── Program.cs                    # Application entry point
```

## Database

The application uses SQLite database (`todos_desktop.db`) which is automatically created on first run. The database file will be created in the application directory.

## Technologies Used

- **Avalonia UI 11.0** - Cross-platform UI framework
- **.NET 8.0** - Runtime and framework
- **Entity Framework Core 8.0** - ORM for data access
- **SQLite** - Embedded database
- **CommunityToolkit.Mvvm** - MVVM helpers
- **ReactiveUI** - Reactive programming support

## Architecture

The application follows the **MVVM (Model-View-ViewModel)** pattern:

- **Model**: `Todo` class represents the data
- **View**: XAML files define the UI
- **ViewModel**: Contains presentation logic and state management

## Building for Distribution

To create a self-contained executable:

```bash
dotnet publish TodoApp.Desktop.csproj -c Release -r linux-x64 --self-contained true
```

Replace `linux-x64` with:
- `win-x64` for Windows
- `osx-x64` or `osx-arm64` for macOS

## License

This project is open source and available for personal and commercial use.

