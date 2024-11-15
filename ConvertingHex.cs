using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace RecodingWord
{
    internal class ConvertingHex
    {

        public string WordToHex(int prefix, int postfix, string word, string path)
        {
            StringBuilder stringBuilder = insertFix(prefix);
            string c = "";
            //Логика преобразования
            Dictionary<string, string> keyValuePairs = ParseJsonToDictionary(path);
            // Преобразуем слово посимвольно
            foreach (char letter in word)
            {
                string letterStr = letter.ToString(); // Преобразуем символ в строку

                // Проверяем, есть ли буква в словаре
                if (keyValuePairs.TryGetValue(letterStr, out string value))
                {
                    // Если найдено, добавляем значение к StringBuilder
                    stringBuilder.Append(value+" ");
                }
                else
                {
                   
                }
            }
            stringBuilder.Append(insertFix(postfix).ToString());
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
            return stringBuilder.ToString();
        }

        

        public StringBuilder insertFix(int value)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < value; i++) {

                stringBuilder.Append("0x00 ");
            }
            return stringBuilder;
        }

        // Метод для парсинга JSON-файла в словарь
        public Dictionary<string, string> ParseJsonToDictionary(string path)
        {
            // Проверяем, существует ли файл
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Файл не найден: {path}");
            }

            // Считываем содержимое JSON-файла с кодировкой ANSI
            string jsonStr = File.ReadAllText(path, Encoding.Default);

            // Парсим JSON в словарь и возвращаем результат
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonStr);
        }





    }
}
