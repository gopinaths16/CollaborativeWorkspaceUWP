using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using CollaborativeWorkspaceUWP.Utilities.Events;
using CollaborativeWorkspaceUWP.Utilities;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CollaborativeWorkspaceUWP.CustomControls.UserControls
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CommentControl : Page
    {
        CommentViewModel commentViewModel;

        public CommentControl()
        {
            this.InitializeComponent();
            commentViewModel = new CommentViewModel();
            CommentAttachments.SetAddAttachmentContext();
        }

        public void SetCurrTask(UserTask currTask)
        {
            commentViewModel.SetCurrTask(currTask);
            if(currTask != null)
            {
                CommentAttachments.SetCurrTaskId(currTask.Id);
            }
        }

        private async void AddAttachmentButton_Click(object sender, RoutedEventArgs e)
        {
            await CommentAttachments.PickAndAddAttachment();
            CommentTextBox.Focus(FocusState.Programmatic);
            VisualStateManager.GoToState(this, "Focused", true);
        }

        private void StackPanel_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if(CommentTextBox.FocusState == FocusState.Unfocused)
            {
                VisualStateManager.GoToState(this, "PointerOver", true);
            }
        }

        private void StackPanel_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if(CommentTextBox.FocusState == FocusState.Unfocused)
            {
                VisualStateManager.GoToState(this, "Normal", true);
            }
        }

        private void StackPanel_GotFocus(object sender, RoutedEventArgs e)
        {
            CommentTextBox.Focus(FocusState.Programmatic);
            VisualStateManager.GoToState(this, "Focused", true);
        }

        private void CommentBox_LostFocus(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Normal", true);
        }

        private async void AddCommentButton_Click(object sender, RoutedEventArgs e)
        {
            Comment comment = commentViewModel.AddCommentToCurrTask();
            comment.Attachments = await CommentAttachments.AddAttachmentsForComment(comment.Id);
            await commentViewModel.NotifyCommentAddition(comment);
            CommentAttachments.SetAttachments(new ObservableCollection<Attachment>());
            CommentAttachments.SetCommentId(comment.Id);
        }
    }
}
