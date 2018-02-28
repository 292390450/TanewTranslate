using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using TranslateLibrary;
using TranslationSpeech.Helper;

namespace TranslationSpeech.ViewModel
{
    public class DaySentenceViewModel:ViewModelBase
    {
        #region 变量

     
        private DaySentence _daySentence;
        private BitmapImage _dayBitmapImage;
        #endregion

        #region 属性

        public BitmapImage DayBitmapImage
        {
            get { return _dayBitmapImage; }
            set
            {
                _dayBitmapImage = value;
                RaisePropertyChanged();
            }
        }
        public DaySentence DaySentence
        {
            get { return _daySentence; }
            set
            {
                _daySentence = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region 命令
        public MusicPlayer.MusicPlayer Player;
        public RelayCommand<Window> MoreDayCommand { get; private set; }
        public RelayCommand<Window> CloseCommand { get; private set; }

        public RelayCommand SpeakCommand { get; private set; }
        #endregion
        #region 构造方法

        public DaySentenceViewModel()
        {
            
            //获取每日模型
            TranslateLibrary.Translator translator=new Translator();
            DaySentence = translator.GetDaySentence();
            //下载图
            Action getDayImageAction=GetDaySentenceImage;
            getDayImageAction.BeginInvoke(null, null);
            //初始化命令
            MoreDayCommand=new RelayCommand<Window>((win) =>
            {
                Hyperlink link=new Hyperlink();
                link.NavigateUri =new Uri("http://news.iciba.com/views/dailysentence/daily.html#!/detail/title/"+DaySentence.DateLine);
                
                Process.Start(new ProcessStartInfo(link.NavigateUri.AbsoluteUri));
                win.Visibility = Visibility.Collapsed;
                if (Player.PlayState==MusicPlayer.StateEnum.Playing)
                {
                    Player.Pause();
                }
            });
            CloseCommand=new RelayCommand<Window>((win)=>
            {
                win.Visibility = Visibility.Collapsed;
                if (Player.PlayState == MusicPlayer.StateEnum.Playing)
                {
                    Player.Pause();
                }
            });
            SpeakCommand=new RelayCommand(() =>
            {
                Player.PlaySong(DaySentence.TtsAddress, 0, MusicPlayer.UriType.OnlineUri);
            });
        }

        #endregion

        #region 内部方法
        /// <summary>
        /// 耗时方法，异步调用
        /// </summary>
        private void GetDaySentenceImage()
        {
            try
            {
                byte[] imageBytes = new WebClient().DownloadData(DaySentence.PicAddress);
                var bitmap = ImageHelper.GetBitmap(imageBytes);
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    DayBitmapImage = ImageHelper.GetBitmapImage(bitmap);
                });
            }
            catch (Exception e)
            {
                App.PrinterException(e);
            }
        }

        #endregion
    }
}
