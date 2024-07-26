using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.Utilities;
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
        private Attachment attachment;

        public AttachmentItemControl()
        {
            this.InitializeComponent();
        }

        private void Grid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "PointerOver", true);
        }

        private void Grid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Normal", true);
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            attachment = this.DataContext as Attachment;
            await SetPreviewItem();
        }

        private async Task SetPreviewItem()
        {
            if(attachment != null)
            {
                switch(attachment.Type)
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
                        bitmapImage.UriSource = new Uri(ApplicationData.Current.LocalFolder.Path + Path.DirectorySeparatorChar + "Attachments" + Path.DirectorySeparatorChar + attachment.Path);
                        PreviewImage.Source = bitmapImage;
                        break;

                }
            }
        }
    }
}
