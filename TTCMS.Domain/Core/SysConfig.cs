
using System;
using System.ComponentModel.DataAnnotations;
namespace TTCMS.Domain
{
    public  class SysConfig
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UniqueKey { get; set; }
        public string Value { get; set; }
        public string Unit { get; set; }
        public string Description { get; set; }
        public System.DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public SysConfig()
        {
            CreatedDate = DateTime.Now;
        }
    }
}
