using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Twitter
{
    public class TwitterSteam
    {
        public string OAuthConsumerSecret { get; set; }        
        public string OAuthConsumerKey { get; set; }

        public async Task<string> GetAccessToken()
        {           
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.twitter.com/oauth2/<token>");            
            var userInfo = Convert.ToBase64String(new UTF8Encoding().GetBytes(OAuthConsumerKey + ":" + OAuthConsumerSecret));
            
            request.Headers.Add("Authorization", "Basic " + userInfo);
            request.Content = new StringContent("grant_type=<client_credentials>", Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = await httpClient.SendAsync(request);
            var responseJsonString = await response.Content.ReadAsStringAsync();
            
            dynamic responseObject = JsonConvert.DeserializeObject<object>(responseJsonString);
            return  responseObject["access_token"];            
        }

        public async Task<IEnumerable<string>> GetTweets(string userName,int count, string accessToken = null)
        {
            if (accessToken == null)
            {
                accessToken = await GetAccessToken();   
            }
            
            var httpClient = new HttpClient();
            var timelineRequest = new HttpRequestMessage(HttpMethod.Get, string.Format("https://api.twitter.com/1.1/statuses/user_timeline.json?count={0}&screen_name={1}&trim_user=1&exclude_replies=1", count, userName));
            timelineRequest.Headers.Add("Authorization", "Bearer " + accessToken);                        
            
            var response = await httpClient.SendAsync(timelineRequest);

            dynamic responseJson = JsonConvert.DeserializeObject<object>(await response.Content.ReadAsStringAsync());
            
            var tweets = (responseJson as IEnumerable<dynamic>);

            if (tweets == null)
            {
                return null;
            }
            return tweets.Select(x => (string)(x["text"].ToString()));                        
        }

    }
}