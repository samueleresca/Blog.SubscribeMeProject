using System.ComponentModel.DataAnnotations;

namespace Blog.SubscribeMeProject.Infrastructure.Requests
 {
     public class SubscriptionRequest
     {
         [Required]
         [EmailAddress]
         public string Email { get; set; }
     }
 }