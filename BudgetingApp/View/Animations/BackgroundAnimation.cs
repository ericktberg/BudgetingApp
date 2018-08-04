using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace BudgetingApp.Animations
{
    public class BackgroundAnimation : ColorAnimation
    {
        public BackgroundAnimation() : base()
        {
            To = Colors.White;
            Duration = (Duration) (Application.Current.TryFindResource("Short") ?? new Duration(TimeSpan.FromSeconds(.1)));
            EasingFunction = new QuadraticEase() { EasingMode = EasingMode.EaseInOut };

            SetValue(Storyboard.TargetPropertyProperty, new PropertyPath("(Background).(SolidColorBrush.Color)"));
        }
    }
}
