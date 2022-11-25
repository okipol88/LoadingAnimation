using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace LoadingAnimation.Adorners
{
  public class ShimmerAdorner : Adorner
  {


    public static bool GetAttach(DependencyObject obj)
    {
      return (bool)obj.GetValue(AttachProperty);
    }

    public static void SetAttach(DependencyObject obj, bool value)
    {
      obj.SetValue(AttachProperty, value);
    }

    // Using a DependencyProperty as the backing store for Attach.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty AttachProperty =
        DependencyProperty.RegisterAttached("Attach", typeof(bool), typeof(ShimmerAdorner), new PropertyMetadata(false, OnAttachChanged));

    private static void OnAttachChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var myAdornerLayer = AdornerLayer.GetAdornerLayer(d as Visual);
      if (d is not UIElement uiElement)
      {
        return;
      }

      if (e.NewValue is true)
      {
        myAdornerLayer.Add(new ShimmerAdorner(uiElement));
      } 
      else
      {
      var item = myAdornerLayer.GetAdorners(uiElement)
          .OfType<ShimmerAdorner>()
          .FirstOrDefault();

        if (item is not ShimmerAdorner shimmerAdorner)
        {
          return;
        }

        myAdornerLayer.Remove(shimmerAdorner);
      }
    }

    


    public ShimmerAdorner(UIElement adornedElement) : base(adornedElement)
    {
      AnimationTransform = MakeTransform();
      Brush = MakeBrush();

      Brush.RelativeTransform = AnimationTransform;
    }

    private TranslateTransform MakeTransform()
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
      var layoutTransform = new TranslateTransform(0, 0);

      layoutTransform.ApplyAnimationClock(TranslateTransform.XProperty, myClock);

      return layoutTransform;
    }

    private static LinearGradientBrush MakeBrush()
    {
      LinearGradientBrush myLinearGradientBrush =
          new LinearGradientBrush();
      myLinearGradientBrush.StartPoint = new Point(0, 0);
      myLinearGradientBrush.EndPoint = new Point(1, 1);
      myLinearGradientBrush.GradientStops.Add(
          new GradientStop(Colors.Yellow, 0.0));
      myLinearGradientBrush.GradientStops.Add(
          new GradientStop(Colors.Red, 0.25));
      myLinearGradientBrush.GradientStops.Add(
          new GradientStop(Colors.Blue, 0.75));
      myLinearGradientBrush.GradientStops.Add(
          new GradientStop(Colors.LimeGreen, 1.0));
      return myLinearGradientBrush;
    }

    public TranslateTransform AnimationTransform { get; }
    public LinearGradientBrush Brush { get; private set; }

    protected override void OnRender(DrawingContext drawingContext)
    {
      Rect adornedElementRect = new Rect(this.AdornedElement.RenderSize);

      // Some arbitrary drawing implements.
      Pen renderPen = new Pen(new SolidColorBrush(Colors.Navy), 1.5);
      double renderRadius = 5.0;

      drawingContext.DrawRectangle(Brush, null, adornedElementRect);



      // Draw a circle at each corner.
      //drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.TopLeft, renderRadius, renderRadius);
      //drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.TopRight, renderRadius, renderRadius);
      //drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.BottomLeft, renderRadius, renderRadius);
      //drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.BottomRight, renderRadius, renderRadius);
    }
  }
}
