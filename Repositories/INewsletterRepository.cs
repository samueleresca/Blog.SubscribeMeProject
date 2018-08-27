using System.ComponentModel.DataAnnotations;
using Blog.SubscribeMeProject.Models;

namespace Blog.SubscribeMeProject.Repositories
{
    public interface INewsletterRepository
    {
        Subscription Get(string email);
        void Add(Subscription subscription);
        void Remove(string email);
    }
}