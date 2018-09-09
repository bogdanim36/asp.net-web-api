using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace WebApi.Api.Account
{
	public class AccountController : ApiController
	{
		[HttpPost]
		[AllowAnonymous]
		[Route("account/getUserInfo")]
		public IHttpActionResult GetUserInfo()
		{
			return Ok("user");
		}
	}
}
