using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Blog.SubscribeMeProject.Infrastructure.Models;
using Blog.SubscribeMeProject.Infrastructure.Repositories;
using Blog.SubscribeMeProject.Infrastructure.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Blog.SubscribeMeProject.Controllers
{
    [Route("api/newsletter")]
    [ApiController]
    public class NewsletterController : ControllerBase
    {
        private readonly ISubscriptionRepository _subscriptionRepository;

        public NewsletterController(ISubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        [HttpGet("{email:required}")]
        public async Task<IActionResult> Get([EmailAddress]string email)
        {
            var subscription =  await _subscriptionRepository.Get(email);

            if (subscription == null) return NotFound();

            return Ok(subscription);
        }

        [HttpPost]
        public async Task<IActionResult> Post(SubscriptionRequest request)
        {
            var subscription = new Subscription
            {
                Email = request.Email,
                IsActive = true
            };
            await _subscriptionRepository.Add(subscription);

            return Created(nameof(Get), new { email= subscription.Email });
        }
        
        [HttpDelete("{email:required}")]
        public async Task<IActionResult> Delete([EmailAddress]string email)
        {
            var target = await _subscriptionRepository.Get(email);

            if (target == null) return NotFound();
        
            await _subscriptionRepository.Remove(target.Email);

            return NoContent();
        }

    }
}
