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
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;
    using System.Xml.Serialization;
    using PublicDomain;

    /// <summary>
    /// Description of MainForm.
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// Gets or sets the associated icon.
        /// </summary>
        /// <value>The associated icon.</value>
        private Icon associatedIcon = null;

        /// <summary>
        /// The launch count.
        /// </summary>
        private int launchCount = 0;

        /// <summary>
        /// The settings data.
        /// </summary>
        private SettingsData settingsData = null;

        /// <summary>
        /// The settings data path.
        /// </summary>
        private string settingsDataPath = $"{Application.ProductName}-SettingsData.txt";

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DropLauncher.MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            // The InitializeComponent() call is required for Windows Forms designer support.
            InitializeComponent();

            /* Set icons */

            // Set associated icon from exe file
            this.associatedIcon = Icon.ExtractAssociatedIcon(typeof(MainForm).GetTypeInfo().Assembly.Location);

            // Set public domain weekly tool strip menu item image
            this.moreReleasesPublicDomainGiftcomToolStripMenuItem.Image = this.associatedIcon.ToBitmap();

            /* Settings data */

            // Check for settings file
            if (!File.Exists(this.settingsDataPath))
            {
                // Create new settings file
                this.SaveSettingsFile(this.settingsDataPath, new SettingsData());
            }

            // Load settings from disk
            this.settingsData = this.LoadSettingsFile(this.settingsDataPath);

            // Topmost
            this.alwaysOnTopToolStripMenuItem.Checked = this.settingsData.AlwaysOnTop;
            this.TopMost = this.settingsData.AlwaysOnTop;

            // Location
            this.rememberLocationToolStripMenuItem.Checked = this.settingsData.RememberLocation;
            this.Location = this.settingsData.Location;
        }

        /// <summary>
        /// Handles the new tool strip menu item click event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnNewToolStripMenuItemClick(object sender, EventArgs e)
        {
            // Reset launch count
            this.launchCount = 0;

            // Update label
            this.countToolStripStatusLabel.Text = this.launchCount.ToString();
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
            // Set regex
            var regex = new Regex(this.settingsData.LinkRegex);

            // Set match
            var match = regex.Match(link);

            // Test match result
            if (!match.Success)
            {
                // Halt flow 
                return string.Empty;
            }

            // Set link
            link = match.Result(this.settingsData.LaunchRegex);

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
                // Raise lanuch count
                this.launchCount++;

                // Update label
                this.countToolStripStatusLabel.Text = this.launchCount.ToString();

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
            // Set settings data
            this.settingsData.AlwaysOnTop = this.alwaysOnTopToolStripMenuItem.Checked;
            this.settingsData.RememberLocation = this.rememberLocationToolStripMenuItem.Checked;
            this.settingsData.Location = this.Location;

            // Save settings data to disk
            this.SaveSettingsFile(this.settingsDataPath, this.settingsData);
        }

        /// <summary>
        /// Handles the about tool strip menu item click event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnAboutToolStripMenuItemClick(object sender, EventArgs e)
        {
            // Set license text
            var licenseText = $"CC0 1.0 Universal (CC0 1.0) - Public Domain Dedication{Environment.NewLine}" +
                $"https://creativecommons.org/publicdomain/zero/1.0/legalcode{Environment.NewLine}{Environment.NewLine}" +
                $"Libraries and icons have separate licenses.{Environment.NewLine}{Environment.NewLine}" +
                $"Rocket icon by dawnydawny - Pixabay License{Environment.NewLine}" +
                $"https://pixabay.com/vectors/rocket-vector-space-launch-2442125/{Environment.NewLine}{Environment.NewLine}" +
                $"Patreon icon used according to published brand guidelines{Environment.NewLine}" +
                $"https://www.patreon.com/brand{Environment.NewLine}{Environment.NewLine}" +
                $"GitHub mark icon used according to published logos and usage guidelines{Environment.NewLine}" +
                $"https://github.com/logos{Environment.NewLine}{Environment.NewLine}" +
                $"DonationCoder icon used with permission{Environment.NewLine}" +
                $"https://www.donationcoder.com/forum/index.php?topic=48718{Environment.NewLine}{Environment.NewLine}" +
                $"PublicDomain icon is based on the following source images:{Environment.NewLine}{Environment.NewLine}" +
                $"Bitcoin by GDJ - Pixabay License{Environment.NewLine}" +
                $"https://pixabay.com/vectors/bitcoin-digital-currency-4130319/{Environment.NewLine}{Environment.NewLine}" +
                $"Letter P by ArtsyBee - Pixabay License{Environment.NewLine}" +
                $"https://pixabay.com/illustrations/p-glamour-gold-lights-2790632/{Environment.NewLine}{Environment.NewLine}" +
                $"Letter D by ArtsyBee - Pixabay License{Environment.NewLine}" +
                $"https://pixabay.com/illustrations/d-glamour-gold-lights-2790573/{Environment.NewLine}{Environment.NewLine}";

            // Set title
            string programTitle = typeof(MainForm).GetTypeInfo().Assembly.GetCustomAttribute<AssemblyTitleAttribute>().Title;

            // Set version for generating semantic version 
            Version version = typeof(MainForm).GetTypeInfo().Assembly.GetName().Version;

            // Set about form
            var aboutForm = new AboutForm(
                $"About {programTitle}",
                $"{programTitle} {version.Major}.{version.Minor}.{version.Build}",
                $"Made for: magician62{Environment.NewLine}DonationCoder.com{Environment.NewLine}Day #99, Week #14 @ April 09, 2021",
                licenseText,
                this.Icon.ToBitmap())
            {
                // Set about form icon
                Icon = this.associatedIcon,

                // Set always on top
                TopMost = this.TopMost
            };

            // Show about form
            aboutForm.ShowDialog();
        }

        /// <summary>
        /// Handles the link regex tool strip menu item click event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnLinkRegexToolStripMenuItemClick(object sender, EventArgs e)
        {
            // TODO Add code
        }

        /// <summary>
        /// Handles the launch regex tool strip menu item click event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnLaunchRegexToolStripMenuItemClick(object sender, EventArgs e)
        {
            // TODO Add code
        }

        /// <summary>
        /// Loads the settings file.
        /// </summary>
        /// <returns>The settings file.</returns>
        /// <param name="settingsFilePath">Settings file path.</param>
        private SettingsData LoadSettingsFile(string settingsFilePath)
        {
            // Use file stream
            using (FileStream fileStream = File.OpenRead(settingsFilePath))
            {
                // Set xml serialzer
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(SettingsData));

                // Return populated settings data
                return xmlSerializer.Deserialize(fileStream) as SettingsData;
            }
        }

        /// <summary>
        /// Saves the settings file.
        /// </summary>
        /// <param name="settingsFilePath">Settings file path.</param>
        /// <param name="settingsDataParam">Settings data parameter.</param>
        private void SaveSettingsFile(string settingsFilePath, SettingsData settingsDataParam)
        {
            try
            {
                // Use stream writer
                using (StreamWriter streamWriter = new StreamWriter(settingsFilePath, false))
                {
                    // Set xml serialzer
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(SettingsData));

                    // Serialize settings data
                    xmlSerializer.Serialize(streamWriter, settingsDataParam);
                }
            }
            catch (Exception exception)
            {
                // Advise user
                MessageBox.Show($"Error saving settings file.{Environment.NewLine}{Environment.NewLine}Message:{Environment.NewLine}{exception.Message}", "File error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
