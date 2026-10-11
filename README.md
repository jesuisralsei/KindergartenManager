# 🎓 Kindergarten Manager | مدير إدارة الحضانة

**English** | [العربية](#-مدير-إدارة-الحضانة)

A WPF desktop application for managing kindergarten students, monthly payments, student records, and thermal receipts. Built with **C#, .NET 8, WPF, and SQLite**, with full Arabic (RTL) and English language support.

تطبيق سطح مكتب لإدارة طلاب الحضانة، ومتابعة المصروفات الشهرية، وسجلات الطلاب، وطباعة الإيصالات الحرارية. تم تطويره باستخدام **C# و.NET 8 وWPF وSQLite**، مع دعم كامل للغتين العربية والإنجليزية واتجاه الكتابة من اليمين إلى اليسار.

---

## ✨ Features | المميزات

- 🔐 **Login System** — Restricted access through a login screen.
- 👨‍🎓 **Student Management** — Add, edit, and delete student records.
- 💰 **Monthly Payment Tracking** — Track payments for all 12 months.
- 📊 **Live Financial Summary** — View collected payments and outstanding balances.
- 🔎 **Search & Filtering** — Find students by name and filter by class.
- 📝 **Student Notes** — Store allergies, pickup instructions, and other important information.
- 🧾 **Thermal Receipt Printing** — Print receipts using the Windows print dialog, with support for approximately 80 mm receipt rolls.
- 🌐 **Bilingual Interface** — Switch between Arabic and English.
- ↔️ **RTL Layout** — Automatically switch the interface direction when Arabic is selected.
- 🗄️ **SQLite Database** — Store student records and payment data locally.
- 🖥️ **Standalone Executable** — Publish a self-contained Windows executable without requiring a separate .NET installation.

### المميزات بالعربية

- 🔐 **نظام تسجيل الدخول:** حماية واجهة التطبيق بشاشة تسجيل دخول.
- 👨‍🎓 **إدارة الطلاب:** إضافة بيانات الطلاب وتعديلها وحذفها.
- 💰 **متابعة المصروفات الشهرية:** تسجيل المدفوعات لكل شهر من يناير إلى ديسمبر.
- 📊 **ملخص مالي مباشر:** عرض إجمالي المدفوعات والمبالغ المتبقية.
- 🔎 **البحث والتصفية:** البحث عن الطلاب بالاسم وتصفيتهم حسب الفصل.
- 📝 **ملاحظات الطلاب:** تسجيل الحساسية وتعليمات الاستلام وأي معلومات مهمة.
- 🧾 **طباعة الإيصالات الحرارية:** طباعة الإيصالات باستخدام نافذة الطباعة في Windows، مع دعم مقاس ورق يقارب 80 مم.
- 🌐 **واجهة ثنائية اللغة:** التبديل بين العربية والإنجليزية.
- ↔️ **دعم RTL:** تغيير اتجاه الواجهة تلقائيًا عند اختيار العربية.
- 🗄️ **قاعدة بيانات SQLite:** تخزين بيانات الطلاب والمدفوعات محليًا.
- 🖥️ **ملف تنفيذي مستقل:** إنشاء نسخة تعمل على Windows دون الحاجة إلى تثبيت .NET بشكل منفصل.

---

## 🛠️ Technologies Used | التقنيات المستخدمة

| Technology | Purpose |
|---|---|
| C# | Application logic |
| .NET 8 | Runtime and framework |
| WPF | Desktop user interface |
| SQLite | Local database |
| Microsoft.Data.Sqlite | SQLite database connectivity |
| XAML | UI layout and styling |

---

## 📋 Requirements | المتطلبات

- Windows 10 or Windows 11
- [.NET 8 SDK](https://dotnet.microsoft.com/download) — required for development
- Visual Studio 2022 with the **.NET desktop development** workload, or the .NET CLI
- Internet connection for the initial NuGet package restore

> **Note:** The published self-contained executable does not require a separate .NET installation.

---

## 🚀 Getting Started | البدء

### 1. Clone the repository | استنساخ المشروع

```bash
git clone https://github.com/Jesuisralsei/KindergartenCRUD.git
cd KindergartenCRUD
```




### 2. Run the application | تشغيل التطبيق

**Option A — Visual Studio**

1. Open `KindergartenCRUD.csproj` in Visual Studio 2022.
2. Make sure the **.NET desktop development** workload is installed.
3. Press `F5` to build and run the application.

**Option B — Command Line**

```bash
dotnet restore
dotnet run
```

The initial restore downloads the required NuGet packages.

### 3. Login credentials | بيانات تسجيل الدخول

The current version uses a single hardcoded account.

| Field | Value |
|---|---|
| Username | `Example` |
| Password | `Example` |

**Security note:** These credentials are hardcoded for the current version. Replace them with a secure authentication system before using the application to manage sensitive real-world records.

**ملاحظة أمنية:** بيانات الدخول الحالية مكتوبة مباشرة داخل الكود. يُنصح باستبدالها بنظام مصادقة آمن قبل استخدام التطبيق لإدارة بيانات حقيقية وحساسة.

---

## 🏗️ Publishing the Application | إنشاء النسخة النهائية

To build a self-contained Windows x64 executable:

```bash
dotnet publish -c Release
```

The published files will be located at:

```text
bin/Release/net8.0-windows/win-x64/publish/
```

The main executable is:

```text
KindergartenCRUD.exe
```

The project is configured for a self-contained publish with an embedded application icon.

> **Note:** A self-contained publish does not automatically guarantee that every dependency and resource is packaged into a single file. The output may include additional files depending on the project configuration.

---

## 📱 Application Pages | صفحات التطبيق

### 1. Login Page | تسجيل الدخول

- Restricts access to the application.
- Uses a single predefined account in the current version.

تمنع الوصول إلى التطبيق دون إدخال بيانات الحساب المحدد في الإصدار الحالي.

### 2. Students Dashboard | لوحة إدارة الطلاب

- Spreadsheet-style student table.
- Student name, age, class, and monthly fee information.
- January–December payment columns.
- Remaining balance calculated dynamically.
- Live search and class filtering.
- Summary of visible students, collected payments, and remaining balances.
- Actions for recording payments, editing, deleting, and printing receipts.
- Color-coded balances: green for fully paid students and red for outstanding balances.

تعرض بيانات الطلاب في جدول منظم يشمل الاسم والعمر والفصل والمدفوعات الشهرية، مع البحث والتصفية والملخص المالي وأدوات إدارة السجلات.

### 3. Add / Edit Student | إضافة طالب أو تعديل بياناته

Student information includes:

- Name
- Date of birth
- Automatically calculated age
- Class
- Monthly fee
- Notes

تشمل بيانات الطالب الاسم وتاريخ الميلاد والعمر المحسوب تلقائيًا والفصل والمصروف الشهري والملاحظات.

### 4. Record Payment | تسجيل المدفوعات

- Requires selecting a month before saving.
- Allows recording a payment for a selected student.
- Loads the existing amount when editing a previously recorded payment.
- Updates the financial summary automatically.

يشترط اختيار الشهر قبل الحفظ، ويتيح تسجيل المدفوعات وتعديل المبلغ المسجل مسبقًا دون إنشاء سجل مكرر للطالب والشهر نفسه.

---

## 🗄️ Database Structure | هيكل قاعدة البيانات

The application uses SQLite and creates its database automatically on the first run.

**Database location:**

```text
%AppData%\KindergartenCRUD\kindergarten.db
```

### Students Table

| Column | Description |
|---|---|
| Id | Student identifier |
| Name | Student name |
| Age | Calculated age |
| DateOfBirth | Date of birth |
| ClassName | Student's class |
| MonthlyFee | Monthly tuition fee |
| Notes | Additional student information |

### Payments Table

| Column | Description |
|---|---|
| StudentId | Associated student |
| Month | Month number (1–12) |
| Amount | Recorded payment amount |

The application calculates the remaining balance dynamically:

```text
Remaining = (MonthlyFee × 12) − TotalRecordedPayments
```

This means the displayed balance is calculated from the current student fee and recorded payments rather than stored as a separate database value.

### بنية قاعدة البيانات

يستخدم التطبيق قاعدة بيانات SQLite يتم إنشاؤها تلقائيًا عند التشغيل لأول مرة.

**مسار قاعدة البيانات:**

```text
%AppData%\KindergartenCRUD\kindergarten.db
```

تحتوي قاعدة البيانات على جدولين:

- **Students:** بيانات الطلاب، بما في ذلك الاسم والعمر وتاريخ الميلاد والفصل والمصروف والملاحظات.
- **Payments:** بيانات المدفوعات، وتشمل الطالب والشهر والمبلغ.

يتم حساب المبلغ المتبقي تلقائيًا من إجمالي المصروفات السنوية والمدفوعات المسجلة.

> You can inspect or back up the database using a compatible SQLite browser, such as [DB Browser for SQLite](https://sqlitebrowser.org/).

---

## 🧾 Thermal Receipt Printing | طباعة الإيصالات الحرارية

The application uses the standard Windows print dialog instead of printer-specific ESC/POS commands.

The receipt can include:

- Student name
- Class
- Age
- Monthly fee
- Total recorded payments
- Remaining balance
- Notes, if available
- Print date

The receipt is designed for an approximately 80 mm thermal paper roll. Actual formatting and output depend on the selected printer and its Windows driver.

يعتمد التطبيق على نافذة الطباعة القياسية في Windows، مما يسمح باختيار الطابعة الحرارية المثبتة على الجهاز.

يمكن أن يتضمن الإيصال اسم الطالب والفصل والعمر والمصروف الشهري وإجمالي المدفوعات والمبلغ المتبقي والملاحظات وتاريخ الطباعة.

---

## 🌐 Localization | دعم اللغات

The application supports Arabic and English through a shared localization service:

```text
Services/Localization.cs
```

The language toggle is available on every page. Switching languages updates interface labels, buttons, table headers, and month names, while adjusting the layout direction.

يدعم التطبيق العربية والإنجليزية من خلال خدمة موحدة لإدارة النصوص والترجمة. يتوفر زر تغيير اللغة في جميع الصفحات، ويؤدي التبديل إلى تحديث النصوص وعناوين الجداول وأسماء الشهور واتجاه الواجهة.

Arabic text can be entered in student names, class names, and notes regardless of the selected interface language.

يمكن إدخال النصوص العربية في أسماء الطلاب والفصول والملاحظات بغض النظر عن لغة الواجهة المختارة.

---

## 🎨 Application Icon | أيقونة التطبيق

The application uses a graduation-cap icon located at:

```text
Assets/icon.ico
```

The icon is configured for use in the application window and the compiled executable. Replace this file with another compatible multi-resolution `.ico` file to change the icon.

يستخدم التطبيق أيقونة قبعة تخرج موجودة داخل مجلد `Assets`. يمكن استبدالها بأيقونة أخرى متوافقة بصيغة `.ico`.

---

## 🔧 Future Improvements | تطويرات مستقبلية

Potential improvements include:

- **Multiple user accounts:** Add a SQLite users table and secure password hashing.
- **Payment audit history:** Record payment timestamps and the user responsible for each transaction.
- **Additional student fields:** Extend the student model and database schema.
- **Improved data protection:** Add authentication hardening and secure backup procedures.

### تطويرات مستقبلية مقترحة

- **تعدد المستخدمين:** إضافة جدول للمستخدمين وتخزين كلمات المرور باستخدام التجزئة الآمنة.
- **سجل تاريخي للمدفوعات:** تسجيل تاريخ كل عملية دفع والمستخدم الذي نفذها.
- **إضافة بيانات للطلاب:** توسيع نموذج الطالب وقاعدة البيانات.
- **تحسين حماية البيانات:** تطوير نظام تسجيل الدخول وآليات النسخ الاحتياطي الآمن.

---

## 📄 License | الترخيص

Add a `LICENSE` file to the repository if you intend to distribute the project under a specific open-source license.

أضف ملف `LICENSE` إلى المستودع إذا كنت ترغب في نشر المشروع بموجب ترخيص مفتوح المصدر محدد.

---

**Built with C# and WPF. | تم تطويره باستخدام C# وWPF.**
