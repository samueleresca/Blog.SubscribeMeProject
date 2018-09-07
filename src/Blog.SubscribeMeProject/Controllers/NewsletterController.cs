using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Blog.SubscribeMeProject.Infrastructure.Models;
using Blog.SubscribeMeProject.Infrastructure.Repositories;
using Blog.SubscribeMeProject.Infrastructure.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Blog.SubscribeMeProject.Controllers
{
    [Route("api/newsletter")]
    [ApiController]
    public class NewsletterController : ControllerBase
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly ILogger<NewsletterController> _logger;

        public NewsletterController(ISubscriptionRepository subscriptionRepository,
            ILogger<NewsletterController> logger)
        {
            _subscriptionRepository = subscriptionRepository;
            _logger = logger;
        }

        [HttpGet("{email:required}")]
        public async Task<IActionResult> Get([EmailAddress]string email)
        {

            try
            {
                var subscription = await _subscriptionRepository.Get(email);

                if (subscription == null) return NotFound();

                return Ok(subscription);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(SubscriptionRequest request)
        {
            try
            {

                var result = await _subscriptionRepository.Get(request.Email);

                if (result != null)
                {
                    return BadRequest();
                }

                var subscription = new Subscription
                {
                    Email = request.Email,
                    IsActive = true
                };

                await _subscriptionRepository.Add(subscription);

                return Created(nameof(Get), new { email = subscription.Email });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500);
            }

        }

        [HttpDelete("{email:required}")]
        public async Task<IActionResult> Delete([EmailAddress]string email)
        {
            try
            {
                var target = await _subscriptionRepository.Get(email);

                if (target == null) return NotFound();

                await _subscriptionRepository.Remove(target.Email);

                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500);
            }
        }

    }
}
