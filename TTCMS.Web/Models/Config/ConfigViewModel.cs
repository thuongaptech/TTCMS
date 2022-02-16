using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TTCMS.Web.Models
{
    public class ConfigViewModel 
    {
        public string Id { get; set; }
        public string TemplateEmail { get; set; }
        public string Title { get; set; }
        public string FileXML { get; set; }
        public string ApplicationURL { get; set; }
        public int pageSize { get; set; }
        public string Favicon { get; set; }
        public string Logo { get; set; }
        public string Seo_Image { get; set; }
        public string Copyright { set; get; }
        public string contactus_setting { get; set; }
        public string google_map { get; set; }
        public string Address { get; set; }
        public bool IsClosed { get; set; }
        public string EmailAccount { get; set; }
        public string EmailPassword { get; set; }
        public string EmailHost { get; set; }
        public string EmailPort { get; set; }
        public bool EmailEnableSSL { get; set; }
        public string RecieveEmail { get; set; }
        //
        public string ApplicationName { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string Page_Footer { get; set; }
        public string LinkFooter { get; set; }
        public string Strfooter { get; set; }
        public string HotLine { get; set; }
        public string Email { get; set; }
        public string LinkMaps { get; set; }
        public string CompanyName { get; set; }
        public string BackgroudColor { get; set; }
        public string ActiveColor { get; set; }
        public string TeamplateDatHang { get; set; }
        public string TeamplateAdminDatHang { get; set; }
        public string SubForgotPassword { get; set; }
        public string BodyForgotPassword { get; set; }

    }
    public class CauHinhViewModel
    {
        public string Id { get; set; }
        public string TemplateEmail { get; set; }
        public string Title { get; set; }
        public string FileXML { get; set; }
        public string ApplicationURL { get; set; }
        public int pageSize { get; set; }
        public string Favicon { get; set; }
        public string Logo { get; set; }
        public string Seo_Image { get; set; }
        public string Copyright { set; get; }
        public string contactus_setting { get; set; }
        public string google_map { get; set; }
        public string Address { get; set; }
        public bool IsClosed { get; set; }
        public string EmailAccount { get; set; }
        public string EmailPassword { get; set; }
        public string EmailHost { get; set; }
        public string EmailPort { get; set; }
        public bool EmailEnableSSL { get; set; }
        public string RecieveEmail { get; set; }
        //
        public string ApplicationName { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string Page_Footer { get; set; }
        public string LinkFooter { get; set; }
        public string Strfooter { get; set; }
        public string HotLine { get; set; }
        public string Email { get; set; }
        public string LinkMaps { get; set; }
        public string CompanyName { get; set; }
        public string BackgroudColor { get; set; }
        public string ActiveColor { get; set; }
        public string TeamplateDatHang { get; set; }
        public string TeamplateAdminDatHang { get; set; }
        public string SubForgotPassword { get; set; }
        public string BodyForgotPassword { get; set; }
    }
}