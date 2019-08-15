using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace SyncOpenDateServices.TextyOrgUaWater
{
    public class TextyOrgUaWaterServiceParser : IStreamParser<TextyOrgUaWaterRow>
    {
        public async Task<ICollection<TextyOrgUaWaterRow>> ParseAsync(Stream stream)
        {
            var data = new List<TextyOrgUaWaterRow>();
            StreamReader reader = new StreamReader(stream);

            //skip line
            reader.ReadLine();

            while (!reader.EndOfStream)
            {
                var text = await reader.ReadLineAsync();
                var tokens = text.Split(',');
                var values = ParseTokens(tokens);

                var row = new TextyOrgUaWaterRow();
                row.Id = Int32.Parse(values[0]);
                row.Name = values[1];
                row.Code = values[2];
                row.River = values[3];
                row.Laboratory = values[4];
                row.Date = DateTime.Parse(values[5]);
                row.Key = values[6];
                row.Value = Double.Parse(values[7], CultureInfo.InvariantCulture);

                data.Add(row);
            }

            return data;
        }

        private string[] ParseTokens(string[] tokens)
        {
            bool quotesOpened = false;
            List<string> values = new List<string>();
            List<string> currentValue = new List<string>();

            for (var i = 0; i < tokens.Length; i++)
            {
                if (!quotesOpened)
                {
                    if (String.IsNullOrEmpty(tokens[i]) || tokens[i][0] != '"')
                    {
                        values.Add(tokens[i]);
                    }
                    else
                    {
                        tokens[i] = tokens[i].Substring(1);
                        quotesOpened = true;
                        currentValue.Clear();
                    }
                }

                if (quotesOpened)
                {
                    if (String.IsNullOrEmpty(tokens[i])) continue;
                    tokens[i] = tokens[i].Replace("\"\"", "~"); //map double quotes

                    currentValue.Add(tokens[i]);

                    if (tokens[i][tokens[i].Length - 1] == '"')
                    {
                        values.Add(String.Join(",", currentValue)
                            .Replace("\"", string.Empty)
                            .Replace("~", "\""));
                        quotesOpened = false;
                    }
                }
            }

            return values.ToArray();
        }
    }
}
