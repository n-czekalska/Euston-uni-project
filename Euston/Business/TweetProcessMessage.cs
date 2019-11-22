using Euston.Data.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Euston.Business
{

    [Export(typeof(IProcessMessage))]
    [ExportMetadata("MessageMetaData", "Tweet")]
    public class TweetProcessMessage : IProcessMessage
    {

        private ObservableCollection<Hashtags> Hashtags = new ObservableCollection<Hashtags>();
        public ObservableCollection<Hashtags> ListHashtags
        {
            get { return Hashtags; }
        }

        private readonly ObservableCollection<TwitterID> IDs = new ObservableCollection<TwitterID>();
        public ObservableCollection<TwitterID> ListIDs
        {
            get { return IDs; }
        }

        private static TweetProcessMessage instance;
        public static TweetProcessMessage Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TweetProcessMessage();
                }
                return instance;
            }

        }

        CommonProcesses processes = new CommonProcesses();

        public List<string> ProcessMessage(string messageId, string messageBody)
        {

            List<string> tweetBody = new List<string>();
            string[] splitMessage;


            CheckTrending(messageBody);
            messageBody = processes.ExpandTextWord(messageBody);
            splitMessage = processes.SplitAndCleanBody(messageBody);
            CheckMentions(splitMessage);

            foreach (string line in splitMessage)
            {
                tweetBody.Add(line);
            }
            SaveMessage(messageId, tweetBody);
            return tweetBody;
        }

        private void CheckTrending(string tweetBody)
        {
            Regex regex = new Regex(@"(?<=\s|^)#(\w*[A-Za-z_]+\w*)");
            var matches = regex.Matches(tweetBody);

            foreach (Match item in matches)
            {
                Hashtags tag = Instance.Hashtags.FirstOrDefault(x => item.Value == x.Hashtag);
                if (tag != null)
                {
                    var index = Instance.Hashtags.IndexOf(tag);
                    tag.Count += 1;
                    Instance.Hashtags[index] = tag;

                }
                else
                {
                    Instance.Hashtags.Add(new Hashtags(item.Value, 1));
                }
            }
        }

        private void CheckMentions(string[] tweetBody)
        {
            string tweet = "";
            for (int i = 1; i < tweetBody.Length; i++)
            {
                tweet += tweetBody[i];
            }

            Regex regex = new Regex(@"(?<=\s|^)@(\w*[A-Za-z_]+\w*)");
            var matches = regex.Matches(tweet);

            foreach (Match item in matches)
            {
                TwitterID Id = Instance.IDs.FirstOrDefault(x => item.Value == x.Id);
                if (Id == null)
                {
                    Instance.IDs.Add(new TwitterID(item.Value));
                }
            }
        }

        public List<string> RunValidation(string messageBody)
        {
            string[] message = processes.SplitAndCleanBody(messageBody);

            var result = new List<string>();

            if (message[0].Length > 16)
            {
                result.Add("TwittedID is too long");
                return result;
            }

            int messageLenght = 0;
            for (int i = 1; i < message.Length; i++)
            {
                messageLenght += message[i].Length;
            }
            if (messageLenght > 140)
            {
                result.Add("Message too long");
                return result;
            }
            return result;
        }

        public void SaveMessage(string messageId, List<string> tweetBody)
        {
            JSONHandler.Write(messageId, tweetBody);
        }
    }
}
