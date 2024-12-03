using System;
using System.IO;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace PManager.Cryptography.Config
{
    public class EncryptionSetup
    {
        public static void GenerateAndSaveKey(string path)
        {
            using (var aes = Aes.Create())
            {
                aes.GenerateKey();
                aes.GenerateIV();

                var config = new EncryptionConfig
                {
                    Key = aes.Key,
                    IV = aes.IV
                };

                var json = JsonConvert.SerializeObject(config);
                File.WriteAllText(path, json);
            }
        }

        public static EncryptionConfig LoadKey(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Configuration file not found: {filePath}");
            }

            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<EncryptionConfig>(json);
        }

        public static void SaveKey(string filePath, EncryptionConfig config)
        {
            var json = JsonConvert.SerializeObject(config);
            File.WriteAllText(filePath, json);
        }
    }
}
