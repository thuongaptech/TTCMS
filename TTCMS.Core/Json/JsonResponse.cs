using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TTCMS.Core.Json
{
    [Serializable]
    public class JsonResponse
    {
        public string Code { get; set; }
        public string Msg { get; set; }
        public string RedirectUrl { get; set; }
        public string PartialViewHtml { get; set; }
        public bool Success { get; set; }
    }
   
}
