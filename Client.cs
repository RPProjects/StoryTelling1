using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thalamus;

namespace StoryTelling1
{

    public enum dialogsCat
    {
        hello,
        start,
        story,
        final
    };
    public interface IClient : Thalamus.BML.ISpeakEvents { }

    public interface IClientPublisher : IThalamusPublisher, IMessageActions, IFMLSpeech, Thalamus.BML.ISpeakActions, Thalamus.BML.ISpeakControlActions { }

    public class Client : ThalamusClient, IClient
    {
        public class ClientPublisher : IClientPublisher
        {
            dynamic publisher;
            public ClientPublisher(dynamic publisher)
            {
                this.publisher = publisher;
            }

            public void ClientMessage(string msg)
            {
                publisher.ClientMessage(msg);
            }

            #region Ispeak
            public void Speak(string id, string text)
            {
                publisher.Speak(id, text);
            }

            public void SpeakBookmarks(string id, string[] text, string[] bookmarks)
            {
                publisher.SpeakBookmarks(id, text, bookmarks);
            }

            public void SpeakStop()
            {
                publisher.SpeakStop();
            }
            #endregion

            #region setLanguage
            public void SetLanguage(Thalamus.BML.SpeechLanguages lang)
            {
                publisher.SetLanguage(lang);
            }
            #endregion

            #region IFML
            public void PerformUtterance(string id, string utterance, string category)
            {
                publisher.PerformUtterance(id, utterance, category);
            }
            public void PerformUtteranceFromLibrary(string id, string category, string subcategory, string[] tagNames, string[] tagValues)
            {
                publisher.PerformUtteranceFromLibrary(id, category, subcategory, tagNames, tagValues);
            }
            public void CancelUtterance(string id)
            {
                publisher.CancelUtterance(id);
            }
            #endregion

            #region Library
            public void ChangeLibrary(string file)
            {
                publisher.ChangeLibrary(file);
            }
            #endregion
        }

        public ClientPublisher CPublisher;
        public MainWindow mainWindow;

        public Client(MainWindow mainWindow, string character = "")
            : base("St1", character)
        //public Client(string cName, string character = "St1") : base(cName)
        {
            SetPublisher<IClientPublisher>();
            CPublisher = new ClientPublisher(Publisher);

            this.mainWindow = mainWindow;
        }

        public void StartGreetings(int lang)
        {
            if (lang == 0)
            {
                CPublisher.SetLanguage(Thalamus.BML.SpeechLanguages.English);
                CPublisher.PerformUtterance("Hello", "Welcome " + mainWindow.participantName + "!! Let's the game begin!", dialogsCat.hello.ToString());
            }
            else
            {
                CPublisher.SetLanguage(Thalamus.BML.SpeechLanguages.Portuguese);
                CPublisher.PerformUtterance("Hello", "<Gaze(person3)>Bem vindo " + mainWindow.participantName + "!! Vamos começar a jogar!", dialogsCat.hello.ToString());
            }
        }
        public string stPrev;
        public void Story(string text, string stId)
        {
            if (stPrev != stId)
            {
                Console.WriteLine("Here: " + stId + " --" + text);
                CPublisher.PerformUtterance(stId, " " + text, dialogsCat.story.ToString());
                stPrev = stId;
            }
            if ((stPrev != stId) && (stId == "77"))
            {
                Console.WriteLine("gaze: " + text);
                CPublisher.PerformUtterance("77", " " + text, dialogsCat.story.ToString());
                stPrev = stId;
            }
        }

        public void stopStory(string stId)
        {
            CPublisher.SpeakStop();
            CPublisher.CancelUtterance(stId);
        }

        #region EMYSSpeak
        public void SpeakFinished(string id)
        {
            if ((id != "51") && (id != "52"))
            {
                mainWindow.storyPage.Dispatcher.Invoke((Action)(() =>
                {//this refer to form in WPF application 
                    mainWindow.storyPage.DP1.Visibility = System.Windows.Visibility.Visible;
                    mainWindow.storyPage.DP2.Visibility = System.Windows.Visibility.Visible;
                    //mainWindow.storyPage.txtDP1.Visibility = System.Windows.Visibility.Visible;
                    //mainWindow.storyPage.txtDP2.Visibility = System.Windows.Visibility.Visible;
                }));
                Console.WriteLine("FINISHED the utterance: " + id);
                mainWindow.storyPage.gazeToBtn();
            }
            if ((id == "51") || (id == "52"))
            {
                mainWindow.storyPage.showFinal("\r\n\r\n\r\n\r\nChame o investigador, por favor.");
            }
        }
        public void SpeakStarted(string id)
        {
            Console.WriteLine("STARTED the utterance: " + id);
            if (id != "77")
            {
                mainWindow.storyPage.Dispatcher.Invoke((Action)(() =>
                {//this refer to form in WPF application 
                    mainWindow.storyPage.DP1.Visibility = System.Windows.Visibility.Hidden;
                    mainWindow.storyPage.DP2.Visibility = System.Windows.Visibility.Hidden;
                    //mainWindow.storyPage.txtDP1.Visibility = System.Windows.Visibility.Hidden;
                    //mainWindow.storyPage.txtDP2.Visibility = System.Windows.Visibility.Hidden;
                }));
            }
        }
        #endregion
    }
}
