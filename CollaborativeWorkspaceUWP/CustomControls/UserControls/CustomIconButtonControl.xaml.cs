using CollaborativeWorkspaceUWP.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    public sealed partial class CustomIconButtonControl : UserControl
    {

        private RoutedEventHandler ButtonClickEvent;

        public string ButtonIcon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public string ButtonContent
        {
            get { return (string)GetValue(ButtonContentProperty); }
            set { SetValue(ButtonContentProperty, value); }
        }

        public event RoutedEventHandler ButtonClick
        {
            add
            {
                ButtonClickEvent += value;
            }
            remove
            {
                ButtonClickEvent -= value;
            }
        }

        public ICommand ButtonCommand
        {
            get { return (ICommand)GetValue(ButtonCommandProperty); }
            set { SetValue(ButtonCommandProperty, value); }
        }

        public Object CommandParameter
        {
            get; set;
        }

        public int Width
        {
            get { return (int)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }

        public SolidColorBrush Background
        {
            get
            {
                return new SolidColorBrush(ButtonBackground);
            }
        }

        public Color ButtonBackground
        {
            get { return (Color)GetValue(ButtonBackgroundProperty); }
            set { SetValue(ButtonBackgroundProperty, value); }
        }

        public Style ButtonStyle
        {
            get { return (Style)GetValue(ButtonStyleProperty); }
            set { SetValue(ButtonStyleProperty, value); }
        }



        public bool IsButtonEnabled
        {
            get { return (bool)GetValue(IsButtonEnabledProperty); }
            set { SetValue(IsButtonEnabledProperty, value); }
        }

        public static readonly DependencyProperty ButtonContentProperty = DependencyProperty.Register("ButtonContent", typeof(string), typeof(CustomIconButtonControl), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("ButtonIcon", typeof(string), typeof(CustomIconButtonControl), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty ButtonCommandProperty = DependencyProperty.Register("ButtonCommand", typeof(ICommand), typeof(CustomIconButtonControl), new PropertyMetadata(null));

        public static readonly DependencyProperty WidthProperty = DependencyProperty.Register("Width", typeof(int), typeof(CustomIconButtonControl), new PropertyMetadata(0));

        public static readonly DependencyProperty ButtonBackgroundProperty = DependencyProperty.Register("ButtonBackground", typeof(Color), typeof(CustomIconButtonControl), null);

        public static readonly DependencyProperty ButtonStyleProperty = DependencyProperty.Register("ButtonStyle", typeof(Style), typeof(CustomIconButtonControl), null);

        public static readonly DependencyProperty IsButtonEnabledProperty = DependencyProperty.Register("IsButtonEnabled", typeof(bool), typeof(CustomIconButtonControl), null);

        public CustomIconButtonControl()
        {
            this.InitializeComponent();
        }
    }
}
