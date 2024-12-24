using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JsonTranslate
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //https://api.mymemory.translated.net/get?q=Hello&langpair=en|el
            // Η βάση URL της API
            string url = "https://api.mymemory.translated.net/get";
            string sourceLang = "en"; // Αγγλικά
            string targetLang = "el"; // gr

            // Παράμετροι για την κλήση της API (π.χ. κείμενο προς μετάφραση και γλώσσες)
            string textToTranslate = "It all started with Pandora box. You probably already know the story of Pandora, but you might not know what happened next. Pandora's box caused more problems!";

            // Δημιουργία HTTP client
            using (HttpClient client = new HttpClient())
            {
                // Δημιουργία πλήρους URL με τα query parameters
                string requestUrl = $"{url}?q={Uri.EscapeDataString(textToTranslate)}&langpair={sourceLang}|{targetLang}";

                try
                {
                    // Κλήση της API
                    HttpResponseMessage response = await client.GetAsync(requestUrl);

                    // Βεβαιώσου ότι η κλήση πέτυχε
                    response.EnsureSuccessStatusCode();

                    // Ανάγνωση του περιεχομένου της απάντησης
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Ανάλυση της JSON απάντησης και εξαγωγή της μετάφρασης
                    JObject jsonResponse = JObject.Parse(responseBody);
                    string translatedText = jsonResponse["responseData"]["translatedText"].ToString();

                    // Εμφάνιση μόνο της μετάφρασης
                    Console.WriteLine("Μετάφραση: " + translatedText);
                }
                catch (Exception e)
                {
                    // Διαχείριση σφαλμάτων
                    Console.WriteLine($"Σφάλμα: {e.Message}");
                }
            }
            Console.ReadLine();
        } 
    }
}
