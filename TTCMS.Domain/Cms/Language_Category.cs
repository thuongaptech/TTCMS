using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TTCMS.Domain
{

    public partial class Language_Category 
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string Route { get; set; }
        public string LanguageId { get; set; }
        public virtual Language Language { get; set; }
    }
}
