using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;

namespace UiTools.Av.ViewModels
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
            var msg = Create(StackableMessageSeverity.Error, "Error", $"Too many messages to display! Max={MaxMessages}", SInternalError);
            msg.IsOpen = false;
            Messages.Add(msg);
        }

        public void ClearMessage(string id)
        {
            var msg = Messages.FirstOrDefault(x => x.Id == id);
            if (msg != null) msg.IsOpen = false;
        }

        public void AddError(string title, string message, string? id = null)
        {
            AddOrReplace(Create(StackableMessageSeverity.Error, title, message, id));
        }
        public void AddWarning(string title, string message, string? id = null)
        {
            AddOrReplace(Create(StackableMessageSeverity.Warning, title, message, id));
        }

        public void AddInfo(string title, string message, string? id = null)
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
        private const int MaxMessages = 10;

        private void ShowOverloadError(bool show)
        {
            var error = Messages.FirstOrDefault(x => x.Id == SInternalError);
            if (error != null) { error.IsOpen = show; }
        }

        private void AddOrReplace(StackableMessage message)
        {
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

                if (Messages.Count <= MaxMessages)
                {
                    Messages.Add(message);
                }
                else
                {
                    ShowOverloadError(true);
                }

            }

            //cleaning up ones closed by user
            var toRemove = Messages.Where(x => x.Id != SInternalError && !x.IsOpen).ToList();
            foreach (var messageToRemove in toRemove) Messages.Remove(messageToRemove);

            if (Messages.Count < MaxMessages) ShowOverloadError(false);



        }

    }

}
