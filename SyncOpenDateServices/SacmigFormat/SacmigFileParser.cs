using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SyncOpenDateServices.SacmigFormat
{
    public class SacmigFileParser : IStreamParser<SacmigFileRow>
    {
        public async Task<ICollection<SacmigFileRow>> ParseAsync(Stream stream)
        {
            ExcelPackage excelDocument = new ExcelPackage(stream);
            ExcelWorksheet workSheet = excelDocument.Workbook.Worksheets.First();
            var startRow = workSheet.Dimension.Start.Row;
            var startColumn = workSheet.Dimension.Start.Column;
            var endRow = workSheet.Dimension.End.Row;
            var endColumn = workSheet.Dimension.End.Column;

            //first column alays is date!
            var keys = ParseHeader(workSheet, startRow, startColumn, endColumn);
            var data = new List<SacmigFileRow>();
            for (int rowId = startRow + 1; rowId <= endRow; rowId++)
            {
                try
                {
                    var row = ParseRow(workSheet, rowId, startColumn, keys);
                    data.Add(row);
                }
                catch
                {
                    continue;
                }
            }

            return data;
        }

        private List<string> ParseHeader(ExcelWorksheet worksheet, int row, int startColumn, int endColumn)
        {
            var keys = new List<string>();
            for (int i = startColumn + 1; i <= endColumn; i++)
            {
                keys.Add(worksheet.Cells[row, i].Value.ToString());
            }
            return keys;
        }

        private SacmigFileRow ParseRow(ExcelWorksheet worksheet, int row, int startColumn, List<string> keys)
        {
            var entry = new SacmigFileRow();

            var dateValue = worksheet.Cells[row, startColumn].Value;
            if (dateValue is double)
            {
                ParseDate(ref entry, (double)dateValue);
            }
            else
            {
                ParseDate(ref entry, dateValue.ToString());
            }

            for (int keyIndex = 0; keyIndex < keys.Count; keyIndex++)
            {
                var value = worksheet.Cells[row, startColumn + keyIndex + 1].Value;
                if (value != null)
                {
                    try
                    {
                        var doubleValue = value is double ? (double)value : Double.Parse(value.ToString(), CultureInfo.InvariantCulture);
                        entry.Characteristics.Add(keys[keyIndex], doubleValue);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            return entry;
        }

        private void ParseDate(ref SacmigFileRow entry, string value)
        {
            var tokens = value.Split('-');
            if (tokens.Length > 1)
            {
                entry.StartDate = DateTime.Parse(tokens[0].Trim());
                entry.EndDate = DateTime.Parse(tokens[1].Trim());
            }
            else
            {
                entry.StartDate = DateTime.Parse(value.Trim());
            }
        }

        private void ParseDate(ref SacmigFileRow entry, double value)
        {
            entry.StartDate = DateTime.FromOADate(value);
        }
    }
}
