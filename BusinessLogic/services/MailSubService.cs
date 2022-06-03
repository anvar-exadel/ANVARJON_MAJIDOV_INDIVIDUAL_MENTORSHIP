using BusinessLogic.interfaces;
using DatabaseAccess;
using Hangfire;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Shared.apiResponse.mailResponse;
using Shared.apiResponse.serviceResponse;
using Shared.apiResponse.weatherResponse;
using Shared.configFiles;
using Shared.dtos.mailDTOs;
using Shared.models;
using Shared.models.mail;
using Shared.models.weatherHistoryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace BusinessLogic.services
{
    public class MailSubService : IMailSubService
    {
        private readonly AppDbContext _context;
        private readonly IWeatherService _weatherService;
        private readonly IMailService _mailService;

        public MailSubService(IMailService mailService, AppDbContext context, IWeatherService weatherService)
        {
            _context = context;
            _mailService = mailService;
            _weatherService = weatherService;
        }

        public ServiceResponse<GetSubscriptionDto> Subscribe(SubsribeUserDto subscribe, int requestTimeout)
        {
            AppUser user = _context.AppUsers.Include(u => u.Subscription).ThenInclude(s => s.Cities).FirstOrDefault(u => u.Id == subscribe.UserId);
            if (user == null) return new ServiceResponse<GetSubscriptionDto>(null, false, "User does not exist", ResponseType.Failed);
            if (user.Subscription != null) return new ServiceResponse<GetSubscriptionDto>(null, false, "User already subscribed", ResponseType.Failed);
            if (!IsIntervalValid(subscribe.IntervalInHours)) return new ServiceResponse<GetSubscriptionDto>(null, false, "Provide proper interval", ResponseType.Failed);

            Subscription subscription = new Subscription()
            {
                Interval = subscribe.IntervalInHours * 3600,
                AppUser = user,
                AppUserId = user.Id,
                Cities = GetCities(subscribe.Cities)
            };

            _context.Subscriptions.Add(subscription);
            _context.SaveChanges();

            string subId = subscription.Id.ToString();
            string cronExpr = $@"0 */{subscribe.IntervalInHours} * * *";
            SendEmail(user.Id, user.Email, requestTimeout);//send first email
            RecurringJob.AddOrUpdate(subId, () => SendEmail(user.Id, user.Email, requestTimeout), cronExpr);

            return new ServiceResponse<GetSubscriptionDto>(new GetSubscriptionDto
            {
                Id = subscription.Id,
                AppUserId = user.Id,
                IntervalInHours = subscribe.IntervalInHours,
                Cities = subscribe.Cities,
            });
        }

        public ServiceResponse<GetSubscriptionDto> Unsubscribe(int userId)
        {
            AppUser user = _context.AppUsers.Include(u => u.Subscription).ThenInclude(s => s.Cities).FirstOrDefault(u => u.Id == userId);
            if (user == null) return new ServiceResponse<GetSubscriptionDto>(null, false, "User does not exist", ResponseType.Failed);
            if (user.Subscription == null) return new ServiceResponse<GetSubscriptionDto>(null, false, "User is not subscribed", ResponseType.Failed);

            Subscription s = user.Subscription;

            RecurringJob.RemoveIfExists(s.Id.ToString());

            _context.Subscriptions.Remove(s);
            _context.SaveChanges();

            return new ServiceResponse<GetSubscriptionDto>(new GetSubscriptionDto
            {
                AppUserId = userId,
                Id = s.Id,
                IntervalInHours = s.Interval / 3600,
                Cities = s.Cities.Select(s => s.Name).ToList(),
            });
        }

        public void SendEmail(int userId, string email, int requestTimeout)
        {
            string body = GetReport(userId, requestTimeout).Data;
            MailRequest mail = new MailRequest
            {
                ToEmail = email,
                Subject = "Weather Report",
                Body = body
            };
            _mailService.SendEmail(mail);
        }

        public ServiceResponse<string> GetReport(int userId, int requestTimeout)
        {
            AppUser user = _context.AppUsers.Include(u => u.Subscription).ThenInclude(s => s.Cities).FirstOrDefault(u => u.Id == userId);
            if (user == null) return new ServiceResponse<string>(null, false, "User not found");
            if (user.Subscription == null) return new ServiceResponse<string>(null, false, "User not subscribed");

            StringBuilder ans = new StringBuilder(); 
            Subscription s = user.Subscription;
            
            ans.AppendLine($@"The report was generated: {DateTime.Now}. Period: {s.Interval/3600}");
            foreach (City city in s.Cities)
            {
                ServiceResponse<List<WeatherHistory>> response = _weatherService.GetWeatherHistory(city.Name, s.Interval, requestTimeout);
                if (!response.Success) ans.AppendLine($@"{city.Name}: no statistics. ");
                else ans.AppendLine($@"{city.Name} average temperature: {GetAvgTemp(response.Data)} C.");
            }
            return new ServiceResponse<string>(ans.ToString());
        }

        private bool IsIntervalValid(int intervalInSeconds)
        {
            //allowed intervals are => 1, 3, 12, 24 hours
            return intervalInSeconds == 1 || intervalInSeconds == 3
                || intervalInSeconds == 12 || intervalInSeconds == 24;
        }

        private double GetAvgTemp(List<WeatherHistory> weatherHistories)
        {
            double avgTemp = 0;
            int count = 0;
            foreach (WeatherHistory weather in weatherHistories)
            {
                foreach (Hour hour in weather.Hours)
                {
                    avgTemp += hour.Temp_c;
                    count++;
                }
            }
            return Math.Round(avgTemp / count, 2);
        }

        private List<City> GetCities(List<string> cities)
        {
            List<City> ans = new List<City>();
            foreach (string city in cities) ans.Add(new City { Name = city});
            return ans;
        }
    }
}
