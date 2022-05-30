using BusinessLogic.interfaces;
using RabbitMQ.Client;
using Shared.apiResponse.mailResponse;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace BusinessLogic.services
{
    public class SendRabbit : IDisposable, ISendRabbit
    {
        private IConnection _connection;
        private IModel _channel;

        public SendRabbit()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void SendMessage(string message,string queue)
        {
            _channel.QueueDeclare(queue: queue,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "",
                                 routingKey: queue,
                                 basicProperties: null,
                                 body: body);
        }

        public void Dispose()
        {
            _channel.Close();
            _connection.Close();
        }
    }
}
