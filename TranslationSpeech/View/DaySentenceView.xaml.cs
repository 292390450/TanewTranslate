using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using TranslationSpeech.ViewModel;


namespace TranslationSpeech.View
{
    /// <summary>
    /// DaySentenceView.xaml 的交互逻辑
    /// </summary>
    public partial class DaySentenceView : Window
    {
        public DaySentenceView(DaySentenceViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
            this.Left = SystemParameters.WorkArea.Width - this.Width - 10;
            
            this.Top = SystemParameters.WorkArea.Height-this.Height-10;
            //把播放器初始化
            if (viewModel != null)
            {
                viewModel.Player = new MusicPlayer.MusicPlayer(new WindowInteropHelper(this).Handle);
            }
        }
    }
}
