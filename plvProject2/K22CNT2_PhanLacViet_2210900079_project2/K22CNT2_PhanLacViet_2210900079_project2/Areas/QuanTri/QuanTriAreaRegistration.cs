using System.Web.Mvc;

namespace K22CNT2_PhanLacViet_2210900079_project2.Areas.QuanTri
{
    public class QuanTriAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "QuanTri";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "QuanTri_default",
                "QuanTri/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}