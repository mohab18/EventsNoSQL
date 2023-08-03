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
       


        public async Task<List<Event>> RetrieveEvents()
        {
            List<Event> events = new List<Event>();
         
            var request = new ScanRequest
            {
                TableName = "Events"
            };
            var response = await _dynamoDBClient.ScanAsync(request);
            foreach (var item in response.Items)
            {
                Event Events = new Event
                {
                    Id = item["id"].S,
                    Name = item["Name"].S,
                    Date = DateTime.Parse(item["Date"].S),
                    Location = item["Location"].S
                };

                events.Add(Events);
            }
            return events;
        }
        public async Task<Event> RetrieveEvent(String id)
        {
            
            var request = new GetItemRequest
            {
                TableName = "Events",
                Key = new Dictionary<string, AttributeValue>
                 {
                             { "id", new AttributeValue { S=id } }
                 }
            };
            var response = await _dynamoDBClient.GetItemAsync(request);
            Event Events = new Event
            {
                Id = response.Item["id"].S,
                Name = response.Item["Name"].S,
                Date = DateTime.Parse(response.Item["Date"].S),
                Location = response.Item["Location"].S
            };


            return Events;
        }
        public async Task<string> AddEventAsync(Event _event)
        {
      
            var request = new PutItemRequest
            {
                TableName = "Events",
                Item = new Dictionary<string, AttributeValue>
                 {
                             { "id", new AttributeValue { S=_event.Id } },
                             { "Name", new AttributeValue {S=_event.Name} },
                             { "Location", new AttributeValue {S=_event.Location} },
                             { "Date", new AttributeValue {S=_event.Date.ToString()} },

                 }
            };
            await _dynamoDBClient.PutItemAsync(request);
            return "Event Added Successfully";
        }
        public async Task<string> UpdateEvent( Event _event)
        {

        
            if (_event == null)
            {
               
                return "Event is null";
            }

            var request = new GetItemRequest
            {
                TableName = "Events",
                Key = new Dictionary<string, AttributeValue>
                 {
                             { "id", new AttributeValue { S=_event.Id } }
                 }
            };
            var response = await _dynamoDBClient.GetItemAsync(request);
            Event ret = new Event
            {
                Id = response.Item["id"].S,
                Name = response.Item["Name"].S,
                Date = DateTime.Parse(response.Item["Date"].S),
                Location = response.Item["Location"].S
            };
            if (ret != null)
            {
                var request1 = new UpdateItemRequest
                {
                    TableName = "Events",
                    Key = new Dictionary<string, AttributeValue>
                    {
                             { "id", new AttributeValue { S=_event.Id } }
                    },
                    ExpressionAttributeNames = new Dictionary<string, string>
                          {
                            { "#attr1", "Location" },
                            { "#attr2", "Date" }
                          },
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                         {
                            { ":val1", new AttributeValue { S = _event.Location } },
                            { ":val2", new AttributeValue { S = _event.Date.ToString() } }
                         },
                    UpdateExpression = "SET #attr1 = :val1, #attr2 = :val2"
                };

                await _dynamoDBClient.UpdateItemAsync(request1);
                return "Event Updated Successfully";
            }
           
            return "Id is not found";

        }
        public async Task<string> DeleteEventAsync(String id)
        {
         
            var request = new GetItemRequest
            {
                TableName = "Events",
                Key = new Dictionary<string, AttributeValue>
                 {
                             { "id", new AttributeValue { S=id } }
                 }
            };
            var response = await _dynamoDBClient.GetItemAsync(request);
            Event ret = new Event
            {
                Id = response.Item["id"].S,
                Name = response.Item["Name"].S,
                Date = DateTime.Parse(response.Item["Date"].S),
                Location = response.Item["Location"].S
            };
            if (ret != null)
            {
                var request1 = new DeleteItemRequest
                {
                    TableName = "Events",
                    Key = new Dictionary<string, AttributeValue>
                 {
                             { "id", new AttributeValue { S=id } }
                 }
                };
                await _dynamoDBClient.DeleteItemAsync(request1);
                return "Event Deleted Successfully";
            }
          
            return "Id is not found";
        }

       
    }
}
