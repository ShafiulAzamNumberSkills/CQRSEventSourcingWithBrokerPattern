using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MediatR;
using static System.Runtime.InteropServices.JavaScript.JSType;
using CleanArchitecture.Application.Commands.Events.PostEvents.CreatePostEvent;
using CleanArchitecture.Application.Events.PostEvents.DeletePostEvent;
using CleanArchitecture.Application.Events.PostEvents.EditPostEvent;
using Microsoft.Extensions.Configuration;


namespace CleanArchitecture.Application.Commands.BrokerManager
{
    public class RabbitMqManager : IBrokerManager
    {
        private readonly ILogger<RabbitMqManager> _logger;
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public RabbitMqManager(ILogger<RabbitMqManager> logger,
                               IMediator mediator,
                               IConfiguration configuration)
        {
            _logger = logger;
            _mediator = mediator;
            _configuration = configuration;
        }

        public static Type GetType(string typeName)
{
    var type = Type.GetType(typeName);
    if (type != null) return type;
    foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
    {
        type = a.GetType(typeName);
        if (type != null)
            return type;
    }
    return null;
}

        public void consume()
        {
            try
            {

                var factory = new ConnectionFactory { 
                    HostName = _configuration.GetSection("AppSettings")["RabbitMQHost"],
                    Port = AmqpTcpEndpoint.UseDefaultPort,
                    UserName = _configuration.GetSection("AppSettings")["RabbitMQUsername"],
                    Password = _configuration.GetSection("AppSettings")["RabbitMQPassword"],
                    VirtualHost = "/",
                    ContinuationTimeout = new TimeSpan(10, 0, 0, 0)
                };
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                channel.QueueDeclare(queue: "PostCommandQueue",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                channel.QueueBind("PostCommandQueue", "PostCommands", "PostEvents");

                Console.WriteLine(" [*] Waiting for messages.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (model, ea) => 
                {


                    _logger.LogInformation("Publish Event: " + ea.BasicProperties.Type);

                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    var eventType = AppDomain.CurrentDomain.GetAssemblies()
                                    .SelectMany(t => t.GetTypes()).Where(t => 
                                    string.Equals(t.Name, ea.BasicProperties.Type, StringComparison.Ordinal)).First();

                    var eventData = JsonConvert.DeserializeObject(message, eventType);

                    _logger.LogInformation("Publish Event Data: " + JsonConvert.SerializeObject(eventData));

                    await _mediator.Publish(eventData);



                };

                channel.BasicConsume(queue: "PostCommandQueue",
                     autoAck: true,
                     consumer: consumer);

                System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);

            }
            catch (Exception ex)
            {
                _logger.LogError(message: ex.Message, ex);
                throw;

            }
        }

        public void publish(object data, string eventType)
        {
            try
            {
                _logger.LogInformation("Publish To Broker: " + JsonConvert.SerializeObject(data));


                var factory = new ConnectionFactory
                {
                    HostName = _configuration.GetSection("AppSettings")["RabbitMQHost"],
                    Port = AmqpTcpEndpoint.UseDefaultPort,
                    UserName = _configuration.GetSection("AppSettings")["RabbitMQUsername"],
                    Password = _configuration.GetSection("AppSettings")["RabbitMQPassword"],
                    VirtualHost = "/",
                    ContinuationTimeout = new TimeSpan(10, 0, 0, 0)
                };

                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                channel.QueueDeclare(queue: "PostCommandQueue",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                properties.ContentType = "application/json";
                properties.Type = eventType;

                channel.BasicPublish(exchange: "PostCommands",
                                     routingKey: "PostEvents",
                                     basicProperties: properties,
                                     body: body);

            }
            catch (Exception ex)
            {
                _logger.LogError(message: ex.Message, ex);
                throw;

            }
        }
    }
}
