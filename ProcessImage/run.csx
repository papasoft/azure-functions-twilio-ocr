#r "Twilio.Api"
#r "Newtonsoft.Json"

using System;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Twilio;

public static void Run(string myQueueItem, out SMSMessage message, TraceWriter log)
{
    log.Info($"C# Queue trigger function processed: {myQueueItem}");
    dynamic data = JsonConvert.DeserializeObject(myQueueItem);

    var msgBody = "";
    var ocrtext = "";

    if (data.imageUrl != null)
    {

        var imagedata = JsonConvert.SerializeObject(new {url = data.imageUrl});
        log.Info($"data: {imagedata}");

        var serviceUrl = "https://eastus2.api.cognitive.microsoft.com/vision/v1.0/ocr?language=unk&detectOrientation=true";

        using(var client = new WebClient())
        {
            string apiKey = System.Environment.GetEnvironmentVariable("OcpApiKey", EnvironmentVariableTarget.Process);

            client.Headers.Add("Ocp-Apim-Subscription-Key", apiKey);
            client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            var ocrResponse = client.UploadString(serviceUrl, "POST", imagedata);
            
            dynamic ocr = JsonConvert.DeserializeObject(ocrResponse);

            foreach (var r in ocr.regions)
            {
                foreach (var l in r.lines)
                {
                    foreach (var w in l.words)
                    {
                        ocrtext += w.text + " ";
                    }
                    ocrtext += "\n";
                }
            }
        }
    }

    if (ocrtext.Length > 0)
    {
        msgBody = ocrtext;
    }
    else
    {
        msgBody = "Not able to read text in image, try another";
    }

    message = new SMSMessage();

    message.To = data.replyTo;
    message.Body = msgBody;
}