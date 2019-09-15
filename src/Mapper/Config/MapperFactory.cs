using System;
using AutoMapper;
using System.Collections.Generic;

namespace Mapper.Config
{
    public interface IMapperFactory
    {
        IMapper GetMapper(string mapperName = "");

        IMapper GetMapper<TProfile>() where TProfile : AutoMapper.Profile;
    }

    public class MapperFactory : IMapperFactory
    {
        public Dictionary<string, IMapper> Mappers { get; set; } = new Dictionary<string, IMapper>();
        public IMapper GetMapper(string mapperName)
        {
            return Mappers[mapperName];
        }
        public IMapper GetMapper<TProfile>() where TProfile : AutoMapper.Profile
        {
            var profile = (AutoMapper.Profile)Activator.CreateInstance(typeof(TProfile));
            var profileName = profile.ProfileName;

            return Mappers[profileName];
        }
    }
}
