using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Resfree.Frontback_End
{
    /// <summary>
    /// Interaction logic for AspectRatioError.xaml
    /// </summary>
    public partial class AspectRatioError : Window
    {
        public AspectRatioError()
        {
            InitializeComponent();
        }

        private void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void tutorialLbl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://youtu.be/A-RJwYJu5O8");
        }

        private void okBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
