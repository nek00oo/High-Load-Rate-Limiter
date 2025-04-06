using UserService.Models.DomainInterfaces;

namespace UserService;

public partial class CreateUserRequest : IUser
{
    public int Id { get; set; }
}
    