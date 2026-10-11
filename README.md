# Kindergarten Manager (WPF)

A WPF desktop app for managing kindergarten students, backed by SQLite, with a
login gate, a filterable spreadsheet-style grid with Jan-Dec payment tracking,
notes, thermal receipt printing, and full Arabic (RTL) support.

## Requirements
- Windows 10/11 (WPF only runs on Windows)
- [.NET 8 SDK](https://dotnet.microsoft.com/download)

## How to run (development)

**Visual Studio**: open `KindergartenCRUD.csproj` (2022, ".NET desktop
development" workload), press F5.

**Command line**:
```
cd KindergartenCRUD
dotnet restore
dotnet run
```
The first `dotnet restore`/build downloads the `Microsoft.Data.Sqlite` NuGet
package automatically (internet connection needed once).

## Building the final .exe

The project is already configured to produce a single, self-contained
`KindergartenCRUD.exe` with the icon embedded - no separate .NET install
needed on the machine you run it on:

```
cd KindergartenCRUD
dotnet publish -c Release
```

The finished executable lands at:
```
bin\Release\net8.0-windows\win-x64\publish\KindergartenCRUD.exe
```

## The four pages
1. **Login** (`Pages/LoginPage`) — only one account exists, hardcoded:
   - Username: `Yasin`
   - Password: `01001285848`
   No registration, no guest mode; nothing else in the app is reachable
   without these exact credentials.

2. **Students / data page** (`Pages/DataPage`) — the spreadsheet view:
   Name, Age, Class, one column per month (Jan-Dec, labeled in Arabic when
   Arabic is selected), Remaining, and Notes. Includes a live name search box,
   a class filter dropdown, and a small totals bar under the grid showing how
   many students are visible plus total collected/remaining for them. Select
   a row to Record Payment, Edit, Delete, or Print Receipt. The Remaining
   column turns green once a student is fully paid up and red while they
   still owe money.

3. **Add / Edit student** (`Pages/StudentFormPage`) — registers a new student
   or edits an existing one: name, date of birth (age is calculated
   automatically), class, monthly fee, and a free-text notes field (allergies,
   pickup instructions, anything staff want to remember).

4. **Record Payment** (`Pages/PaymentPage`) — the only way to record a
   payment. You must pick a month from the dropdown first; Save is refused
   with a message until a month is chosen. Picking a month that's already
   been paid pre-fills its existing amount, so re-opening it acts as an edit
   rather than creating a duplicate.

## Database
Two SQLite tables, created automatically on first run, stored at:
`%AppData%\KindergartenCRUD\kindergarten.db`

- **Students** — Id, Name, Age, DateOfBirth, ClassName, MonthlyFee, Notes
- **Payments** — StudentId, Month (1-12), Amount — one row per student per
  month that's had a payment recorded. `Remaining` is always calculated on
  the fly as `MonthlyFee × 12 − sum of all recorded payments`, so it's never
  stale.

You can open the .db file with any SQLite browser (e.g. DB Browser for
SQLite) to inspect or back up the data directly.

## Thermal receipt printing
Selecting a student and clicking **Print Receipt** opens the normal Windows
print dialog so you can pick your printer. This avoids printer-specific
ESC/POS commands: almost every USB/serial thermal receipt printer ships a
Windows driver and shows up in that dialog like any other printer. The
receipt prints name, class, age, monthly fee, total paid, remaining, notes
(if any), and the date - sized for an ~80mm roll, in whichever language is
currently selected.

## Arabic language support
Every page has a language toggle button (top-right corner: "العربية" / "English").
Toggling it switches all labels, buttons, grid headers, and month names to
Arabic, and flips the whole layout to right-to-left. The choice is a single
global switch (`Services/Localization.cs`) shared across all four pages, so
it stays consistent as you navigate. Arabic text can also be typed directly
into any Name/Class/Notes field regardless of the toggle.

## App icon
A cute graduation-cap icon (`Assets/icon.ico`) is embedded in both the
compiled .exe and the app window itself. Replace `Assets/icon.ico` with any
multi-resolution .ico file of the same name to swap it.

## Extending it
- **More users**: replace the two `const` strings in `LoginPage.xaml.cs` with
  a small `Users` table in SQLite (plus password hashing) if you'll ever have
  more than one staff login.
- **Payment history**: the Payments table already stores one row per
  student/month; if you want a full audit trail (who recorded what, when),
  add a `RecordedAt` column and stop overwriting on `ON CONFLICT`.
- **More fields**: add a property to `Models/Student.cs`, a column in
  `DatabaseService.Initialize()`, a control on `StudentFormPage`, and wire it
  into `AddStudentParams`/`GetAll`/`SaveButton_Click`.
