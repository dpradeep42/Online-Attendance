# Online-Attendance

An online attendance system built with ASP.NET MVC 5 and Entity Framework 6. It has three roles:

- **Admin** - manage classes, faculty and students
- **Faculty** - view assigned classes and mark students present or absent; parents are notified by email
- **Student** - a basic student landing page (not built out yet)

Pages are protected with session-check action filters, and the data model uses an Entity Framework database-first (EDMX) model on SQL Server.

## Tech stack

- C#, ASP.NET MVC 5, .NET Framework 4.8
- Entity Framework 6 (database first)
- SQL Server
- Bootstrap, jQuery

## How to run

1. Restore the `attendance.bak` backup to a local SQL Server instance as a database named `Attendance`.
2. Check the connection string in `OnlineAttendance/Web.config` (it uses `localhost` with integrated security).
3. To send emails, set your own Gmail account details in the `GMailer` class in `OnlineAttendance/Models/CustomModel.cs`.
4. Open `OnlineAttendance.sln` in Visual Studio, restore NuGet packages and run with IIS Express.

## Structure

- `OnlineAttendance/Controllers` - Admin, Faculty, Student and Home (login) controllers
- `OnlineAttendance/Models` - EF model and view models
- `OnlineAttendance/Views` - Razor views
- `attendance.bak` - SQL Server database backup
