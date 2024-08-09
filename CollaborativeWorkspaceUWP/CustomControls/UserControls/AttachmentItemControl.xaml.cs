using CollaborativeWorkspaceUWP.DAL;
using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.Utilities;
using CollaborativeWorkspaceUWP.Utilities.Events;
using CollaborativeWorkspaceUWP.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CollaborativeWorkspaceUWP.CustomControls.UserControls
{
    public sealed partial class AttachmentItemControl : UserControl
    {
        AttachmentItemViewModel attachmentItemViewModel;

        public Attachment Attachment
        {
            get { return (Attachment)GetValue(AttachmentProperty); }
            set { SetValue(AttachmentProperty, value); }
        }

        public static readonly DependencyProperty AttachmentProperty = DependencyProperty.Register("Attachment", typeof(Attachment), typeof(AttachmentItemControl), new PropertyMetadata(null));

        public AttachmentItemControl()
        {
            this.InitializeComponent();
            attachmentItemViewModel = new AttachmentItemViewModel();
        }

        private void Grid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Grid.SetColumnSpan(AttachmentName, 2);
            VisualStateManager.GoToState(this, "PointerOver", true);
        }

        private void Grid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            Grid.SetColumnSpan(AttachmentName, 4);
            VisualStateManager.GoToState(this, "Normal", true);
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await SetPreviewItem();
        }

        private async Task SetPreviewItem()
        {
            if(Attachment != null)
            {
                switch(Attachment.Type)
                {
                    case "text/plain":
                        IconElement.Visibility = Visibility.Visible;
                        IconElement.Glyph = "\ue922";
                        IconElement.Foreground = Util.CreateBrushFromHex("#4593fb");
                        break;

                    case "application/pdf":
                        IconElement.Visibility = Visibility.Visible;
                        IconElement.Glyph = "\ueadf";
                        IconElement.Foreground = Util.CreateBrushFromHex("#f70000");
                        break;

                    case "image/png":
                    case "image/jpeg":
                    case "image/jpg":
                        PreviewImage.Visibility = Visibility.Visible;
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.UriSource = new Uri(ApplicationData.Current.LocalFolder.Path + Path.DirectorySeparatorChar + "Attachments" + Path.DirectorySeparatorChar + Attachment.Path);
                        PreviewImage.Source = bitmapImage;
                        break;

                }
            }
        }

        private async void DeleteAttachmentButton_Click(object sender, RoutedEventArgs e)
        {
            await attachmentItemViewModel.DeleteAttachment(Attachment);
            await ViewmodelEventHandler.Instance.Publish(new DeleteAttachmentEvent() { Attachment = Attachment });
        }
    }
}
