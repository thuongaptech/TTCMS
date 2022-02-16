using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TTCMS.Domain
{

    public partial class Language_GAction 
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string GActionId { get; set; }
        public string LanguageId { get; set; }
        public virtual GAction GAction { get; set; }
        public virtual Language Language { get; set; }
    }
}
