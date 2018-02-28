using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace TranslationSpeech.Controls
{
    public class ScrollPanel : StackPanel, IScrollInfo
    {
        private TranslateTransform _translate;
        private Size _screenSize;
        private Size _totalSize;
        public ScrollPanel()
        {
            _translate = new TranslateTransform();
            this.RenderTransform = _translate;

        }



        protected override Size MeasureOverride(Size constraint)
        {
            _screenSize = constraint;
            if (Orientation == Orientation.Horizontal)
            {
                constraint = new Size(constraint.Width, double.PositiveInfinity);
            }
            else
            {
                constraint = new Size(double.PositiveInfinity, constraint.Height);
            }
            _totalSize = base.MeasureOverride(constraint);
            return _totalSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var size = base.ArrangeOverride(finalSize);
            if (ScrollOwner != null)
            {
                var yOffsetAnimation = new DoubleAnimation() { To = -VerticalOffset, Duration = TimeSpan.FromSeconds(0.15) };
                _translate.BeginAnimation(TranslateTransform.YProperty, yOffsetAnimation);

                var xOffsetAnimation = new DoubleAnimation() { To = -HorizontalOffset, Duration = TimeSpan.FromSeconds(0.15) };
                _translate.BeginAnimation(TranslateTransform.XProperty, xOffsetAnimation);

                ScrollOwner.InvalidateScrollInfo();

            }
            return _screenSize;
        }

        public bool CanVerticallyScroll { get; set; }
        public bool CanHorizontallyScroll
        {
            get; set;
        }

        public double ExtentWidth => _totalSize.Width;

        public double ExtentHeight => _totalSize.Height;

        public double ViewportWidth => _screenSize.Width;

        public double ViewportHeight => _screenSize.Height;

        public double HorizontalOffset { get; private set; }

        public double VerticalOffset { get; private set; }

        public ScrollViewer ScrollOwner
        {
            get; set;
        }

        const double _lineOffset = 20;
        const double _wheelOffset = 40;

        //

        void Appendoffset(double x, double y)
        {
            var offset = new Vector(HorizontalOffset + x, VerticalOffset + y);
            offset.Y = range(offset.Y, 0, _totalSize.Height - _screenSize.Height);
            offset.X = range(offset.X, 0, _totalSize.Width - _screenSize.Width);
            HorizontalOffset = offset.X;
            VerticalOffset = offset.Y;
            InvalidateArrange();
        }

        double range(double value, double value1, double value2)
        {
            var min = Math.Min(value1, value2);
            var max = Math.Max(value1, value2);
            value = Math.Max(value, min);
            value = Math.Min(value, max);
            return value;

        }

        public new void LineDown()
        {
            Appendoffset(0, _lineOffset);

        }

        public new void LineLeft()
        {
            Appendoffset(-_lineOffset, 0);

        }

        public void LineRight()
        {
            Appendoffset(_lineOffset, 0);

        }

        public new void LineUp()
        {
            Appendoffset(0, -_lineOffset);

        }


        public new  void MouseWheelDown()
        {
            Appendoffset(0, _wheelOffset);

        }

        public void MouseWheelLeft()
        {
            Appendoffset(-_wheelOffset, 0);

        }

        public void MouseWheelRight()
        {
            Appendoffset(_wheelOffset, 0);
        }

        public new void MouseWheelUp()
        {
            Appendoffset(0, -_wheelOffset);

        }

        public  void PageDown()
        {
            Appendoffset(0, _screenSize.Height);
        }

        public void PageLeft()
        {
            Appendoffset(-_screenSize.Width, 0);
        }

        public void PageRight()
        {
            Appendoffset(_screenSize.Width, 0);
        }

        public void PageUp()
        {
            Appendoffset(0, -_screenSize.Height);
        }

        public void SetHorizontalOffset(double offset)
        {
            this.Appendoffset(offset - HorizontalOffset, VerticalOffset);

        }

        public void SetVerticalOffset(double offset)
        {
            this.Appendoffset(HorizontalOffset, offset - VerticalOffset);

        }
    }
}
