using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Scire.ChatGPT
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer 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");
                var content = new StringContent("{\"prompt\":\"Hello, world!\",\"temperature\":0.7,\"max_tokens\":60,\"top_p\":1,\"frequency_penalty\":0,\"presence_penalty\":0}", Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://api.openai.com/v1/engines/ada/completions", content);
                var result = await response.Content.ReadAsStringAsync();
                Console.WriteLine(result);


                // response = await client.GetAsync("https://api.openai.com/v1/engines");
                //result = await response.Content.ReadAsStringAsync();
                //Console.WriteLine(result);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
