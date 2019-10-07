using System.Windows;
using System.Windows.Input;

namespace NugetPublisher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class WinMess : Window
    {
        
        public WinMess()
        {
            InitializeComponent();
        }

        private void txtmass_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            
           if (e.ButtonState == MouseButtonState.Pressed)
            {
               this.DragMove();
            }
        }

        private void txtmsg_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // this.Height = e.NewSize.Height - 54 + 140;
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void txtbc_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

       
       

        
	}
}
