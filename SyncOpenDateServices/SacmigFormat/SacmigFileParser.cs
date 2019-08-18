using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace SyncOpenDateServices.SacmigFormat
{
    public class SacmigFileParser
    {
        public SacmigFileData Parse(Stream stream)
        {
            ExcelPackage excelDocument = new ExcelPackage(stream);
            ExcelWorksheet workSheet = excelDocument.Workbook.Worksheets.First();

            var startRow = workSheet.Dimension.Start.Row;
            var startColumn = workSheet.Dimension.Start.Column;
            var endRow = workSheet.Dimension.End.Row;
            var endColumn = workSheet.Dimension.End.Column;

            var keys = ParseHeader(workSheet, startRow, startColumn, endColumn);
            var data = new SacmigFileData();
            for (int rowId = startRow + 1; rowId <= endRow; rowId++)
            {
                try
                {
                    ParseRow(workSheet, rowId, startColumn, keys, data);
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

        private void ParseRow(ExcelWorksheet worksheet, int row, int startColumn, List<string> keys, SacmigFileData data)
        {
            //first column alays is date!
            var dateValue = worksheet.Cells[row, startColumn].Value;
            var dateTuple = dateValue is double ?
                ParseDateCell((double)dateValue) : ParseDateCell(dateValue.ToString());

            for (int keyIndex = 0; keyIndex < keys.Count; keyIndex++)
            {
                var value = worksheet.Cells[row, startColumn + keyIndex + 1].Value;
                if (value != null)
                {
                    try
                    {
                        var doubleValue = value is double ?
                            (double)value : Double.Parse(value.ToString(), CultureInfo.InvariantCulture);
                        data.AddCharacteristicValue(keys[keyIndex], doubleValue, dateTuple.Item1, dateTuple.Item2);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
        }

        private Tuple<DateTime, DateTime?> ParseDateCell(string value)
        {
            var tokens = value.Split('-');
            if (tokens.Length > 1)
            {
                var startDate = DateTime.Parse(tokens[0].Trim());
                var endDate = DateTime.Parse(tokens[1].Trim());
                return new Tuple<DateTime, DateTime?>(startDate, endDate);
            }
            else
            {
                var startDate = DateTime.Parse(value.Trim());
                return new Tuple<DateTime, DateTime?>(startDate, null);
            }
        }

        private Tuple<DateTime, DateTime?> ParseDateCell(double value)
        {
            var startDate = DateTime.FromOADate(value);
            return new Tuple<DateTime, DateTime?>(startDate, null);
        }
    }
}
