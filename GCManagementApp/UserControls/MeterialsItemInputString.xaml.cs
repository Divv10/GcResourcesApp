using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for MeterialsItemInputString.xaml
    /// </summary>
    public partial class MeterialsItemInputString : UserControl
    {
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(MeterialsItemInputString));
        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register("ImageSource", typeof(string), typeof(MeterialsItemInputString));
        public static readonly DependencyProperty ItemCountProperty = DependencyProperty.Register("ItemCount", typeof(string), typeof(MeterialsItemInputString));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public string ImageSource
        {
            get => (string)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        public string ItemCount
        {
            get => (string)GetValue(ItemCountProperty);
            set => SetValue(ItemCountProperty, value);
        }

        public MeterialsItemInputString()
        {
            InitializeComponent();
        }
    }
}
