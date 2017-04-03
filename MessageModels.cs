using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thalamus;
using Thalamus.BML;

namespace StoryTelling1
{
    public interface IMessageActions : Thalamus.IAction
    {
        void ClientMessage(string msg);
    }

    public interface IFMLSpeech : Thalamus.IAction
    {
        //The id is used in case you need to listen for the UtteranceStarted and UtteranceFinished events.
        //If you don't need it, id can be ""
        //The tagNames and TagValues should have the same dimension, 
        //and in case you have tags that should be replaced in the utterance (like 'playerName'), 
        //then the name and value should correspond to the same index in the arrays
        void PerformUtterance(string id, string utterance, string category);
        void PerformUtteranceFromLibrary(string id, string category, string subcategory, string[] tagNames, string[] tagValues);
        void CancelUtterance(string id);
    }
}
