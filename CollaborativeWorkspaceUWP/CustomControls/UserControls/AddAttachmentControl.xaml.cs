using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class AddAttachmentControl : UserControl
    {
        private RoutedEventHandler cancelButtonClickEventHandler;
        private RoutedEventHandler addButtonClickEventHandler;

        private AddAttachmentViewModel addAttachmentViewModel;

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

        public AddAttachmentControl()
        {
            this.InitializeComponent();

            addAttachmentViewModel = new AddAttachmentViewModel();
        }

        private void AddAttachmentDialog_Click(object sender, RoutedEventArgs e)
        {
            addAttachmentViewModel.AddAttachmentsToTask();
            addButtonClickEventHandler?.Invoke(sender, e);
        }

        private void CloseAttachmentDialogButton_Click(object sender, RoutedEventArgs e)
        {
            cancelButtonClickEventHandler?.Invoke(sender, e);
        }

        public void SetCurrTask(UserTask task)
        {
            addAttachmentViewModel.SetCurrTask(task);
        }

        private async void AddAttachmentButton_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                addAttachmentViewModel.AddAttachmentToList(file);
            }
        }

        private void DeleteAttachmentFromListButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
