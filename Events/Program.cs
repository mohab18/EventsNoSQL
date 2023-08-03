using Amazon.APIGateway;
using Amazon.APIGateway.Model;
using Amazon.Auth.AccessControlPolicy.ActionIdentifiers;
using Amazon.CloudWatchLogs;
using Amazon.DynamoDBv2;
using Amazon.Lambda;
using Events.Controllers;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Sinks.AwsCloudWatch;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddAWSService<IAmazonDynamoDB>();
builder.Services.AddAWSService<IAmazonLambda>();
builder.Services.AddAWSService<IAmazonAPIGateway>();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Events", Version = "v1" });
});
var app = builder.Build();
// Configure the HTTP request pipeline.
app.UseExceptionHandler("/Home/Error");
app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Events");
});
app.Run();

