// <copyright file="MainForm.cs" company="PUblicDomain.com">
//     CC0 1.0 Universal (CC0 1.0) - Public Domain Dedication
//     https://creativecommons.org/publicdomain/zero/1.0/legalcode
// </copyright>

namespace DropLauncher
{
    // Directoves
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;
    using PublicDomain;

    /// <summary>
    /// Description of MainForm.
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:DropLauncher.MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            // The InitializeComponent() call is required for Windows Forms designer support.
            InitializeComponent();

            // TODO Set topmost by saved settings
            this.TopMost = true;
        }

        /// <summary>
        /// Handles the new tool strip menu item click event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnNewToolStripMenuItemClick(object sender, EventArgs e)
        {
            // TODO Add code
        }

        /// <summary>
        /// Handles the exit tool strip menu item click event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnExitToolStripMenuItemClick(object sender, EventArgs e)
        {
            // Close program
            this.Close();
        }

        /// <summary>
        /// Handles the always on top tool strip menu item click event.
        /// </summary>
        //// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnAlwaysOnTopToolStripMenuItemClick(object sender, EventArgs e)
        {
            // Toggle checkbox
            this.alwaysOnTopToolStripMenuItem.Checked = !this.alwaysOnTopToolStripMenuItem.Checked;

            // Set topmost 
            this.TopMost = this.alwaysOnTopToolStripMenuItem.Checked;
        }

        /// <summary>
        /// Inputs the format tool strip menu item click.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void InputFormatToolStripMenuItemClick(object sender, EventArgs e)
        {
            // TODO Add code
        }

        /// <summary>
        /// Outputs the format tool strip menu item click.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void OutputFormatToolStripMenuItemClick(object sender, EventArgs e)
        {
            // TODO Add code
        }

        /// <summary>
        /// Handles the output mods tool strip menu item click event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnOutputModsToolStripMenuItemClick(object sender, EventArgs e)
        {
            // TODO Add codep
        }

        /// <summary>
        /// Handles the more releases public domain giftcom tool strip menu item click event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnMoreReleasesPublicDomainGiftcomToolStripMenuItemClick(object sender, EventArgs e)
        {
            // Open current website
            Process.Start("https://publicdomaingift.com");
        }

        /// <summary>
        /// Handles the original thread donation codercom tool strip menu item click event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnOriginalThreadDonationCodercomToolStripMenuItemClick(object sender, EventArgs e)
        {
            // IDEA Drop Zone javascript link extractor and launcher
            Process.Start("https://www.donationcoder.com/forum/index.php?topic=51227.0");
        }

        /// <summary>
        /// Handles the source code githubcom tool strip menu item click event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnSourceCodeGithubcomToolStripMenuItemClick(object sender, EventArgs e)
        {
            // Open GitHub repository
            Process.Start("https://github.com/publicdomain/drop-launcher");
        }

        /// <summary>
        /// Handles the open tool strip menu item click event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnOpenToolStripMenuItemClick(object sender, EventArgs e)
        {
            // TODO Add code
        }

        /// <summary>
        /// Handles the save tool strip menu item click event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnSaveToolStripMenuItemClick(object sender, EventArgs e)
        {
            // TODO Add code
        }

        /// <summary>
        /// Remembers the location tool strip menu item click event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnRememberLocationToolStripMenuItemClick(object sender, EventArgs e)
        {
            // TODO Add code
        }

        /// <summary>
        /// Processes the link.
        /// </summary>
        /// <returns>The processed link.</returns>
        /// <param name="link">Input link.</param>
        private string ProcessLink(string link)
        {
            // TODO Check for a javascript link [Make it generic for v0.1.0+]
            if (link.StartsWith("javascript:SetCfpId(", StringComparison.InvariantCultureIgnoreCase))
            {
                // Generate link according to magician62/John's requirements
                link = $"https://www.ancestry.co.uk/family-tree/tree/12345678/family/familyview?cfpid={new Regex(@"\d+").Match(link).Value}";
            }

            // Return validated link
            if (this.ValidateUri(link))
            {
                // Return valid link
                return link;
            }

            // Default return
            return string.Empty;
        }

        /// <summary>
        /// Handles the main form drag drop event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnMainFormDragDrop(object sender, DragEventArgs e)
        {
            // List of lines to test
            List<string> lineList = new List<string>();

            // Check for Text
            if (e.Data.GetDataPresent(DataFormats.Text))
            {
                // TODO Append line(s) [A single line should do; multiple lines just to be sure]
                lineList.AddRange(this.ReadAllLines(e.Data.GetData(DataFormats.StringFormat).ToString()));
            }
            // Check for file drop
            else if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Get drop files data
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                // Iterate files
                foreach (string file in files)
                {
                    // Add lines 
                    lineList.AddRange(this.ReadAllLines(file));
                }
            }

            // Declare extracted link list
            List<string> linkList = new List<string>();

            // Validate lines
            foreach (string line in lineList)
            {
                // Declare link
                string link;

                // Check for .url/.website line
                if (line.StartsWith("URL=", StringComparison.InvariantCultureIgnoreCase))
                {
                    // Extract link
                    link = line.Split(new char[] { '=' })[1];
                }
                else
                {
                    // TODO Set link [Can perform other preliminay checks prior to URI testing]
                    link = line;
                }

                // Add valid links
                if (!string.IsNullOrEmpty(this.ProcessLink(link)))
                {
                    // Add to link list
                    linkList.Add(link);
                }
            }

            // Iterate links
            foreach (string link in linkList)
            {
                // Open replaced/prpocessed link
                Process.Start(link);
            }
        }

        /// <summary>
        /// Handles the main form drag enter event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnMainFormDragEnter(object sender, DragEventArgs e)
        {
            // Check for possible link
            {
                if (e.Data.GetDataPresent(DataFormats.Text) || e.Data.GetDataPresent("UniformResourceLocator"))
                {
                    // Set link effect
                    e.Effect = DragDropEffects.Link;
                }
                else
                {
                    // No effect
                    e.Effect = DragDropEffects.None;
                }
            }
        }

        /// <summary>
        /// Validates the URI.
        /// </summary>
        /// <returns><c>true</c>, if URI was validated, <c>false</c> otherwise.</returns>
        /// <param name="possibleUri">Possible URI.</param>
        private bool ValidateUri(string possibleUri)
        {
            // Return TryCreate result
            return Uri.TryCreate(possibleUri, UriKind.Absolute, out var uri) &&
                            (uri.Scheme == Uri.UriSchemeHttps ||
                            uri.Scheme == Uri.UriSchemeHttp ||
                            uri.Scheme == Uri.UriSchemeFtp ||
                            uri.Scheme == Uri.UriSchemeMailto ||
                            uri.Scheme == Uri.UriSchemeFile ||
                            uri.Scheme == Uri.UriSchemeNews ||
                            uri.Scheme == Uri.UriSchemeNntp ||
                            uri.Scheme == Uri.UriSchemeGopher ||
                            uri.Scheme == Uri.UriSchemeNetTcp ||
                            uri.Scheme == Uri.UriSchemeNetPipe);
        }

        /// <summary>
        /// Reads all lines.
        /// </summary>
        /// <returns>Read lines.</returns>
        /// <param name="inputString">Input string.</param>
        private IEnumerable<string> ReadAllLines(string inputString)
        {
            // Check input string
            if (string.IsNullOrEmpty(inputString))
            {
                // Halt
                yield break;
            }

            // Use string reader
            using (StringReader stringReader = new StringReader(inputString))
            {
                // Declare current line
                string line;

                // Process lines
                while ((line = stringReader.ReadLine()) is object)
                {
                    // Assign
                    yield return line;
                }
            }
        }

        /// <summary>
        /// Handles the main form form closing event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnMainFormFormClosing(object sender, FormClosingEventArgs e)
        {
            // TODO Add code
        }
    }
}
