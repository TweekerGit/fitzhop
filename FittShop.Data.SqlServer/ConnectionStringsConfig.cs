using System.Collections.Generic;

namespace FittShop.Data.SqlServer
{
    public class ConnectionStringsConfig
    {
        public string SelectedConnection { get; set; }
        public IReadOnlyDictionary<string, string> ConnectionStrings { get; set; }
    }
}