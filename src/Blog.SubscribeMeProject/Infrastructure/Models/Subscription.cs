using Newtonsoft.Json;

namespace Blog.SubscribeMeProject.Infrastructure.Models
{
    public class Subscription
    {
        [JsonProperty(PropertyName = "id")]
        public string Email { get; set; }
        public bool IsActive { get; set; }
        
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}