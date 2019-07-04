using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace AsyncExample.Api.Controllers
{
    public class ValuesController : ApiController
    {
        public string Get(int? id)
        {
            return "Hello World " + id;
        }
        
        public IResponseMessage Post(RequestMessage requestMessage)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                if (requestMessage == null)
                {
                    response.Message = "invalid request";
                    return response;
                }

                List<string> validRequestMessage = new List<string>()
                {
                    "Hello","Bye","Ping"
                };

                if (!validRequestMessage.Contains(requestMessage.Message))
                {
                    response.Message = "invalid request";
                    return response;
                }

                if (requestMessage.Message == "Hello")
                {
                    response.Message = "Hi";
                }

                if (requestMessage.Message == "Ping")
                {
                    response.Message = "Pong";
                }
            }
            catch (Exception exception)
            {
                response.Message = exception.ToString();
            }
            finally
            {
                //Log Request and/or Response to db
            }
            
            return response;
        }
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
}
