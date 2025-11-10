using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsCompetitionTask3.Utilities
{
    public static class JsonHelper
    {
        public static List<T> ReadJsonList<T>(string path)
        {
            //string fullPath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);
            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<List<T>>(json)!;
        }
    }
}
