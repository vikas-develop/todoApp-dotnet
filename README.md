# Todo App

A modern, full-featured Todo application built with ASP.NET Core MVC and Entity Framework Core.

## Features

- ✅ Create, Read, Update, and Delete todos
- ✅ Mark todos as complete/incomplete
- ✅ Beautiful, modern UI with responsive design
- ✅ SQLite database for data persistence
- ✅ Clean MVC architecture

## Prerequisites

- .NET 8.0 SDK or later

## Getting Started

1. **Restore dependencies:**
   ```bash
   dotnet restore
   ```

2. **Run the application:**
   ```bash
   dotnet run
   ```

3. **Open your browser:**
   Navigate to `https://localhost:5001` or `http://localhost:5000`

## Project Structure

```
TodoApp/
├── Controllers/
│   └── TodoController.cs      # Handles all todo operations
├── Data/
│   └── TodoDbContext.cs       # Entity Framework context
├── Models/
│   └── Todo.cs                # Todo model
├── Views/
│   ├── Todo/                  # Todo views
│   └── Shared/                # Shared layouts
├── wwwroot/
│   └── css/
│       └── site.css           # Modern styling
└── Program.cs                 # Application entry point
```

## Database

The application uses SQLite database (`todos.db`) which is automatically created on first run. The database file will be created in the project root directory.

## Technologies Used

- ASP.NET Core 8.0
- Entity Framework Core 8.0
- SQLite
- MVC Pattern
- Modern CSS with CSS Variables

## License

This project is open source and available for personal and commercial use.

