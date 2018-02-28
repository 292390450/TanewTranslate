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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TranslationSpeech.Helper;
using TranslationSpeech.ViewModel;

namespace TranslationSpeech
{
    /// <summary>
    /// Shell.xaml 的交互逻辑
    /// </summary>
    public partial class Shell : Window
    {
        public Shell(ShellViewModel viewModel)
        {
            this.DataContext = viewModel;
            InitializeComponent();
            viewModel.CheckSuccess += ViewModel_CheckSuccess;
            if (Environment.OSVersion.Version.Major == 10)
            {
                WindowBlurHelper.Win10BlurHelper.EnableBlur(this);
            }
            else
            {
                this.BorderThickness = new Thickness(0.5);
                this.BorderBrush = Brushes.DarkGray;
                WindowBlurHelper.Win7BlurHelper.EnableAeroGlass(this);
            }
        }

        private void ViewModel_CheckSuccess()
        {
            this.Close();
        }
    }
}
