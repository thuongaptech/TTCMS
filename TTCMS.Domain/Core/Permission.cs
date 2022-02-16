using System;
using System.ComponentModel.DataAnnotations;
namespace TTCMS.Domain
{
    public class Permission
    {
        public int Id { get; set; }
        public string GActionId { get; set; }
        public string RoleId { get; set; }
        public string FunctionId { get; set; }
        public virtual GAction GAction { set; get; }
        public virtual ApplicationRole Role { set; get; }
        public virtual Function Function { set; get; }
    }
}
