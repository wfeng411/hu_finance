using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace hu_app.Shared
{
    public class HuResponse
    {
        public bool Ok { get; set; }
        public object Data { get; set; }
        public List<string> Errors { get; set; }
        public string Position { get; set; }

        public static OkObjectResult Send(HuResponse response)
        {
            return new OkObjectResult(response);
        }
    }
}
