using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;

namespace UiTools.Uno.My.ViewModels
{


    public enum StackableMessageSeverity
    {
        Informational,
        Success,
        Warning,
        Error
    }



    /// <summary>
    /// Holds a list of messages designed to be bound to a ListView of InforBars or similar.
    /// If a message has an ID and ther is already a message with the same ID in list the new message replaces the older one instead of using new slot.
    /// This allows not to use up all screen in case of periodic errors.
    /// </summary>
    public partial class MessageStack : ObservableObject
    {

        [ObservableProperty]
        private ObservableCollection<StackableMessage> _messages = new();

        public MessageStack()
        {
            var msg = Create(StackableMessageSeverity.Error, "Error", $"Too many messages to display! Max={_maxMessages}", SInternalError);
            msg.IsOpen = false;
            Messages.Add(msg);
        }

        public void ClearMessage(string id)
        {
            var msg = Messages.FirstOrDefault(x => x.Id == id);
            if (msg != null) msg.IsOpen = false;
        }

        public void AddError(string title, string message, string? id = default)
        {
            AddOrReplace(Create(StackableMessageSeverity.Error, title, message, id));
        }
        public void AddWarning(string title, string message, string? id = default)
        {
            AddOrReplace(Create(StackableMessageSeverity.Warning, title, message, id));
        }

        public void AddInfo(string title, string message, string? id = default)
        {
            AddOrReplace(Create(StackableMessageSeverity.Informational, title, message, id));
        }

        private StackableMessage Create(StackableMessageSeverity severity, string title, string message, string? id)
        {
            if (string.IsNullOrWhiteSpace(id)) id = Guid.NewGuid().ToString();
            return (new StackableMessage { Severity = severity, Id = id, MessageText = message, Title = title, IsOpen = true }); ;
        }


        private string PrefixedMessage(string? message)
        {
            return $"[{DateTime.Now}]: {message}";
        }


        private const string SInternalError = ">>internal_err<<";
        private const int _maxMessages = 10;

        private void ShowOverloadError(bool show)
        {
            var error = Messages.FirstOrDefault(x => x.Id == SInternalError);
            if (error != null) { error.IsOpen = show; }
        }

        private void AddOrReplace(StackableMessage message)
        {

            
            //todo clean connection error
            //todo "too many"

            var existing = Messages.FirstOrDefault(x => x.Id == message.Id);  // && !x.IsClosed
            if (existing != null)
            {
                existing.Severity = message.Severity;
                existing.MessageText = PrefixedMessage(message.MessageText);
                existing.Title = message.Title;
                existing.IsOpen = true;
            }
            else
            {                
                message.MessageText = PrefixedMessage(message.MessageText);

                if (Messages.Count <= _maxMessages) 
                { 
                    Messages.Add(message); 
                }
                else
                {
                    ShowOverloadError(true);
                }
                
            }

            //todo clean
            //cleaning up ones closed by user
            var toRemove = Messages.Where(x => x.Id != SInternalError && !x.IsOpen).ToList();
            foreach (var messageToRemove in toRemove) Messages.Remove(messageToRemove);

            if (Messages.Count < _maxMessages) ShowOverloadError(false);



        }

    }
}
