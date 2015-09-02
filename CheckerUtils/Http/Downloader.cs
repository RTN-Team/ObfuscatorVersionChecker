using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CheckerUtils.Http
{
    public delegate void ResponseInfoDelegate(string statusDescr, string contentLength);
    public delegate void ProgressDelegate(int totalBytes, double pctComplete, double transferRate);
    public delegate void DoneDelegate(RequestState reqState);
    public class RequestState
    {
        public Uri FileURI { get; set; }
        public int BytesRead { get; set; }
        public long TotalBytes { get; set; }
        public double ProgIncrement { get; set; }
        public DateTime TransferStart {get; set;}
        public int BufferSize { get; private set; }
        public MemoryStream ResponseContent { get; set; }
        public byte[] BufferRead { get; set; }
        public HttpWebRequest Request { get; set; }
        public HttpWebResponse Response { get; set; }
        public Stream ResponseStream { get; set; }

        public RequestState()
        {
            BufferSize = 4096;
            BufferRead = new byte[BufferSize];
            ResponseContent = new MemoryStream();
            Request = null;
            ResponseStream = null;
        }
    }
    public class Downloader
    {
        public ResponseInfoDelegate ResponseHandler { get; set; }
        public ProgressDelegate ProgressHandler { get; set; }
        public DoneDelegate DoneHandler { get; set; }

        public void FileAsync(string url)
        {
            try
            {

                Uri fileURI = new Uri(url);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(fileURI);
                request.Method = "GET";
                request.Proxy = null;

                RequestState state = new RequestState();
                state.Request = request;
                state.FileURI = fileURI;
                state.TransferStart = DateTime.Now;

                IAsyncResult result = request.BeginGetResponse(new AsyncCallback(ResponseCallback), state);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void ResponseCallback(IAsyncResult result)
        {
            try
            {
                // Get and fill the RequestState
                RequestState state = (RequestState)result.AsyncState;
                HttpWebRequest request = (HttpWebRequest)state.Request;
                // End the Asynchronous response and get the actual resonse object
                state.Response = (HttpWebResponse)request.EndGetResponse(result);                
                state.TotalBytes = state.Response.ContentLength;
                
                // Get this info back to the GUI -- max # bytes, so we can do progress bar
                if (!string.IsNullOrWhiteSpace(state.Response.StatusDescription))
                    ResponseHandler(state.Response.StatusDescription, state.Response.ContentLength.ToString());

                Stream responseStream = state.Response.GetResponseStream();
                state.ResponseStream = responseStream;

                // Begin async reading of the contents
                IAsyncResult readResult = responseStream.BeginRead(state.BufferRead, 0, state.BufferSize, new AsyncCallback(ReadCallback), state);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                // Error handling
                RequestState state = (RequestState)result.AsyncState;
                if (state.Response != null)
                    state.Response.Close();
            }
        }

        private void ReadCallback(IAsyncResult result)
        {
            try
            {
                RequestState state = (RequestState)result.AsyncState;
                int bytesRead = state.ResponseStream.EndRead(result);
                if (bytesRead > 0)
                {
                    state.BytesRead += bytesRead;
                    double pctComplete = ((double)state.BytesRead / (double)state.TotalBytes) * 100.0f;
                    //just in case
                    pctComplete = (pctComplete > 100) ? 100 : pctComplete;

                    // Note: bytesRead/totalMS is in bytes/ms.  Convert to kb/sec.
                    TimeSpan totalTime = DateTime.Now - state.TransferStart;
                    double kbPerSec = (state.BytesRead * 1000.0f) / (totalTime.TotalMilliseconds * 1024.0f);

                    ProgressHandler(state.BytesRead, pctComplete, kbPerSec);

                    state.ResponseContent.Write(state.BufferRead, 0, bytesRead);
                    state.ResponseStream.BeginRead(state.BufferRead, 0, state.BufferSize, new AsyncCallback(ReadCallback), state);
                }
                else
                {
                    if (state.ResponseContent.Length > 0)
                    {
                        state.ResponseStream.Close();
                        state.Response.Close();
                        DoneHandler(state);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                // Error handling
                RequestState state = (RequestState)result.AsyncState;
                if (state.Response != null)
                    state.Response.Close();
            }
        }
    }
}
