using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MiControllerBase : ControllerBase
    {
        private IMediator _mediator;
        protected IMediator mediator => _mediator ?? (_mediator = HttpContext.RequestServices.GetService<IMediator>());
    }
}
