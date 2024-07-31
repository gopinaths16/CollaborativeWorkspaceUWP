using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CollaborativeWorkspaceUWP.CustomControls.UserControls
{
    public sealed partial class AttachmentCotrol : UserControl
    {
        private RoutedEventHandler cancelButtonClickEventHandler;
        private RoutedEventHandler addButtonClickEventHandler;

        private AttachmentViewModel attachmentViewModel;

        public event RoutedEventHandler CancelButtonClick
        {
            add { cancelButtonClickEventHandler += value; }
            remove { cancelButtonClickEventHandler -= value; }
        }

        public event RoutedEventHandler AddButtonClick
        {
            add { addButtonClickEventHandler += value; }
            remove { addButtonClickEventHandler -= value; }
        }

        public bool AllowAdditionFromUi
        {
            get { return (bool)GetValue(AllowAdditionFromUiProperty); }
            set { SetValue(AllowAdditionFromUiProperty, value); }
        }

        public static readonly DependencyProperty AllowAdditionFromUiProperty = DependencyProperty.Register("AllowAdditionFromUi", typeof(bool), typeof(AttachmentCotrol), new PropertyMetadata(true));

        public AttachmentCotrol()
        {
            this.InitializeComponent();

            attachmentViewModel = new AttachmentViewModel();
        }

        private void CloseAttachmentDialogButton_Click(object sender, RoutedEventArgs e)
        {
            attachmentViewModel.ClearAttachmentList();
            cancelButtonClickEventHandler?.Invoke(sender, e);
        }

        public void SetCurrTask(UserTask task)
        {
            attachmentViewModel.SetCurrTask(task);
        }

        public void SetCurrTaskId(long taskId)
        {
            attachmentViewModel.CurrTask.Id = taskId;
        }

        public void SetAddAttachmentContext()
        {
            attachmentViewModel.SetCurrTask(new UserTask());
        }

        private async void AddAttachmentButton_Click(object sender, RoutedEventArgs e)
        {
            await PickAndAddAttachment();
        }

        public async Task PickAndAddAttachment()
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            picker.FileTypeFilter.Add(".pdf");
            picker.FileTypeFilter.Add(".txt");

            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                attachmentViewModel.AddAttachmentToTask(file);
            }
        }

        private void DeleteAttachmentFromListButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Attachment attachment = (Attachment)button.Tag;
            attachmentViewModel.DeleteAttachmentFromList(attachment);
        }

        private async void AttachmentListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Attachment attachment = (Attachment)e.ClickedItem;
            await DefaultLaunch(attachment);
        }

        private async Task DefaultLaunch(Attachment attachment)
        {
            try
            {
                StorageFolder storageFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync("Attachments");
                var file = await storageFolder.GetFileAsync(attachment.Path);

                if (file != null)
                {
                    var success = await Windows.System.Launcher.LaunchFileAsync(file);

                    if (success)
                    {
                    }
                    else
                    {
                    }
                }
            }
            catch(Exception e)
            {

            }
        }

        public void AddAttachmentsForComment(long commentId)
        {
            attachmentViewModel.AddAttachmentForComment(commentId);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            attachmentViewModel.AdditionAllowedFromUI = AllowAdditionFromUi;
        }
    }
}
