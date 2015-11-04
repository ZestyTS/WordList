using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WordList
{
    class Program
    {
        static void Main()
        {
            var directory = Environment.CurrentDirectory;
            File.Delete(directory + "/WordList.txt");
            var files = Directory.GetFiles(directory, "*.txt");
            Console.WriteLine("This program will read all files with the '.txt' extension in the directory of + " + directory + " which has " + files.Length + " files in it. If this is not okay please close this program immediately. Press Enter to start.");
            Console.ReadLine();
            var count = 1;
            var word = new List<string>();
            var duplicate = new List<int>();
            foreach (var file in files)
            {
                string all;
                using (var sr = new StreamReader(file))
                {
                    all = sr.ReadToEnd();
                }
                all = DecodeQuotedPrintables(all);
                all = new string(all.Where(c => !char.IsPunctuation(c)).ToArray());
                var temp = Regex.Split(all, @"\W+");

                foreach (var t in temp)
                {
                    if (word.Count == 0)
                    {
                        word.Add(t);
                        duplicate.Add(1);
                    }
                    else if (word.Contains(t))
                    {
                        duplicate[word.IndexOf(t)]++;
                    }
                    else
                    {
                        word.Add(t);
                        duplicate.Add(1);
                    }
                }

                Console.WriteLine("Currently on file " + count + " of " + files.Length);
                count++;
            }

            Console.WriteLine("Currently creating the WordList file please wait.");

            using (var newfile = new StreamWriter(directory + "/WordList.txt"))
            {
                for (var i = 0; i < word.Count; i++)
                    newfile.WriteLine(word[i] + " " + duplicate[i]);
            }
            Console.WriteLine("All done! Program can be closed now.");
            Console.ReadLine();
        }

        private static string DecodeQuotedPrintables(string input)
        {
            var enc = new UTF8Encoding();

            var occurences = new Regex(@"(=[0-9A-Z]{2}){1,}", RegexOptions.Multiline);
            var matches = occurences.Matches(input);

            foreach (Match match in matches)
            {
                try
                {
                    var b = new byte[match.Groups[0].Value.Length / 3];
                    for (var i = 0; i < match.Groups[0].Value.Length / 3; i++)
                    {
                        b[i] = byte.Parse(match.Groups[0].Value.Substring(i * 3 + 1, 2), NumberStyles.AllowHexSpecifier);
                    }
                    var hexChar = enc.GetChars(b);
                    input = input.Replace(match.Groups[0].Value, hexChar[0].ToString());
                }
                catch
                {
                    // ignored
                }
            }
            input = input.Replace("?=", "").Replace("=\r\n", "");

            return input;
        }

    }
}
