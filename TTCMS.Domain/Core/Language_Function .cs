using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TTCMS.Domain
{

    public partial class Language_Function 
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Text { get; set; }
        public string FunctionId { get; set; }
        public string LanguageId { get; set; }
        public virtual Function Function { get; set; }
        public virtual Language Language { get; set; }
    }
}
