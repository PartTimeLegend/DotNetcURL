using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Net;
using System.Security;
using System.Text;

namespace DotNetcURL
{
    public class Curl
    {
        /// <summary>
        /// Simulates the *nix command cURL as used in PHP, Perl and Terminal.
        /// </summary>
        /// <param name="jsonData">The body to be sent to the endpoint as JSON.</param>
        /// <param name="endPoint">The Endpoint Service to send the cURL request to.</param>
        /// <param name="basicAuth">If basic authentication is being used, add the base64 encoded string for the header.</param>
        /// <returns>String containing the result of the invokation.</returns>
        /// <exception cref="ObjectDisposedException"><see cref="P:System.IO.StreamWriter.AutoFlush" /> is true or the <see cref="T:System.IO.StreamWriter" /> buffer is full, and current writer is closed. </exception>
        /// <exception cref="NotSupportedException"><see cref="P:System.IO.StreamWriter.AutoFlush" /> is true or the <see cref="T:System.IO.StreamWriter" /> buffer is full, and the contents of the buffer cannot be written to the underlying fixed size stream because the <see cref="T:System.IO.StreamWriter" /> is at the end the stream. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        /// <exception cref="EncoderFallbackException">The current encoding does not support displaying half of a Unicode surrogate pair. </exception>
        /// <exception cref="ArgumentNullException"><paramref name="oldValue" /> is null. </exception>
        /// <exception cref="ArgumentException"><paramref name="oldValue" /> is the empty string (""). </exception>
        /// <exception cref="ArgumentOutOfRangeException">The length of <paramref name="value" /> is greater than 65535. </exception>
        /// <exception cref="InvalidOperationException">The request has already been sent. </exception>
        /// <exception cref="ProtocolViolationException">The <see cref="P:System.Net.HttpWebRequest.Method" /> property is GET or HEAD.-or- <see cref="P:System.Net.HttpWebRequest.KeepAlive" /> is true, <see cref="P:System.Net.HttpWebRequest.AllowWriteStreamBuffering" /> is false, <see cref="P:System.Net.HttpWebRequest.ContentLength" /> is -1, <see cref="P:System.Net.HttpWebRequest.SendChunked" /> is false, and <see cref="P:System.Net.HttpWebRequest.Method" /> is POST or PUT. </exception>
        /// <exception cref="WebException"><see cref="M:System.Net.HttpWebRequest.Abort" /> was previously called.-or- The time-out period for the request expired.-or- An error occurred while processing the request. </exception>
        /// <exception cref="UriFormatException">The URI specified in <paramref name="requestUriString" /> is not a valid URI. </exception>
        /// <exception cref="SecurityException">The caller does not have permission to connect to the requested URI or a URI that the request is redirected to. </exception>
        /// <exception cref="OutOfMemoryException">There is insufficient memory to allocate a buffer for the returned string. </exception>
        public static string PostJsonDataToApi(string jsonData, string endPoint, string basicAuth = null)
        {
            Contract.Requires(jsonData != null);
            Contract.Requires(endPoint != null);
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(endPoint);
            httpWebRequest.ReadWriteTimeout = 100000; 
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Accept = "*/*";
            httpWebRequest.Method = "POST";
            if (basicAuth != null)
                httpWebRequest.Headers.Add("Authorization", "Basic " + basicAuth);
            using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                jsonData = jsonData.Replace("\n", "").Replace("\r", "");
                streamWriter.Write(jsonData);
                streamWriter.Flush();
                streamWriter.Close();
            }

            HttpWebResponse resp = (HttpWebResponse)httpWebRequest.GetResponse();
            string respStr = new StreamReader(resp.GetResponseStream()).ReadToEnd();
            return respStr; 
        }
    }
}
