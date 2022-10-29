using System.Collections.Generic;

namespace hu_app.Shared
{
    public class HuMediatorException : HuException
    {
        public HuMediatorException(List<string> errors) : base(errors) { }
    }
}
