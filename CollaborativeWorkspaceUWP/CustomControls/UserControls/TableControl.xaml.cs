using CollaborativeWorkspaceUWP.Models;
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
    public sealed partial class TableControl : UserControl
    {
        public static readonly DependencyProperty Temp = DependencyProperty.Register(nameof(TableTemplate), typeof(DataTemplate), typeof(TableControl), new PropertyMetadata(default(DataTemplate)));
        public static readonly DependencyProperty Source = DependencyProperty.Register(nameof(TableSource), typeof(List<UserTask>), typeof(TableControl), new PropertyMetadata(default(List<UserTask>)));

        public DataTemplate TableTemplate
        {
            get => (DataTemplate)GetValue(Temp);
            set => SetValue(Temp, value);
        }

        public List<UserTask> TableSource
        {
            get => (List<UserTask>)GetValue(Source);
            set => SetValue(Source, value);
        }

        public UserTask Test { get; set; }

        public TableControl()
        {
            this.InitializeComponent();
        }
    }
}
