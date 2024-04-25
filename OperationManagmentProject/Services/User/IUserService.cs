using OperationManagmentProject.Entites;
using OperationManagmentProject.Models;

namespace OperationManagmentProject.Services.User
{
    public interface IUserService
    {
        UserModel GetDetailedUserInformation(UserEntity user);
    }
}
