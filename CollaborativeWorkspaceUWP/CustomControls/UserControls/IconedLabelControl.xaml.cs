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
    public sealed partial class IconedLabelControl : UserControl
    {

        public string Icon
        {
            get { return (string)GetValue(LabelIconProperty); }
            set { SetValue(LabelIconProperty, value); }
        }

        public string ContentText
        {
            get { return (string)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public static readonly DependencyProperty LabelIconProperty = DependencyProperty.Register("Icon", typeof(string), typeof(IconedLabelControl), null);

        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("ContentText", typeof(string), typeof(IconedLabelControl), null);


        public IconedLabelControl()
        {
            this.InitializeComponent();
        }
    }
}
