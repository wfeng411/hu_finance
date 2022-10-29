using System.Collections.Generic;

namespace hu_app.Shared
{
    public class HuResult
    {
        public static HuResponse Ok(object data = null)
        {
            var response = new HuResponse
            {
                Ok = true,
                Data = data
            };
            return response;
        }

        public static HuResponse Error(string error)
        {
            var response = new HuResponse
            {
                Ok = false,
                Errors = new List<string> { error }
            };
            return response;
        }

        public static HuResponse Errors(List<string> errors)
        {
            var response = new HuResponse
            {
                Ok = false,
                Errors = errors
            };
            return response;
        }
    }
}
