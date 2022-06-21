using TweetAppAPI.Model;

namespace TweetAppAPI.Repository
{
    public interface IAuthRepository
    {
        UserResponse Login(UserRequest userRequest);
        UserCreationStatus Register(User userRequest);
        UserResponse AuthenticateUser(UserRequest userRequest);
        UserResponse ResetPassword(ResetPassword resetPassword, string username);
    }
}