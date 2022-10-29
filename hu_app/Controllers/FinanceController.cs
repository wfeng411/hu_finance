using hu_app.Components.Finance.Merchant;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace hu_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinanceController : ControllerBase
    {
        protected readonly IMediator _mediator;
        public FinanceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("Merchants")]
        public async Task<IActionResult> GetMerchants()
        {
            var data = await _mediator.Send(new GetMerchantsRequest());
            return Ok(data);
        }
    }
}
