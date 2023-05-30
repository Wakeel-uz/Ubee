using Ubee.Domain.Entities;

namespace Ubee.Service.DTOs.Users;

public class UserForCreationDto
{
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Phone { get; set; }
    public string Currency { get; set; }
}
