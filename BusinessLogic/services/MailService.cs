using BusinessLogic.interfaces;
using DatabaseAccess;
using Microsoft.EntityFrameworkCore;
using Shared.apiResponse.serviceResponse;
using Shared.dtos.mailDTOs;
using Shared.models;
using Shared.models.mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic.services
{
    public class MailService : IMailService
    {
        private readonly AppDbContext _context;

        public MailService(AppDbContext context)
        {
            _context = context;
        }

        public ServiceResponse<GetSubscriptionDto> Subscribe(SubsribeUserDto subscribe)
        {
            AppUser user = _context.AppUsers.Include(u => u.Subscription).FirstOrDefault(u => u.Id == subscribe.UserId);
            if (user == null) return new ServiceResponse<GetSubscriptionDto>(null, false, "User does not exist", ResponseType.Failed);
            if (user.Subscription != null) return new ServiceResponse<GetSubscriptionDto>(null, false, "User already subscribed", ResponseType.Failed);

            Subscription subscription = new Subscription()
            {
                Interval = subscribe.IntervalInSeconds,
                AppUser = user,
                AppUserId = user.Id,
                Cities = GetCities(subscribe.Cities)
            };
            _context.Subscriptions.Add(subscription);
            _context.SaveChanges();

            return new ServiceResponse<GetSubscriptionDto>(new GetSubscriptionDto
            {
                Id = subscription.Id,
                AppUserId = user.Id,
                Interval = subscribe.IntervalInSeconds,
                Cities = subscribe.Cities,
            });
        }

        public ServiceResponse<GetSubscriptionDto> Unsubscribe(int userId)
        {
            AppUser user = _context.AppUsers.Include(u => u.Subscription).ThenInclude(s => s.Cities).FirstOrDefault(u => u.Id == userId);
            if (user == null) return new ServiceResponse<GetSubscriptionDto>(null, false, "User does not exist", ResponseType.Failed);
            if (user.Subscription == null) return new ServiceResponse<GetSubscriptionDto>(null, false, "User is not subscribed", ResponseType.Failed);

            Subscription s = user.Subscription;
            _context.Subscriptions.Remove(s);
            _context.SaveChanges();

            return new ServiceResponse<GetSubscriptionDto>(new GetSubscriptionDto
            {
                AppUserId = userId,
                Id = s.Id,
                Interval = s.Interval,
                Cities = s.Cities.Select(s => s.Name).ToList(),
            });
        }

        private List<City> GetCities(List<string> cities)
        {
            List<City> ans = new List<City>();
            foreach (string city in cities) ans.Add(new City { Name = city});
            return ans;
        }
    }
}
