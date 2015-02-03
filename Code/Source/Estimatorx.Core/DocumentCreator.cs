using System;
using System.IO;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Font = iTextSharp.text.Font;

namespace Estimatorx.Core
{
    public class DocumentCreator
    {
        public byte[] CreatePdf(Project project)
        {
            byte[] buffer;

            using (var memoryStream = new MemoryStream())
            {
                CreatePdf(project, memoryStream);
                
                buffer = memoryStream.ToArray();
            }

            return buffer;
        }

        public void CreatePdf(Project project, string fileName)
        {
            using (var fileStream = File.Create(fileName))
                CreatePdf(project, fileStream);
        }

        public void CreatePdf(Project project, Stream writeStream)
        {
            var document = new Document();

            // landscape
            document.SetPageSize(PageSize.A4.Rotate());
            document.AddTitle(project.Name);
            document.SetMargins(36f, 36f, 36f, 36f);

            var writer = PdfWriter.GetInstance(document, writeStream);

            document.Open();

            AddName(project, document);
            AddDescription(project, document);
            AddAssumptions(project, document);
            AddFactors(project, document);
            AddTasks(project, document);
            AddSummary(project, document);

            writer.Flush();
            document.Close();
        }


        private void AddName(Project project, Document document)
        {
            var paragraph = new Paragraph(project.Name, HeaderFont());
            paragraph.SpacingAfter = 6f;

            document.Add(paragraph);
        }

        private void AddDescription(Project project, Document document)
        {
            if (string.IsNullOrWhiteSpace(project.Description))
                return;

            var element = new Paragraph(project.Description);
            document.Add(element);
        }

        private void AddAssumptions(Project project, Document document)
        {
            if (project.Assumptions == null || project.Assumptions.Count < 1)
                return;

            var element = new Paragraph("Assumptions", TitleFont());
            element.SpacingBefore = 5f;
            document.Add(element);

            var list = new List(false, false, 10);
            list.SetListSymbol("\u2022");
            list.IndentationLeft = 10f;

            foreach (var assumption in project.Assumptions)
            {
                var item = new ListItem(assumption);
                list.Add(item);
            }

            document.Add(list);
        }

        private void AddFactors(Project project, Document document)
        {
            var element = new Paragraph("Factors", TitleFont());
            element.SpacingBefore = 5f;
            document.Add(element);

            var table = new PdfPTable(6);
            table.SetWidths(new[] { 45, 11, 11, 11, 11, 11 });
            table.WidthPercentage = 100;
            table.SpacingBefore = 10f;

            table.AddCell(HeaderCell("Name"));
            table.AddCell(HeaderCell("Very Simple"));
            table.AddCell(HeaderCell("Simple"));
            table.AddCell(HeaderCell("Medium"));
            table.AddCell(HeaderCell("Complex"));
            table.AddCell(HeaderCell("Very Complex"));

            foreach (var factor in project.Factors)
            {
                table.AddCell(RowCell(factor.Name));
                table.AddCell(RowCell(factor.VerySimple.ToString()));
                table.AddCell(RowCell(factor.Simple.ToString()));
                table.AddCell(RowCell(factor.Medium.ToString()));
                table.AddCell(RowCell(factor.Complex.ToString()));
                table.AddCell(RowCell(factor.VeryComplex.ToString()));
            }

            document.Add(table);
        }

        private void AddTasks(Project project, Document document)
        {
            var element = new Paragraph("Tasks", TitleFont());
            element.SpacingBefore = 5f;
            document.Add(element);

            var table = new PdfPTable(9);

            table.SetWidths(new[] { 31, 20, 7, 7, 7, 7, 7, 7, 7 });
            table.WidthPercentage = 100;
            table.SpacingBefore = 10f;

            table.AddCell(HeaderCell("Name"));
            table.AddCell(HeaderCell("Factor"));
            table.AddCell(HeaderCell("Very Simple"));
            table.AddCell(HeaderCell("Simple"));
            table.AddCell(HeaderCell("Medium"));
            table.AddCell(HeaderCell("Complex"));
            table.AddCell(HeaderCell("Very Complex"));
            table.AddCell(HeaderCell("Total Tasks"));
            table.AddCell(HeaderCell("Total Hours"));

            foreach (var section in project.Sections)
            {
                table.AddCell(SectionCell(section.Name, 7));
                table.AddCell(SectionCell(section.TotalTasks.ToString()));
                table.AddCell(SectionCell(section.TotalHours.ToString()));

                foreach (var task in section.Tasks)
                {
                    var factorName = project.Factors
                        .Where(f => f.Id == task.FactorId)
                        .Select(f => f.Name)
                        .FirstOrDefault() ?? string.Empty;

                    table.AddCell(RowCell(task.Name));
                    table.AddCell(RowCell(factorName));
                    table.AddCell(RowCell(task.VerySimple.ToString()));
                    table.AddCell(RowCell(task.Simple.ToString()));
                    table.AddCell(RowCell(task.Medium.ToString()));
                    table.AddCell(RowCell(task.Complex.ToString()));
                    table.AddCell(RowCell(task.VeryComplex.ToString()));
                    table.AddCell(TotalCell(task.TotalTasks.ToString()));
                    table.AddCell(TotalCell(task.TotalHours.ToString()));
                }
            }

            document.Add(table);
        }

        private void AddSummary(Project project, Document document)
        {
            var element = new Paragraph("Summary", TitleFont());
            element.SpacingBefore = 5f;
            document.Add(element);

            var table = new PdfPTable(5);
            table.SetWidths(new[] { 15, 30, 10, 15, 30 });
            table.WidthPercentage = 100;
            table.SpacingBefore = 10f;
            table.SpacingAfter = 10f;

            table.AddCell(PropertyCell("Total Tasks:"));
            table.AddCell(RowCell(project.TotalTasks.ToString()));
            table.AddCell(BlankCell());
            table.AddCell(PropertyCell("Created:"));
            table.AddCell(RowCell(project.Created.ToString()));

            table.AddCell(PropertyCell("Total Hours:"));
            table.AddCell(RowCell(project.TotalHours.ToString()));
            table.AddCell(BlankCell());
            table.AddCell(PropertyCell("Creator:"));
            table.AddCell(RowCell(project.Creator));

            table.AddCell(PropertyCell("Contingency Hours:"));
            table.AddCell(RowCell(project.ContingencyHours.ToString()));
            table.AddCell(BlankCell());
            table.AddCell(PropertyCell("Updated:"));
            table.AddCell(RowCell(project.Updated.ToString()));

            table.AddCell(PropertyCell("Total Weeks:"));
            table.AddCell(RowCell(project.TotalWeeks.ToString()));
            table.AddCell(BlankCell());
            table.AddCell(PropertyCell("Updater:"));
            table.AddCell(RowCell(project.Updater));

            table.AddCell(PropertyCell("Contingency Weeks:"));
            table.AddCell(RowCell(project.ContingencyWeeks.ToString()));
            table.AddCell(BlankCell(3));

            table.AddCell(PropertyCell("Hours Per Week:"));
            table.AddCell(RowCell(project.HoursPerWeek.ToString()));
            table.AddCell(BlankCell(3));

            table.AddCell(PropertyCell("Contingency Rate:"));
            table.AddCell(RowCell(project.ContingencyRate.ToString()));
            table.AddCell(BlankCell(3));
            
            document.Add(table);

        }


        private static PdfPCell SectionCell(string text, int colspan = 1)
        {
            var phrase = new Phrase(text, StandardFont(10));
            var cell = new PdfPCell(phrase);
            cell.Colspan = colspan;
            cell.BackgroundColor = new BaseColor(252, 248, 227);

            SetDefaults(cell);

            return cell;
        }

        private static PdfPCell HeaderCell(string text)
        {
            var phrase = new Phrase(text, TitleFont(10));

            var cell = new PdfPCell(phrase);
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_BOTTOM;
            cell.BackgroundColor = new BaseColor(217, 237, 247);

            SetDefaults(cell);

            return cell;
        }

        private static PdfPCell RowCell(string text)
        {
            var phrase = new Phrase(text, StandardFont(10));
            var cell = new PdfPCell(phrase);

            SetDefaults(cell);

            return cell;
        }

        private static PdfPCell TotalCell(string text)
        {
            var phrase = new Phrase(text, StandardFont(10));
            var cell = new PdfPCell(phrase);
            cell.BackgroundColor = new BaseColor(245, 245, 245);

            SetDefaults(cell);

            return cell;
        }

        private static PdfPCell PropertyCell(string text)
        {
            var phrase = new Phrase(text, TitleFont(10));

            var cell = new PdfPCell(phrase);
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.BackgroundColor = new BaseColor(245, 245, 245);

            SetDefaults(cell);

            return cell;
        }

        private static PdfPCell BlankCell(int colspan = 1)
        {

            var cell = new PdfPCell();
            cell.Colspan = colspan;
            cell.BorderWidth = 0;
            
            return cell;
        }

        private static void SetDefaults(PdfPCell cell)
        {
            cell.BorderColor = new BaseColor(221, 221, 221);
            cell.PaddingTop = 5;
            cell.PaddingLeft = 6;
            cell.PaddingBottom = 5;
            cell.PaddingLeft = 6;
        }
        

        private static Font TitleFont(float size = 14f)
        {
            return FontFactory.GetFont("Arial", size, Font.BOLD);
        }

        private static Font HeaderFont(float size = 24f)
        {
            return FontFactory.GetFont("Arial", size, Font.BOLD);
        }

        private static Font StandardFont(float size = 12f)
        {
            return FontFactory.GetFont("Arial", size, Font.NORMAL);
        }

    }
}
