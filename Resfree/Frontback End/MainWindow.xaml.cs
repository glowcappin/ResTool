using Resfree.Frontback_End;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Resfree
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string resultX = null; // define results for GameUserSettings
        string resultY = null;
        string fullscreenMode = null;
        string windowfullscreenMode = null;
        string windowedMode = null;
        string fpsRate = null;
        string existsAlr = "";

        public MainWindow()
        {
            InitializeComponent();
            Console.Write("Found path...reading");
            using (WebClient client = new WebClient())
            {
                string vers = client.DownloadString("https://pastebin.com/raw/eY4DPhMf");
                string needsUpdate = client.DownloadString("https://pastebin.com/BGV23FmY");
                if (needsUpdate == "no") // check if ResFree needs an update
                {
                    Title = "Restool " + vers;
                }
                else
                {
                    System.Diagnostics.Process.Start(needsUpdate); // if so, open the link in the pastebin
                }
            }
            
        }

        private void addResolution_Click(object sender, RoutedEventArgs e)
        {
            string localFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData); // get LocalAppData
            string absolutePath = (System.IO.Path.Combine(localFolder, "FortniteGame\\Saved\\Config\\WindowsClient\\GameUserSettings.ini")); // get GameUserSettings

            resolutionXFunc(); // call func for finding X res (func for cleaner code)
            resolutionYFunc(); // call func for finding Y res
            fpsCheck(); // call func for finding FPS limit

            File.SetAttributes(absolutePath, FileAttributes.Normal); // remove read-only attributes, if applicable

            Console.WriteLine("Resolution has been found and written to memory, continuing.");

            string gameUSx = File.ReadAllText(absolutePath); // read lines in GameUserSettings
            gameUSx = gameUSx.Replace(resultX, "ResolutionSizeX=" + resWidth.Text); // replace current ResolutionSizeX with desired.
            File.WriteAllText(absolutePath, gameUSx); // write to GameUserSettings

            Thread.Sleep(1000); // wait, incase of incomplete save times.

            string gameUSY = File.ReadAllText(absolutePath); // read lines in GameUserSettings
            gameUSY = gameUSY.Replace(resultY, "ResolutionSizeY=" + resHeight.Text); // replace current ResolutionSizeY with desired.
            File.WriteAllText(absolutePath, gameUSY); // write to GameUserSettings

            Thread.Sleep(1000); // wait, incase of incomplete save times.

            string gameFPS = File.ReadAllText(absolutePath); // read lines in GameUserSettings
            gameFPS = gameFPS.Replace(fpsRate, "FrameRateLimit=" + fpsLimit.Text + ".000000"); // replace current FPS with desired.
            File.WriteAllText(absolutePath, gameFPS); // write to GameUserSettings

            Thread.Sleep(1000); // wait, incase of incomplete save times.

            if (isFullScreen.IsChecked == true) // check if fullscreen is desired.
            {
                fullscreenCheck(); // call func for finding current fullscreen mode.
                if (existsAlr == "yes")
                {
                    string gameFS = File.ReadAllText(absolutePath); // read lines in GameUserSettings
                    gameFS = gameFS.Replace(fullscreenMode, "FullscreenMode=" + "0"); // replace current fullscreenmode with desired.
                    File.WriteAllText(absolutePath, gameFS);
                }
                else
                {
                    lineChanger("FullscreenMode=0", absolutePath, 4); // add fullscreen mode to line 4 GameUserSettings 
                    existsAlr = "non";
                    Console.WriteLine("FullscreenMode added in fullscreenCheck func, continuing...");
                }
            }
            else if (isWindowFull.IsChecked == true)
            {
                windowFullscreenCheck();
                if (existsAlr == "yes")
                {
                    string gameWFS = File.ReadAllText(absolutePath); // read lines in GameUserSettings
                    gameWFS = gameWFS.Replace(windowfullscreenMode, "FullscreenMode=" + "1");
                    File.WriteAllText(absolutePath, gameWFS);
                }
                else
                {
                    lineChanger("FullscreenMode=1", absolutePath, 4); // add fullscreen mode to line 4 GameUserSettings 
                    existsAlr = "non";
                    Console.WriteLine("FullscreenMode added in fullscreenCheck func, continuing...");
                }
            }
            else if (isWindowed.IsChecked == true)
            {
                windowedCheck();
                if (existsAlr == "yes")
                {
                    string gameWM = File.ReadAllText(absolutePath); // read lines in GameUserSettings
                    gameWM = gameWM.Replace(windowedMode, "FullscreenMode=" + "2");
                    File.WriteAllText(absolutePath, gameWM);
                }
                else
                {
                    lineChanger("FullscreenMode=2", absolutePath, 4); // add fullscreen mode to line 4 GameUserSettings 
                    existsAlr = "non";
                    Console.WriteLine("FullscreenMode added in fullscreenCheck func, continuing...");
                }
            }

            if (isReadOnly.IsChecked == true) // check if read-only is desired.
            {
                File.SetAttributes(absolutePath, FileAttributes.ReadOnly);
            }
            else
            {
                File.SetAttributes(absolutePath, FileAttributes.Normal);
            }
            Console.WriteLine("Desired resolution added to GameUserSettings, ReadOnly and Fullscreen have been checked if desired.");
            AspectRatioError win2 = new AspectRatioError();
            win2.Show();
            SystemSounds.Hand.Play();
        }

        // Functions
        private void resolutionXFunc()
        {
            string localFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData); // get LocalAppData
            string absolutePath = System.IO.Path.Combine(localFolder, "FortniteGame\\Saved\\Config\\WindowsClient\\GameUserSettings.ini"); // get GameUserSettings

            var lines = File.ReadAllLines(absolutePath); // read config lines.

            foreach (var line in lines)
            {
                // Check if the line contains our search text, note this will find the first match not necessarily the best match
                if (line.Contains("ResolutionSizeX"))
                {
                    // Result found, assign result and break out of loop
                    resultX = line; // write X res to mem.
                    break;
                }
            }
        }

        private void resolutionYFunc() // separated for easy and clean distinction of code. 
        {
            string localFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string absolutePath = System.IO.Path.Combine(localFolder, "FortniteGame\\Saved\\Config\\WindowsClient\\GameUserSettings.ini");

            var lines = File.ReadAllLines(absolutePath);

            foreach (var line in lines)
            {
                // Check if the line contains our search text, note this will find the first match not necessarily the best match
                if (line.Contains("ResolutionSizeY"))
                {
                    // Result found, assign result and break out of loop
                    resultY = line;
                    break;
                }
            }
        }

        public void fullscreenCheck() // separated for easy and clean distinction of code. 
        {
            string localFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string absolutePath = System.IO.Path.Combine(localFolder, "FortniteGame\\Saved\\Config\\WindowsClient\\GameUserSettings.ini");
            var lines = File.ReadAllLines(absolutePath);

            foreach (var line in lines)
            {
                // Check if the line contains our search text, note this will find the first match not necessarily the best match
                if (line.Contains("FullscreenMode="))
                {
                    // Result found, assign result and break out of loop
                    fullscreenMode = line;
                    existsAlr = "yes";
                    break;
                }
            }
        }

        public void windowFullscreenCheck() // separated for easy and clean distinction of code. 
        {
            string localFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string absolutePath = System.IO.Path.Combine(localFolder, "FortniteGame\\Saved\\Config\\WindowsClient\\GameUserSettings.ini");
            var lines = File.ReadAllLines(absolutePath);

            foreach (var line in lines)
            {
                // Check if the line contains our search text, note this will find the first match not necessarily the best match
                if (line.Contains("FullscreenMode="))
                {
                    // Result found, assign result and break out of loop
                    windowfullscreenMode = line;
                    existsAlr = "yes";
                    break;
                }
            }
        }

        public void windowedCheck() // separated for easy and clean distinction of code. 
        {
            string localFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string absolutePath = System.IO.Path.Combine(localFolder, "FortniteGame\\Saved\\Config\\WindowsClient\\GameUserSettings.ini");
            var lines = File.ReadAllLines(absolutePath);

            foreach (var line in lines)
            {
                // Check if the line contains our search text, note this will find the first match not necessarily the best match
                if (line.Contains("FullscreenMode="))
                {
                    // Result found, assign result and break out of loop
                    windowedMode = line;
                    existsAlr = "yes";
                    break;
                }
            }
        }

        public void fpsCheck() // separated for easy and clean distinction of code. 
        {
            string localFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string absolutePath = System.IO.Path.Combine(localFolder, "FortniteGame\\Saved\\Config\\WindowsClient\\GameUserSettings.ini");
            var lines = File.ReadAllLines(absolutePath);

            foreach (var line in lines)
            {
                // Check if the line contains our search text, note this will find the first match not necessarily the best match
                if (line.Contains("FrameRateLimit="))
                {
                    // Result found, assign result and break out of loop
                    fpsRate = line;
                    break;
                }
            }
        }

        private void isWindowFull_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        static void lineChanger(string newText, string fileName, int line_to_edit)
        {
            string[] arrLine = File.ReadAllLines(fileName);
            arrLine[line_to_edit - 1] = newText;
            File.WriteAllLines(fileName, arrLine);
        }
    }
}
