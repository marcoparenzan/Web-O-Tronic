using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Newtonsoft.Json.Linq;

// https://github.com/Azure-Samples/signalr-service-quickstart-serverless-chat
namespace WebOTronic.SignalRFunctionApp
{
    public static class SignalRHub
    {
        [FunctionName("GetSignalRInfo")]
        public static SignalRConnectionInfo GetSignalRInfo(
            [HttpTrigger(AuthorizationLevel.Anonymous)]HttpRequest req,
            [SignalRConnectionInfo(HubName = "hub", ConnectionStringSetting ="AzureSignalR", UserId="{headers.x-ms-client-principal-id}")]
            SignalRConnectionInfo connectionInfo)
        {
            return connectionInfo;
        }

        [FunctionName("PlayWith")]
        public async static Task PlayWith(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")]string messageJson,
            [SignalR(HubName = "hub", ConnectionStringSetting ="AzureSignalR")]
            IAsyncCollector<SignalRMessage> signalRMessages)
        {
            var message = JsonConvert.DeserializeObject<JObject>(messageJson);
            var profile = new
            {
                leftPaddle = message.Value<string>("leftPaddle"),
                rightPaddle = message.Value<string>("rightPaddle")
            };
            await signalRMessages.AddAsync(
                new SignalRMessage
                { 
                    // the message will only be sent to these user IDs
                    UserId = profile.leftPaddle,
                    Target = "begin",
                    Arguments = new object[] { profile.leftPaddle, profile.rightPaddle }
                });
            await signalRMessages.AddAsync(
                new SignalRMessage
                {
                    UserId = profile.rightPaddle,
                    Target = "begin",
                    Arguments = new object[] { profile.leftPaddle, profile.rightPaddle }
                });
        }

        [FunctionName("NotifyLeftPaddle")]
        public async static Task NotifyLeftPaddle(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")]string argsJson,
            [SignalR(HubName = "hub", ConnectionStringSetting ="AzureSignalR")]
            IAsyncCollector<SignalRMessage> signalRMessages)
        {
            var args = JsonConvert.DeserializeObject<JObject>(argsJson);
            await signalRMessages.AddAsync(
                new SignalRMessage
                {
                    // the message will only be sent to these user IDs
                    UserId = args.Value<string>("userId"),
                    Target = "leftpaddleupdate",
                    Arguments = new object[] { args }
                });
        }

        [FunctionName("NotifyRightPaddle")]
        public async static Task NotifyRightPaddle(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")]string argsJson,
            [SignalR(HubName = "hub", ConnectionStringSetting ="AzureSignalR")]
            IAsyncCollector<SignalRMessage> signalRMessages)
        {
            var args = JsonConvert.DeserializeObject<JObject>(argsJson);
            await signalRMessages.AddAsync(
                new SignalRMessage
                {
                    // the message will only be sent to these user IDs
                    UserId = args.Value<string>("userId"),
                    Target = "rightpaddleupdate",
                    Arguments = new object[] { args }
                });
        }

        [FunctionName("NotifyBall")]
        public async static Task NotifyBall(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")]string argsJson,
            [SignalR(HubName = "hub", ConnectionStringSetting ="AzureSignalR")]
            IAsyncCollector<SignalRMessage> signalRMessages)
        {
            var args = JsonConvert.DeserializeObject<JObject>(argsJson);
            await signalRMessages.AddAsync(
                new SignalRMessage
                {
                    // the message will only be sent to these user IDs
                    UserId = args.Value<string>("userId"),
                    Target = "ballupdate",
                    Arguments = new object[] { args }
                });
        }
    }
}
