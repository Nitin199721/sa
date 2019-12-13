using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace XTabAPI.Controllers
{
    public class TestController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetDetails()
        {
            Debug.Write($"AuthenticationType: {User.Identity.AuthenticationType}");
            Debug.Write($"IsAuthenticated: {User.Identity.IsAuthenticated}");
            Debug.Write($"Name: {User.Identity.Name}");



            return Ok("Authenticated: " + User.Identity.Name + "- This is Test Response");
            // return Ok("Authenticated: yourdomain1\\yourUserName1 - This is Test Response");
        }
    }

}
