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

        private void Form2_Load(object sender, EventArgs e)
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
            files.AddRange((string[]) e.Data.GetData(DataFormats.FileDrop));
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
            files = files.Distinct().ToList();
            ButtonSwap(true);
            var punctuation = txtWhiteList.Text;
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
                    temp[i] = temp[i].TrimEnd('?');
                    temp[i] = temp[i].TrimEnd(',');
                    temp[i] = temp[i].TrimEnd('!');
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

                lbOutput.Items.Add("Currently on file " + count + " of " + files.Count);
                count++;
            }

            lbOutput.Items.Add("Currently creating the WordList file please wait.");
            var sfd = new SaveFileDialog
            {
                Filter = @"Text File|*.txt",
                Title = @"Save the WordList File"
            };
            sfd.ShowDialog();

            string location;
            if (string.IsNullOrEmpty(sfd.FileName))
                location = Environment.CurrentDirectory + "/WordList.txt";
            else
                location = sfd.FileName;

            using (var newfile = new StreamWriter(location))
            {
                for (var i = 0; i < word.Count; i++)
                    newfile.WriteLine(word[i] + " " + duplicate[i]);
            }
            ButtonSwap(false);
            lbOutput.Items.Clear();
            lbFiles.Items.Clear();
            lblFiles.Text = @"Files to Use";
            txtWhiteList.Text = "";
            files.Clear();
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

        private void ButtonSwap(bool start)
        {
            btnWordList.Enabled = !start;
            lbOutput.Enabled = !start;
            lbOutput.Visible = start;
            lblOutput.Visible = start;
        }

        private void btnCurrentDir_Click(object sender, EventArgs e)
        {
            files.AddRange(Directory.GetFiles(Environment.CurrentDirectory, "*.txt"));
            foreach (var file in files.Where(file => !lbFiles.Items.Contains(file)))
                lbFiles.Items.Add(file);
        }
    }
}
