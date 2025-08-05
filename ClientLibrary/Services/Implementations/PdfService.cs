using ClientLibrary.Services.Contracts;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.JSInterop;
using System.Text.RegularExpressions;

namespace ClientLibrary.Services.Implementations
{
    public class PdfService(IJSRuntime jsRuntime) : IPdfService
    {
        private readonly IJSRuntime _jsRuntime = jsRuntime;

        public async Task Export<T>(IEnumerable<T> data, string fileName, bool landscape = true)
        {
            var pageSize = landscape ? PageSize.A4.Rotate() : PageSize.A4;

            Document doc = new(pageSize, 10, 10, 10, 10);

            var stream = new MemoryStream();

            PdfWriter.GetInstance(doc, stream);

            doc.Open();

            Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);

            Paragraph title = new(fileName + "\n", titleFont)
            {
                Alignment = Element.ALIGN_CENTER
            };

            doc.Add(title);

            doc.Add(new Paragraph("\n"));

            var propertyNames = typeof(T).GetProperties().Select(p => p.Name).ToList();

            PdfPTable table = new(propertyNames.Count)
            {
                WidthPercentage = 100
            };

            foreach (var propertyName in propertyNames)
            {
                PdfPCell headerCell = new(new Phrase(propertyName.SplitCamelCase(), FontFactory.GetFont(FontFactory.HELVETICA, 11f)))
                {
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    ExtraParagraphSpace = 2
                };
                table.AddCell(headerCell);
            }

            foreach (var item in data)
            {
                foreach (var propertyName in propertyNames)
                {
                    var property = typeof(T).GetProperty(propertyName);
                    if (property != null)
                    {
                        var value = property.GetValue(item);
                        PdfPCell dataCell = new(new Phrase(value == null ? "" : value.ToString(), new Font(Font.HELVETICA, 11f)))
                        {
                            VerticalAlignment = Element.ALIGN_MIDDLE,
                            ExtraParagraphSpace = 2
                        };
                        table.AddCell(dataCell);
                    }
                }
            }

            doc.Add(table);

            doc.Close();

            await _jsRuntime.InvokeVoidAsync("saveAsFile", $"{fileName}.pdf", stream.ToArray());
        }
    }

    // Agrega este método de extensión en el mismo archivo o en un archivo de utilidades
    public static class StringExtensions
    {
        public static string SplitCamelCase(this string str)
        {
            if (string.IsNullOrEmpty(str)) return str;
            return Regex.Replace(
                str,
                "(\\B[A-Z])",
                " $1"
            );
        }
    }
}
