using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TTCMS.Domain
{

    public partial class Language_Setting 
    {
        public int Id { get; set; }
        public string ApplicationName { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string Page_Footer { get; set; }
        public string SettingId { get; set; }
        public string LanguageId { get; set; }
        public virtual Setting Setting { get; set; }
        public virtual Language Language { get; set; }
    }
}
