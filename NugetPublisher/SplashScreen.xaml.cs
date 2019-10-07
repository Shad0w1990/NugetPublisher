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
using System.Windows.Shapes;

namespace NugetPublisher
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : UserControl
    {
        public SplashScreen()
        {
            InitializeComponent();
            Visibility = Visibility.Hidden;
        }
        private UIElement _parent;

        public void SetParent(UIElement parent)
        {
            _parent = parent;
        }
        public bool ShowHandlerDialog()
        {

            Visibility = Visibility.Visible;
            Focusable = true;
            _parent.IsEnabled = false;

            return true;
        }
        public bool HideHandlerDialog()
        {

            Visibility = Visibility.Hidden;
            Focusable = false;
            _parent.IsEnabled = true;

            return true;
        }
    }
}
