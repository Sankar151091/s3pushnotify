using Amazon.S3;
using Amazon.SimpleNotificationService;
using app.PN.AWSHelper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
public class Program
{
    public static async Task<int> Main(string[] args)
    {

        //HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
        ////builder.Services.AddSingleton<IAmazonS3,AmazonS3Client>();
        ////builder.Services.AddSingleton<IAmazonSimpleNotificationService,AmazonSimpleNotificationServiceClient>();
        //builder.Build();
         
        PushNotificationHelper pn = new PushNotificationHelper(new AmazonS3Client(), new AmazonSimpleNotificationServiceClient());
        return await pn.SendNotification();

    }


}