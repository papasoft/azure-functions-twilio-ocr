# Azure Functions Demo with Twilio and Image OCR
Azure Function app to process an image sent from Twilio text message and using Microsoft Cognitive Services image OCR to reply with text in the image

You'll need a Twilio Account and phone number that supports SMS and MMS. Set the Post Webhook for Inbound Messages from your phone number to the AddMessageToQueue function url.

AddMessageToQueue receives the inbound message from Twilio and checks to see if there are any images attached. If so, the first image url is added to an Azure Storage Queue along with the phone number to send reply.

ProcessImage reads from the storage queue and sends the image url to Microsoft Cognitive Services OCR recognition (Project Oxford) and receives JSON response with any text found on the image. Read the doc for more info on what you can do with this API.

The text found in the image is formatted into lines and added to a new response text message sent back to the phone number that sent the image.

This project was a quick demo for a dotNetMiami meetup.

This is NOT a valid Visual Studio function app project. It is a download of the wwwroot files for an Azure Functions App project created through the Azure Portal. You maybe do something similar by create an app and functions in the UI and copy/paste this code in.

Rick Tuttle
rick@papasoft.com