using System;
using Twitter;

namespace ConsoleApplication1
{
    public class Program
    {
        static void Main(string[] args)
        {
            var stream = new TwitterSteam
            {
                OAuthConsumerKey = "<OAuth Consumer Key>",
                OAuthConsumerSecret = "<OAuth Consumer Secret>"
            };

            var tweets = stream.GetTweets("twitterUserName", 10).Result;

            foreach (var tweet in tweets)
            {
                Console.WriteLine(tweet + "\n");
            }
            Console.ReadKey();
        }
    }
}
