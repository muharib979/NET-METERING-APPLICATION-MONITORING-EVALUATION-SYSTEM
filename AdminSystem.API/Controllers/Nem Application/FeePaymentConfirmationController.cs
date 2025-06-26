using Core.Application.Queries.Consumer;
using Core.Application.Queries.Payment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.CustomerDto;

namespace AdminSystem.API.Controllers.Nem_Application
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeePaymentConfirmationController : ControllerBase
    {
        private IMediator _mediator;
        protected IMediator _mediatr => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        [AllowAnonymous]
        [HttpPost("/api/confirmApplicationFeePayment")]


        public async Task<IActionResult> PaymentConfirmation([FromBody] PaymentConfirmationQuery command)
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
                        Message = "Bill Number is not valid."
                    }
                }
                };

                return Unauthorized(errorResponse);

            }
        }

        [AllowAnonymous]
        [HttpPost("/api/checkApplicationFeeStatus")]
        public async Task<IActionResult> PaymentStutus([FromBody] PaymentStutusQuery command)
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
                        Message = "Bill Number is not valid."
                    }
                }
                };

                return Unauthorized(errorResponse);

            }
        }
    }
}
