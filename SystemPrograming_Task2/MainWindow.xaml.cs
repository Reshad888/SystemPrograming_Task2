using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace SystemPrograming_Task2
{
    public partial class MainWindow : Window
    {
        public string fromPath = "";
        public string toPath = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonFileFrom_Click(object sender, RoutedEventArgs e)
        {
            fromPath = File_Dialog(TextBoxFrom);
        }

        private void ButtonFileTo_Click(object sender, RoutedEventArgs e)
        {
            toPath = File_Dialog(TextBoxTo);
        }

        public string File_Dialog(TextBox tb)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = ".|*.txt|.|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string file = openFileDialog.FileName;
                tb.Text = file;
                return file;
            }
            return "";
        }

        Thread thread1;

        private void ButtonCopy_Click(object sender, RoutedEventArgs e)
        {
            thread1 = new Thread(() =>
            {
                if (fromPath != "" && toPath != "")
                {
                    using (FileStream fsRead = new FileStream(fromPath, FileMode.Open, FileAccess.Read))
                    {
                        Dispatcher.Invoke(() => ProgBar.Maximum = fsRead.Length);

                        using (FileStream fsWrite = new FileStream(toPath, FileMode.Create, FileAccess.Write))
                        {
                            var len = 10;
                            var fileSize = fsRead.Length;
                            byte[] buffer = new byte[len];

                            do
                            {
                                Thread.Sleep(10);
                                len = fsRead.Read(buffer, 0, buffer.Length);
                                fsWrite.Write(buffer, 0, len);

                                fileSize -= len;

                                Dispatcher.Invoke(() => ProgBar.Value += 10);
                            } while (len != 0);
                        }
                    }
                }
            });

            thread1.Start();
        }

        private void ButtonSuspend_Click(object sender, RoutedEventArgs e)
        {
            thread1.Suspend();
        }

        private void ButtonResume_Click(object sender, RoutedEventArgs e)
        {
            thread1.Resume();
        }

        private void ButtonAbort_Click(object sender, RoutedEventArgs e)
        {
            thread1.Abort();
        }
    }
}