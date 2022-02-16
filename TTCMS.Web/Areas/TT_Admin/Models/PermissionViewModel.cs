using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TTCMS.Domain;

namespace TTCMS.Web.Areas.TT_Admin.Models
{
    public class PermissionViewModel
    {
        public FunctionViewModel Functions { set; get; }
        public List<ActionLangViewModel> GAction { set; get; }
    }
    public class SectionPermissionViewModel
    {
        public string FunctionId { set; get; }
        public string Role_Id { set; get; }
        public string UserGActionId { set; get; }
    }
}