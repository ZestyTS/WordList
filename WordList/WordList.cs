using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class frmWordList : Form
    {
        public List<string> files = new List<string>();
        public frmWordList()
        {
            InitializeComponent();
        }

        //Emails have "@" and "." in them
        private void btnEmail_Click(object sender, EventArgs e)
        {
            if (!txtWhiteList.Text.Contains("@"))
                txtWhiteList.Text += @" @";

            if (!txtWhiteList.Text.Contains("."))
                txtWhiteList.Text += @" .";
        }

        private void btnHythen_Click(object sender, EventArgs e)
        {
            if (!txtWhiteList.Text.Contains("-"))
                txtWhiteList.Text += @" -";
        }

        private void btnTime_Click(object sender, EventArgs e)
        {
            if (!txtWhiteList.Text.Contains(":"))
                txtWhiteList.Text += @" :";
        }

        private void frmWordList_Load(object sender, EventArgs e)
        {
            lbFiles.AllowDrop = true;
            lbFiles.DragDrop += lbDragDrop;
            lbFiles.DragEnter += lbDragEnter;
        }
        private static void lbDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void lbDragDrop(object sender, DragEventArgs e)
        {
            //adds all the files into the List<string> files
            files.AddRange((string[]) e.Data.GetData(DataFormats.FileDrop));

            //checks if a file is a duplicate before adding it to lbFiles
            foreach (var file in files.Where(file => !lbFiles.Items.Contains(file)))
                lbFiles.Items.Add(file);
            lblFiles.Text = lbFiles.Items.Count + @" Files to Use";
        }

        private void btnWordList_Click(object sender, EventArgs e)
        {
            if (lbFiles.Items.Count <= 0)
            {
                MessageBox.Show(@"Please add files before clicking this button");
                return;
            }
            //in case a duplicate snuck in, I'm secretly removing it here
            files = files.Distinct().ToList();

            //stopping buttons that would break the program
            ButtonSwap(true);

            var punctuation = txtWhiteList.Text;
            var count = 1;

            //stores all the words
            var word = new List<string>();

            //stores how many time a word appears in the same index as List<string> word
            var duplicate = new List<int>();

            foreach (var file in files)
            {
                string all;
                using (var sr = new StreamReader(file))
                {
                    all = sr.ReadToEnd().ToLower();
                }
                //making everything in the file human readable for string manipulation
                all = DecodeQuotedPrintables(all);

                //if nothing is put into the whitelist textbox, then I'm just removing all forms of non digits and non letters
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
                                //making sure the "." isn't the last part of the word and only part of an email address
                                if ((counthelper != all.Length && counthelper < all.Length) &&
                                    !string.IsNullOrEmpty(all[all.IndexOf(sb.ToString()) + 2].ToString()))
                                {
                                    sb.Append(c);
                                }
                                else if (punctuation.Contains("@"))
                                {
                                    var reverse = sb.ToString().ToArray();
                                    Array.Reverse(reverse);

                                    //checking if the @ sign is part of a single word, aka an email address
                                    if (reverse.ToString().IndexOf("@") < reverse.ToString().IndexOf(" "))
                                        sb.Append(c);
                                }

                            }
                            else
                                sb.Append(c);

                        }
                        //I've seen these in a lot of email headers, so I'm just manually removing them
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
                    //removing all the punctuation
                    temp[i] = temp[i].TrimEnd('.');
                    temp[i] = temp[i].TrimEnd('?');
                    temp[i] = temp[i].TrimEnd(',');
                    temp[i] = temp[i].TrimEnd('!');

                    //making sure I'm only removing < or > if they aren't part of the word
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

                    //after removing all that stuff, I might have an empty string, so I just skip to next iteration
                    if (temp[i] == "")
                        continue;

                    //first word
                    if (word.Count == 0)
                    {
                        word.Add(temp[i]);
                        duplicate.Add(1);
                    }
                    //duplicate++
                    else if (word.Contains(temp[i]))
                    {
                        duplicate[word.IndexOf(temp[i])]++;
                    }
                    //new word found
                    else
                    {
                        word.Add(temp[i]);
                        duplicate.Add(1);
                    }
                }

                //showing the user what file it's on so they know it's still running
                lbOutput.Items.Add("Currently on file " + count + " of " + files.Count);
                count++;
            }

            lbOutput.Items.Add("Currently creating the WordList file please wait.");

            //pops up a file dialog so the user can save
            var sfd = new SaveFileDialog
            {
                Filter = @"Text File|*.txt",
                Title = @"Save the WordList File"
            };
            sfd.ShowDialog();

            string location;
            //in case the user doesn't care, I save the file at the location of the program
            if (string.IsNullOrEmpty(sfd.FileName))
                location = Environment.CurrentDirectory + "/WordList.txt";
            else
                location = sfd.FileName;

            using (var newfile = new StreamWriter(location))
            {
                for (var i = 0; i < word.Count; i++)
                    newfile.WriteLine(word[i] + " " + duplicate[i]);
            }

            //resetting everything
            ButtonSwap(false);
            lbOutput.Items.Clear();
            lbFiles.Items.Clear();
            lblFiles.Text = @"Files to Use";
            txtWhiteList.Text = "";
            files.Clear();
        }

        //Emails are encoded, so I'm decoding them here
        //It's pattern for encoded things begin with an equal sign
        private static string DecodeQuotedPrintables(string input)
        {
            var enc = new UTF8Encoding();

            //checks if there are words or digits with an equal sign before them
            var occurences = new Regex(@"(=[0-9A-Z]{2}){1,}", RegexOptions.Multiline);
            var matches = occurences.Matches(input);

            foreach (Match match in matches)
            {
                //tries to convert the encoded pattern to something human readable
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

        private void ButtonSwap(bool start)
        {
            //Switching buttons on and off to not break program
            btnWordList.Enabled = !start;
            lbOutput.Enabled = !start;
            lbOutput.Visible = start;
            lblOutput.Visible = start;
        }

        private void btnCurrentDir_Click(object sender, EventArgs e)
        {
            //grabs all the .txt files in the program's current directory
            files.AddRange(Directory.GetFiles(Environment.CurrentDirectory, "*.txt"));

            //checks if a file is a duplicate, if not adds it to lbFiles
            foreach (var file in files.Where(file => !lbFiles.Items.Contains(file)))
                lbFiles.Items.Add(file);
        }
    }
}
