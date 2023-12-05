using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.IO;
using System.Windows.Shapes;
using System;

namespace Glow_s_Res_Tool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string resultX = null; // define results for GameUserSettings
        string resultY = null;
        string fullscreenMode = null;
        public MainWindow()
        {
            InitializeComponent();
            Console.Write("Found path...reading");
        }

        private void resWidth_GotFocus(object sender, RoutedEventArgs e)
        {
            resWidth.Text = "";
            resWidth.Foreground = new SolidColorBrush(Colors.Black); // UI Function
        }

        private void resHeight_GotFocus(object sender, RoutedEventArgs e)
        {
            resHeight.Text = "";
            resHeight.Foreground = new SolidColorBrush(Colors.Black); // UI Function
        }

        private void addResolution_Click(object sender, RoutedEventArgs e)
        {
            string localFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData); // get LocalAppData
            string absolutePath = (System.IO.Path.Combine(localFolder, "FortniteGame\\Saved\\Config\\WindowsClient\\GameUserSettings.ini")); // get GameUserSettings

            resolutionXFunc(); // call func for finding X res (func for cleaner code)
            resolutionYFunc(); // call func for finding Y res

            File.SetAttributes(absolutePath, FileAttributes.Normal);

            Console.WriteLine("Resolution has been found and written to memory, continuing.");

            string gameUSx = File.ReadAllText(absolutePath); // read lines in GameUserSettings
            gameUSx = gameUSx.Replace(resultX, "ResolutionSizeX=" + resWidth.Text); // replace current ResolutionSizeX with desired.
            File.WriteAllText(absolutePath, gameUSx); // write to GameUserSettings

            string gameUSY = File.ReadAllText(absolutePath); // read lines in GameUserSettings
            gameUSY = gameUSY.Replace(resultX, "ResolutionSizeY=" + resHeight.Text); // replace current ResolutionSizeY with desired.
            File.WriteAllText(absolutePath, gameUSY); // write to GameUserSettings

            if(isReadOnly.IsChecked == true)
            {
                File.SetAttributes(absolutePath, FileAttributes.ReadOnly);
            }
            else if(isFullScreen.IsChecked == true)
            {
                string gameFS = File.ReadAllText(absolutePath); // read lines in GameUserSettings
                gameFS = gameFS.Replace(resultX, "FullscreenMode=" + "0"); // replace current FullscreenMode with desired.
                File.WriteAllText(absolutePath, gameFS); // write to GameUserSettings
            }

            Console.WriteLine("Desired resolution added to GameUserSettings, ReadOnly and Fullscreen have been checked if desired.");
            MessageBox.Show("Resolution added! Please relog your game if it is open. If you are receiving black bars please press the help button."); // disclaimer for open game and nvidia control panel.
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
                    MessageBox.Show(resultX);
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
                    MessageBox.Show(resultY);
                    break;
                }
            }
        }

        private void fullscreenCheck() // separated for easy and clean distinction of code. 
        {
            string localFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string absolutePath = System.IO.Path.Combine(localFolder, "FortniteGame\\Saved\\Config\\WindowsClient\\GameUserSettings.ini");

            var lines = File.ReadAllLines(absolutePath);

            foreach (var line in lines)
            {
                // Check if the line contains our search text, note this will find the first match not necessarily the best match
                if (line.Contains("FullscreenMode"))
                {
                    // Result found, assign result and break out of loop
                    fullscreenMode = line;
                    MessageBox.Show(fullscreenMode);
                    break;
                }
            }
        }
    }
}