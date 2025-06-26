using Shared.DTOs.Common.Wrappers;

namespace Core.Application.Queries.Dbo.GetSVGIconList;

public class GetSVGIconListQurey : IRequest<Response<List<SVGICon>>>
{
    public class Handler : IRequestHandler<GetSVGIconListQurey, Response<List<SVGICon>>>
    {
        private readonly IMenuRepository _menurepository;
        public Handler(IMenuRepository menurepository) => _menurepository = menurepository;

        public async Task<Response<List<SVGICon>>> Handle(GetSVGIconListQurey request, CancellationToken cancellationToken)
        {
            var result = await _menurepository.GetSVGIconList();
            return Response<List<SVGICon>>.Success(result, "Successfully Retrived All SVG Icon List");

        }
    }
}