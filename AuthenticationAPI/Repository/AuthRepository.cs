using TweetAppAPI.Model;
using TweetAppAPI.Model.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;


namespace TweetAppAPI.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthRepository(UserDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public UserResponse Login(UserRequest userRequest)
        {
            try
            {
                UserResponse userResponse = AuthenticateUser(userRequest);
                return userResponse;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public UserCreationStatus Register(User user)
        {
            try
            {
                _context.Users.Add(new User() { First_Name = user.First_Name, Last_Name = user.Last_Name, Gender = user.Gender, DOB = user.DOB, Email = user.Email, Password = user.Password });
                _context.SaveChanges();
                return new UserCreationStatus { UserId = user.UserId, Message = "User's Account is Created Successfully" };

            }
            catch (Exception e)
            {
                throw;
            }
        }

        public UserResponse AuthenticateUser(UserRequest userRequest)
        {
            UserResponse userResponse = new UserResponse();
            try
            {
                User user = _context.Users.Where(c => c.Email == userRequest.Email).SingleOrDefault();

                if(!userRequest.Password.Equals(user.Password))
                {
                    userResponse.Id = -2;
                    userResponse.Message = $"Wrong Password! Try Again";
                    return userResponse;
                }

                string token = GenerateJsonWebToken(user.UserId);
                userResponse.Id = user.UserId;
                userResponse.Token = token;
                userResponse.Message = "Login Successful";
                userResponse.UserName = user.Email;
                return userResponse;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public UserResponse ResetPassword(ResetPassword resetPassword, string username)
        {
            UserResponse userResponse = new UserResponse();
            try
            {

                UserRequest userRequest = new UserRequest()
                {
                    Email = username,
                    Password = resetPassword.CurrentPassword
                };

                UserResponse response = AuthenticateUser(userRequest);
                if (response.Id > 0)
                {
                    User user = _context.Users.Where(u => u.Email.Equals(username)).FirstOrDefault();
                    user.Password = resetPassword.NewPassword;
                    _context.SaveChanges();
                    return new UserResponse()
                    {
                        Id = 1,
                        Message = $"Password Successfully changed for username {username}! Try to Login with New Password",
                        UserName = username
                    };
                }
                else
                {
                    return new UserResponse()
                    {
                        Id = response.Id,
                        Message = response.Message,
                        UserName = response.UserName
                    };

                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private string GenerateJsonWebToken(int customerId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,customerId.ToString())
            };
            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.Now.AddMinutes(5),
                claims: claims,
                signingCredentials: signingCredentials
                );
            string token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
            return token;
        }
    }
}
