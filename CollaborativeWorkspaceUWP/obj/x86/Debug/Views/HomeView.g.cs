﻿#pragma checksum "D:\Personal\repos\CollaborativeWorkspaceUWP\CollaborativeWorkspaceUWP\Views\HomeView.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "32537D3700FB7FF8B902BBC485A4C4C91F5517B780FD90901C7469B4B4466502"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CollaborativeWorkspaceUWP.Views
{
    partial class HomeView : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 0.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1: // Views\HomeView.xaml line 1
                {
                    global::Windows.UI.Xaml.Controls.Page element1 = (global::Windows.UI.Xaml.Controls.Page)(target);
                    ((global::Windows.UI.Xaml.Controls.Page)element1).Loaded += this.Page_Loaded;
                }
                break;
            case 2: // Views\HomeView.xaml line 20
                {
                    this.RetractableMenuButton = (global::Windows.UI.Xaml.Style)(target);
                }
                break;
            case 3: // Views\HomeView.xaml line 29
                {
                    this.HomwButton = (global::Windows.UI.Xaml.Style)(target);
                }
                break;
            case 4: // Views\HomeView.xaml line 38
                {
                    this.ProjectViewButton = (global::Windows.UI.Xaml.Style)(target);
                }
                break;
            case 5: // Views\HomeView.xaml line 47
                {
                    this.TaskViewButton = (global::Windows.UI.Xaml.Style)(target);
                }
                break;
            case 6: // Views\HomeView.xaml line 56
                {
                    this.SprintViewButton = (global::Windows.UI.Xaml.Style)(target);
                }
                break;
            case 7: // Views\HomeView.xaml line 78
                {
                    this.TopBar = (global::Windows.UI.Xaml.Controls.StackPanel)(target);
                }
                break;
            case 8: // Views\HomeView.xaml line 96
                {
                    this.HomeViewFrame = (global::Windows.UI.Xaml.Controls.Frame)(target);
                }
                break;
            case 9: // Views\HomeView.xaml line 110
                {
                    this.TaskTitleIcon = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 10: // Views\HomeView.xaml line 113
                {
                    this.TaskTitle = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 11: // Views\HomeView.xaml line 82
                {
                    this.HomeViewBtn = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.HomeViewBtn).Click += this.HomeButton_Click;
                }
                break;
            case 12: // Views\HomeView.xaml line 85
                {
                    this.ProjectViewBtn = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.ProjectViewBtn).Click += this.ProjectViewButton_Click;
                }
                break;
            case 13: // Views\HomeView.xaml line 88
                {
                    this.TaskViewBtn = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.TaskViewBtn).Click += this.TaskViewButton_Click;
                }
                break;
            case 14: // Views\HomeView.xaml line 91
                {
                    this.SprintViewBtn = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.SprintViewBtn).Click += this.SprintViewButton_Click;
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 0.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

