using ClientLibrary.Services.Contracts;
using ClosedXML.Excel;
using ClosedXML.Graphics;
using Microsoft.JSInterop;

namespace ClientLibrary.Services.Implementations
{
    public class ExcelService(IJSRuntime jsRuntime, HttpClient httpClient) : IExcelService
    {
        private readonly IJSRuntime _jsRuntime = jsRuntime;
        private readonly HttpClient _httpClient = httpClient;

        /// <summary>
        /// Generic method to export data to an Excel file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task Export<T>(IEnumerable<T> data, string fileName)
        {
            using var workbook = new XLWorkbook();
            var propertyNames = typeof(T).GetProperties().Select(p => p.Name).ToList();

            var worksheet = workbook.Worksheets.Add("Data");

            // Adjust the width of all used columns in the worksheet to fit their contents
            worksheet.ColumnsUsed().AdjustToContents();

            // Write header row
            for (int i = 0; i < propertyNames.Count; i++)
            {
                worksheet.Cell(1, i + 1).Value = propertyNames[i];
            }

            // Write data rows
            var rowData = data.ToList();
            for (int rowIndex = 0; rowIndex < rowData.Count; rowIndex++)
            {
                for (int colIndex = 0; colIndex < propertyNames.Count; colIndex++)
                {
                    var propertyName = propertyNames[colIndex];
                    var propertyValue = typeof(T).GetProperty(propertyName)?.GetValue(rowData[rowIndex])?.ToString();
                    worksheet.Cell(rowIndex + 2, colIndex + 1).Value = propertyValue;
                }
            }

            // Create Table
            worksheet?.RangeUsed()?.CreateTable();

            // Save the workbook to a memory stream
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            await _jsRuntime.InvokeVoidAsync("saveAsFile", fileName, stream.ToArray());
        }

        /// <summary>
        /// Generic method to read data from an Excel file in a MemoryStream and map it to a list of type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ms"></param>
        /// <returns></returns>
        public async Task<List<T>> Read<T>(MemoryStream ms)
        {
            List<T> items = [];

            // You can use any font that exists in your project's wwwroot/fonts directory. If there no font file just download and add one.
            var fallbackFontStream = await _httpClient.GetStreamAsync("fonts/SourceSansPro-Regular.woff");

            var loadOptions = new LoadOptions {
                // Create a default graphic engine that uses only fallback font and additional fonts passed as streams. It also uses system fonts.
                GraphicEngine = DefaultGraphicEngine.CreateWithFontsAndSystemFonts(fallbackFontStream)
            };

            using (var workbook = new XLWorkbook(ms, loadOptions))
            {
                var worksheet = workbook.Worksheet(1);
                var table = worksheet.Table("Table1");

                var headers = table.HeadersRow().Cells().Select(c => c.Value.ToString()).ToList();

                foreach (var row in table.DataRange.RowsUsed())
                {
                    T item = Activator.CreateInstance<T>();

                    for (int i = 1; i <= headers.Count; i++)
                    {
                        var header = headers[i - 1].ToString();
                        var cellValue = row.Cell(i).Value.ToString();

                        if (!string.IsNullOrEmpty(cellValue))
                        {
                            var property = typeof(T).GetProperty(header);

                            if (property != null)
                            {
                                var targetType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

                                if (targetType == typeof(double?))
                                {
                                    if (double.TryParse(cellValue, out double parsedValue))
                                    {
                                        property.SetValue(item, parsedValue);
                                    }
                                    else
                                    {
                                        property.SetValue(item, null);
                                    }
                                }
                                else if (targetType == typeof(DateTime))
                                {
                                    if (DateTime.TryParse(cellValue, out DateTime parsedValue))
                                    {
                                        property.SetValue(item, parsedValue);
                                    }
                                    else
                                    {
                                        property.SetValue(item, null);
                                    }
                                }
                                else
                                {
                                    var value = Convert.ChangeType(cellValue, targetType);
                                    property.SetValue(item, value);
                                }
                            }
                        }
                    }

                    items.Add(item);
                }
            }

            return items;
        }
    }
}
