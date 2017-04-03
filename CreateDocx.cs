using System;
using Novacode;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace StoryTelling1
{
    public class CreateDocx
    {
        public void createDocument(string fileName, string txt, int scene)
        {
            // CREATING DOCX FILE, TO RECORD THE USER STORY
            string pathFileName = @"C:/Users/parad/Desktop/StoryTelling1/StoryTelling1/bin/Debug/" + fileName;
            string headlineText = "Defend Your Kingdom \n Your Interactive Storytelling";
            string paraOne = "" + txt;

            // A formatting object for our normal paragraph text:
            var paraScene = new Formatting();
            paraScene.FontFamily = new System.Drawing.FontFamily("Calibri");
            paraScene.Size = 9D;
            paraScene.Bold = true;

            //Console.WriteLine("HERE ---- " + pathFileName + " || " + paraOne);

            //string curFile = @"c:\temp\test.txt";  //Your path
            if (!File.Exists(pathFileName))
            {

                Console.WriteLine("Docx -- " + pathFileName);
                // A formatting object for our headline:
                var headLineFormat = new Formatting();
                headLineFormat.FontFamily = new System.Drawing.FontFamily("Arial Black");
                headLineFormat.Size = 14D;
                headLineFormat.Position = 12;
                headLineFormat.FontColor = System.Drawing.Color.Green;

                // Create the document in memory:
                var doc = DocX.Create(fileName);

                // A formatting object for our normal paragraph text:
                var paraFormat = new Formatting();
                paraFormat.FontFamily = new System.Drawing.FontFamily("Calibri");
                paraFormat.Size = 9D;

                Paragraph title = doc.InsertParagraph(headlineText, false, headLineFormat);
                title.Alignment = Alignment.center;

                Paragraph scenes = doc.InsertParagraph("Scene " + scene, false, paraScene);
                scenes.Alignment = Alignment.left;

                Paragraph texts = doc.InsertParagraph(paraOne, false, paraFormat);

                // Save to the output directory:
                doc.Save();
            } else
            {
                // Create the document in memory:
                using (DocX doc = DocX.Load(@fileName))
                {

                    doc.ReplaceText("Novacode.DocX", "", true, RegexOptions.IgnoreCase);

                    doc.InsertParagraph(Environment.NewLine);

                    //string docText = doc.ToString() + txt;

                    var paraFormat = new Formatting();
                    paraFormat.FontFamily = new System.Drawing.FontFamily("Calibri");
                    paraFormat.Size = 9D;
                  
                    Paragraph scenes = doc.InsertParagraph("Scene " + scene, false, paraScene);
                    scenes.Alignment = Alignment.left;

                    //doc.InsertParagraph(docText, false, paraFormat);
                    doc.InsertParagraph(txt, false, paraFormat);

                    // Save to the output directory:
                    doc.Save();
                }               
            }
        }

        public void recordOptionMade(string fileName, string txt)
        {
            string pathFileName = @"C:/Users/parad/Desktop/StoryTelling1/StoryTelling1/bin/Debug/" + fileName;
            string paraOne = "" + txt;

            //Console.WriteLine("HERE ---- " + pathFileName + " || " + paraOne);

            //string curFile = @"c:\temp\test.txt";  //Your path
            if (File.Exists(pathFileName))
            {
                // Create the document in memory:
                using (DocX doc = DocX.Load(@fileName))
                {
                    doc.ReplaceText("Novacode.DocX", "", true, RegexOptions.IgnoreCase);

                    //string docText = doc.ToString() + txt;
                    //string docText = doc.ToString();

                    doc.InsertParagraph(Environment.NewLine);

                    var paraCondition = new Formatting();
                    paraCondition.FontFamily = new System.Drawing.FontFamily("Calibri");
                    paraCondition.Size = 9D;
                    paraCondition.Bold = true;

                    var paraFormat = new Formatting();
                    paraFormat.FontFamily = new System.Drawing.FontFamily("Calibri");
                    paraFormat.Size = 9D;                  

                    Paragraph decision = doc.InsertParagraph("Decision: ", false, paraCondition);
                    decision.Alignment = Alignment.left;

                    Paragraph paragraph = doc.InsertParagraph(txt, false, paraFormat);
                    paragraph.Alignment = Alignment.left;

                    //doc.InsertParagraph(decision.ToString(), false, paraFormat);

                    // Save to the output directory:
                    doc.Save();
                }
            }
        }
    }
}
