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
    public class ProjectListViewTemplateSelector : DataTemplateSelector 
    {
        public DataTemplate ProjectItemTemplate { get; set; }
        public DataTemplate GroupItemTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            if (item is Project)
            {
                return ProjectItemTemplate;
            }
            return GroupItemTemplate;
        }
    }
}
