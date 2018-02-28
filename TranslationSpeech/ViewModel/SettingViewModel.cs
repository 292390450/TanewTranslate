using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace TranslationSpeech.ViewModel
{
    public class SettingViewModel:ViewModelBase
    {
        #region 变量

        private bool _isAutoDeleCatche = Properties.Settings.Default.IsAutoDeleCatche;
        private bool _isOPenAutoAudio = Properties.Settings.Default.IsOPenAutoAudio;

        #endregion
        #region 属性

        public bool IsOPenAutoAudio
        {
            get { return _isOPenAutoAudio; }
            set
            {
                _isOPenAutoAudio = value;
                Properties.Settings.Default.IsOPenAutoAudio = value;
                Properties.Settings.Default.Save();
                RaisePropertyChanged();
            }
        }

        public bool IsAutoDeleCatche
        {
            get { return _isAutoDeleCatche; }
            set
            {
                _isAutoDeleCatche = value;
                Properties.Settings.Default.IsAutoDeleCatche = value;
                Properties.Settings.Default.Save();
                RaisePropertyChanged();
            }
        }
        #endregion

        #region 命令

        public RelayCommand ClearCacheCommand { get; private set; }


        #endregion

        public SettingViewModel()
        {
            ClearCacheCommand=new RelayCommand(() =>
            {
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                try
                {

                    DirectoryInfo recorDirectory = Directory.CreateDirectory(basePath + @"\RecordCache");
                    var files = recorDirectory.GetFiles("*.wav");
                    foreach (var fileInfo in files)
                    {
                        File.Delete(fileInfo.FullName);
                    }
                    DirectoryInfo audioDirectory = Directory.CreateDirectory(basePath + @"\AudioCache");
                    var audios = audioDirectory.GetFiles("*.mp3");
                    foreach (var fileInfo in audios)
                    {
                        File.Delete(fileInfo.FullName);
                    }
                }
                catch (Exception e)
                {
                    App.PrinterException(e);
                }
            });
        }
    }
}
