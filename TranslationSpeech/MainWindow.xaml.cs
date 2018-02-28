using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MusicPlayer;
using TranslationSpeech.Helper;
using TranslationSpeech.ViewModel;
using System.Windows.Forms;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using Size = System.Windows.Size;

namespace TranslationSpeech
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Windows.Forms.NotifyIcon notifyIcon;
        public MainWindow(MainViewModel viewModel)
        {
            this.DataContext = viewModel;
            InitializeComponent();
           viewModel.AudioPlayer=new MusicPlayer.MusicPlayer(new WindowInteropHelper(this).Handle);
            SystemTrayParameter pars = new SystemTrayParameter("翻译", "", 0, notifyIcon_MouseDoubleClick);
            this.notifyIcon = WPFSystemTray.SetSystemTray(pars, GetList());
        }
        public static byte[] BitMapImageToByteArray(BitmapImage bmp)
        {
            byte[] bytearray = null;
            try
            {
                Stream smarket = bmp.StreamSource; ;
                if (smarket != null && smarket.Length > 0)
                {
                    //设置当前位置
                    smarket.Position = 0;
                    using (BinaryReader br = new BinaryReader(smarket))
                    {
                        bytearray = br.ReadBytes((int)smarket.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                App.PrinterException(ex);
            }
            return bytearray;
        }


        /// <summary>
        /// 字节转成图片
        /// </summary>
        /// <param name="fullpath"></param>
        /// <returns></returns>
        public static Bitmap ConvertByteToImg(byte[] bytes)
        {
            Bitmap img = null;
            try
            {
                if (bytes != null && bytes.Length != 0)
                {
                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        img = new Bitmap(ms);
                    }
                    return img;
                }
            }
            catch (Exception ex)
            {
                App.PrinterException(ex);
            }
            return img;
        }


        void exit_Click(object sender, EventArgs e)
     {
            this.Close();
         notifyIcon.Visible = false;
           System.Windows.Application.Current.Shutdown();
     }

        private List<SystemTrayMenu> GetList()
         {
             List<SystemTrayMenu> ls = new List<SystemTrayMenu>();
            ls.Add(new SystemTrayMenu() { Txt = "打开主面板", Icon = "", Click = mainWin_Click });
             ls.Add(new SystemTrayMenu() { Txt = "退出", Icon = "/img/exit.png", Click = exit_Click });
              return ls;
        }
        void mainWin_Click(object sender, EventArgs e)
        {
          this.Show();
            //this.notifyIcon.Visible = false;
        }

        void notifyIcon_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            this.Show();
            this.WindowState = WindowState.Normal;
            // this.notifyIcon.Visible = false;
        }

        private void PART_MaxButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Maximized;
            PART_MaxButton.Visibility = Visibility.Collapsed;
            PART_RestoreButton.Visibility = Visibility.Visible;

        }

        private void PART_RestoreButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Normal;
            PART_RestoreButton.Visibility = Visibility.Collapsed;
            PART_MaxButton.Visibility = Visibility.Visible;
        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
            
        }

        private void MainWindow_OnLayoutUpdated(object sender, EventArgs e)
        {
            this.Clip=new RectangleGeometry(new Rect(new Size(Width,Height)),3,3);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void closeWindow(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        
    }
}
