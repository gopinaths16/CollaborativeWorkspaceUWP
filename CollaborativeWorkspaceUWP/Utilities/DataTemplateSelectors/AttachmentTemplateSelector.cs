using CollaborativeWorkspaceUWP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CollaborativeWorkspaceUWP.Utilities
{
    public class AttachmentTemplateSelector : DataTemplateSelector
    {
        public DataTemplate PrimaryAttachmentTemplate { get; set; }
        public DataTemplate SecondaryAttachmentTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            Attachment attachment = (Attachment)item;
            if(attachment.IsOnlyForAddition)
            {
                return SecondaryAttachmentTemplate;
            }
            return PrimaryAttachmentTemplate;
        }
    }
}
