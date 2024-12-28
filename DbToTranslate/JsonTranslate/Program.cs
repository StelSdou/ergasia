using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace JsonTranslate
{
    class Data
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public string History { get; set; }

        public Data(int id, string category, string title, string history)
        {
            Id = id;
            Category = category;
            Title = title;
            History = history;
        }
    }

    //https://api.mymemory.translated.net/get?q=Hello&langpair=en|el
    internal class Program
    {
        static string defSourceLang = "en";
        
        static async Task Main(string[] args)
        {
            List<Data> list = new List<Data>();
            var sql = "SELECT * FROM 'Tales'";

            try
            {
                using (var con = new SQLiteConnection("Data Source=DataBase.db"))
                {
                    con.Open();
                    using (var com = new SQLiteCommand(sql, con))
                    {
                        using (var r = com.ExecuteReader())
                        {
                            while (r.Read())
                            {
                                list.Add(new Data(r.GetInt32(0), r.GetString(1), r.GetString(2), r.GetString(3)));
                            }
                        }
                        com.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error " + ex.Message);
            }

            foreach (var data in list) 
            { 
                string t = await getTrans(defSourceLang, "el", data.Title);
                Console.WriteLine(data.Id + ")" + t);
            }
            
            Console.ReadLine();
        }

        static async Task<string> getTrans(string sourceLang, string targetLang, string text)
        {
            string url = "https://api.mymemory.translated.net/get";
            //string sourceLang = "en"; // Αγγλικά
            //string targetLang = "el"; // gr

            // Δημιουργία HTTP client
            using (HttpClient client = new HttpClient())
            {
                // Δημιουργία πλήρους URL με τα query parameters
                string requestUrl = $"{url}?q={Uri.EscapeDataString(text)}&langpair={sourceLang}|{targetLang}";

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
                    return translatedText;
                }
                catch (Exception e)
                {
                    // Διαχείριση σφαλμάτων
                    return $"Σφάλμα: {e.Message}";
                }
            }
        }
    }
}
