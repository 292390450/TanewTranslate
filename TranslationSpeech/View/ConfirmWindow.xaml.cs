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
using TranslationSpeech.ViewModel;

namespace TranslationSpeech.View
{
    /// <summary>
    /// ConfirmWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ConfirmWindow : Window
    {
        public ConfirmWindow(ConfirmWindowViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }

        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
