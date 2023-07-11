using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Application.Helpers
{
    public class CollectionTypes
    {
        private readonly string[] _types = new[]
        {
            INTEGER_NUMBER,
            STRING,
            TEXT,
            BOOL,
            DATE
        };

        public const string INTEGER_NUMBER = "INTEGER";
        public const string STRING = "STRING";
        public const string TEXT = "TEXT";
        public const string BOOL = "BOOLEAN";
        public const string DATE = "DATE";

        public string[] Types => _types;
    }
}
