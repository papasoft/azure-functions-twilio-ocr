#r "System.Runtime"
#r "Newtonsoft.Json"

using System.Net;
using System.Text;
using Twilio.TwiML;
using Newtonsoft.Json;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, IAsyncCollector<string> outputQueueItem, TraceWriter log)
{
    var data = await req.Content.ReadAsStringAsync();
    var formValues = data.Split('&')
        .Select(value => value.Split('='))
        .ToDictionary(pair => Uri.UnescapeDataString(pair[0]).Replace("+", " "), 
                      pair => Uri.UnescapeDataString(pair[1]).Replace("+", " "));

    var reply = "";

    if (formValues.ContainsKey("MediaUrl0"))
    {
        var qdata = JsonConvert.SerializeObject(new { replyTo = formValues["From"], imageUrl = formValues["MediaUrl0"]});
        await outputQueueItem.AddAsync(qdata);
        log.Info($"adding to queue: {qdata}");
        reply = "Got it. Processing OCR...";
    }
    else
    {
        reply = "Please send an image";
    }

    var response = new MessagingResponse().Message(reply);
    var twiml = response.ToString();
    twiml = twiml.Replace("utf-16", "utf-8");

    return new HttpResponseMessage
    {
        Content = new StringContent(twiml, Encoding.UTF8, "application/xml")
    };

}