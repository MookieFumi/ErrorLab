using System;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ErrorLab.WebUI.Infrastructure
{
    /// <summary>
    /// ASP.NET MVC and Json.NET
    /// </summary>
    /// <remarks>http://james.newtonking.com/archive/2008/10/16/asp-net-mvc-and-json-net</remarks>
    public class JsonNetResult : ActionResult
    {
        public JsonNetResult()
        {
            SerializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            Data = "OK";
            StatusCode = 200;
        }

        public JsonNetResult(object data)
            : this()
        {
            Data = data;
        }

        public JsonNetResult(object data, int statusCode)
            : this(data)
        {
            StatusCode = statusCode;
        }

        public JsonNetResult(object data, int statusCode, string statusDescription)
            : this(data, statusCode)
        {
            StatusDescription = statusDescription;
        }

        public Encoding ContentEncoding { get; set; }
        public string ContentType { get; set; }
        public object Data { get; set; }
        public Formatting Formatting { get; set; }
        public JsonSerializerSettings SerializerSettings { get; set; }
        public int StatusCode { get; set; }
        public string StatusDescription { get; set; }

        public static JsonNetResult CreateInternalServerError(string statusDescription)
        {
            return new JsonNetResult(null, 500, statusDescription);
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var response = context.HttpContext.Response;

            response.StatusCode = StatusCode;

            if (response.StatusCode == 500)
            {
                response.TrySkipIisCustomErrors = true;
            }

            if (!string.IsNullOrWhiteSpace(StatusDescription))
            {
                response.StatusDescription = StatusDescription;
            }

            response.ContentType = !string.IsNullOrEmpty(ContentType)
                ? ContentType
                : "application/json";

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            if (Data != null)
            {
                var writer = new JsonTextWriter(response.Output)
                {
                    Formatting = Formatting
                };
                var serializer = JsonSerializer.Create(SerializerSettings);
                serializer.Serialize(writer, Data);

                writer.Flush();
            }
        }
    }
}