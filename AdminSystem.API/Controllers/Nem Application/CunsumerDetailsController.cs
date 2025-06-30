using CFEMS.API.Controllers.Common;
using Core.Application.Commands.Reconciliation;
using Core.Application.Queries.App_Management;
using Core.Application.Queries.Consumer;
using Core.Application.Queries.MiscBilling;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.CustomerDto;

namespace AdminSystem.API.Controllers.Nem_Application
{
 
    public class CunsumerDetailsController : ControllerBase
    {
        private IMediator _mediator;
        protected IMediator _mediatr => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        [AllowAnonymous]
        [HttpPost("/api/consumerinfo")]

        
        public async Task<IActionResult> GetConsumerDetails([FromBody] ConsumerDetailQuery command)
        {
            var result = await _mediatr.Send(command);
            if (result != null)
            {
                return Ok(new { status = 200, Data = result });

            }
            else
            {

                var errorResponse = new ErrorResponse
                {
                    Status = 401,
                    Data = new { },
                    Errors = new List<ErrorDetail>
                {
                    new ErrorDetail
                    {
                        Code = "401.1",
                        Message = "Consumer Number is not valid."
                    }
                }
                };

                return Unauthorized(errorResponse);
                //return Ok(new { status = false, result = "Data Not Found" });

            }
        }
    }
}
