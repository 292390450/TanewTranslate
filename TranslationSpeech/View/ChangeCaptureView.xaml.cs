using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using MahApps.Metro.Controls;
using NAudio.CoreAudioApi;
using TranslationSpeech.ViewModel;

namespace TranslationSpeech.View
{
    /// <summary>
    /// ChangeCaptureView.xaml 的交互逻辑
    /// </summary>
    public partial class ChangeCaptureView 
    {
        public ChangeCaptureView(ChangeCaptureViewModel viewModel)
        {
            this.DataContext = viewModel;
            InitializeComponent();
        }

        private void ChangeCaptureView_OnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
            this.Visibility = Visibility.Collapsed;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            this.Visibility = Visibility.Collapsed;
        }
    }

    public static class WindowEx
    {
        public static void ShowChangeDeviceWindow(this MetroWindow win,RecordLibrary.Record recordor)
        {

            var dataContex = win.DataContext as ChangeCaptureViewModel;
            var nowDevice = recordor.GetCurrentDevice();
            var enumerator = new MMDeviceEnumerator();
            //清空 
            dataContex.Devices.Clear();
            dataContex.Devices.AddRange(enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active).ToList());
            dataContex.SelectedItem =
                dataContex.Devices.FirstOrDefault(x => x.FriendlyName == nowDevice.FriendlyName);
            dataContex.Record = recordor;
            win.ShowDialog();
        }
    }
    
}
