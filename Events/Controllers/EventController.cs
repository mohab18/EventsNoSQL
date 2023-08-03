using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Amazon.CloudWatchLogs;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Events.Models;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Sinks.AwsCloudWatch;

namespace Events.Controllers
{
    public class EventController
    {
        
        private readonly IAmazonDynamoDB _dynamoDBClient= new AmazonDynamoDBClient();
       
        public EventController()
        {
            
        }
       


       

       
    }
}
