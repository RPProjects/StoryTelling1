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
using System.IO;

namespace StoryTelling1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public StartPage startPage;
        public StoryPage storyPage;

        public string participantName;
        public int lang = -1;
        public Client thalamusClient;

        public int persuasionLevel = 0;
        // E N F P || 
        // I S T J 
        // If the persuasion is according to the user's PT, insert the his/her PT. If the persuasion is against user's PT, insert other MBTI PT.      
        public string MBTIPersonality = "e-n-f-p";

        public string pathImg = "pack://application:,,,/GUI/image/";
        public MainWindow()
        {
            InitializeComponent();

            ImageBrush myBrush = new ImageBrush();
            myBrush.ImageSource =
                new BitmapImage(new Uri(pathImg + "background1.jpg", UriKind.Absolute));
            this.Background = myBrush;

            this.WindowState = WindowState.Maximized;
            this.WindowStyle = WindowStyle.None;
            this.ResizeMode = ResizeMode.NoResize;
            //string character = "St1";
            thalamusClient = new Client(this, "St1");
            //thalamusClient = new Client("St1", character);

            this.startPage = new StartPage(this);
            this.storyPage = new StoryPage();
            this.NavigateTo("start");
        }

        private void closedWindow(object sender, EventArgs e)
        {
            if (thalamusClient.IsConnected == true)
                thalamusClient.Dispose();
            Environment.Exit(0);
        }
        public void NavigateTo(String nextPage)
        {
            switch (nextPage)
            {
                case "start":
                    MainFrame.Navigate(startPage);
                    break;

                case "story":
                    storyPage.thalamusClientSTPage = thalamusClient;

                    ImageBrush myBrush = new ImageBrush();
                    myBrush.ImageSource =
                        new BitmapImage(new Uri(pathImg + "background3.jpg", UriKind.Absolute));
                    this.Background = myBrush;
                    storyPage.setImageNumber(1);
                    storyPage.SetParticipantName(participantName);
                    storyPage.SetParticipantLang(lang);
                    Random rnd = new Random(DateTime.Now.Millisecond);
                    string fileName = DateTime.Today.Date.ToString("dd-MM-yyyy") + "--" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + rnd.Next().ToString() + ".txt";
                    storyPage.setFileName(fileName);
                    storyPage.OpenFileStory();
                    // Greetings to the player
                    //thalamusClient.StartGreetings(lang);
                    

                    MainFrame.Navigate(storyPage);
                    //thalamusClient.Story("Here");
                    break;
            }
        }
    }
}
