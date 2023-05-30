using Ubee.Domain.Configurations;
using Ubee.Service.DTOs.Users;

namespace Ubee.Service.Interfaces;

public interface IUserService 
{
    Task<UserForResultDto> AddAsync(UserForCreationDto dto);
    Task<IEnumerable<UserForResultDto>> RetrieveAllAsync(PaginationParams @params);
    Task<UserForResultDto> RetrieveByIdAsync(long id);
    Task<bool> RemoveAsync(long id);
    Task<UserForResultDto> ModifyAsync(long id, UserForCreationDto dto);
    Task<UserForResultDto> ChangePasswordAsync(UserForChangePasswordDto dto);

}
