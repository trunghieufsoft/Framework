using System;
using System.Linq;
using Mapper.Config;
using Mapper.Profile;
using DataAccess.Entity;
using Asset.Common.Services;
using Asset.Common.ViewModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebClient.Controllers.Api.Base;
using BusinessAccess.Service.Interface;
using Asset.Common.Exceptions;
using Asset.Common.Enumerations;
using Serilog;
using Microsoft.AspNetCore.Authorization;

namespace WebClient.Controllers.Api
{
    [Authorize]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IMapperFactory _factory;

        public UserController(IUserService userService, IMapperFactory factory)
        {
            _userService = userService;
            _factory = factory;
        }

        // GET: api/User
        [HttpGet]
        [AllowAnonymous]
        [Route("Get-Users")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _userService.ToListAsync();
            return Json(users);
        }

        // GET: api/User/5
        [HttpGet]
        [AllowAnonymous]
        [Route("Get-By-Id")]
        public async Task<ActionResult<User>> GetUserEntity(Guid id)
        {
            var userEntity = await _userService.FindAsync(id);

            if (userEntity == null)
            {
                return NotFound();
            }

            return Json(userEntity);
        }

        // PUT: api/User/5
        [HttpPut]
        [Route("Modified")]
        public async Task<IActionResult> Modified(string idOrCode, ModifiedInfoUserModel user)
        {
            if (string.IsNullOrEmpty(idOrCode) || string.IsNullOrWhiteSpace(idOrCode))
                return BadRequest();

            try
            {
                var mapper = _factory.GetMapper<UserProfile>();
                var userEntity = mapper.Map<ModifiedInfoUserModel, User>(user);
                userEntity.Code = idOrCode;
                userEntity.Password = EncryptService.Encrypt(user.Password);

                var data = await _userService.UpdateAsync(userEntity);
                return Json(data);
            }
            catch (ExceptionError e)
            {
                if (e.Error.ErrorCode.Equals((int)ErrorCodeEnum.FailData))
                {
                    Log.Error(e.Error.Data);
                    throw;
                }
                else
                {
                    return NotFound();
                }
            }
        }

        // POST: api/User
        [HttpPost]
        [Route("Insert")]
        public async Task<ActionResult<User>> Insert(UserModel user)
        {
            var mapper = _factory.GetMapper<UserProfile>();
            var userEntity = mapper.Map<UserModel, User>(user);
            userEntity.Password = EncryptService.Encrypt("Password");

            var data = await _userService.AddAsync(userEntity);
            return Json(data);
        }

        // DELETE: api/User/5
        [HttpDelete]
        [Route("Delete")]
        public async Task<ActionResult<User>> Delete(Guid id)
        {
            var userEntity = await _userService.FindAsync(id);
            if (userEntity == null)
            {
                return NotFound();
            }

            await _userService.DeleteAsync(userEntity);

            return Json(userEntity);
        }
    }
}
