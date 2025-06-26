using Core.Application.Commands.ProsoftDataSync.CreateUser;
using Core.Application.Interfaces.ProsoftDataSync.RepositoryInterface;
using Core.Application.Interfaces.ProsoftDataSync.ServiceInterface;
using Core.Domain.ProsoftDataSync;
using DocumentFormat.OpenXml.Math;
using Shared.DTOs.Enums;
using Shared.DTOs.ProsoftDataSync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Services.ProsoftDataSync
{
    public class ProsoftUserService : IProsoftUserService
    {
        private readonly IProsoftUserRepository userRepository;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;

        public ProsoftUserService(IProsoftUserRepository userRepository, IConfiguration configuration, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.configuration = configuration;
            this.mapper = mapper;
        }
        public async Task<int> AddProsoftUser(CreateUserCommand command)
        {
            var prosoftUsers = await userRepository.GetUserByName(command.UserName);
            if (prosoftUsers != null) throw new Exception("User Name Already Exists");

            var hmac = new HMACSHA512();
            var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(configuration["SecuritySettings:ProsoftUserDefaultPassword"]));

            var prosoftNewUser = mapper.Map<ProsoftUsers>(command);
            prosoftNewUser.PASSWORD = Convert.ToBase64String(passwordHash);
            prosoftNewUser.PASSWORD_SALT = Convert.ToBase64String(hmac.Key);
            prosoftNewUser.ENTRY_BY = "System";
            prosoftNewUser.ENTRY_DATE = DateTime.Now;
            prosoftNewUser.IS_ACTIVE = (int)BooleanEnum.TRUE;

            return await userRepository.AddProsoftUser(prosoftNewUser);
        }

        public Task<ProsoftUsers> GetUserByName(string name)
        {
            return userRepository.GetUserByName(name);
        }

        public async Task<int> UpdateUserTokenInfo(ProsoftUsers users)
        {
            return await userRepository.UpdateUserTokenInfo(users);
        }

        public async Task<ProsoftUsers> GetByRefreshtokenAsync(string refreshToken)
        {
            return await userRepository.GetByRefreshtokenAsync(refreshToken);
        }


        public async Task<ProsoftUsers> GetUserByIdAsync(int userid)
        {
            return await userRepository.GetUserByIdAsync(userid);
        }


        public Task<int> AddAsync(ProsoftUserDTO entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddListAsync(List<ProsoftUserDTO> entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ProsoftUserDTO>> GetAllAsync(PaginationParams pParams)
        {
            throw new NotImplementedException();
        }

        public Task<ProsoftUserDTO> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetTotalCountAsync(string searchBy)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(ProsoftUserDTO entity)
        {
            throw new NotImplementedException();
        }

    }
}