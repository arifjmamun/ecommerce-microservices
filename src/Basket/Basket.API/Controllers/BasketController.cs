using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using EventBus.Common;
using EventBus.Events;
using EventBus.Producers;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;
        private readonly IMapper _mapper;
        private readonly EventBusProducer _eventBus;

        public BasketController(IBasketRepository repository, IMapper mapper, EventBusProducer eventBus)
        {
            _repository = repository;
            _mapper = mapper;
            _eventBus = eventBus;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BasketCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<BasketCart>> GetBasket(string userName)
        {
            var basket = await _repository.GetBasket(userName);
            return Ok(basket);
        }

        [HttpPut]
        [ProducesResponseType(typeof(BasketCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<BasketCart>> UpdateBasket(BasketCart basket)
        {
            var updated = await _repository.UpdateBasket(basket);
            return Ok(updated);
        }

        [HttpDelete("{userName}")]
        [ProducesResponseType(typeof(BasketCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> UpdateBasket(string userName)
        {
            var deleted = await _repository.DeleteBasket(userName);
            return Ok(deleted);
        }

        [HttpPost("checkout")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> Checkout(BasketCheckout basketCheckout)
        {
            // get total price of basket
            // remove the basket
            // send checkout event to rabbitmq

            var basket = await _repository.GetBasket(basketCheckout.UserName);
            if (basket == null) return BadRequest();

            var basketRemoved = await _repository.DeleteBasket(basket.UserName);
            if (!basketRemoved) return BadRequest();

            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMessage.RequestId = Guid.NewGuid();
            eventMessage.TotalPrice = basket.TotalPrice;

            try
            {
                _eventBus.PublishBasketCheckout(EventBusConstants.BasketCheckoutQueue, eventMessage);
            }
            catch (Exception)
            {
                throw;
            }
            return Accepted();
        }
    }
}
