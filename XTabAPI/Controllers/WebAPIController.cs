using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace XTabAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
    public partial class WebAPIController : ApiController
    {

        /// <summary>  
        /// This method contains Authorize attribute for authentication and authroization  
        /// </summary>  
        /// <returns></returns>  
        [HttpGet]
        [Route("api/AuthenticateUser")]

        public HttpResponseMessage AuthenticateUser()
        {
            if (User != null)
            {
               // var username1 = WindowsIdentity.GetCurrent();
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    status = (int)HttpStatusCode.OK,
                    isAuthenticated = true,
                   // isLibraryAdmin = User.IsInRole(@"domain\AdminGroup"),
                   username = User.Identity.Name.Substring(User.Identity.Name.LastIndexOf(@"\") + 1)
                   // username = HttpContext.Current.User.Identity.Name
                });
            }
            else
            {
                //This code never execute as we have used Authorize attribute on action method  
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    status = (int)HttpStatusCode.BadRequest,
                    isAuthenticated = false,
                    isLibraryAdmin = false,
                    username = ""
                });

            }
        }
    }
}
