using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JenkinsFreeStyle
{
    class Program
    {
        static int times = 1;
        private static string PowerBIPushURL = "https://api.powerbi.cn/beta/99d86385-9b07-4221-9a3a-facb274186b5/datasets/73c63bf3-e463-4d2f-8436-34b1c0f0cd00/rows?key=EzQCY3%2B%2FKkgrX1wroPWulvcFTRcH1HFNJHnYUsz8TNEnN5MREMth5NN0HQwC4iPij4Yd1QGg3ery7XYYqxgr1A%3D%3D";
        static void Main(string[] args)
        {
            PostToPowerBI(PowerBIPushURL);
        }
        static void PostToPowerBI(string PostUri)
        {
            while (times < 50)
            {
                try
                {
                    // Declare values that we're about to send
                    String currentTime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");  //2017-11-03T06:23:35.521Z
                    Random r = new Random();
                    int currentValue = r.Next(0, 100);

                    // Send POST request to the push URL
                    // Uses the WebRequest sample code as documented here: https://msdn.microsoft.com/en-us/library/debx8sh9(v=vs.110).aspx
                    WebRequest request = WebRequest.Create(PostUri);
                    request.Method = "POST";
                    string postData = String.Format("[{{ \"time\": \"{0}\", \"latancy\": {1}}}]", currentTime, currentValue);
                    Console.WriteLine(String.Format("Making POST request with data: {0}", postData));

                    // Prepare request for sending
                    byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                    request.ContentLength = byteArray.Length;

                    // Get the request stream.
                    Stream dataStream = request.GetRequestStream();

                    // Write the data to the request stream.
                    dataStream.Write(byteArray, 0, byteArray.Length);

                    // Close the Stream object.
                    dataStream.Close();

                    // Get the response.
                    WebResponse response = request.GetResponse();

                    // Display the status.
                    Console.WriteLine(String.Format("Service response: {0}", ((HttpWebResponse)response).StatusCode));

                    // Get the stream containing content returned by the server.
                    dataStream = response.GetResponseStream();

                    // Open the stream using a StreamReader for easy access.
                    StreamReader reader = new StreamReader(dataStream);

                    // Read the content.
                    string responseFromServer = reader.ReadToEnd();

                    // Display the content.
                    Console.WriteLine(responseFromServer);

                    // Clean up the streams.
                    reader.Close();
                    dataStream.Close();
                    response.Close();

                    times++;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

            }
        }
    }
}
