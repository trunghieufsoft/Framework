using System;
using AutoMapper;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Mapper.Config
{
    public class MapperConfig
    {
        public static MapperConfig Register { get; set; } = new MapperConfig();

        public MapperFactory AutoMapperConfig(IServiceCollection services)
        {
            var mapperFactory = new MapperFactory();
            IMapper defaultMapper = null;
            var assembly = this.GetType().GetTypeInfo().Assembly;
            var types = assembly.GetTypes()
                .Where(x => x.GetTypeInfo().IsClass && typeof(AutoMapper.Profile).IsAssignableFrom(x) && x.GetTypeInfo().BaseType == typeof(AutoMapper.Profile)).ToList();
            foreach (var type in types)
            {
                var profileName = string.Empty;
                var config = new MapperConfiguration(cfg =>
                {
                    var profile = (AutoMapper.Profile)Activator.CreateInstance(type);
                    profileName = profile.ProfileName;
                    cfg.AddProfile(profile);
                });

                var mapper = config.CreateMapper();
                mapperFactory.Mappers.Add(profileName, mapper);

                // If we still want normal functionality with a default injected IMapper
                if (defaultMapper == null)
                {
                    defaultMapper = mapper;
                    services.AddSingleton(defaultMapper);
                }
            }
            return mapperFactory;
        }
    }
}
