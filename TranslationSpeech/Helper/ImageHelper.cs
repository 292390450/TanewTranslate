using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace TranslationSpeech.Helper
{
    public class ImageHelper
    {
        public static Bitmap FormBitmapImageGetBitmap(BitmapSource bs)
        {
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(bs.PixelWidth, bs.PixelHeight);
            try
            {
                System.Drawing.Imaging.BitmapData data = bmp.LockBits(
                    new System.Drawing.Rectangle(System.Drawing.Point.Empty, bmp.Size), System.Drawing.Imaging.ImageLockMode.WriteOnly, PixelFormat.Format32bppPArgb);

                bs.CopyPixels(Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride);
                bmp.UnlockBits(data);
            }
            catch (Exception e)
            {
                return bmp;
            }
            return bmp;
        }
        public static Bitmap GetBitmap(byte[] buff)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    ms.Write(buff, 0, buff.Length);
                    Bitmap bit = new Bitmap(Bitmap.FromStream(ms));
                    bit = ChangeImageStyle(bit);
                    return bit;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static BitmapImage GetBitmapImage(Bitmap bit)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bit.Save(ms, ImageFormat.Bmp);
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = ms;

                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    return bitmap;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        #region 方法

        /// <summary>
        /// 重绘图片，更改格式为位图
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static Bitmap ChangeImageStyle(Bitmap bitmap)
        {
            Bitmap newbitmap = new Bitmap(bitmap.Width, bitmap.Height);
            System.Drawing.Color pixel;
            for (int x = 1; x < bitmap.Width; x++)
            {
                for (int y = 1; y < bitmap.Height; y++)
                {
                    int r, g, b;
                    pixel = bitmap.GetPixel(x, y);
                    r = pixel.R;
                    g = pixel.G;
                    b = pixel.B;
                    newbitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(r, g, b));
                }
            }
            return newbitmap;
        }
        #endregion
    }
}
