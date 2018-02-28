using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MahApps.Metro.Controls;
using NAudio.CoreAudioApi;
using RecordLibrary;

namespace TranslationSpeech.ViewModel
{
    public class ChangeCaptureViewModel:ViewModelBase
    {
        #region 变量

        
        private MMDevice _selectedItem;
        private ObservableCollection<MMDevice> _devices=new ObservableCollection<MMDevice>();
        #endregion

        #region 属性
        public Record Record;
        public MMDevice SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<MMDevice> Devices
        {
            get { return _devices; }
            set
            {
                _devices = value;
                RaisePropertyChanged();
            }
        }

        #endregion
        #region 命令
        public RelayCommand<MetroWindow> ConfirmCommand { get; private set; }
        #endregion

        public ChangeCaptureViewModel()
        {
            ConfirmCommand=new RelayCommand<MetroWindow>((win) =>
            {
                Record.ChangeCapture(SelectedItem);
                win.Visibility = System.Windows.Visibility.Hidden;
                win.Visibility = System.Windows.Visibility.Collapsed;
            });
        }

    }
}
