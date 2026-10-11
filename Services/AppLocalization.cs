using System.Collections.Generic;

namespace KindergartenCRUD.Services
{
    /// <summary>
    /// Minimal English/Arabic localization. IsArabic is a global switch (toggled
    /// from any page) so the whole app stays in sync as you navigate between pages.
    /// </summary>
    public static class AppLocalization
    {
        public static bool IsArabic = false;

        private static readonly Dictionary<string, (string en, string ar)> Map = new()
        {
            ["AppTitle"] = ("Kindergarten Manager", "إدارة الحضانة"),
            ["Login"] = ("Login", "تسجيل الدخول"),
            ["Username"] = ("Username", "اسم المستخدم"),
            ["Password"] = ("Password", "كلمة المرور"),
            ["InvalidCredentials"] = ("Invalid username or password.", "اسم المستخدم أو كلمة المرور غير صحيحة."),

            ["Students"] = ("Students", "الطلاب"),
            ["Name"] = ("Name", "الاسم"),
            ["Age"] = ("Age", "العمر"),
            ["Class"] = ("Class", "الفصل"),
            ["MonthlyFee"] = ("Monthly Fee", "الرسم الشهري"),
            ["Remaining"] = ("Remaining", "المتبقي"),
            ["Notes"] = ("Notes", "ملاحظات"),
            ["Search"] = ("Search", "بحث"),
            ["FilterClass"] = ("All Classes", "كل الفصول"),
            ["AddNew"] = ("Add New Student", "إضافة طالب جديد"),
            ["RecordPayment"] = ("Record Payment", "تسجيل دفعة"),
            ["Edit"] = ("Edit", "تعديل"),
            ["Delete"] = ("Delete", "حذف"),
            ["Print"] = ("Print Receipt", "طباعة الإيصال"),
            ["Logout"] = ("Logout", "تسجيل الخروج"),
            ["Save"] = ("Save", "حفظ"),
            ["Cancel"] = ("Cancel", "إلغاء"),
            ["DateOfBirth"] = ("Date of Birth", "تاريخ الميلاد"),
            ["SelectRowFirst"] = ("Select a student first.", "الرجاء اختيار طالب أولاً."),
            ["ConfirmDelete"] = ("Delete this student?", "هل تريد حذف هذا الطالب؟"),
            ["Month"] = ("Month", "الشهر"),
            ["Amount"] = ("Amount", "المبلغ"),
            ["SelectMonth"] = ("Please select a month.", "الرجاء اختيار الشهر."),
            ["TotalCollected"] = ("Total Collected", "إجمالي المحصّل"),
            ["TotalRemaining"] = ("Total Remaining", "إجمالي المتبقي"),
            ["StudentsCount"] = ("Students", "عدد الطلاب"),

            // Month headers - abbreviated in English to keep grid columns narrow,
            // full names in Arabic (there's no common Arabic abbreviation).
            ["Month1"] = ("Jan", "يناير"),
            ["Month2"] = ("Feb", "فبراير"),
            ["Month3"] = ("Mar", "مارس"),
            ["Month4"] = ("Apr", "أبريل"),
            ["Month5"] = ("May", "مايو"),
            ["Month6"] = ("Jun", "يونيو"),
            ["Month7"] = ("Jul", "يوليو"),
            ["Month8"] = ("Aug", "أغسطس"),
            ["Month9"] = ("Sep", "سبتمبر"),
            ["Month10"] = ("Oct", "أكتوبر"),
            ["Month11"] = ("Nov", "نوفمبر"),
            ["Month12"] = ("Dec", "ديسمبر"),
        };

        public static string T(string key) =>
            Map.TryGetValue(key, out var v) ? (IsArabic ? v.ar : v.en) : key;

        /// <summary>Localized month name for the Record Payment dropdown and receipts (1 = January).</summary>
        public static string MonthName(int month) => T($"Month{month}");
    }
}
