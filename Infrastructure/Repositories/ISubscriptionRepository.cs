using System.Threading.Tasks;
using Blog.SubscribeMeProject.Infrastructure.Models;

namespace Blog.SubscribeMeProject.Infrastructure.Repositories
{
    public interface ISubscriptionRepository
    {
        Task<Subscription> Get(string email);
        Task Add(Subscription subscription);
        Task Remove(string email);
    }
}