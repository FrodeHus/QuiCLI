using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuiCLI.Help
{
    internal class HelpSection(string title)
    {
        internal HelpTitle Title { get; init; } = title;
        internal List<IHelpItem> Items { get; init; } = new List<IHelpItem>();

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.AppendLine(Title);
            foreach (var item in Items)
            {
                sb.AppendLine($"\t{item}");
            }
            return sb.ToString();
        }
    }
}
