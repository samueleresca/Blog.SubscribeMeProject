using Blog.SubscribeMeProject.Models;
using Blog.SubscribeMeProject.Repositories;
using Blog.SubscribeMeProject.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Blog.SubscribeMeProject.Controllers
{
    [Route("api/newsletter")]
    [ApiController]
    public class NewsletterController : ControllerBase
    {
        private readonly INewsletterRepository _newsletterRepository;

        public NewsletterController(INewsletterRepository newsletterRepository)
        {
            _newsletterRepository = newsletterRepository;
        }

        [HttpGet("{email:email}")]
        public IActionResult Get(string email)
        {
            var subscription = _newsletterRepository.Get(email);

            if (subscription == null) return NotFound();

            return Ok(subscription);
        }

        [HttpPost]
        public IActionResult Post(SubscriptionRequest request)
        {
            var subscription = new Subscription
            {
                Email = request.Email,
                IsActive = true
            };
            _newsletterRepository.Add(subscription);

            return Created(nameof(Get), new { email= subscription.Email });
        }
        
        [HttpDelete]
        public IActionResult Delete(SubscriptionRequest request)
        {
            var target = _newsletterRepository.Get(request.Email);

            if (target == null) return NotFound();
        
            _newsletterRepository.Remove(target.Email);

            return NoContent();
        }

    }
}
