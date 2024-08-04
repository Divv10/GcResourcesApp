using GCManagementApp.Models;
using GCManagementApp.Static;
using GCManagementApp.ViewModels;
using GongSolutions.Wpf.DragDrop;
using Microsoft.Practices.Prism;
using PixelLab.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GCManagementApp.UserControls
{
    /// <summary>
    /// Interaction logic for TeamBuilderUserControl.xaml
    /// </summary>
    public partial class TeamBuilderUserControl : UserControl
    {     
        public TeamBuilderUserControl()
        {
            InitializeComponent();
            DataContext = new TeamBuilderViewModel();            
        }        
    }
}
