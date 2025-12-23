# Todo App - Desktop Version

A modern, cross-platform desktop Todo application built with Avalonia UI and .NET 8, featuring a comprehensive set of productivity features.

## ✨ Features

### Core Functionality
- ✅ Create, Read, Update, and Delete todos
- ✅ Mark todos as complete/incomplete
- ✅ Beautiful, modern UI with Fluent Design
- ✅ SQLite database for data persistence
- ✅ MVVM architecture with CommunityToolkit.Mvvm
- ✅ Cross-platform (Windows, Linux, macOS)

### Advanced Features

#### 🔍 Search & Filter
- **Search**: Search todos by title or description in real-time
- **Status Filter**: Filter by All, Pending, or Completed todos
- **Category Filter**: Filter todos by assigned category
- **Sort Options**: Sort by:
  - Date (Newest/Oldest First)
  - Title (A-Z / Z-A)
  - Status (Pending/Completed First)
  - Priority (High/Low First)

#### 🏷️ Categories & Tags
- Create custom categories with color coding
- Assign categories to todos
- Visual category indicators in todo list
- Filter todos by category
- Manage categories from dedicated view

#### ⚡ Priority Levels
- Set priority levels: **High**, **Medium**, or **Low**
- Visual priority indicators with color-coded badges:
  - 🔴 High Priority (Red)
  - 🟡 Medium Priority (Amber)
  - 🟢 Low Priority (Green)
- Sort todos by priority to focus on important tasks

#### 💾 Data Export & Import
- **Export to JSON**: Export all todos to a formatted JSON file
- **Export to CSV**: Export todos to CSV for spreadsheet applications
- **Create Backup**: Complete backup including todos and categories with metadata
- **Import from JSON**: Import todos from JSON files
- **Import from CSV**: Import todos from CSV files
- **Restore Backup**: Restore complete application state from backup

#### 🌙 Dark Mode
- Toggle between light and dark themes
- Theme preference persists across sessions
- Access via menu: **Theme → Toggle Dark Mode** or **Ctrl+D**

#### ⌨️ Keyboard Shortcuts
- **Ctrl+N**: New Todo (navigate to Todos view)
- **Ctrl+S**: Save Todo (when editing)
- **Escape**: Cancel Edit
- **Delete**: Delete selected todo
- **Ctrl+D**: Toggle Dark Mode

#### 🎨 UI/UX Enhancements
- Smooth animations and transitions
- Toast notifications for user feedback
- Confirmation dialogs for critical actions
- Collapsible sections for better organization
- Real-time input validation with error messages
- Visual indicators for todos (status, category, priority)

#### ✅ Validation & Error Handling
- Title validation (required, max 200 characters)
- Description validation (max 1000 characters)
- Real-time error messages
- Confirmation dialogs for delete operations
- Toast notifications for success and error states

## 🚀 Getting Started

### Prerequisites

- .NET 8.0 SDK or later

### Running the Application

1. **Build the application:**
   ```bash
   dotnet build TodoApp.Desktop.csproj
   ```

2. **Run the application:**
   ```bash
   dotnet run --project TodoApp.Desktop.csproj
   ```

## 📁 Project Structure

```
TodoApp/
├── ViewModels/
│   ├── MainWindowViewModel.cs    # Main view model with business logic
│   ├── TodoViewModel.cs           # Todo item view model
│   ├── ViewModelBase.cs          # Base view model class
│   ├── FilterStatus.cs           # Filter status enum
│   └── SortOption.cs             # Sort options enum
├── Views/
│   ├── MainWindow.axaml          # Main window UI (XAML)
│   └── MainWindow.axaml.cs      # Code-behind
├── Models/
│   ├── Todo.cs                   # Todo data model
│   ├── Category.cs               # Category model
│   └── PriorityLevel.cs          # Priority level enum
├── Services/
│   ├── TodoService.cs            # Data service layer
│   ├── CategoryService.cs        # Category management
│   ├── ExportService.cs          # Export functionality
│   ├── ImportService.cs          # Import functionality
│   ├── ThemeService.cs           # Theme management
│   ├── NotificationService.cs    # Toast notifications
│   └── ConfirmationService.cs    # Confirmation dialogs
├── Data/
│   └── TodoDbContext.cs          # Entity Framework context
├── Converters/
│   └── *.cs                      # Value converters for bindings
├── Assets/
│   ├── appicon.png               # Application icon (PNG)
│   └── appicon.ico               # Application icon (ICO)
└── Program.cs                    # Application entry point
```

## 🗄️ Database

The application uses SQLite database (`todos_desktop.db`) which is automatically created on first run. The database file will be created in the application directory.

### Database Schema
- **Todos Table**: Stores todo items with title, description, completion status, dates, category, and priority
- **Categories Table**: Stores category information with name and color
- **Automatic Migration**: The app automatically updates the database schema when new features are added

## 🎯 Usage Guide

### Creating a Todo
1. Navigate to the Todos view
2. Expand the "Add New Todo" section
3. Enter a title (required, max 200 characters)
4. Optionally add a description (max 1000 characters)
5. Select a category (optional)
6. Choose a priority level (default: Medium)
7. Click "Add Todo"

### Managing Categories
1. Navigate to **View → Categories** or **Tools → Manage Categories**
2. Click "Add Category"
3. Enter category name and choose a color
4. Categories can be edited or deleted

### Exporting Data
1. Go to **File → Export**
2. Choose export format:
   - **Export to JSON**: For JSON format
   - **Export to CSV**: For spreadsheet applications
   - **Create Backup**: Complete backup with metadata

### Importing Data
1. Go to **File → Import**
2. Select import type:
   - **Import from JSON**: Import from JSON file
   - **Import from CSV**: Import from CSV file
   - **Restore Backup**: Restore from backup file
3. Confirm the import operation

### Keyboard Shortcuts
- Use keyboard shortcuts for faster navigation and actions
- All shortcuts are listed in the menu items
- **Ctrl+D** toggles dark mode instantly

## 🛠️ Technologies Used

- **Avalonia UI 11.0** - Cross-platform UI framework
- **.NET 8.0** - Runtime and framework
- **Entity Framework Core 8.0** - ORM for data access
- **SQLite** - Embedded database
- **CommunityToolkit.Mvvm** - MVVM helpers and commands
- **ReactiveUI** - Reactive programming support

## 🏗️ Architecture

The application follows the **MVVM (Model-View-ViewModel)** pattern:

- **Model**: `Todo`, `Category`, and `PriorityLevel` classes represent the data
- **View**: XAML files define the UI with data bindings
- **ViewModel**: Contains presentation logic, state management, and commands
- **Services**: Modular services handle data access, export/import, themes, and notifications

### Key Components

#### Services
- **TodoService**: Handles CRUD operations for todos
- **CategoryService**: Manages category operations
- **ExportService**: Handles data export to JSON/CSV
- **ImportService**: Handles data import from files
- **ThemeService**: Manages theme switching and persistence
- **NotificationService**: Displays toast notifications
- **ConfirmationService**: Shows confirmation dialogs

#### ViewModels
- **MainWindowViewModel**: Main application logic and state
- **TodoViewModel**: Individual todo item presentation logic

#### Converters
- Value converters for data transformation (status, colors, text decorations, etc.)

## 📦 Building for Distribution

### Self-Contained Executable

To create a self-contained executable:

**Windows:**
```bash
dotnet publish TodoApp.Desktop.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true
```

**Linux:**
```bash
dotnet publish TodoApp.Desktop.csproj -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true
```

**macOS:**
```bash
dotnet publish TodoApp.Desktop.csproj -c Release -r osx-x64 --self-contained true -p:PublishSingleFile=true
```

The executable will be in: `bin/Release/net8.0/{runtime}/publish/`

### Application Icon

The application includes a custom icon (`appicon.ico` for Windows) that will be used when packaging the application.

## 🔧 Configuration

### Theme Configuration
Theme preference is stored in `theme.config` file in the application directory. The file contains either "Light" or "Dark" and is automatically managed by the application.

### Database Location
The SQLite database (`todos_desktop.db`) is created in the application directory. For self-contained deployments, it will be in the same directory as the executable.

## 🐛 Troubleshooting

### Export/Import Not Working
- Ensure the application has file system permissions
- Check that the file paths are accessible
- Verify file formats match the expected format (JSON/CSV)

### Theme Not Persisting
- Check that the application has write permissions in its directory
- Verify `theme.config` file is not read-only

### Database Issues
- The database is automatically created and migrated
- If issues occur, you can delete `todos_desktop.db` to start fresh (⚠️ this will delete all data)

## 📝 License

This project is open source and available for personal and commercial use.

## 🔗 Repository

GitHub: [https://github.com/vikas-develop/todoApp-dotnet](https://github.com/vikas-develop/todoApp-dotnet)

---

**Built with ❤️ using .NET 8.0 and Avalonia UI**
