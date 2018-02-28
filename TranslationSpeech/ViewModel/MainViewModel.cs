using System;
using GalaSoft.MvvmLight;
using System.Device.Location;
using System.IO;
using System.Threading;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using System.Windows.Threading;
using TranslateLibrary;
using System.Threading.Tasks;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Threading;
using MahApps.Metro.Controls;
using Microsoft.Practices.Unity;
using TtsAndAsrLibrary;
using RecordLibrary;
using TranslationSpeech.View;

namespace TranslationSpeech.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region ����


        private MetroWindow _changeCaptureWindow;
        private UserControl _settingView;
        private Visibility _isBusyVisibility = Visibility.Collapsed;
        private Visibility _keyVisibility=Visibility.Collapsed;
        private Visibility _pronunciationVisibility=Visibility.Collapsed;
        private TranslateResultModel _translateResultModel;
        private Record _recorder;
        private TtsClient _ttsClient;
        private int _selectedIndex=0;
        private Translator _translator;
        private string _orignal;
        private string _translateStr;
        private string _tipContent = "��ס������";
        private double _recordTime = 0;
        private DispatcherTimer _timer;
        private Visibility _volumVisibility = Visibility.Collapsed;
        private Visibility _textVisibility = Visibility.Visible;
        #endregion
        #region ����

        public UserControl SettingView
        {
            get { return _settingView; }
            set
            {
                _settingView = value;
                RaisePropertyChanged();
            }
        }
        public Visibility IsBusyVisibility
        {
            get { return _isBusyVisibility; }
            set
            {
                _isBusyVisibility = value;
                RaisePropertyChanged();
            }
        }
        public Visibility KeyVisibility
        {
            get { return _keyVisibility; }
            set
            {
                _keyVisibility = value;
                RaisePropertyChanged();
            }
        }

        public Visibility PronunciationVisibility
        {
            get { return _pronunciationVisibility; }
            set
            {
                _pronunciationVisibility = value;
                RaisePropertyChanged();
            }
        }
        public TranslateResultModel TranslateResultModel
        {
            get { return _translateResultModel; }
            set
            {
                _translateResultModel = value;
                PronunciationVisibility = string.IsNullOrEmpty(TranslateResultModel.AmPronunciation.Ps)
                    ? Visibility.Collapsed
                    : Visibility.Visible;
              
                    KeyVisibility = string.IsNullOrEmpty(TranslateResultModel.Key)
                        ? Visibility.Collapsed
                        : Visibility.Visible;
              
                RaisePropertyChanged();
            }
        }
        public Record Recorder
        {
            get { return _recorder; }
            set
            {
                _recorder = value;
                RaisePropertyChanged();
            }
        }
        public MusicPlayer.MusicPlayer AudioPlayer;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                RaisePropertyChanged();
            }
        }
        public string Orignal
        {
            get { return _orignal; }
            set
            {
                _orignal = value;
                RaisePropertyChanged();
            }
        }

        public string TranslateStr
        {
            get
            {
                return _translateStr;
            }
            set
            {
                _translateStr = value;
                IsBusyVisibility = Visibility.Collapsed;
                RaisePropertyChanged();
            }
        }
        public string TipContent
        {
            get { return _tipContent; }
            set
            {
                _tipContent = value; 
                RaisePropertyChanged();
            }
        }
        public Visibility VolumVisibility
        {
            get { return _volumVisibility; }
            set
            {
                _volumVisibility = value;
                RaisePropertyChanged();
            }
        }

        public Visibility TextVisibility
        {
            get
            {
                return _textVisibility;
                
            }
            set
            {
                _textVisibility = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region ����
        public RelayCommand ChangeDeviceCommand { get; private set; }
        public RelayCommand<KeyEventArgs> InputKeyUpCommand { get; private set; }
        public RelayCommand<string> SpeakCommand { get; private set; }
        public RelayCommand<MouseButtonEventArgs> StartRecordCommand { get; private set; }
        public RelayCommand<MouseButtonEventArgs> EndRecordCommand { get; private set; }
        public RelayCommand TranslateCommand { get; private set; }
        #endregion
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IUnityContainer container)
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
            /// ��ʼ��������
            _translator=new Translator();
            #region ��ʼ������ʶ��ͻ���

            _ttsClient = new TtsClient();

            #endregion
            //��ʼ��¼������
            Recorder=new Record(AppDomain.CurrentDomain.BaseDirectory+ @"\RecordCache",SynchronizationContext.Current);
            Recorder.Success += Recorder_Success;
            #region ��ʼ����ʱ��
            _timer = new DispatcherTimer();
            _timer.Interval=TimeSpan.FromSeconds(1);
            _timer.Tick += _timer_Tick;
            #endregion
            //��ʼ�����ý���
            SettingView = container.Resolve<SettingView>();
            _changeCaptureWindow = container.Resolve<ChangeCaptureView>();
            #region ��ʼ������

            SpeakCommand=new RelayCommand<string>(SpeakCommandExcute);
            StartRecordCommand=new RelayCommand<MouseButtonEventArgs>((e) =>
            {
                Recorder.Start();
                _timer.Start();
                TextVisibility = Visibility.Collapsed;
                VolumVisibility = Visibility.Visible;
            });
            EndRecordCommand=new RelayCommand<MouseButtonEventArgs>((e) =>
            {
                
                _timer.Stop();
                if (_recordTime>0.5)
                {
                    IsBusyVisibility = Visibility.Visible;
                }
                Recorder.Stop();
                
                TextVisibility = Visibility.Visible;
                VolumVisibility = Visibility.Collapsed;
               
            });
            TranslateCommand=new RelayCommand(TranslateCommandExcute);

            InputKeyUpCommand=new RelayCommand<KeyEventArgs>((e) =>
            {
                if (e.Key==Key.Enter)
                {
                    //ִ�з���
                    TranslateCommandExcute();
                }
            });
            ChangeDeviceCommand=new RelayCommand(() =>
            {
                _changeCaptureWindow.ShowChangeDeviceWindow(Recorder);
            });
            #endregion


        }

      

        #region ˽�з���

        private void TranslateCommandExcute()
        {
            IsBusyVisibility = Visibility.Visible;
            LanguageType type = LanguageType.auto;
            switch (SelectedIndex)
            {
                case 0:
                    type = LanguageType.auto;
                    break;
                case 1:
                    type = LanguageType.zh;
                    break;
                case 2:
                    type = LanguageType.en;
                    break;
                case 3:
                    type = LanguageType.yue;
                    break;
                case 4:
                    type = LanguageType.jp;
                    break;
                case 5:
                    type = LanguageType.kor;
                    break;
                case 6:
                    type = LanguageType.fra;
                    break;
                default:
                    type = LanguageType.auto;
                    break;
            }
            Func<string, LanguageType, string> translateAction = _translator.Translate;
            translateAction.BeginInvoke(Orignal, type, delegate (IAsyncResult r)
            {
                var str = translateAction.EndInvoke(r);
                
                    GetTranslateResultModel(Orignal);
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        TranslateStr = str;
                    });
               
            }, null);
        }

        private void SpeakCommandExcute(string e)
        {
            string ttsStr;
            if (e == "translateBtn")
            {
                ttsStr = TranslateStr;
            }
            else
            {
                ttsStr = Orignal;
            }
            //ֹͣ��ǰ�Ĳ���
            AudioPlayer.Stop();
            if (!string.IsNullOrEmpty(ttsStr))
            {
                Func<string, byte[]> ttsFunc = _ttsClient.Tts;
                ttsFunc.BeginInvoke(ttsStr, delegate (IAsyncResult r)
                {
                    var resultBuff = ttsFunc.EndInvoke(r);
                    if (resultBuff != null)
                    {
                        var path = AppDomain.CurrentDomain.BaseDirectory + @"\AudioCache" + @"\" +
                                   DateTime.Now.ToLongDateString() +
                                   "orignal" + @".mp3";
                        File.WriteAllBytes(path.Trim(':', '-'), resultBuff);
                        AudioPlayer.PlaySong(path, 0, MusicPlayer.UriType.LocalFile);
                    }
                }, null);
            }
        }
        private void _timer_Tick(object sender, EventArgs e)
        {
            _recordTime=_recordTime+0.5;
        }
        private void Recorder_Success(string resultPath)
        {
            TipContent = "��ס������";
            if (_recordTime <= 0.5)
            {
                //¼��ʱ������һ��,ɾ���Ѿ���ɱ�����ļ�,��¼��ʱ������
                TipContent = "¼��ʱ��̫��";
                IsBusyVisibility = Visibility.Collapsed;
                _recordTime = 0;
                try
                {
                    File.Delete(resultPath);
                }
                catch (Exception e)
                {
                    App.PrinterException(e);
                }
            }
            else
            {
                //���ļ��ϴ�����������ʶ��,��¼��ʱ������
                _recordTime = 0;
                Func<string,string> asrFunc = _ttsClient.AsrData;
                asrFunc.BeginInvoke(resultPath, delegate(IAsyncResult r)
                {
                    var res=asrFunc.EndInvoke(r);
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        Orignal = res;
                        LanguageType type = LanguageType.auto;
                        switch (SelectedIndex)
                        {
                            case 0:
                                type = LanguageType.auto;
                                break;
                            case 1:
                                type = LanguageType.zh;
                                break;
                            case 2:
                                type = LanguageType.en;
                                break;
                            case 3:
                                type = LanguageType.yue;
                                break;
                            case 4:
                                type = LanguageType.jp;
                                break;
                            case 5:
                                type = LanguageType.kor;
                                break;
                            case 6:
                                type = LanguageType.fra;
                                break;
                            default:
                                type = LanguageType.auto;
                                break;
                        }
                        Func<string, LanguageType, string> translateAction = _translator.Translate;
                        translateAction.BeginInvoke(Orignal, type, delegate (IAsyncResult re)
                        {
                            var str = translateAction.EndInvoke(re);
                            if (!string.IsNullOrEmpty(str))
                            {
                                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                                {
                                    TranslateStr = str;
                                    if (Properties.Settings.Default.IsOPenAutoAudio)
                                    {
                                        SpeakCommandExcute("translateBtn");
                                    }
                                });
                            }
                        }, null);
                    });
                  
                }, null);
            }
        }
        /// <summary>
        /// ��ȡ�µĹؼ����й���Ϣ
        /// </summary>
        /// <param name="str"></param>
        private void GetTranslateResultModel(string str)
        {
            Func<string, TranslateLibrary.TranslateResultModel> getTranslateModel = _translator.GeTranslateResultModel;
            getTranslateModel.BeginInvoke(str, delegate(IAsyncResult r)
            {
                var result = getTranslateModel.EndInvoke(r);
                if (result!=null)
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        TranslateResultModel = result;
                    });
                }
            }, null);
        }
        #endregion
        
    }
}