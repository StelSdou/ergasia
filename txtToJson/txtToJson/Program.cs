using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace txtToJson
{
    class jsonconvert
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public string History { get; set; }

        public jsonconvert(int id, string category, string title, string history)
        {
            Id = id;
            Category = category;
            Title = title;
            History = history;
        }
    }

    internal class Program
    {
        private static string path = "Data.json";
        static void Main(string[] args)
        {
            List<jsonconvert> list = new List<jsonconvert>();

            string filePath = Console.ReadLine();

            if (File.Exists(filePath))
            {
                string text = File.ReadAllText(filePath);
                string newText = text.Replace("\r", "").Replace("\n", "_");
                File.WriteAllText(filePath, newText);

                text = File.ReadAllText(filePath);
                newText = text.Replace(" ", "_").Replace("\"_", "\" ").Replace("_\"", " \"");
                File.WriteAllText(filePath, newText);

                int id = 0;
                string cat = "";
                string tit = "";
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string line = sr.ReadLine();

                    string[] words = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    if (words.Length != 0)
                        for (int i = 0; i < words.Length; i += 2)
                            if (words[i].ToLower() == "\"category\"")
                                cat = words[i + 1].Replace("_", " ");
                            else if (words[i].ToLower() == "\"title\"")
                                tit = words[i + 1].Replace("_", " ");
                            else if (words[i].ToLower() == "\"history\"")
                                list.Add(new jsonconvert(id++, cat, tit, words[i + 1].Replace("_", " ")));

                    Console.WriteLine(id);
                }
            }

            File.WriteAllText(path, JsonSerializer.Serialize(list));
            Console.ReadLine();
        }
    }
}
