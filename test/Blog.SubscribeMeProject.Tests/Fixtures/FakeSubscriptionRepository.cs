using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.SubscribeMeProject.Infrastructure.Models;
using Blog.SubscribeMeProject.Infrastructure.Repositories;

namespace Blog.SubscribeMeProject.Tests.Fixtures
{
    public class FakeSubscriptionRepository : ISubscriptionRepository
    {
        private readonly IList<Subscription> _repository;
       
        public FakeSubscriptionRepository()
        {
            _repository = new List<Subscription>{
                new Subscription{ Email= "samuele.resca@gmail.com", IsActive = true}
            };
        }

        public async Task Add(Subscription subscription)
        {
            await Task.Run(() =>
            {
                _repository.Add(subscription);
            });
        }

        public async Task<Subscription> Get(string email) =>
        await Task.Run(() => _repository.FirstOrDefault(_ => _.Email == email));

        public async Task Remove(string email)
        {
            await Task.Run(() =>
            {
                _repository.Remove(_repository.FirstOrDefault(_ => _.Email == email));
            });
                
        }
    }
}
