using System;
using System.Linq;
using DataAccess.Entity;
using Asset.Common.Services;
using System.Threading.Tasks;
using DataAccess.Entity.EnumType;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace DataAccess.DbContext
{
    public class DbInitializer
    {
        private readonly DataDbContext _context;
        private readonly IConfiguration _configuration;

        public DbInitializer(DataDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task Seed()
        {
            var change = false;
            if (!_context.Users.Any(x => x.UserType == UserTypeEnum.SuperAdmin))
            {
                List<User> listUser = new List<User>()
                {
                    new User(){Code = string.Join("", Guid.NewGuid().ToString().Split("-")),UserType=UserTypeEnum.SuperAdmin,Username=_configuration["SuperAdmin:Username"],FullName=_configuration["SuperAdmin:Fullname"],Phone=_configuration["SuperAdmin:Default"],Email=_configuration["SuperAdmin:Email"],Address=_configuration["SuperAdmin:Default"],Status=StatusEnum.Active,Password=EncryptService.Encrypt(_configuration["SuperAdmin:Password"]),CreatedBy=_configuration["Auto:Create"]},
                };

                var entities = _context.Set<User>();
                entities.AddRange(listUser);
                change = true;
            }

            if (change)
            {
                await _context.SaveChangesAsync();
            }
            return;
        }
    }
}
