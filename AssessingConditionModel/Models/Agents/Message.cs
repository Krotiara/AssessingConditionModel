using System.Collections.Generic;

namespace AssessingConditionModel.Models.Agents
{
    public enum MessageType
    {
        FromAgent,
        FromSystem
    }

    public class Message
    {

        public Message(string from, string to, string text, string[] args)
        {
            From = from;
            To = to;
            Text = text;
            Args = args;
        }

        public Message(string from, string to, string text)
        {
            From = from;
            To = to;
            Text = text;
            Args = new List<string>().ToArray();
        }

        //public MessageType MessageType { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Text { get; set; }
        public string[] Args { get; set; }

    }
}
