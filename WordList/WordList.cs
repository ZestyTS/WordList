using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WordList
{
    public partial class FrmWordList : Form
    {
        private readonly BackgroundWorker _bw = new BackgroundWorker();

        //stores all the words
        private readonly List<string> _word = new List<string>();

        //stores how many time a _word appears in the same index as List<string> _word
        private readonly List<int> _duplicate = new List<int>();

        public List<string> Files = new List<string>();
        public FrmWordList()
        {
            InitializeComponent();

            _bw.WorkerReportsProgress = true;
            _bw.WorkerSupportsCancellation = true;
            _bw.DoWork += btnWordList_Work;
            _bw.ProgressChanged += ProgressUpdate;
            _bw.RunWorkerCompleted += WorkerCompleted;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown || e.CloseReason == CloseReason.ApplicationExitCall || e.CloseReason == CloseReason.TaskManagerClosing)
                return;

            // Confirm user wants to close
            switch (MessageBox.Show(this, @"Are you sure you want to close?", @"Closing", MessageBoxButtons.YesNo))
            {
                case DialogResult.No:
                    e.Cancel = true;
                    break;
                case DialogResult.Yes:
                    Application.Exit();
                    break;
            }
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
            lbFiles.DragDrop += LbDragDrop;
            lbFiles.DragEnter += LbDragEnter;
        }
        private static void LbDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if(e.Error != null)
            {
                lblOutput.Text = @"Error: " + e.Error.Message + @": " + e.Error.InnerException;
            }
            else
            {
                lblOutput.Text = @"DONE!";
                ShowSaveDialog();
            }
        }

        private void LbDragDrop(object sender, DragEventArgs e)
        {
            //adds all the Files into the List<string> Files
            Files.AddRange((string[]) e.Data.GetData(DataFormats.FileDrop));

            //checks if a file is a _duplicate before adding it to lbFiles
            foreach (var file in Files.Where(file => !lbFiles.Items.Contains(file)))
                lbFiles.Items.Add(file);
            lblFiles.Text = lbFiles.Items.Count + @" Files to Use";
        }

        private void btnWordList_Click(object sender, EventArgs e)
        {
            if (_bw.IsBusy)
                return;

            //stopping buttons that would break the program
            ButtonSwap(true);
            _bw.RunWorkerAsync();
        }

        private void btnWordList_Work(object sender, EventArgs e)
        {
            try {
                if (lbFiles.Items.Count <= 0)
                {
                    MessageBox.Show(@"Please add Files before clicking this button");
                    return;
                }

                var worker = sender as BackgroundWorker;

                //in case a _duplicate snuck in, I'm secretly removing it here
                Files = Files.Distinct().ToList();

                var punctuation = txtWhiteList.Text;
                var count = 1;

                foreach (var file in Files)
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
                                    //making sure the "." isn't the last part of the _word and only part of an email address
                                    if ((counthelper != all.Length && counthelper < all.Length) &&
                                        !string.IsNullOrEmpty(all[all.IndexOf(sb.ToString()) + 2].ToString()))
                                    {
                                        sb.Append(c);
                                    }
                                    else if (punctuation.Contains("@"))
                                    {
                                        var reverse = sb.ToString().ToArray();
                                        Array.Reverse(reverse);

                                        //checking if the @ sign is part of a single _word, aka an email address
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

                        //making sure I'm only removing < or > if they aren't part of the _word
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

                        //first _word
                        if (_word.Count == 0)
                        {
                            _word.Add(temp[i]);
                            _duplicate.Add(1);
                        }
                        //_duplicate++
                        else if (_word.Contains(temp[i]))
                        {
                            _duplicate[_word.IndexOf(temp[i])]++;
                        }
                        //new _word found
                        else
                        {
                            _word.Add(temp[i]);
                            _duplicate.Add(1);
                        }
                    }

                    //showing the user what file it's on so they know it's still running
                    count++;

                    worker?.ReportProgress(count*100/Files.Count);
                }

                //here was saving, plus those 2 lists were at the top before the first for each

                //resetting everything
                //ButtonSwap(false);
                //lbFiles.Items.Clear();
                //lblFiles.Text = @"Files to Use";
                //txtWhiteList.Text = "";
                Files.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + @": " + ex.InnerException);
            }
        }

        public void ShowSaveDialog()
        {
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
                for (var i = 0; i < _word.Count; i++)
                    newfile.WriteLine(_word[i] + " " + _duplicate[i]);
            }
        }

        private void ProgressUpdate(object sender, ProgressChangedEventArgs e)
        {
            lblOutput.Text = e.ProgressPercentage + @"%";
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
            lblOutput.Visible = start;
        }

        private void btnCurrentDir_Click(object sender, EventArgs e)
        {
            //grabs all the .txt Files in the program's current directory
            Files.AddRange(Directory.GetFiles(Environment.CurrentDirectory, "*.txt"));

            //checks if a file is a _duplicate, if not adds it to lbFiles
            foreach (var file in Files.Where(file => !lbFiles.Items.Contains(file)))
                lbFiles.Items.Add(file);
        }
    }
}
