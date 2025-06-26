using Core.Application.Interfaces.OfficeStuff.RepositoryInterface;
using Core.Application.Interfaces.OfficeStuff.ServiceInterface;
using Shared.DTOs.Common.Wrappers;

namespace Core.Application.Commands.OfficeStuff.AddOfficeStuff
{
    public class AddOfficeStuffHandler : IRequestHandler<AddOfficeStuffCommand, Response<IActionResult>>
    {
        private readonly IOfficeStuffRepository _repository;
        public AddOfficeStuffHandler(IOfficeStuffRepository repository)
        {
            _repository = repository;
        }
        public async Task<Response<IActionResult>> Handle(AddOfficeStuffCommand request, CancellationToken cancellationToken)
        {
            try
            {
                int result = await _repository.AddAsync(request);
                return result > 0 ? Response<IActionResult>.Success("Office Stuff Added Successfully") : Response<IActionResult>.Fail("Problem Saving Changes");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Response<IActionResult>.Fail("Problem Saving Changes");
            }
        }
    }
}
