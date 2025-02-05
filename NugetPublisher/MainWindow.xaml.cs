﻿using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NugetPublisher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static WinMess messageBox;
        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            messageBox = new WinMess();
            messageBox.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //modalDialog.SetParent(MainView);
            //splashScrean.SetParent(MainView);

        }


        private void btnFileBrowser_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "NugetFile Files|*.nupkg" }; ;
            if (openFileDialog.ShowDialog() == true)
                txtAddressNuget.Text = openFileDialog.FileName.Trim();
        }

        private void btnPublish_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtAddressNuget.Text))
                    MessageBox.Show(owner: GetWindow(this), "لطفا آدرس فایل Nuget را انتخاب کنید", "", 
                        MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading);

                else if (string.IsNullOrEmpty(txtServerAddress.Text))
                    MessageBox.Show(owner: GetWindow(this), "لطفا آدرس سرور Nuget را وارد کنید", "",
                        MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading);
                else if (string.IsNullOrEmpty(txtApiKey.Password))
                    MessageBox.Show(owner: GetWindow(this), "لطفا شناسه سرور Nuget را وارد کنید", "",
                        MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading);
                else
                {
                    //  splashScrean.ShowHandlerDialog();
                    SplashScreen SS = new SplashScreen("Assets/wait.jpg");
                    SS.Show(false);



                    string pathnuget = txtAddressNuget.Text;
                    string command = $"nuget push {pathnuget} -source {txtServerAddress.Text} {txtApiKey.Password}";

                    var s = Task.Run(() => RunScript(command)).Result;
                    //splashScrean.HideHandlerDialog();
                    SS.Close(TimeSpan.FromSeconds(1));
                    txtAddressNuget.Text = string.Empty;
                    if (string.IsNullOrEmpty(s.Trim()))
                    {
                        MessageBox.Show("خطا در اطلاعات ورودی، لطفا اطلاعات ورودی را بررسی کنید");
                    }
                    else
                    {
                        MessageBox.Show(s);
                        txtAddressNuget.Text = "";
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(owner: GetWindow(this), "لطفا از فعال سازی سرویس Nuget  برروی سیستم خود اطمینان حاصل نمایید", "", 
                    MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading);

            }

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
            //var res = modalDialog.ShowHandlerDialog();
            messageBox.Show();

        }


        private void txtAddressNuget_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
                if (files != null && files.Length > 0)
                {
                    if (!files[0].EndsWith(".nupkg"))
                    {
                        ((TextBox)sender).Text = "";
                        MessageBox.Show("فقط فایل Nuget مجاز می باشد");
                    }
                    else
                        ((TextBox)sender).Text = files[0];
                }
            }
        }

        private void txtAddressNuget_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }


        private void txtAddressNuget_PreviewDrop(object sender, DragEventArgs e)
        {
            object text = e.Data.GetData(DataFormats.FileDrop);

            txtAddressNuget.Text = string.Format("{0}", ((string[])text)[0]);

        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            messageBox.Close();
        }

        private void txtAddressNuget_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
