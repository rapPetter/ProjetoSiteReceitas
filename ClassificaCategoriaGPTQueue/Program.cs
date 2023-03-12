using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Net.Http.Headers;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using System.Text;
using SalvaReceitaQueue.Models;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ClassificaCategoriaGPTQueue.Models;
using SalvaReceitaQueue;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System;
using ClassificaCategoriaGPTQueue.Enum;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;


var factory = new ConnectionFactory()
{
    HostName = "142.93.173.18",
    UserName = "admin",
    Password = "devintwitter"
};

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();


channel.QueueDeclare(queue: "ClassificaCategoriaGPTQueue",
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
    string tirarespaco = ReceitaModel.Ingredientes;
    Console.WriteLine($"Receita numero : {ReceitaModel.Id} contendo os seguintes ingredientes : {ReceitaModel.Ingredientes}.");

    try
    {

        using var cxt = new CoreApiContext();
        Receita receita = cxt.Receitas.First(e => e.Id == ReceitaModel.Id);

        if (receita == null)
        {
            throw new Exception("Id invalido");
        }

        receita.Caloria = Convert.ToInt32(ConsultaCaloriasGPT(ReceitaModel.Ingredientes.Replace("\r\n", " ")));
        receita.Categoria = (CategoriaEnum)Enum.Parse(typeof(CategoriaEnum), chamaApi(ReceitaModel.Ingredientes));
        if (receita.Categoria != (CategoriaEnum)Enum.Parse(typeof(CategoriaEnum), "0"))
        {
            receita.Status = 1;

        }
        else
        {
            receita.Categoria = null;
            receita.Status = 0;
            Console.WriteLine("Não alterou !");
        }
        cxt.Entry(receita).State = EntityState.Modified;

        await cxt.SaveChangesAsync();
        Console.WriteLine("Salvou");

    }
    catch (Exception e)
    {
        Console.WriteLine(e);
    }
};


static string chamaApi(string ingredientes)
{
    int tokens = 10;
    string engine = "text-davinci-003";
    double temperature = 0.2;
    int topP = 1;
    int frequencyPenalty = 0;
    int presencePenalty = 0;


    var openAikey = "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";

    var apiCall = "https://api.openai.com/v1/engines/" + engine + "/completions";
    try
    {
        using (var httpClient = new HttpClient())
        {
            using (var request = new HttpRequestMessage(new HttpMethod("POST"), apiCall))
            {
                request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + openAikey);
                request.Content = new StringContent("{\n \"prompt\": \"" + "classifique a seguinte receita atraves destes ingredientes:" + ingredientes + ". Classifique em numero 1 para Bolos ,numero 2 para Carnes,numero 3 para Aves ,numero 4 para Peixes,numero 5 para Saladas,numero 6 para Sopas,numero 7 para Massas,numero 8 para Bebidas,numero 9 para Sobremesas ou numero 10 para Lanches. Responda somente o numero corresponde a classificação. Se nenhuma dessas classificações corresponder coloque o digito 0 ." + "\",\n  \"temperature\": " + temperature.ToString(CultureInfo.InvariantCulture) + ",\n  \"max_tokens\": " + tokens + ",\n   \"top_p\": " + topP + ",\n  \"frequency_penalty\": " + frequencyPenalty + ",\n  \"presence_penalty\": " + presencePenalty + "\n}");

                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                var response = httpClient.SendAsync(request).Result;
                var json = response.Content.ReadAsStringAsync().Result;

                dynamic dynObj = JsonConvert.DeserializeObject(json);

                if (dynObj != null)
                {
                    Console.WriteLine(dynObj.choices[0].text.ToString());
                    return dynObj.choices[0].text.ToString();
                }
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
    return null;
}
static string ConsultaCaloriasGPT(string ingredientes)
{
    int tokens = 10;
    string engine = "text-davinci-003";
    double temperature = 0.2;
    int topP = 1;
    int frequencyPenalty = 0;
    int presencePenalty = 0;


    var openAikey = "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";

    var apiCall = "https://api.openai.com/v1/engines/" + engine + "/completions";
    try
    {
        using (var httpClient = new HttpClient())
        {
            using (var request = new HttpRequestMessage(new HttpMethod("POST"), apiCall))
            {
                request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + openAikey);
                request.Content = new StringContent("{\n \"prompt\": \"" + "Informe a quantidade total de calorias aproximadas desses ingredientes: " + ingredientes + ". Responda somente os numeros correspondentes as calorias, para usar essa informação em um campo int do C#, o numero informado não deve conter ponto." + "\",\n  \"temperature\": " + temperature.ToString(CultureInfo.InvariantCulture) + ",\n  \"max_tokens\": " + tokens + ",\n   \"top_p\": " + topP + ",\n  \"frequency_penalty\": " + frequencyPenalty + ",\n  \"presence_penalty\": " + presencePenalty + "\n}");

                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                var response = httpClient.SendAsync(request).Result;
                var json = response.Content.ReadAsStringAsync().Result;

                dynamic dynObj = JsonConvert.DeserializeObject(json);

                if (dynObj != null)
                {
                    Console.WriteLine(dynObj.choices[0].text.ToString());
                    return dynObj.choices[0].text.ToString();
                }
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
    return null;
}

channel.BasicConsume(queue: "ClassificaCategoriaGPTQueue",
                        autoAck: true,
                        consumer: consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();