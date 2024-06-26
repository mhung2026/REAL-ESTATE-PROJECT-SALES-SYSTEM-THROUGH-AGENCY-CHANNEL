﻿using Microsoft.IdentityModel.Tokens;
using RealEstateTestApi.DTO;
using RealEstateTestApi.IRepository;
using RealEstateTestApi.IService;
using RealEstateTestApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RealEstateTestApi.ServiceImpl
{
    public class AccountServiceImpl : IAccountService
    {
        private IAccountRepository accountRepository;
        public AccountServiceImpl(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public UserTokenDto loginIntoServer(LoginDto loginDto)
        {
            Account account = accountRepository.findUsernameAndPasswordToLogin(loginDto);
            UserLoginBasicInformationDto dto = new UserLoginBasicInformationDto();
            if (account == null)
            {
                return null;

            }
            else
            {
                if (account.Email.Equals(loginDto.Email) && account.Password.Equals(loginDto.Password))
                {
                    dto.AccountId = account.Id;
                    dto.Username = account.Username;
                    dto.Email = account.Email;
                    dto.Password = account.Password;
                    dto.RoleName = account.Role.RoleName;
                    var access_Token = createJwtToken(account);
                    UserTokenDto userTokenDto = new UserTokenDto()
                    {
                        accessToken = access_Token,
                        userLoginBasicInformationDto = dto

                    };

                    return userTokenDto;
                }
                return null;
               
            }
           /* if (account != null)
            {
              
                dto.AccountId = account.Id;
                dto.Username = account.Username;
                dto.Email = account.Email;
                dto.Password = account.Password;
                dto.RoleName = account.Role.RoleName;
            }*/

          /*  if (account != null && dto.Email != null && dto.Password !=null)
            {
                var access_Token = createJwtToken(account);
                UserTokenDto userTokenDto = new UserTokenDto()
                {
                    accessToken = access_Token,
                    userLoginBasicInformationDto = dto

                };

                return userTokenDto;
            }
            return null;*/
        }

        
        public string createJwtToken(Account account)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("helloearththisismysecrectkeyforjwt123456789")
            );
            var credentials = new SigningCredentials(
                symmetricSecurityKey,
                SecurityAlgorithms.HmacSha256
            );
         
            var userCliams = new List<Claim>();
            userCliams.Add(new Claim("email", account.Email));
            userCliams.Add(new Claim("username", account.Username));
            userCliams.Add(new Claim("password", account.Password));
            userCliams.Add(new Claim(ClaimTypes.Role, account.Role.RoleName));


            var jwtToken = new JwtSecurityToken(
                issuer: "http://realestatesmart-001-site1.etempurl.com",
                expires: DateTime.Now.AddHours(5),
                signingCredentials: credentials,
                claims: userCliams,
                audience: "http://realestatesmart-001-site1.etempurl.com"
            );
            string token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return token;
        }

        public Account createAccount(AccountDto dto)
        {
            Account account = new Account()
            {
                RoleId = dto.RoleId,
                Address = dto.Address,
                Username = dto.Username,
                Password = dto.Password,
                CreateAt = dto.CreateAt,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                Status = true
            };

            if (account == null)
            {
                return null;
            }
            accountRepository.createAccount(account);
            return account;
        }

        public Account updateAccountByAccountId(int accountId, AccountUpdateDto dto)
        {
            Account account = accountRepository.findAccountById(accountId);
            if (account != null)
            {
                account.Username = dto.Username;
                account.Password = dto.Password;
                account.UpdateAt = dto.UpdateAt;
                account.PhoneNumber = dto.PhoneNumber;
                account.Address = dto.Address;
                accountRepository.updateAccount(account);
                return account;
            }
            return null;
        }

        public Account forgotPassword(string email, AccountForgotPasswordDto dto)
        {
            Account account = accountRepository.findAccountByEmail(email.Trim());
            if(account != null)
            {
                account.Password = dto.Password;
                account.UpdateAt = dto.UpdateAt;
                accountRepository.updateAccount(account);
                return account;
            }
            return null;
        }
    }
}
