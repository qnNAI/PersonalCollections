using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{
    public class CollectionTypeMapping
    {
        public Dictionary<string, Type> TypeMappings => new()
        {
            { "INTEGER", typeof(int) },
            { "STRING", typeof(string) },
            { "TEXT", typeof(string) },
            { "BOOLEAN", typeof(bool) },
            { "DATETIME", typeof(DateTime) }
        };

    }
}
