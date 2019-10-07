using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
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

namespace NugetPublisher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //public static ModalDialog modeldialog;
        public MainWindow()
        {
            InitializeComponent();
           
            modalDialog.SetParent(MainView);
            //splashScrean.SetParent(MainView);
           
        }


        private void btnFileBrowser_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "NugetFile Files|*.nupkg"}; ;
            if (openFileDialog.ShowDialog() == true)
                txtAddressNuget.Text = openFileDialog.FileName.Trim();
        }

        private void btnPublish_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtAddressNuget.Text)) 
                 MessageBox.Show(owner: GetWindow(this), "لطفا آدرس فایل Nuget را انتخاب کنید", "", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading);
                    
                else if (string.IsNullOrEmpty(txtServerAddress.Text))
                    MessageBox.Show(owner: GetWindow(this), "لطفا آدرس سرور Nuget را وارد کنید", "", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading);
                else if (string.IsNullOrEmpty(txtApiKey.Password))
                    MessageBox.Show(owner: GetWindow(this), "لطفا شناسه سرور Nuget را وارد کنید","", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading);
                else
                {
                    //  splashScrean.ShowHandlerDialog();
                      Wating();
                    string pathnuget = txtAddressNuget.Text;
                    string command = $"nuget push {pathnuget} -source {txtServerAddress.Text} {txtApiKey.Password}";

                    var s =Task.Run(() => RunScript(command)).Result;
                    //splashScrean.HideHandlerDialog();

                    if (string.IsNullOrEmpty(s.Trim()))
                    {
                        MessageBox.Show("خطا در اطلاعات ورودی، لطفا اطلاعات ورودی را بررسی کنید");
                    }
                    else
                        MessageBox.Show(s);

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(owner: GetWindow(this), "لطفا از فعال سازی سرویس Nuget  برروی سیستم خود اطمینان حاصل نمایید","", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading);
                
            }
            
        }
        private void OnDoWork(object o, DoWorkEventArgs args)
        {
            Task.Delay(2000).Wait(); // Pretend to work
        }

        private void OnRunWorkerCompleted(object o, RunWorkerCompletedEventArgs args)
        {
            loadingGif.Visibility = Visibility.Hidden;
        }
        private void Wating()
        {
            loadingGif.Visibility = Visibility.Visible;
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += OnDoWork;
            worker.RunWorkerCompleted += OnRunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        private string RunScript(string scriptText)
        {
            // create Powershell runspace
            Runspace runspace = RunspaceFactory.CreateRunspace();

            // open it
            runspace.Open();

            // create a pipeline and feed it the script text
            Pipeline pipeline = runspace.CreatePipeline();
            pipeline.Commands.AddScript(scriptText);

            // add an extra command to transform the script output objects into nicely formatted strings
            // remove this line to get the actual objects that the script returns. For example, the script
            // "Get-Process" returns a collection of System.Diagnostics.Process instances.
            pipeline.Commands.Add("Out-String");

            // execute the script
            Collection<PSObject> results = pipeline.Invoke();

            // close the runspace
            runspace.Close();

            // convert the script result into a single string
            StringBuilder stringBuilder = new StringBuilder();
            foreach (PSObject obj in results)
            {
                stringBuilder.AppendLine(obj.ToString());
            }

            return stringBuilder.ToString();
        }
        private void ShowModalDialog_Click(object sender, RoutedEventArgs e)
        {
            var res = modalDialog.ShowHandlerDialog();
          
          
        }

        private void txtAddressNuget_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Note that you can have more than one file.
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                MessageBox.Show( files[0]); 
                // Assuming you have one file that you care about, pass it off to whatever
                // handling code you have defined.

            }
        }
        private void txtAddressNuget_PreviewDragEnter(object sender, DragEventArgs e)
        {
           
        }

        private void txtAddressNuget_PreviewDrop(object sender, DragEventArgs e)
        {
            object text = e.Data.GetData(DataFormats.FileDrop);

            txtAddressNuget.Text = string.Format("{0}", ((string[])text)[0]);
            
        }
    }
}
