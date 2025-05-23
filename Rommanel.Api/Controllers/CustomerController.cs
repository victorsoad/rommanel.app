using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rommanel.Application.Commands;
using Rommanel.Application.Queries;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST: api/customers
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCustomerCommand command, CancellationToken cancellationToken)
        {
            var id = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }

        // PUT: api/customers/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCustomerCommand command, CancellationToken cancellationToken)
        {
            if (id != command.Id)
            {
                return BadRequest("Id in URL does not match Id in payload.");
            }

            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        // DELETE: api/customers/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteCustomerCommand(id);
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        // GET: api/customers/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetCustomerQuery(id);
            var customer = await _mediator.Send(query, cancellationToken);
            if (customer == null)
                return NotFound();

            return Ok(customer);
        }

        // GET: api/customers?pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
        {
            var query = new GetAllCustomersQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }
    }
}
