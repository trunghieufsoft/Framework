using System;
using Asset.Common.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace WebClient.Controllers.Api.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : Controller
    {
        protected readonly bool success = true;
        public override JsonResult Json(object data)
        {
            return new JsonResult(new ResponseModel
            {
                Data = data,
                Success = true
            });
        }

        protected JwtSecurityToken GetTokenFromRequestHeader()
        {
            var handler = new JwtSecurityTokenHandler();
            string authorization = Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authorization))
            {
                return null;
            }
            string token = authorization.Replace("Bearer ", string.Empty);

            return handler.ReadToken(token) as JwtSecurityToken;
        }

        protected Guid? GetUserIdFromHeader()
        {
            JwtSecurityToken token = GetTokenFromRequestHeader();
            if (token == null)
            {
                return null;
            }
            string idStr = token.Payload["UserId"].ToString();
            if (string.IsNullOrEmpty(idStr))
            {
                return Guid.Empty;
            }
            else
            {
                return Guid.Parse(idStr);
            }
        }

        protected string GetCurrentUser()
        {
            JwtSecurityToken token = GetTokenFromRequestHeader();
            if (token == null)
            {
                return string.Empty;
            }
            return token.Payload["Username"].ToString();
        }

        protected string GetToken()
        {
            JwtSecurityToken token = GetTokenFromRequestHeader();
            if (token == null)
            {
                return string.Empty;
            }
            return token.Payload["jti"].ToString();
        }
    }
}