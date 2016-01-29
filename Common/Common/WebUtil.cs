using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace Common
{
	public class WebUtil
	{

		// Send http request		
		// method is POST or GET
		public static string SendHttpRequest(string url, string method, string apiKeyHeader = null, string dataAsJson = null)
		{
			ArgumentUtil.IsNotWhiteSpaceOrNull(url, "url", "SendHttpRequest");
			ArgumentUtil.IsNotWhiteSpaceOrNull(method, "method", "SendHttpRequest", "Method must be GET or POST");

			HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
			request.Method = method;
			request.Accept = "application/json";
			request.ContentType = "application/json";


			// Some sites require api key as part of the header of the request instead of in the url
			// in the example urls above, api.pipelinedeals.com has the api key in the url, whereas api.myintervals.com doesn't
			if (apiKeyHeader != null)
				request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(apiKeyHeader)));


			// If method is post, add data to the request object
			if (method == "POST")
			{
				byte[] postData = Encoding.UTF8.GetBytes(dataAsJson);
				request.ContentLength = postData.Length;

				using (Stream stream = request.GetRequestStream())
				{
					stream.Write(postData, 0, postData.Length);
				}
			}


			// Send request
			string responseText = null;
			try
			{
				using (WebResponse response = request.GetResponse())
				using (StreamReader reader = new StreamReader(response.GetResponseStream()))
				{
					responseText = HttpUtility.HtmlDecode(reader.ReadToEnd());  // HtmlDecode converts http symbols into text, such as &amp; into &
				}
			}

			// return any error messages from server if there's a problem
			catch (WebException wex)
			{
				using (Stream stream = wex.Response.GetResponseStream())
				using (StreamReader reader = new StreamReader(stream))
				{
					responseText = HttpUtility.HtmlDecode(reader.ReadToEnd());
				}
			}

			return responseText;
		}




		// Checks the host is an available web address.  Fails if connection not achieved within 3 seconds		
		public bool CheckInternetConnection(string url = "")
		{
			if (url == null || url == "")
				url = "https://www.google.com.au";

			try
			{
				WebRequest request = WebRequest.Create(url);
				request.Timeout = 3000;

				using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
					return response.StatusCode == HttpStatusCode.OK ? true : false;
			}
			catch
			{
				return false; // ignore WebException - Timeout
			}			
		}
	}
}
