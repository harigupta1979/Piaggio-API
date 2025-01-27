using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Healper
{
    public class APICall
    {
        public static Object GetDataAPI(Object obj,string BaseUri,string Uri)
        {
            string ErrorMessage = string.Empty;
            Object ResponseObject = new Object();
            try
            {
                try
                {
                    HttpClient httpClient = new HttpClient();
                    using (httpClient)
                    {
                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        httpClient.BaseAddress = new Uri(BaseUri);
                        httpClient.DefaultRequestHeaders.Accept.Clear();
                        httpClient.DefaultRequestHeaders.Accept.Add(
                            new MediaTypeWithQualityHeaderValue("application/json"));
                        var requestUrl = httpClient.BaseAddress + Uri;
                        var postTask = httpClient.GetAsync(requestUrl);
                        postTask.Wait();
                        var readTask = postTask.Result.Content.ReadAsStringAsync();
                        readTask.Wait();
                        var outPut = readTask.Result;
                        if (postTask.Result.StatusCode != System.Net.HttpStatusCode.OK)
                        {
                            ErrorMessage = postTask.Result.ReasonPhrase + " : " + outPut;
                        }
                        else
                        {
                            ResponseObject = JsonConvert.DeserializeObject<Object>(outPut);
                        }
                    }
                }
                catch (Exception e)
                {
                }
            }
            catch (Exception e)
            {

            }
            return ResponseObject;
        }

        public static Object PostDataAPI(Object obj, string BaseUri, string Uri)
        {
            string ErrorMessage = string.Empty;
            Object ResponseObject = new Object();
            try
            {
                try
                {
                    HttpClient httpClient = new HttpClient();
                    using (httpClient)
                    {
                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        httpClient.BaseAddress = new Uri(BaseUri);
                        var myContent = JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                        httpClient.DefaultRequestHeaders.Accept.Clear();
                        httpClient.DefaultRequestHeaders.Accept.Add(
                            new MediaTypeWithQualityHeaderValue("application/json"));
                        var requestUrl = httpClient.BaseAddress.ToString();
                        var httpContent = new StringContent(myContent, Encoding.UTF8, "application/json");
                        var postTask = httpClient.PostAsync(requestUrl, httpContent);
                        postTask.Wait();
                        var readTask = postTask.Result.Content.ReadAsStringAsync();
                        readTask.Wait();
                        var outPut = readTask.Result;
                        if (postTask.Result.StatusCode != System.Net.HttpStatusCode.OK)
                        {
                            ErrorMessage = postTask.Result.ReasonPhrase + " : " + outPut;
                        }
                        else
                        {
                            ResponseObject = JsonConvert.DeserializeObject<Object>(outPut);
                        }
                    }
                }
                catch (Exception e)
                {
                }
            }
            catch (Exception ex)
            {
            }
            return ResponseObject;
        }
    }
}
