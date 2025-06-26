using Newtonsoft.Json;
using Shared.DTOs.Common.Wrappers;

namespace Core.Application.Commands.Dbo.MenuAdd;

public class MenuAddCommand : MenuDto, IRequest<Response<IActionResult>> 
{
    public class Handler : IRequestHandler<MenuAddCommand, Response<IActionResult>>
    {
        private readonly IMenuRepository _repository;
        private readonly IMapper _mapper;

        public Handler(IMenuRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IActionResult>> Handle(MenuAddCommand request, CancellationToken cancellationToken)
        {

            if (!string.IsNullOrEmpty(request.Icon) && !string.IsNullOrEmpty(request.IconSVG))
            {
                var SVGIconresult = await SaveSVGIcon(request.Icon, request.IconSVG);

                if (SVGIconresult == 0) return Response<IActionResult>.Fail("Problem saving changes");
            }
            int result = await _repository.AddAsync(_mapper.Map<Menu>(request));


            return result > 0 ? Response<IActionResult>.Success("Menu Successfully Created") : Response<IActionResult>.Fail("Problem saving changes");
        }


        private async Task<int> SaveSVGIcon(string icon, string svg)
        {
            var status = 0;
            var filePath = $"Uploades/SVGIcons/iconsvg.json";

            if (!File.Exists(filePath))
                return status;

            string json = await File.ReadAllTextAsync(filePath);
            var DataOfSvgs = JsonConvert.DeserializeObject<List<SVGICon>>(json) ?? new List<SVGICon>();

            if (DataOfSvgs.Count > 0) DataOfSvgs.RemoveAll(x => x.Name == icon);


            DataOfSvgs.Add(new SVGICon
            {
                Name = icon,
                Value = svg
            });
            string updatedJson = JsonConvert.SerializeObject(DataOfSvgs, Formatting.Indented);
            await File.WriteAllTextAsync(filePath, updatedJson);

            status = 1;

            return status;
        }
    }
}
