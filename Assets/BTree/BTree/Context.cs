using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTree
{
    public class Context
    {



    }

    public class ContextMissingException : Exception
    {
        public ContextMissingException() { }
        public ContextMissingException(string message) : base(message) { }
        public ContextMissingException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class ContextNullException : Exception
    {
        public ContextNullException() { }
        public ContextNullException(string message) : base(message) { }
        public ContextNullException(string message, Exception innerException) : base(message, innerException) { }
    }
}
