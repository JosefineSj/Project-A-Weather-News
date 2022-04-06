
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json; //Requires nuget package System.Net.Http.Json
using System.Threading.Tasks;

using Assignment_A2_02.Models;
using Assignment_A2_02.ModelsSampleData;
namespace Assignment_A2_02.Services
{
    public class NewsService
    {

        HttpClient httpClient = new HttpClient();
        readonly string apiKey = "d7ee1b401fe34f1ab46845840212f712";

        public event EventHandler<string> NewsAvailable;

        public virtual void OnNewsAvailable(string e)
        {
            NewsAvailable?.Invoke(this, e);
        }

        public async Task<News> GetNewsAsync(NewsCategory category)
        {
           
#if UseNewsApiSample 
            NewsApiData nd = await NewsApiSampleData.GetNewsApiSampleAsync(category);


#else
            //https://newsapi.org/docs/endpoints/top-headlines
            var uri = $"https://newsapi.org/v2/top-headlines?country=se&category={category}&apiKey={apiKey}";

           // Your code here to get live data

            HttpResponseMessage response = await httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            NewsApiData nd = await response.Content.ReadFromJsonAsync<NewsApiData>();

            News news = new();

            try
            {
                news.Category = category;
                news.Articles = nd.Articles.Select(item => new NewsItem
                {
                    DateTime = item.PublishedAt,
                    Title = item.Title,
                    Description = item.Description,
                    Url = item.Url,
                    UrlToImage = item.UrlToImage

                }).ToList();
            }
            catch (Exception)
            {

                throw;
            }
            OnNewsAvailable($"News in category is available: {category}");
#endif

            return news;
        }

      
    }
}
