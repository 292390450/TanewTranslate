using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight.Threading;
using TranslationSpeech.Properties;

namespace TranslationSpeech
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            DispatcherHelper.Initialize();
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        }

        public App()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            //构建目录
            if (!Directory.Exists(basePath+@"\AudioCache"))
            {
                try
                {

                    Directory.CreateDirectory(basePath + @"\AudioCache");
                }
                catch (Exception e)
                {
                    
                }
            }
            if (!Directory.Exists(basePath + @"\RecordCache"))
            {
                try
                {

                    Directory.CreateDirectory(basePath + @"\RecordCache");
                }
                catch (Exception e)
                {
                    
                }
            }
            //创建日志文件
            if (!File.Exists(basePath + @"\log.txt"))
            {
                try
                {
                    File.Create(basePath + @"\log.txt");
                }
                catch (Exception e)
                {
                    App.PrinterException(e);
                }
            }
            //删除缓存
            if (Settings.Default.IsAutoDeleCatche)
            {
                try
                {
                    DirectoryInfo recorDirectory = Directory.CreateDirectory(basePath + @"\RecordCache");
                    var files = recorDirectory.GetFiles("*.wav");
                    foreach (var fileInfo in files)
                    {
                        File.Delete(fileInfo.FullName);
                    }
                    DirectoryInfo audioDirectory = Directory.CreateDirectory(basePath + @"\AudioCache");
                    var audios= audioDirectory.GetFiles("*.mp3");
                    foreach (var fileInfo in audios)
                    {
                        File.Delete(fileInfo.FullName);
                    }
                }
                catch (Exception e)
                {
                    
                }
            }
            //错误
            Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("explorer.exe ", AppDomain.CurrentDomain.BaseDirectory);
                string error = "\r\n" + ((Exception) e.ExceptionObject).Message + "\r\n 导致错误的对象名称:" +
                               ((Exception) e.ExceptionObject).Source + "\r\n 引发异常的方法:" +
                               ((Exception) e.ExceptionObject).TargetSite + "\r\n  帮助链接:" +
                               ((Exception) e.ExceptionObject).HelpLink + "\r\n 调用堆:" +
                               ((Exception) e.ExceptionObject).StackTrace;
                FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"log.txt", FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(error);
                sw.Flush();
                sw.Close();
                fs.Close();
            }
            catch (Exception exception)
            {

            }
        }

        public static void PrinterException(Exception e)
        {
            try
            {
                
                string error = "\r\n" + e.Message + "\r\n 导致错误的对象名称:" + e.Source +
                               "\r\n 引发异常的方法:" + e.TargetSite + "\r\n  帮助链接:" +
                               e.HelpLink + "\r\n 调用堆:" + e.StackTrace;
                FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"log.txt", FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(error);
                sw.Flush();
                sw.Close();
                fs.Close();
            }
            catch (Exception exception)
            {
                
            }
        }
        private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                e.Handled = true;
                System.Diagnostics.Process.Start("explorer.exe ", AppDomain.CurrentDomain.BaseDirectory);
                string error ="\r\n"+ e.Exception.Message + "\r\n 导致错误的对象名称:" + ((Exception) e.Exception).Source +
                               "\r\n 引发异常的方法:" + ((Exception) e.Exception).TargetSite + "\r\n  帮助链接:" +
                               ((Exception) e.Exception).HelpLink + "\r\n 调用堆:" + ((Exception) e.Exception).StackTrace;
                FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"log.txt", FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(error);
                sw.Flush();
                sw.Close();
                fs.Close();
            }
            catch (Exception exception)
            {
               
            }
        }
    }
}
