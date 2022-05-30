using EmailService.interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Hosting;
using MimeKit;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.apiResponse.mailResponse;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace EmailService.services
{
    public class ReceiveRabbit : BackgroundService
    {
        private IConnection _connection;
        private IModel _channel;
        private readonly IMailService _mailService;

        public ReceiveRabbit(IMailService mailService)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "emailQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            _mailService = mailService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (stoppingToken.IsCancellationRequested)
            {
                _channel.Dispose();
                _connection.Dispose();
                return Task.CompletedTask;
            }

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                string message = Encoding.UTF8.GetString(body);

                //send email here
                MailRequest mail = JsonSerializer.Deserialize<MailRequest>(message);
                _mailService.SendEmail(mail);
            };


            _channel.BasicConsume(queue: "emailQueue",
                                 autoAck: true,
                                 consumer: consumer);

            return Task.CompletedTask;
        }
    }
}
