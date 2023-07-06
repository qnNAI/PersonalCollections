using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{
    public class CollectionTypes
    {
        public string[] Types { get; } = new[] {
            "INTEGER",
            "STRING",
            "TEXT",
            "BOOLEAN",
            "DATETIME"
        };

        public string IntegerNumber => Types[0];
        public string String => Types[1];
        public string Text => Types[2];
        public string Boolean => Types[3];
        public string Date => Types[4];
    }
}
