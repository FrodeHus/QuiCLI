using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuiCLI.Output
{
    internal class TableFormatter : IOutputFormatter
    {
        public string Format(object? value)
        {
            if (value is not IEnumerable<object> enumerable)
            {
                throw new ArgumentException("Value must be an enumerable");
            }

            var headers = enumerable.First().GetType().GetProperties().Select(p => p.Name).ToArray();
            var rows = enumerable.Select(e => e.GetType().GetProperties().Select(p => p.GetValue(e)).ToArray()).ToArray();

            var columnWidths = new int[headers.Length];
            for (int i = 0; i < headers.Length; i++)
            {
                columnWidths[i] = Math.Max(headers[i].Length, rows.Max(r => r[i]?.ToString()?.Length ?? 0));
            }

            var sb = new StringBuilder();
            sb.AppendJoin(" | ", headers.Select((h, i) => h.PadRight(columnWidths[i]))).AppendLine();
            sb.AppendJoin(" | ", columnWidths.Select(w => new string('-', w))).AppendLine();
            foreach (var row in rows)
            {
                sb.AppendJoin(" | ", row.Select((r, i) => r?.ToString()?.PadRight(columnWidths[i]))).AppendLine();
            }

            return sb.ToString();
        }
    }
}
