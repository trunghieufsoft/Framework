using DataAccess.Entity;
using Asset.Common.ViewModel;

namespace Mapper.Profile
{
    public class UserProfile : AutoMapper.Profile
    {
        public UserProfile()
        {
            // map UserEntity -> UserProfileModel
            CreateMap<User, UserProfileModel>();
            // UserProfileModel -> map UserEntity
            CreateMap<UserProfileModel, User>();

            // map UserEntity -> UserModel
            CreateMap<User, UserModel>();
            // UserModel -> map UserEntity
            CreateMap<UserModel, User>();

            // map UserEntity -> ModifiedInfoUserModel
            CreateMap<User, ModifiedInfoUserModel>();
            // ModifiedInfoUserModel -> map UserEntity
            CreateMap<ModifiedInfoUserModel, User>()
                .ForMember(src => src.CreatedBy, option => option.Ignore())
                .ForMember(src => src.CreationTime, option => option.Ignore())
                .ForMember(src => src.Username, option => option.Ignore());
        }
    }
}
