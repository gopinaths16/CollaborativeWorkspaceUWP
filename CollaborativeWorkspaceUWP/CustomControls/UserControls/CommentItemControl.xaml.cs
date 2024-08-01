using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CollaborativeWorkspaceUWP.CustomControls.UserControls
{
    public sealed partial class CommentItemControl : UserControl
    {
        CommentItemViewModel commentItemViewModel;

        public Comment Comment
        {
            get { return (Comment)GetValue(CommentProperty); }
            set { SetValue(CommentProperty, value); }
        }

        public static readonly DependencyProperty CommentProperty = DependencyProperty.Register("Comment", typeof(Comment), typeof(CommentItemControl), new PropertyMetadata(null));

        public CommentItemControl()
        {
            this.InitializeComponent();

            commentItemViewModel = new CommentItemViewModel();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Comment comment = commentItemViewModel.SetComment(Comment);
            CommentAttachmentDialog.SetCurrTask(new UserTask() { Id = comment.TaskId });
            CommentAttachmentDialog.SetAttachments(comment.Attachments);
        }
    }
}
