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
using System.Data;
using System.Data.OleDb;
using System.IO;

namespace StoryTelling1
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class StoryPage : Page
    {
        private int langChoosen = -1;
        private string sheetSt;
        private string sheetDP;
        public DataSet ds = new DataSet();
        public DataSet dsAn = new DataSet();
        public DataSet dsDP = new DataSet();
        public string formatText;
        private int dpAtual;
        private string logName;
        private string logNameWeight;
        private string logNamePersuasion1;
        private string pName;
        public string btnPath;
        public string dpPath;

        public int numberDP = 0;
        public int numberDP1 = 0, numberDP2 = 0;
        public int numberShowText = 0;

        // this matrix has the first row represents the count of chose by each preference
        // the second is the weights of each preference according with the option 'a' or 'b'
        // e  | i  | s  | n  | t  | f  | j  | p
        // we | we | ws | ws | wt | wt | wj | wj
        // wi | wi | wn | wn | wf | wf | wp | wp
        public static double[,] total = new double[3, 8];

        // For the story
        public int imgNumber;
        public string text = "";
        public string textUtterance = "";

        // Id of the story
        public string stId;

        public string gazeTo = "";

        public Client thalamusClientSTPage;

        public StoryPage()
        {
            InitializeComponent();
        }

        public void setImageNumber(int num)
        {
            imgNumber = num;
        }
        
        private void setTextStory(string txt)
        {
            //BlockUIContainer blockC = new BlockUIContainer();
            //blockC.Child = myBrush2;
            FlowDocument doc = new FlowDocument();
            Paragraph p = new Paragraph();
            Image sideImg = new Image();
            sideImg.Source =
                //new BitmapImage(new Uri("C:/Users/monstrengo/Desktop/StoryTelling1/StoryTelling1/GUI/image/" + imgNumber + ".jpg", UriKind.Relative));
            new BitmapImage(new Uri("C:/Users/parad/Desktop/StoryTelling1/StoryTelling1/GUI/image/" + imgNumber + ".jpg", UriKind.Relative));
            //new BitmapImage(new Uri(@"pack://application:,,,/image/" + imgNumber + ".jpg", UriKind.Relative));
            //new BitmapImage(new Uri("C:/Users/parad/Documents/Visual Studio 2015/Projects/StoryTelling1/StoryTelling1/GUI/image/" + imgNumber + ".jpg", UriKind.Relative));
            sideImg.Width = 300;

            BlockUIContainer blockC = new BlockUIContainer();
            blockC.Child = sideImg;
            Floater floater = new Floater(blockC);
            floater.Width = 330;
            floater.HorizontalAlignment = HorizontalAlignment.Left;
            floater.Margin = new Thickness(0, 0, 5, 5);
            

            //p.Inlines.Add(txt);
            p.Inlines.Clear();
            p.Inlines.Add(floater);
            //p.TextAlignment = TextAlignment.Justify;
            //p.FontSize = 22;
            //p.FontFamily = new FontFamily("Comic Sans MS");
            doc.Blocks.Add(p);
            fdViewer.Document = doc;
            doc.Blocks.Remove(blockC);
        }

        public void showFinal(string txt)
        {
            FlowDocument doc = new FlowDocument();
            Paragraph p = new Paragraph();
            p.Inlines.Add(txt);
            p.TextAlignment = TextAlignment.Center;
            p.FontSize = 32;
            p.FontFamily = new FontFamily("Comic Sans MS");
            doc.Blocks.Add(p);
            fdViewerFinal.Document = doc;

            Console.WriteLine("Show the end of the game!");
        }       
        
        public void SetParticipantName(string participantName)
        {
            //this.NameLabel.Content = "Welcome, " + participantName + "!";
            pName = participantName;
        }

        #region defineLanguageAndExcellSheets
        public void SetParticipantLang(int lang)
        {
            langChoosen = lang;
            if (lang == 0)
            {
                sheetSt = "StoryEN";
                sheetDP = "DPEN";
            } else
            {
                sheetSt = "StoryPT";
                sheetDP = "DPPT";
            }
        }
        #endregion

        #region openExcelFileStory
        public void OpenFileStory()
        {
            // File with story and animation
            using (OleDbConnection connan = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=utterances_an.xlsx;Extended Properties=Excel 12.0"))
            {
                //SQL SELECT         
                string strSQLAn = "SELECT * FROM [" + sheetSt + "$]";
                OleDbDataAdapter adapter = new OleDbDataAdapter(strSQLAn, connan);
                try
                {
                    connan.Open();
                    adapter.Fill(dsAn);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error to access the data: " + ex.Message);
                }
                finally
                {
                    connan.Close();
                }
            }

            // File with story and without animation
            using (OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=utterances_st.xlsx;Extended Properties=Excel 12.0"))
            {
                //SQL SELECT         
                string strSQL = "SELECT * FROM [" + sheetSt + "$]";
                OleDbDataAdapter adapter = new OleDbDataAdapter(strSQL, conn);
                try
                {
                    conn.Open();
                    adapter.Fill(ds);
                    OpenFileDP();
                    showStory(ds.Tables[0].Rows[0].ItemArray[1].ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error to access the data: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        #endregion

        #region openExcelFileDP
        public void OpenFileDP()
        {
            using (OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=utterances_st.xlsx;Extended Properties=Excel 12.0"))
            {
                //SQL SELECT         
                string strSQL = "SELECT * FROM [" + sheetDP + "$]";
                OleDbDataAdapter adapter = new OleDbDataAdapter(strSQL, conn);
                try
                {
                    conn.Open();
                    adapter.Fill(dsDP);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error to access the data: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        #endregion

        public void showStory(string nextStOrDP)
        {
            //Console.WriteLine("------- NEXT : " + nextStOrDP);
            if (numberDP == 3) setImageNumber(2);
            if (numberDP == 6) setImageNumber(3);
            if (numberDP == 9) setImageNumber(4);
            if (numberDP == 12) setImageNumber(5);
            if (numberDP == 15) setImageNumber(6);
            if (numberDP == 18) setImageNumber(7);
            // introduction
            if (nextStOrDP == "Intro")
            {
                nextStOrDP = "DP1";
                formatText = ds.Tables[0].Rows[0].ItemArray[2].ToString();
                textUtterance = dsAn.Tables[0].Rows[0].ItemArray[2].ToString();
                //this.story.Text += formatText.Replace("\\r\\n", "\r\n");
                text = formatText.Replace("\\r\\n", "\r\n");
                text = text.Replace("\\t", "   ");
                stId = ds.Tables[0].Rows[0].ItemArray[0].ToString();
                //setTextStory(formatText.Replace("\\r\\n", "\r\n"));
            }
            #region endOfStory
            if (nextStOrDP == "-")
            {
                this.Dispatcher.Invoke((Action)(() =>
                {//this refer to form in WPF application 
                    this.DP1.Visibility = System.Windows.Visibility.Hidden;
                    this.DP2.Visibility = System.Windows.Visibility.Hidden;
                }));
                setTextStory(text);
                //thalamusClientSTPage.Story(text, stId);
                thalamusClientSTPage.Story(textUtterance, stId);

                setImageNumber(8);
                recordEndGame();
                                
                // end game
                //showFinal("\r\n\r\n\r\n\r\nChame o investigador, por favor.");
            }
            else
            {
            #endregion
                // verify if the next is a dp, could be a st or a number that comes from the press button.
                int index = nextStOrDP.IndexOf("P");

                // It is a DP
                if (index > 0)
                {
                    setTextStory(text);
                    /*
                    Insert in the utterances the command <Gaze(btn)> when the robot should Gaze to a button.
                    Then, here it will be replaced the comand Gaze by the real command of gaze, but according with the side that 
                        the robot must look. Depending of the MBTI and the persuasion's level.
                    */

                    //Console.WriteLine("Txt ------- " + text);
                    // search for the correct utterance
                    // verify if the next is a st or not
                    /*int correct = 0;
                    for (int i = 0; i < dsAn.Tables[0].Rows.Count; i++)
                    {
                        if (stId == dsAn.Tables[0].Rows[i].ItemArray[0].ToString())
                        {
                            correct = i;
                            break;
                        }
                    }*/
                    //thalamusClientSTPage.Story(dsAn.Tables[0].Rows[correct].ItemArray[2].ToString(), stId);

                    #region persuasion1
                    // Persuasion Level 1, just gaze! Gaze to the respective Button and to the user's face.
                    if (thalamusClientSTPage.mainWindow.persuasionLevel == 1)
                    {
                        Console.WriteLine("PERSUASION - " + thalamusClientSTPage.mainWindow.persuasionLevel + " Pref 1" + dsDP.Tables[0].Rows[dpAtual].ItemArray[3].ToString() + " Pref 2" + dsDP.Tables[0].Rows[dpAtual].ItemArray[4].ToString());
                        string[] MBTI = thalamusClientSTPage.mainWindow.MBTIPersonality.Split('-');
                        foreach (var word in MBTI)
                            Console.WriteLine("HERE -> " + word);
                        string side = "0";
                        switch (dsDP.Tables[0].Rows[dpAtual].ItemArray[3].ToString())
                        {
                            case "e":
                            case "i":
                                // E || I
                                if (MBTI[0] == dsDP.Tables[0].Rows[dpAtual].ItemArray[3].ToString())
                                {
                                    Console.WriteLine("GAZE TO LEFT USER BUTTON");
                                    gazeTo = "<Gaze(person3)><break time=\"1s\"/><Gaze(bottomLeft)><break time=\"1s\"/><Gaze(bottomRight)>";
                                    //textUtterance = textUtterance.Replace("<Gaze(btn)>", "<Gaze(person3)><break time=\"1s\"/><Gaze(bottomLeft)><break time=\"1s\"/><Gaze(bottomRight)>");
                                    side = "l"; // left user button
                                }
                                else
                                {
                                    Console.WriteLine("GAZE TO RIGHT USER BUTTON");
                                    gazeTo = "<Gaze(person3)><break time=\"1s\"/><Gaze(bottomLeft)><break time=\"1s\"/><Gaze(bottomRight)><break time=\"1s\"/><Gaze(bottomLeft)>";
                                    //textUtterance = textUtterance.Replace("<Gaze(btn)>", "<Gaze(person3)><break time=\"1s\"/><Gaze(bottomLeft)><break time=\"1s\"/><Gaze(bottomRight)><break time=\"1s\"/><Gaze(bottomLeft)>");
                                    side = "r"; // right user button
                                }
                                break;
                            case "n":
                            case "s":
                                // N || S
                                if (MBTI[1] == dsDP.Tables[0].Rows[dpAtual].ItemArray[3].ToString())
                                {
                                    Console.WriteLine("GAZE TO LEFT USER BUTTON");
                                    gazeTo = "<Gaze(person3)><break time=\"1s\"/><Gaze(bottomLeft)><break time=\"1s\"/><Gaze(bottomRight)>";
                                    textUtterance = textUtterance.Replace("<Gaze(btn)>", " ");
                                    //textUtterance = textUtterance.Replace("<Gaze(btn)>", "<Gaze(person3)><break time=\"1s\"/><Gaze(bottomLeft)><break time=\"1s\"/><Gaze(bottomRight)>");
                                    side = "l";
                                }
                                else
                                {
                                    Console.WriteLine("GAZE TO RIGHT USER BUTTON");
                                    gazeTo = "<Gaze(person3)><break time=\"1s\"/><Gaze(bottomLeft)><break time=\"1s\"/><Gaze(bottomRight)><break time=\"1s\"/><Gaze(bottomLeft)>";
                                    textUtterance = textUtterance.Replace("<Gaze(btn)>", " ");
                                    //textUtterance = textUtterance.Replace("<Gaze(btn)>", "<Gaze(person3)><break time=\"1s\"/><Gaze(bottomLeft)><break time=\"1s\"/><Gaze(bottomRight)><break time=\"1s\"/><Gaze(bottomLeft)>");
                                    side = "r";
                                }
                                break;
                            case "f":
                            case "t":
                                // F || T
                                if (MBTI[2] == dsDP.Tables[0].Rows[dpAtual].ItemArray[3].ToString())
                                {
                                    Console.WriteLine("GAZE TO LEFT USER BUTTON");
                                    gazeTo = "<Gaze(person3)><break time=\"1s\"/><Gaze(bottomLeft)><break time=\"1s\"/><Gaze(bottomRight)>";
                                    textUtterance = textUtterance.Replace("<Gaze(btn)>", " ");
                                    //textUtterance = textUtterance.Replace("<Gaze(btn)>", "<Gaze(person3)><break time=\"1s\"/><Gaze(bottomLeft)><break time=\"1s\"/><Gaze(bottomRight)>");
                                    side = "l";
                                }
                                else
                                {
                                    Console.WriteLine("GAZE TO RIGHT USER BUTTON");
                                    gazeTo = "<Gaze(person3)><break time=\"1s\"/><Gaze(bottomLeft)><break time=\"1s\"/><Gaze(bottomRight)><break time=\"1s\"/><Gaze(bottomLeft)>";
                                    textUtterance = textUtterance.Replace("<Gaze(btn)>", " ");
                                    //textUtterance = textUtterance.Replace("<Gaze(btn)>", "<Gaze(person3)><break time=\"1s\"/><Gaze(bottomLeft)><break time=\"1s\"/><Gaze(bottomRight)><break time=\"1s\"/><Gaze(bottomLeft)>");
                                    side = "r";
                                }
                                break;
                            case "p":
                            case "j":
                                // P || J
                                if (MBTI[3] == dsDP.Tables[0].Rows[dpAtual].ItemArray[3].ToString())
                                {
                                    Console.WriteLine("GAZE TO LEFT USER BUTTON");
                                    gazeTo = "<Gaze(person3)><break time=\"1s\"/><Gaze(bottomLeft)><break time=\"1s\"/><Gaze(bottomRight)>";
                                    textUtterance = textUtterance.Replace("<Gaze(btn)>", " ");
                                    //textUtterance = textUtterance.Replace("<Gaze(btn)>", "<Gaze(person3)><break time=\"1s\"/><Gaze(bottomLeft)><break time=\"1s\"/><Gaze(bottomRight)>");
                                    side = "l";
                                }
                                else
                                {
                                    Console.WriteLine("GAZE TO RIGHT USER BUTTON");
                                    gazeTo = "<Gaze(person3)><break time=\"1s\"/><Gaze(bottomLeft)><break time=\"1s\"/><Gaze(bottomRight)><break time=\"1s\"/><Gaze(bottomLeft)>";
                                    textUtterance = textUtterance.Replace("<Gaze(btn)>", " ");
                                    //textUtterance = textUtterance.Replace("<Gaze(btn)>", "<Gaze(person3)><break time=\"1s\"/><Gaze(bottomLeft)><break time=\"1s\"/><Gaze(bottomRight)><break time=\"1s\"/><Gaze(bottomLeft)>");
                                    side = "r";
                                }
                                break;
                        }
                        recordLogP1(side, "0");
                    }
                    #endregion
                    thalamusClientSTPage.Story(textUtterance, stId);
                    for (int i = 0; i < dsDP.Tables[0].Rows.Count; i++)
                    {
                        if (nextStOrDP == dsDP.Tables[0].Rows[i].ItemArray[1].ToString())
                        {
                            this.txtDP1.Text = dsDP.Tables[0].Rows[i].ItemArray[5].ToString();
                            this.txtDP2.Text = dsDP.Tables[0].Rows[i].ItemArray[6].ToString();
                            dpAtual = i;
                        }
                    }
                }
                else
                {
                    int indexST = nextStOrDP.IndexOf("T");
                    if (indexST > 0)
                    {
                        // verify if the next is a st or not
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            if (nextStOrDP == ds.Tables[0].Rows[i].ItemArray[1].ToString())
                            {
                                formatText = ds.Tables[0].Rows[i].ItemArray[2].ToString();
                                textUtterance += dsAn.Tables[0].Rows[i].ItemArray[2].ToString();
                                text += formatText.Replace("\\r\\n", "\r\n");
                                text = text.Replace("\\t", "   ");
                                // this.story.Text += formatText.Replace("\\r\\n", "\r\n");
                                stId = ds.Tables[0].Rows[i].ItemArray[0].ToString();
                                showStory(ds.Tables[0].Rows[i].ItemArray[3].ToString());
                                //Console.WriteLine("HERE --" + ds.Tables[0].Rows[i].ItemArray[3].ToString());
                            }
                        }
                    }
                    else
                    {
                        // when a button is pressed
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            if (nextStOrDP == ds.Tables[0].Rows[i].ItemArray[0].ToString())
                            {
                                formatText = ds.Tables[0].Rows[i].ItemArray[2].ToString();
                                textUtterance = dsAn.Tables[0].Rows[i].ItemArray[2].ToString();
                                text = formatText.Replace("\\r\\n", "\r\n");
                                text = text.Replace("\\t", "   ");
                                stId = ds.Tables[0].Rows[i].ItemArray[0].ToString();
                                //this.story.Text += formatText.Replace("\\r\\n", "\r\n");
                                showStory(ds.Tables[0].Rows[i].ItemArray[3].ToString());
                            }
                        }
                    }
                }
            }
        }

        public void gazeToBtn()
        {
            thalamusClientSTPage.Story(gazeTo, "77");
        }

        private void btnClicked1(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {//this refer to form in WPF application 
                this.DP1.Visibility = System.Windows.Visibility.Hidden;
                this.DP2.Visibility = System.Windows.Visibility.Hidden;
            }));
            
            thalamusClientSTPage.stopStory(stId);
            // clean the text
            // this.story.Text = "";
            text = "";
            textUtterance = "";
            numberDP++;
            numberDP1++;
            recordLogP1("0", "l");
            recordLog(dsDP.Tables[0].Rows[dpAtual].ItemArray[3].ToString(), dsDP.Tables[0].Rows[dpAtual].ItemArray[4].ToString(), Math.Round(Convert.ToDouble(dsDP.Tables[0].Rows[dpAtual].ItemArray[7]),2), Math.Round(Convert.ToDouble(dsDP.Tables[0].Rows[dpAtual].ItemArray[8].ToString()),2), 1);
            showStory(dsDP.Tables[0].Rows[dpAtual].ItemArray[9].ToString());
        }

        private void btnClicked2(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {//this refer to form in WPF application 
                this.DP1.Visibility = System.Windows.Visibility.Hidden;
                this.DP2.Visibility = System.Windows.Visibility.Hidden;
            }));
            
            thalamusClientSTPage.stopStory(stId);
            // clean the text
            // this.story.Text = "";
            text = "";
            textUtterance = "";
            numberDP++;
            numberDP2++;
            recordLogP1("0", "r");
            recordLog(dsDP.Tables[0].Rows[dpAtual].ItemArray[3].ToString(), dsDP.Tables[0].Rows[dpAtual].ItemArray[4].ToString(), Math.Round(Convert.ToDouble(dsDP.Tables[0].Rows[dpAtual].ItemArray[11]), 2), Math.Round(Convert.ToDouble(dsDP.Tables[0].Rows[dpAtual].ItemArray[12].ToString()), 2), 2);
            showStory(dsDP.Tables[0].Rows[dpAtual].ItemArray[10].ToString());
        }

        #region defineFileName
        public void setFileName (string fileName)
        {
            logName = "Logs\\" + pName + "-- Track --" + fileName;
            logNameWeight = "Logs\\" + pName + "-- Weight --" + fileName;
            logNamePersuasion1 = "Logs\\" + pName + "-- Persuasion1 --" + fileName;
            try
            {
                if (!File.Exists(logName))
                {
                    FileStream fs = File.Open(logName.ToString(), FileMode.Append);
                    FileStream fsW = File.Open(logNameWeight.ToString(), FileMode.Append);
                    FileStream fsP1 = File.Open(logNamePersuasion1.ToString(), FileMode.Append);
                }
            }
            catch (Exception e) { Console.WriteLine(" Error Record data: " + e); }
        }
        #endregion

        #region recordPersuasion1
        private void recordLogP1(string should, string did)
        {
            try
            {
                if (File.Exists(logNamePersuasion1))
                {
                    //Console.WriteLine(" Record !!! " + logName);
                    using (FileStream fsp1 = File.Open(logNamePersuasion1.ToString(), FileMode.Append))
                    using (StreamWriter writer = new StreamWriter(fsp1))
                    {
                        if (should != "0")
                            writer.Write("Should: " + should);
                        else
                        {
                            writer.WriteLine(" || " + "Did: " + did);
                            writer.WriteLine("--------------------- ");
                        }
                    }
                }
            }
            catch (Exception e) { Console.WriteLine(" Error Record data: " + e); }
        }
        #endregion

        #region recordRealTimeLog
        private void recordLog(string pref1, string pref2, double valueP1, double valueP2, int btnPressed)
        {
            // Console.WriteLine("Data FILE: " + pref1 + "||" + pref2 + "||" + valueP1 + "||" + valueP2);
            // Console.WriteLine("NAME FILE: " + logName);
            btnPath = btnPath + " - " + btnPressed;
            dpPath = dpPath + " - DP" + (dpAtual+1);
            //Console.WriteLine("BTN = " + btnPath);
            //Console.WriteLine("Dec = " + dpPath);
            try
            {
                if (File.Exists(logName))
                {
                    //Console.WriteLine(" Record !!! " + logName);
                    using (FileStream fs = File.Open(logName.ToString(), FileMode.Append))
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        writer.WriteLine("Button: " + btnPath);
                        writer.WriteLine("--------------------- ");
                        writer.WriteLine("DecPoi: " + dpPath);
                        writer.WriteLine("--------------------- ");
                    }
                }
            }
            catch (Exception e) { Console.WriteLine(" Error Record data: " + e); }

            // record weight
            string pref;
            if (btnPressed == 1) pref = pref1;
            else pref = pref2;

            switch (pref)
            {
                case "e":
                    total[0, 0] = total[0, 0] + 1;
                    total[1, 0] = total[1, 0] + valueP1;
                    total[2, 0] = total[2, 0] + valueP2;
                    break;
                case "i":
                    total[0, 1] = total[0, 1] + 1;
                    total[1, 1] = total[1, 1] + valueP1;
                    total[2, 1] = total[2, 1] + valueP2;
                    break;
                case "s":
                    total[0, 2] = total[0, 2] + 1;
                    total[1, 2] = total[1, 2] + valueP1;
                    total[2, 2] = total[2, 2] + valueP2;
                    break;
                case "n":
                    total[0, 3] = total[0, 3] + 1;
                    total[1, 3] = total[1, 3] + valueP1;
                    total[2, 3] = total[2, 3] + valueP2;
                    break;
                case "t":
                    total[0, 4] = total[0, 4] + 1;
                    total[1, 4] = total[1, 4] + valueP1;
                    total[2, 4] = total[2, 4] + valueP2;
                    break;
                case "f":
                    total[0, 5] = total[0, 5] + 1;
                    total[1, 5] = total[1, 5] + valueP1;
                    total[2, 5] = total[2, 5] + valueP2;
                    break;
                case "j":
                    total[0, 6] = total[0, 6] + 1;
                    total[1, 6] = total[1, 6] + valueP1;
                    total[2, 6] = total[2, 6] + valueP2;
                    break;
                case "p":
                    total[0, 7] = total[0, 7] + 1;
                    total[1, 7] = total[1, 7] + valueP1;
                    total[2, 7] = total[2, 7] + valueP2;
                    break;
            }

            // record file weights
            try
            {
                if (File.Exists(logNameWeight))
                {
                    //Console.WriteLine(" Record !!! " + logName);
                    using (FileStream fs = File.Open(logNameWeight.ToString(), FileMode.Append))
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        writer.WriteLine("Path: " + dpPath);
                        for (int i = 0; i <= 2; i++)
                        {
                            for (int j = 0; j < 8; j++)
                            {
                                if (i == 0) writer.Write("|  " + total[i, j]);
                                else writer.Write("|" + total[i, j]);
                            }
                            writer.Write("|");
                            writer.WriteLine();
                        }
                    }
                }
            }
            catch (Exception e) { Console.WriteLine(" Error Record data: " + e); }
        }
        #endregion

        #region recordEndGame
        public void recordEndGame()
        {
            #region qtDecisionMade
            try
            {
                if (File.Exists(logName))
                {
                    //Console.WriteLine(" Record !!! " + logName);
                    using (FileStream fs = File.Open(logName.ToString(), FileMode.Append))
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        writer.WriteLine();
                        writer.WriteLine("---------------------");
                        writer.WriteLine("Qt Decisions: " + numberDP);
                        writer.WriteLine("---------------------");
                        writer.WriteLine("Qt DP 1: " + numberDP1);
                        writer.WriteLine("Qt DP 2: " + numberDP2);
                        writer.WriteLine("---------------------");
                        writer.WriteLine("Qt Show Text: " + numberShowText);
                        writer.WriteLine("---------------------");
                    }
                }
            }
            catch (Exception error) { Console.WriteLine(" Error Record data: " + error); }
            #endregion

            #region reportInLog
            string mbti = "";
            string mbtiPer = "";
            if (total[0, 0] > total[0, 1]) mbti = mbti + "e";
            else mbti = mbti + "i";
            if (total[0, 2] > total[0, 3]) mbti = mbti + "s";
            else mbti = mbti + "n";
            if (total[0, 4] > total[0, 5]) mbti = mbti + "t";
            else mbti = mbti + "f";
            if (total[0, 6] > total[0, 7]) mbti = mbti + "j";
            else mbti = mbti + "p";

            if (total[1, 0] > total[2, 1]) mbtiPer = mbtiPer + "e = " + total[1, 0].ToString() + "% || ";
            else mbtiPer = mbtiPer + "i = " + total[2, 1].ToString() + "% || ";
            if (total[1, 2] > total[2, 3]) mbtiPer = mbtiPer + "s = " + total[1, 2].ToString() + "% || ";
            else mbtiPer = mbtiPer + "n = " + total[2, 3].ToString() + "% || ";
            if (total[1, 4] > total[2, 5]) mbtiPer = mbtiPer + "t = " + total[1, 4].ToString() + "% || ";
            else mbtiPer = mbtiPer + "f = " + total[2, 5].ToString() + "% || ";
            if (total[1, 6] > total[2, 7]) mbtiPer = mbtiPer + "j = " + total[1, 6].ToString() + "% || ";
            else mbtiPer = mbtiPer + "p = " + total[2, 7].ToString() + "% || ";

            // calculating the mbti weight
            double e = Convert.ToDouble(total[1, 0]) + Convert.ToDouble(total[1, 1]);
            double i = Convert.ToDouble(total[2, 0]) + Convert.ToDouble(total[2, 1]);
            double s = Convert.ToDouble(total[1, 2]) + Convert.ToDouble(total[1, 3]);
            double n = Convert.ToDouble(total[2, 2]) + Convert.ToDouble(total[2, 3]);
            double t = Convert.ToDouble(total[1, 4]) + Convert.ToDouble(total[1, 5]);
            double f = Convert.ToDouble(total[2, 4]) + Convert.ToDouble(total[2, 5]);
            double j = Convert.ToDouble(total[1, 6]) + Convert.ToDouble(total[1, 7]);
            double p = Convert.ToDouble(total[2, 6]) + Convert.ToDouble(total[2, 7]);

            string mbtiWeightPer = "e = " + e.ToString() + "%\r\ni = " + i.ToString()
                                + "%\r\ns = " + s.ToString() + "%\r\nn = " + n.ToString()
                                + "%\r\nt = " + t.ToString() + "%\r\nf = " + f.ToString()
                                + "%\r\nj = " + j.ToString() + "%\r\np = " + p.ToString() + "%";

            string mbtiWeight = "";
            if (e > i) mbtiWeight = mbtiWeight + "e";
            else mbtiWeight = mbtiWeight + "i";
            if (s > n) mbtiWeight = mbtiWeight + "s";
            else mbtiWeight = mbtiWeight + "n";
            if (t > f) mbtiWeight = mbtiWeight + "t";
            else mbtiWeight = mbtiWeight + "f";
            if (j > p) mbtiWeight = mbtiWeight + "j";
            else mbtiWeight = mbtiWeight + "p";
          
            using (FileStream fs = File.Open(logNameWeight, FileMode.Append))
            using (StreamWriter writer = new StreamWriter(fs))
            {
                writer.WriteLine("--------------------- ");
                writer.WriteLine("MBTI: " + mbti);
                writer.WriteLine("--------------------- ");
                writer.WriteLine("--------------------- ");
                writer.WriteLine("MBTI Per. Preference: " + mbtiPer);
                writer.WriteLine("--------------------- ");
                writer.WriteLine("--------------------- ");
                writer.WriteLine("MBTI Weight: " + mbtiWeight);
                writer.WriteLine("--------------------- ");
                writer.WriteLine("--------------------- ");
                writer.WriteLine("MBTI Per. Weight: ");
                writer.WriteLine(mbtiWeightPer);
                writer.WriteLine("--------------------- ");
                writer.WriteLine("--------------------- ");
                writer.WriteLine("End time: " + DateTime.Now.ToString());
                writer.WriteLine("--------------------- ");
                writer.WriteLine("--------------------- ");
            }
            #endregion
        }
        #endregion

        private void showText(object sender, MouseButtonEventArgs e)
        {
            FlowDocument doc = new FlowDocument();
            Paragraph p = new Paragraph();
            Image sideImg = new Image();
            sideImg.Source =
            //new BitmapImage(new Uri("C:/Users/monstrengo/Desktop/StoryTelling1/StoryTelling1/GUI/image/" + imgNumber + ".jpg", UriKind.Relative));
            new BitmapImage(new Uri("C:/Users/parad/Desktop/StoryTelling1/StoryTelling1/GUI/image/" + imgNumber + ".jpg", UriKind.Relative));
            //new BitmapImage(new Uri(@"pack://application:,,,/image/" + imgNumber + ".jpg", UriKind.Relative));
            //new BitmapImage(new Uri("C:/Users/parad/Documents/Visual Studio 2015/Projects/StoryTelling1/StoryTelling1/GUI/image/" + imgNumber + ".jpg", UriKind.Relative));
            sideImg.Width = 300;

            BlockUIContainer blockC = new BlockUIContainer();
            blockC.Child = sideImg;
            Floater floater = new Floater(blockC);
            floater.Width = 330;
            floater.HorizontalAlignment = HorizontalAlignment.Left;
            floater.Margin = new Thickness(0, 0, 5, 5);
            p.Inlines.Add(floater);

            p.Inlines.Add(text);
            p.TextAlignment = TextAlignment.Justify;
            p.FontSize = 22;
            p.FontFamily = new FontFamily("Comic Sans MS");
            doc.Blocks.Add(p);
            fdViewer.Document = doc;
            doc.Blocks.Remove(blockC);

            numberShowText++;
        }
    }

}