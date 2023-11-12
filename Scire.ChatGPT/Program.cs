using System;
using System.IO;
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
                // 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
                // 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

                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer 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");

                // Carregar o conteúdo do arquivo envio.json
                string arquivoPath = Path.Combine("C:\\Users\\jsra\\source\\repos\\Estudos\\Scire.ChatGPT\\", "envio.json");
                string jsonContent = File.ReadAllText(arquivoPath);

                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://api.openai.com/v1/completions", content);
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
