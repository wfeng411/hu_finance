using System;
using System.Collections.Generic;

namespace hu_app.Shared
{
    public class HuException : Exception
    {
        public HuException(string error)
        {
            Error = error;
        }

        public HuException(List<string> errors)
        {
            Errors = errors;
        }

        public string Error { get; set; }
        public List<string> Errors { get; set; }
    }
}
