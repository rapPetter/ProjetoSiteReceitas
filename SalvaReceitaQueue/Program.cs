using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using System.Text;
using SalvaReceitaQueue.Models;
using SalvaReceitaQueue;
using System.Net;
using Microsoft.EntityFrameworkCore;



var factory = new ConnectionFactory()
{
    HostName = "142.93.173.18",
    UserName = "admin",
    Password = "devintwitter"
};

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();


channel.QueueDeclare(queue: "SalvaReceitaQueue",
                           durable: true,
                           exclusive: false,
                           autoDelete: false,
                           arguments: null);

var consumer = new EventingBasicConsumer(channel);

consumer.Received += async (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    ReceitaViewModel ReceitaModel = JsonConvert.DeserializeObject<ReceitaViewModel>(message);
    Console.WriteLine($"Recebido {ReceitaModel.Descricao} de {ReceitaModel.Email} {ReceitaModel.NomeReceita}.");
    using var ctx = new CoreApiContext();
    try
    {

        Receita Receita = await SalvaReceita(ReceitaModel);
        await PublicaNaGPTQueue(channel, Receita);
        var aluno = await ctx.Usuarios.ToListAsync();
        var Procura = aluno.Where(w => w.Email == ReceitaModel.Email).ToList();
        if (Procura.Count == 0)
        {
            Usuario usuario = await SalvaUsuario(ReceitaModel);
        }
        else
        {

           Usuario usuario = ctx.Usuarios.First(e => e.Email == ReceitaModel.Email);

            usuario.Cidade = BuscaCidadeByIPV4(ReceitaModel.Ipv4);
            usuario.QuantidadeDeReceitasPostadas =  usuario.QuantidadeDeReceitasPostadas +1;


            ctx.Entry(usuario).State = EntityState.Modified;

            await ctx.SaveChangesAsync();
            Console.WriteLine("Salvou");

        }



    }
    catch (Exception e)
    {
        Console.WriteLine("falha ao salvar");
    }
};

channel.BasicConsume(queue: "SalvaReceitaQueue",
                        autoAck: true,
                        consumer: consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();



async Task<Receita> SalvaReceita(ReceitaViewModel receitaViewModel)
{
    using var ctx = new CoreApiContext();
    var receita = new Receita
    {
        Id = 0,
        Email = receitaViewModel.Email,
        NomeReceita = receitaViewModel.NomeReceita,
        Ingredientes = receitaViewModel.Ingredientes,
        TempoPreparo = receitaViewModel.TempoPreparo,
        Porcoes = receitaViewModel.Porcoes,
        Descricao = receitaViewModel.Descricao,
        DataPublicacao = (DateTime)receitaViewModel.DataPublicacao,
        Caloria = 0,
        Status = 0,
    };

    ctx.Receitas.Add(receita);
    await ctx.SaveChangesAsync();

    return receita;
}

async Task<Usuario> SalvaUsuario(ReceitaViewModel receitaViewModel)
{
    using var ctx = new CoreApiContext();

    var usuario = new Usuario
    {

        Id = 0,
        Email = receitaViewModel.Email,
        Cidade = BuscaCidadeByIPV4(receitaViewModel.Ipv4),
        QuantidadeDeReceitasPostadas = 1,
    };

    ctx.Usuarios.Add(usuario);
    await ctx.SaveChangesAsync();

    return usuario;
}

async Task<Usuario> AlteraUsuario(ReceitaViewModel receitaViewModel)
{
    using var ctx = new CoreApiContext();
    Usuario usuario = await ctx.Usuarios.FindAsync(receitaViewModel.Email);

    usuario.Cidade = BuscaCidadeByIPV4(receitaViewModel.Ipv4);
    usuario.QuantidadeDeReceitasPostadas = +1;


    ctx.Entry(usuario).State = EntityState.Modified;

    await ctx.SaveChangesAsync();
    Console.WriteLine("Salvou");
    return usuario;

}

async Task PublicaNaGPTQueue(IModel channel, Receita receita)
{
    channel.QueueDeclare(queue: "ClassificaCategoriaGPTQueue",
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

    var stringJson = JsonConvert.SerializeObject(new
    {
        Id = receita.Id,
        Ingredientes = receita.Ingredientes,
        Descricao = receita.Descricao,
    });

    var body = Encoding.UTF8.GetBytes(stringJson);

    channel.BasicPublish(exchange: string.Empty,
                            routingKey: "ClassificaCategoriaGPTQueue",
                            basicProperties: null,
                            body: body);

}

string BuscaCidadeByIPV4(string ipv4)
{

    if (ipv4 != "::1")
    {
        string info = new WebClient().DownloadString("http://ipinfo.io/" + ipv4);
        var ipInfo = JsonConvert.DeserializeObject<dynamic>(info);

        return ipInfo.city;
    }
    else
    {
        return "Desconhecido";
    }
}
