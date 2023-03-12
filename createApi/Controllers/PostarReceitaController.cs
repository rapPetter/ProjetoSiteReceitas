using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;
using createApi.Models;
using Newtonsoft.Json;



namespace createApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostarReceitaController : ControllerBase
    {
        private readonly ConnectionFactory _factory;
        public PostarReceitaController()
        {
            _factory = new ConnectionFactory()
            {
                HostName = "142.93.173.18",
                UserName = "admin",
                Password = "devintwitter"
            };
        }

        [HttpPost]
        public ActionResult Post([FromBody] PostarReceitaViewModel model)
        {
            if (model == null)
            {
                return BadRequest(ModelState);
            }

            var receita = new PostarReceita()
            {
                Email = model.Email,
                NomeReceita = model.NomeReceita,
                TempoPreparo = model.TempoPreparo,
                Porcoes = model.Porcoes,
                Ingredientes = model.Ingredientes,
                Descricao = model.Descricao,
                Ipv4 = GetIpv4()

            };
            using var connection = _factory.CreateConnection();
            using var channel = connection.CreateModel();

            try
            {
                PublicaReceita(receita, channel);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        private void PublicaReceita(PostarReceita model, IModel channel)
        {
            channel.QueueDeclare(queue: "SalvaReceitaQueue",
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
            arguments: null);

            var body = JsonConvert.SerializeObject(model);

            var modelBytes = Encoding.UTF8.GetBytes(body);
            channel.BasicPublish(exchange: string.Empty,
                            routingKey: "SalvaReceitaQueue",
                            basicProperties: null,
                            body: modelBytes);
        }

        private string GetIpv4()
        {
            return Response.HttpContext.Connection.RemoteIpAddress.ToString();
        }

    }
}