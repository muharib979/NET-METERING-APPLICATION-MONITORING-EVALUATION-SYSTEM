using Core.Application.Commands.Dbo.UserAddByCenterLocation;
using Shared.DTOs.Common.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Dbo.GetUserRole
{
    public class GetNewUserByCenterLocationByIdQuery : IRequest<Response<NewUserResponse>>
    {
        public int Id { get; set; }
        public class Handler : IRequestHandler<GetNewUserByCenterLocationByIdQuery, Response<NewUserResponse>>
        {
            private readonly IUserRepository _repository;
            private readonly IMapper _mapper;

            public Handler(IUserRepository repository,IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }
            public async Task<Response<NewUserResponse>> Handle(GetNewUserByCenterLocationByIdQuery request, CancellationToken cancellationToken)
            {
                // var user = await _repository.GetNewUserByCenterLocationById(request.Id);

                var newuserResponse = new NewUserResponse();

                var user = await _repository.GetByIdAsync(request.Id);
                if (user == null)
                    return Response<NewUserResponse>.Success("No User Found!");

                
                List<GetUserCreateByCenterLocationModel> result = await _repository.GetNewUserByCenterLocationById(request.Id);
                if (result.Count() == 0)
                    return Response<NewUserResponse>.Success("No assigned values of user was found");

                var dbList = GetNewUserDbList(result);
                var locationList = new List<int>();

                if (dbList.Count() >= 1)
                {
                    locationList = GetNewUserLocationList(result);
                }
                       
                newuserResponse.content = _mapper.Map<GetUserCreateByCenterLocationModelDto>(result.FirstOrDefault());
                newuserResponse.locationList = locationList;
                newuserResponse.dbList = dbList;

                return Response<NewUserResponse>.Success(newuserResponse, "Successfully Retrived User!");
            }
        }

        #region methods
        /// <summary>
        /// Gets new user's assigned database list.
        /// </summary>
        /// <param name="GetUserCreateByCenterLocationModelDtos">List<GetUserCreateByCenterLocationModelDto> GetUserCreateByCenterLocationModelDtos is the list of new user values</param>
        /// <returns>List<string> list of new user's assigned database list</returns>
        private static List<string> GetNewUserDbList(List<GetUserCreateByCenterLocationModel> getUserCreateByCenterLocationModelDtos)
        {
            var prevDbId = "0";
            var list = new List<string>();
            getUserCreateByCenterLocationModelDtos.ForEach(x =>
            {
                if (prevDbId != x.DB_CODE)
                    list.Add(x.DB_CODE);

                prevDbId = x.DB_CODE;
            });

            return list;
        }

        /// <summary>
        /// Gets new user's assigned location list.
        /// </summary>
        /// <param name="GetUserCreateByCenterLocationModelDtos">List<GetUserCreateByCenterLocationModelDto> GetUserCreateByCenterLocationModelDtos is the list of new user values</param>
        /// <returns>List<string> list of new user's assigned location list</returns>
        private static List<int> GetNewUserLocationList(List<GetUserCreateByCenterLocationModel> GetUserCreateByCenterLocationModelDtos)
        {
            if (GetUserCreateByCenterLocationModelDtos[0].LOCATION_ID == 0)
            {
                return null;
            }
            var list = new List<int>();
            GetUserCreateByCenterLocationModelDtos.ForEach(x => { list.Add(x.LOCATION_ID); });

            return list;
        }


        #endregion methods
    }

    public class NewUserResponse
    {
        public object content { get; set; }
        public List<string> dbList { get; set; }
        public List<int> locationList { get; set; }
    }
}
