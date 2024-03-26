using Amazon.S3;
using Amazon.SimpleNotificationService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Text;

namespace app.PN.AWSHelper
{
    public class PushNotificationHelper
    {
        
        
        private IAmazonS3 _s3client { get; set; }
        private IAmazonSimpleNotificationService _snsclient { get; set; }
        public PushNotificationHelper(AmazonS3Client s3Client , AmazonSimpleNotificationServiceClient snsclient)
        {
            this._s3client = s3Client;
            this._snsclient = snsclient;
          
        }

        public async Task<int> SendNotification(string bucketname= "dataprosesslmdsqssns",
            string filename= "payload.csv")
        { 

            var envVariable = Environment.GetEnvironmentVariable("myVariableName");
            byte[] filedata;
            if (!string.IsNullOrEmpty(bucketname))
            {
                MemoryStream ms = null; 
                using (var response = await _s3client.GetObjectAsync(bucketname, filename))
                {
                    if (response.HttpStatusCode == HttpStatusCode.OK)
                    {
                        using (ms = new MemoryStream())
                        {
                            await response.ResponseStream.CopyToAsync(ms);
                        }
                    }
                }
                if (ms is null || ms.ToArray().Length < 1)
                    throw new FileNotFoundException(string.Format("The document '{0}' is not found", filename));
                filedata = ms.ToArray();


                string utfString = Encoding.UTF8.GetString(filedata, 0, filedata.Length);
                if (!string.IsNullOrEmpty(utfString))
                {
                    Console.Out.WriteLineAsync(utfString);
                    return (int)HttpStatusCode.OK;
                }
                return (int)HttpStatusCode.NotFound;
            }
            return 1;
        }
    }
}
