using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.OfficeStuff.AddOfficeStuff
{
    public class AddOfficeStuffCommandValidator: AbstractValidator<AddOfficeStuffCommand>
    {
        public AddOfficeStuffCommandValidator()
        {
            RuleFor(x=>x.OfficeStuffName).NotEmpty();
        }
    }
}
