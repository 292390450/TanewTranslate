using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight.Command;

namespace TranslationSpeech.ViewModel
{
    public class ConfirmWindowViewModel:ViewModelBase
    {
        #region 命令
        public RelayCommand ConfirmCommand { get; private set; }
        #endregion

        #region 构造

        public ConfirmWindowViewModel()
        {
            #region 初始化命令

            ConfirmCommand=new RelayCommand(() =>
            {
                App.Current.Shutdown();
            });

            #endregion
        }

        #endregion
    }
}
