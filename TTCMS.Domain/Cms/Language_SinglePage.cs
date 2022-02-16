using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TTCMS.Domain
{

    public partial class Language_SinglePage
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Body { get; set; }
        public string Route { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string Tag { get; set; }
        public int SinglePageId { get; set; }
        public string LanguageId { get; set; }
        public virtual SinglePage SinglePage { get; set; }
        public virtual Language Language { get; set; }
    }
}
