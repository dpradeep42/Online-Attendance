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
2. Connection strings in `OnlineAttendance/Web.config` use `localhost` and Integrated Security, so no SQL password is stored in the repo. If the server requires SQL authentication, copy `OnlineAttendance/ConnectionStrings.secrets.config.example` to `OnlineAttendance/ConnectionStrings.secrets.config`, replace `YOUR_SQL_USER` and `YOUR_SQL_PASSWORD`, and switch the `connectionStrings` element to `configSource="ConnectionStrings.secrets.config"`. That secrets file is gitignored. Do not commit a password in `Web.config`.
3. Mail is sent through `GMailer` (`OnlineAttendance/Models/CustomModel.cs`). It reads SMTP settings from configuration, not from source:
   - Copy `OnlineAttendance/AppSettings.secrets.config.example` to `OnlineAttendance/AppSettings.secrets.config` and set `SmtpUser` and `SmtpPassword`. `Web.config` merges that file with `appSettings file="AppSettings.secrets.config"`. The file is gitignored. If it is missing, the app still starts.
   - Or set environment variables. Each app setting name works (`SmtpUser`, `SmtpPassword`, `SmtpHost`, `SmtpPort`, `SmtpEnableSsl`), and so do `SMTP_USER`, `SMTP_PASSWORD`, `SMTP_HOST`, `SMTP_PORT` and `SMTP_ENABLE_SSL`. Environment variables override the secrets file.
   - Unset host, port and SSL keep the previous defaults: `smtp.gmail.com`, port `587`, SSL on. For Gmail with 2-step verification, use an app password.
   - If a mailbox password was ever committed in source, revoke it in the mail provider and put only the new value in the secrets file or the environment.
4. Open `OnlineAttendance.sln` in Visual Studio, restore NuGet packages and run with IIS Express.

## Structure

- `OnlineAttendance/Controllers` - Admin, Faculty, Student and Home (login) controllers
- `OnlineAttendance/Models` - EF model and view models
- `OnlineAttendance/Views` - Razor views
- `attendance.bak` - SQL Server database backup
