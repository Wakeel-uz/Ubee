using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Ubee.Data.IRepositories;
using Ubee.Domain.Configurations;
using Ubee.Domain.Entities;
using Ubee.Service.DTOs.Users;
using Ubee.Service.Exceptions;
using Ubee.Service.Extensions;
using Ubee.Service.Helpers;
using Ubee.Service.Interfaces;

namespace Ubee.Service.Services;

public class UserService : IUserService
{
    private readonly IRepository<User> userRepository;
    private readonly IMapper mapper;

    public UserService(IRepository<User> userRepository, IMapper mapper)
    {
        this.userRepository = userRepository;
        this.mapper = mapper;
    }

    public async Task<UserForResultDto> AddAsync(UserForCreationDto dto)
    {
        var existUser = await this.userRepository.SelectAsync(u => u.Email == dto.Email);
        if (existUser is not null && !existUser.IsDeleted)
            throw new UbeeException(409, "User already exist");

        var mapped = this.mapper.Map<User>(dto);
        mapped.CreatedAt = DateTime.UtcNow;

        var addedUser = await this.userRepository.InsertAsync(mapped);
        await this.userRepository.SaveAsync();

        return this.mapper.Map<UserForResultDto>(addedUser);
    }

    public async Task<UserForResultDto> ChangePasswordAsync(UserForChangePasswordDto dto)
    {
        var user = await this.userRepository.SelectAsync(u => u.Email == dto.Email);
        if (user is null || user.IsDeleted)
            throw new UbeeException(404, "User not found");

        if (!dto.OldPassword.Equals(user.Password))
            throw new UbeeException(400, "Password is incorrect");

        if (dto.NewPassword != dto.ComfirmPassword)
            throw new UbeeException(400, "Confirm password is not equal to new password");

        user.Password = PasswordHelper.Hash(dto.NewPassword);

        await this.userRepository.SaveAsync();

        return this.mapper.Map<UserForResultDto>(user);
    }

    public async Task<UserForResultDto> ModifyAsync(long id, UserForCreationDto dto)
    {
        var user = await this.userRepository.SelectAsync(u => u.Id == id);
        if (user is null || user.IsDeleted)
            throw new UbeeException(404, "Couldn't find user for this given id");

        var modifiedUser = this.mapper.Map(dto, user);
        modifiedUser.UpdatedAt = DateTime.UtcNow;

        await this.userRepository.SaveAsync();

        return this.mapper.Map<UserForResultDto>(user);
    }

    public async Task<bool> RemoveAsync(long id)
    {
        var user = await this.userRepository.SelectAsync(u => u.Id == id);
        if (user is null || user.IsDeleted)
            throw new UbeeException(404, "Couldn't find user for this given id");

        await this.userRepository.DeleteAsync(u => u.Id == id);

        await this.userRepository.SaveAsync();

        return true;
    }

    public async Task<IEnumerable<UserForResultDto>> RetrieveAllAsync(PaginationParams @params)
    {
        var users = await this.userRepository.SelectAll()
            .Where(u => u.IsDeleted == false)
            .ToPagedList(@params)
            .ToListAsync();

        return this.mapper.Map<IEnumerable<UserForResultDto>>(users);
    }

    public async Task<UserForResultDto> RetrieveByIdAsync(long id)
    {
        var user = await this.userRepository.SelectAsync(u => u.Id == id);
        if (user is null || user.IsDeleted)
            throw new UbeeException(404, "User not found");

        return this.mapper.Map<UserForResultDto>(user);
    }
}
