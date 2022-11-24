using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace LoadingAnimation.Attached
{

  public class ShimmerAdorner : Adorner
  {
    public ShimmerAdorner(UIElement adornedElement) : base(adornedElement)
    {
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
      base.OnRender(drawingContext);

      
    }
  }

  public static class AttachedAnimation
  {

    private static AnimationClock clock;

    public static bool GetShimmerAnimation(DependencyObject obj)
    {
      return (bool)obj.GetValue(ShimmerAnimationProperty);
    }

    public static void SetShimmerAnimation(DependencyObject obj, bool value)
    {
      obj.SetValue(ShimmerAnimationProperty, value);
    }

    // Using a DependencyProperty as the backing store for ShimmerAnimation.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ShimmerAnimationProperty =
        DependencyProperty.RegisterAttached("ShimmerAnimation", typeof(bool), typeof(AttachedAnimation), new PropertyMetadata(false, OnChanged));

    private static void OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
     if (d is not LinearGradientBrush lb)
      {
        return;
      }

      if (clock is not AnimationClock c) 
      {

        DoubleAnimation myAnimation =
                       new DoubleAnimation(
                           -1, // "From" value
                           1, // "To" value
                           new Duration(TimeSpan.FromSeconds(0.5))
                       );
        myAnimation.RepeatBehavior = RepeatBehavior.Forever;
        // Create a clock the for the animation.
        AnimationClock myClock = myAnimation.CreateClock();

        clock = myClock;
      }

      var layoutTransform = new TranslateTransform(0, 0);

      layoutTransform.ApplyAnimationClock(TranslateTransform.XProperty, clock);


      lb.RelativeTransform = layoutTransform;
    }
  }

  //public class ShimmerBehavior : Behavior<FrameworkElement>
  //{

  //}
}
