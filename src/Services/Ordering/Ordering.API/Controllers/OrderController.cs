using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.Orders.Queries.GetOrdersList;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Ordering.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public IMapper Mapper { get; }
        public IOrderRepository Repository { get; }

        public OrderController(IMediator mediator, IMapper mapper, IOrderRepository _repository)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            Mapper = mapper;
            Repository = _repository;
        }

        [HttpGet("{userName}", Name = "GetOrder")]
        [ProducesResponseType(typeof(IEnumerable<OrdersVm>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<OrdersVm>>> GetOrdersByUserName(string userName)
        {
            var orders = await Repository.GetOrdersByUserName(userName);
            /* var query = new GetOrdersListQuery(userName);
             var orders = await _mediator.Send(query);*/
            return Ok(orders);
        }

        // testing purpose
        [HttpPost(Name = "CheckoutOrder")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<int>> CheckoutOrder([FromBody] CheckoutOrderCommand command)
        {
            var order = new Order();
            order.UserName = command.UserName;
            order.AddressLine = command.AddressLine;
            order.LastName = command.LastName;
            order.FirstName = command.FirstName;
            order.CardName = command.CardName;
            order.CardNumber = command.CardNumber;
            order.CVV = command.CVV;
            order.EmailAddress = command.EmailAddress;
            order.Country = command.Country;
            order.ZipCode = command.ZipCode;
            order.Expiration = command.Expiration;
            order.TotalPrice = command.TotalPrice;
            order.PaymentMethod = command.PaymentMethod;

            //var orderEntity = Mapper.Map<Order>(command);
            var newOrder = await Repository.AddAsync(order);
            return Ok(newOrder);
        }

        [HttpPut(Name = "UpdateOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateOrder([FromBody] UpdateOrderCommand command)
        {
            var orderToUpdate = await Repository.GetByIdAsync(command.Id);
            var order = new Order();
            orderToUpdate.UserName = command.UserName;
            orderToUpdate.AddressLine = command.AddressLine;
            orderToUpdate.LastName = command.LastName;
            orderToUpdate.FirstName = command.FirstName;
            orderToUpdate.CardName = command.CardName;
            orderToUpdate.CardNumber = command.CardNumber;
            orderToUpdate.CVV = command.CVV;
            orderToUpdate.EmailAddress = command.EmailAddress;
            orderToUpdate.Country = command.Country;
            orderToUpdate.ZipCode = command.ZipCode;
            orderToUpdate.Expiration = command.Expiration;
            orderToUpdate.TotalPrice = command.TotalPrice;
            orderToUpdate.PaymentMethod = command.PaymentMethod;
            //var orderEntity = Mapper.Map<Order>(command);
            await Repository.UpdateAsync(orderToUpdate);
            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            var orderToDelete = await Repository.GetByIdAsync(id);
            if (orderToDelete == null)
            {
                throw new NotFoundException(nameof(Order), id);
            }

            await Repository.DeleteAsync(orderToDelete);
            return NoContent();
        }
    }
}
