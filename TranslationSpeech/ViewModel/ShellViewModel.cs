using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using BaiduWeb;
using GalaSoft.MvvmLight.Threading;
using Microsoft.Practices.Unity;
using Newtonsoft.Json.Linq;
using TranslationSpeech.View;

namespace TranslationSpeech.ViewModel
{
    public delegate void UpSuccess();
    public class ShellViewModel:ViewModelBase
    {
        #region 变量

        private Window _errorWindow;
        private Window _mainWindow;
        private Window _daySentenceWindow;
        private string _macAddress = string.Empty;
        private string _ipAddress = string.Empty;
        private string _deviceName=string.Empty;
        private string _windowsVersion = string.Empty;
        private string _userName = string.Empty;
        private string _userInfo = string.Empty;
        private IUnityContainer _container;

        #endregion
        #region 属性

        public event UpSuccess CheckSuccess;
       
        #endregion
        #region 命令

        #endregion

        #region 构造

        public ShellViewModel(IUnityContainer container)
        {
            _container = container;
            _macAddress = BaiduData.GetMacAddress();
            _ipAddress = BaiduData.GetIpAddress();
            
            _deviceName = Environment.MachineName;
            _windowsVersion = Environment.OSVersion.VersionString;
            _userName = Environment.UserName;
            _userInfo = _deviceName + "|" + _userName + "|" + _windowsVersion;

            //视图赋值
            _mainWindow = _container.Resolve<MainWindow>();
            _errorWindow = _container.Resolve<ConfirmWindow>();
           _daySentenceWindow = _container.Resolve<DaySentenceView>();
            //异步调用查询方法
            Func<bool> checkClould=new Func<bool>(UpData);
            checkClould.BeginInvoke(delegate(IAsyncResult r)
            {
                var result = checkClould.EndInvoke(r);
                if (result)//成功,打开主窗口,每日一句
                {
                    Thread.Sleep(1500);
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        CheckSuccess?.Invoke();
                        _mainWindow.Show();
                        _daySentenceWindow.Show();
                    });
                }
                else
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        _errorWindow.Show();
                    });
                }
            }, null);
        }
        #endregion
        #region 私有方法
        /// <summary>
        /// 耗时的网络调用方法
        /// </summary>
        /// <returns></returns>
        private bool UpData()
        {
            var resExist = BaiduData.QueryExist(_macAddress);
            if (string.IsNullOrEmpty(resExist))
            {
                return false;
            }
            JObject exitable = JObject.Parse(resExist);
            if (exitable["status"].Value<string>() == "0")
            {
                if (exitable["total"].Value<int>() > 0)
                {
                    //存在插入的数据
                    return true;
                }
                else //插入
                {

                    var locationModel = BaiduData.GetLocation();    //获取位置信息
                    var latitude = locationModel.Latitude;
                    var longitude = locationModel.Longitude;
                    var res = JObject.Parse(BaiduData.InsertARecord(latitude, longitude, _ipAddress, "170560", _macAddress, _userInfo));
                    if (res["status"].Value<string>() == "0")   //插入成功
                    {
                        return true;
                    }
                    return false;
                }
            }
            return false;
        }
        #endregion
    }
}
