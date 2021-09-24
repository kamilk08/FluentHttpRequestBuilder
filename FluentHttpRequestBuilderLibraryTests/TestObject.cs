using System;
using System.Collections.Generic;

namespace FluentHttpRequestBuilderLibraryTests
{
    public class TestObject
    {
        public Guid Guid { get; set; }
        
        public int Value { get; set; }
        
        public List<char> Chars { get; set; }

        public TestObject(Guid guid, int value, List<char> chars)
        {
            Guid = guid;
            Value = value;
            Chars = chars;
        }
    }
}