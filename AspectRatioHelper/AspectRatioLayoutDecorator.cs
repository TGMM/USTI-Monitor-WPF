using System;
using System.Windows;
using System.Windows.Controls;

namespace AspectRatioHelper
{
    // Helper class to maintain the core aspect ratio on resize
    public class AspectRatioLayoutDecorator : Decorator
    {
        public static readonly DependencyProperty AspectRatioProperty =
            DependencyProperty.Register(
                "AspectRatio",
                typeof(double),
                typeof(AspectRatioLayoutDecorator),
                new FrameworkPropertyMetadata
                (
                    1d,
                    FrameworkPropertyMetadataOptions.AffectsMeasure
                ),
                ValidateAspectRatio);

        private static bool ValidateAspectRatio(object value)
        {
            if (!(value is double))
            {
                return false;
            }

            var aspectRatio = (double)value;
            return aspectRatio > 0
                   && !double.IsInfinity(aspectRatio)
                   && !double.IsNaN(aspectRatio);
        }

        public double AspectRatio
        {
            get => (double)GetValue(AspectRatioProperty);
            set => SetValue(AspectRatioProperty, value);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            if (Child == null) return new Size(0, 0);
            constraint = SizeToRatio(constraint, false);
            Child.Measure(constraint);

            if (double.IsInfinity(constraint.Width)
                || double.IsInfinity(constraint.Height))
            {
                return SizeToRatio(Child.DesiredSize, true);
            }

            return constraint;

            // we don't have a child, so we don't need any space
        }

        public Size SizeToRatio(Size size, bool expand)
        {
            var ratio = AspectRatio;

            var height = size.Width / ratio;
            var width = size.Height * ratio;

            if (expand)
            {
                width = Math.Max(width, size.Width);
                height = Math.Max(height, size.Height);
            }
            else
            {
                width = Math.Min(width, size.Width);
                height = Math.Min(height, size.Height);
            }

            return new Size(width, height);
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            if (Child == null) return arrangeSize;
            var newSize = SizeToRatio(arrangeSize, false);

            var widthDelta = arrangeSize.Width - newSize.Width;
            var heightDelta = arrangeSize.Height - newSize.Height;

            double top = 0;
            double left = 0;

            if (!double.IsNaN(widthDelta)
                && !double.IsInfinity(widthDelta))
            {
                left = widthDelta / 2;
            }

            if (!double.IsNaN(heightDelta)
                && !double.IsInfinity(heightDelta))
            {
                top = heightDelta / 2;
            }

            var finalRect = new Rect(new Point(left, top), newSize);
            Child.Arrange(finalRect);

            return arrangeSize;
        }
    }
}
