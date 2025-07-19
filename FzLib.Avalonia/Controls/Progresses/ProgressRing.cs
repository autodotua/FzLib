using Avalonia;
using Avalonia.Controls.Primitives;
using System;

namespace FzLib.Avalonia.Controls
{
    //https://github.com/Deadpikle/AvaloniaProgressRing
    public partial class ProgressRing : TemplatedControl
    {
        public static readonly DirectProperty<ProgressRing, double> EllipseDiameterProperty =
            AvaloniaProperty.RegisterDirect<ProgressRing, double>(
               nameof(EllipseDiameter),
               o => o.EllipseDiameter);

        public static readonly DirectProperty<ProgressRing, Thickness> EllipseOffsetProperty =
            AvaloniaProperty.RegisterDirect<ProgressRing, Thickness>(
               nameof(EllipseOffset),
               o => o.EllipseOffset);

        public static readonly StyledProperty<bool> IsActiveProperty =
            AvaloniaProperty.Register<ProgressRing, bool>(
                nameof(IsActive),
                defaultValue: true);

        public static readonly DirectProperty<ProgressRing, double> MaxSideLengthProperty =
            AvaloniaProperty.RegisterDirect<ProgressRing, double>(
               nameof(MaxSideLength),
               o => o.MaxSideLength);

        private const string ActiveState = ":active";
        private const string InactiveState = ":inactive";
        private const string LargeState = ":large";
        private const string SmallState = ":small";
        private double ellipseDiameter = 10;
        private Thickness ellipseOffset = new Thickness(2);
        private double maxSideLength = 10;
        public double EllipseDiameter
        {
            get => ellipseDiameter;
            private set => SetAndRaise(EllipseDiameterProperty, ref ellipseDiameter, value);
        }

        public Thickness EllipseOffset
        {
            get => ellipseOffset;
            private set => SetAndRaise(EllipseOffsetProperty, ref ellipseOffset, value);
        }

        public bool IsActive
        {
            get => (bool)GetValue(IsActiveProperty);
            set => SetValue(IsActiveProperty, value);
        }
        public double MaxSideLength
        {
            get => maxSideLength;
            private set => SetAndRaise(MaxSideLengthProperty, ref maxSideLength, value);
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            double maxSideLength = Math.Min(this.Width, this.Height);
            double ellipseDiameter = 0.1 * maxSideLength;
            if (maxSideLength <= 40)
            {
                ellipseDiameter += 1;
            }

            EllipseDiameter = ellipseDiameter;
            MaxSideLength = maxSideLength;
            EllipseOffset = new Thickness(0, maxSideLength / 2 - ellipseDiameter, 0, 0);
            UpdateVisualStates();
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == IsActiveProperty)
            {
                UpdateVisualStates();
            }
        }

        private static void OnIsActiveChanged(AvaloniaObject obj, bool arg2)
        {
            ((ProgressRing)obj).UpdateVisualStates();
        }

        private void UpdateVisualStates()
        {
            PseudoClasses.Remove(ActiveState);
            PseudoClasses.Remove(InactiveState);
            PseudoClasses.Remove(SmallState);
            PseudoClasses.Remove(LargeState);
            PseudoClasses.Add(IsActive ? ActiveState : InactiveState);
            PseudoClasses.Add(maxSideLength < 60 ? SmallState : LargeState);
        }
    }
}
