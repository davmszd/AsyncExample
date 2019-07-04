using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AsyncExample
{
    class Program
    {
        static void Main(string[] args)
        {
            CallApi();

            var message = Console.ReadLine();
            if (message != "end")
            {
                CallApi();
            }
        }

        private async static void CallApi()
        {
            IRequestMessage requestMessage = new RequestMessage()
            {
                Message = "Hello"
            };

            ResponseMessage responseMessage =  await new Client().SendAsync<ResponseMessage>(requestMessage);
        }
    }

    public interface IClient
    {
        Task<TResponseMessage> SendAsync<TResponseMessage>(IRequestMessage requestMessage) where TResponseMessage : IResponseMessage;
    }

    public interface IRequestMessage
    {
        string Message { get; set; }
    }

    public class RequestMessage : IRequestMessage
    {
        public string Message { get; set; }
    }

    public interface IResponseMessage
    {
        string Message { get; set; }
    }

    public class ResponseMessage : IResponseMessage
    {
        public string Message { get; set; }
    }

    public class Client : IClient
    {
        public Task<TResponseMessage> SendAsync<TResponseMessage>(IRequestMessage requestMessage) where TResponseMessage : IResponseMessage
        {
            string apiUrl = "http://localhost:19408/api/Values";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
            using (var client = new System.Net.Http.HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var w = client.PostAsJsonAsync(apiUrl,requestMessage);
                w.Wait();
                HttpResponseMessage response = w.Result;
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsAsync<TResponseMessage>();
                    result.Wait();
                    return result;
                }
                return default(Task<TResponseMessage>);
            }
        }
    }
}
