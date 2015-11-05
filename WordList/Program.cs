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
            Console.WriteLine("This program removes any character that isn't a letter or a digit. Please list any non letter or digit you would like to appear. Please note that adding things like periods or commas may lead to unwanted results. If there is nothing you want whitelisted please just press Enter to continue.");
            var punctuation = Console.ReadLine();
            var count = 1;
            var word = new List<string>();
            var duplicate = new List<int>();
            foreach (var file in files)
            {
                string all;
                using (var sr = new StreamReader(file))
                {
                    all = sr.ReadToEnd().ToLower();
                }
                all = DecodeQuotedPrintables(all);

                if (string.IsNullOrEmpty(punctuation))
                    all = new string(all.Where(c => !char.IsPunctuation(c)).ToArray()).Replace("<", "").Replace(">", "");

                else
                {
                    var sb = new StringBuilder();
                    var counthelper = 0;
                    foreach (var c in all)
                    {
                        counthelper++;
                        if (punctuation.Contains(c))
                        {
                            if (punctuation.Contains("."))
                            {
                                if ((counthelper != all.Length && counthelper < all.Length) &&
                                    !string.IsNullOrEmpty(all[all.IndexOf(sb.ToString()) + 2].ToString()))
                                {
                                    sb.Append(c);
                                }
                                else if (punctuation.Contains("@"))
                                {
                                    var reverse = sb.ToString().ToArray();
                                    Array.Reverse(reverse);
                                    if (reverse.ToString().IndexOf("@") < reverse.ToString().IndexOf(" "))
                                        sb.Append(c);
                                }

                            }
                            else
                                sb.Append(c);

                        }
                        else if (c.ToString() == "<" || c.ToString() == ">")
                        { }

                        else if (!char.IsPunctuation(c))
                            sb.Append(c);

                    }
                    all = sb.ToString();
                }
                var temp = Regex.Split(all, @"\s+");

                for (var i = 0; i < temp.Length; i++)
                {
                    temp[i] = temp[i].TrimEnd('.');
                    if (temp[i].Contains("<"))
                    {
                        if (!temp[i].Contains(">"))
                            temp[i] = temp[i].Replace("<", "");
                    }
                    else if (temp[i].Contains(">"))
                    {
                        if (!temp[i].Contains("<"))
                            temp[i] = temp[i].Replace(">", "");
                    }
                    if (temp[i] == "")
                        continue;

                    if (word.Count == 0)
                    {
                        word.Add(temp[i]);
                        duplicate.Add(1);
                    }
                    else if (word.Contains(temp[i]))
                    {
                        duplicate[word.IndexOf(temp[i])]++;
                    }
                    else
                    {
                        word.Add(temp[i]);
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
