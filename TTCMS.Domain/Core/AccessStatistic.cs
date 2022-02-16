
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TTCMS.Domain
{
    public  class AccessStatistic
    {
        [Key]
        public int DayId { set; get; }
         [Key]
        public int MonthId { set; get; }
         [Key]
        public int YearId { set; get; }
        public int Count { set; get; }
        public string IP { set; get; }
    }
}
