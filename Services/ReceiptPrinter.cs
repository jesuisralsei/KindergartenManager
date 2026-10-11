using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using KindergartenCRUD.Models;

namespace KindergartenCRUD.Services
{
    /// <summary>
    /// Prints a small receipt for a single student. Uses plain GDI printing
    /// (System.Drawing.Printing) rather than raw ESC/POS commands, because
    /// almost every USB/serial thermal receipt printer ships a Windows driver
    /// and shows up as a normal printer - so no special protocol is needed.
    /// </summary>
    public static class ReceiptPrinter
    {
        public static void PrintReceipt(Student student, bool arabic)
        {
            using var document = new PrintDocument();

            // Most receipt rolls are 80mm (~3.15in) or 58mm (~2.28in) wide.
            // Try to find a paper size close to 80mm on the selected printer;
            // otherwise fall back to whatever the driver defaults to.
            foreach (PaperSize size in document.PrinterSettings.PaperSizes)
            {
                if (Math.Abs(size.Width - 315) < 40)
                {
                    document.DefaultPageSettings.PaperSize = size;
                    break;
                }
            }
            document.DefaultPageSettings.Margins = new Margins(10, 10, 10, 10);

            document.PrintPage += (sender, e) => DrawReceipt(e, student, arabic);

            using var dialog = new PrintDialog
            {
                Document = document,
                AllowSomePages = false,
                AllowSelection = false,
                AllowPrintToFile = false,
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                document.PrinterSettings = dialog.PrinterSettings;
                document.Print();
            }
        }

        private static void DrawReceipt(PrintPageEventArgs e, Student student, bool arabic)
        {
            var g = e.Graphics;
            if (g == null) return;

            using var titleFont = new Font("Tahoma", 13, FontStyle.Bold);
            using var textFont = new Font("Tahoma", 10);

            var format = new StringFormat();
            if (arabic) format.FormatFlags |= StringFormatFlags.DirectionRightToLeft;

            float y = e.MarginBounds.Top;
            float width = e.MarginBounds.Width;
            float left = e.MarginBounds.Left;

            void WriteLine(string text, Font font, bool center = false)
            {
                SizeF size = g.MeasureString(text, font, (int)width, format);
                float x = center ? left + Math.Max(0, (width - size.Width) / 2) : left;
                g.DrawString(text, font, Brushes.Black, new RectangleF(x, y, width, size.Height + 2), format);
                y += size.Height + 2;
            }

            string title = arabic ? "إيصال الحضانة" : "Kindergarten Receipt";
            string divider = new string('-', 32);

            WriteLine(title, titleFont, center: true);
            WriteLine(divider, textFont, center: true);

            WriteLine((arabic ? "الاسم: " : "Name: ") + student.Name, textFont);
            WriteLine((arabic ? "الفصل: " : "Class: ") + student.ClassName, textFont);
            WriteLine((arabic ? "العمر: " : "Age: ") + student.Age, textFont);

            WriteLine(divider, textFont, center: true);

            WriteLine((arabic ? "الرسم الشهري: " : "Monthly Fee: ") + student.MonthlyFee.ToString("0.00"), textFont);
            WriteLine((arabic ? "إجمالي المدفوع: " : "Total Paid: ") + student.TotalPaid.ToString("0.00"), textFont);
            WriteLine((arabic ? "المتبقي: " : "Remaining: ") + student.Remaining.ToString("0.00"), textFont);

            if (!string.IsNullOrWhiteSpace(student.Notes))
            {
                WriteLine(divider, textFont, center: true);
                WriteLine((arabic ? "ملاحظات: " : "Notes: ") + student.Notes, textFont);
            }

            WriteLine(divider, textFont, center: true);
            WriteLine((arabic ? "التاريخ: " : "Date: ") + DateTime.Now.ToString("yyyy-MM-dd HH:mm"), textFont);
        }
    }
}
