﻿#pragma checksum "..\..\..\Login.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "5A8B504E82150EDA98079BA8560C95C8ED5F764A"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using ClientApp;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace ClientApp {
    
    
    /// <summary>
    /// Login
    /// </summary>
    public partial class Login : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\..\Login.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid LoginGrid;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\Login.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid TopBar;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\Login.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Server;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\Login.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Username;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\Login.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock fehler;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\Login.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid loading;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\..\Login.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Media.RotateTransform rotateTransform;
        
        #line default
        #line hidden
        
        
        #line 82 "..\..\..\Login.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Media.RotateTransform rotateTransform1;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ClientApp;component/login.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Login.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.LoginGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            this.TopBar = ((System.Windows.Controls.Grid)(target));
            
            #line 16 "..\..\..\Login.xaml"
            this.TopBar.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Top_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 3:
            this.Server = ((System.Windows.Controls.TextBox)(target));
            
            #line 36 "..\..\..\Login.xaml"
            this.Server.LostFocus += new System.Windows.RoutedEventHandler(this.Server_LostFocus);
            
            #line default
            #line hidden
            
            #line 36 "..\..\..\Login.xaml"
            this.Server.PreviewMouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Server_PreviewMouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 4:
            this.Username = ((System.Windows.Controls.TextBox)(target));
            
            #line 37 "..\..\..\Login.xaml"
            this.Username.LostFocus += new System.Windows.RoutedEventHandler(this.Username_LostFocus);
            
            #line default
            #line hidden
            
            #line 37 "..\..\..\Login.xaml"
            this.Username.PreviewMouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Username_PreviewMouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 38 "..\..\..\Login.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.fehler = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 7:
            this.loading = ((System.Windows.Controls.Grid)(target));
            return;
            case 8:
            this.rotateTransform = ((System.Windows.Media.RotateTransform)(target));
            return;
            case 9:
            this.rotateTransform1 = ((System.Windows.Media.RotateTransform)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

