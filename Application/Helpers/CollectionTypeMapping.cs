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
            { "INT", typeof(int) },
            { "STR", typeof(string) },
            { "TEXT", typeof(string) },
            { "BOOL", typeof(bool) },
            { "DATE", typeof(DateTime) }
        };
    }
}
