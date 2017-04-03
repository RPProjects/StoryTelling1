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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StoryTelling1
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class StartPage : Page
    {
        public MainWindow mainWindow;
        public Boolean hasChoosenLang = false;
        // 0 = English; 1 = Portuguese; -1 = Not selected.
        public int language = -1;

        public StartPage(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();
        }

        private void start(object sender, RoutedEventArgs e)
        {
            if (participantName.Text.Length == 0)
            {
                NameLabel.Foreground = Brushes.Red;
                NameLabel.FontWeight = FontWeights.Bold;
            }
            else
            {
                if (hasChoosenLang)
                {
                    mainWindow.participantName = participantName.Text;
                    mainWindow.lang = language;
                    // Greetings to the player
                    mainWindow.thalamusClient.StartGreetings(mainWindow.lang);

                    mainWindow.NavigateTo("story");
                    // mainWindow.SaveInfo(participantDonation);
                }
                else
                {
                    ErrorLabel.Foreground = Brushes.Red;
                    ErrorLabel.Visibility = System.Windows.Visibility.Visible;
                }
            }
        }
        private void RadioButton_PT_Checked(object sender, RoutedEventArgs e)
        {
            language = 1;
            hasChoosenLang = true;
        }
        private void RadioButton_EN_Checked(object sender, RoutedEventArgs e)
        {
            language = 0;
            hasChoosenLang = true;
        }
    }
}