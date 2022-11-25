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
                         new Duration(TimeSpan.FromSeconds(2))
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
      myLinearGradientBrush.StartPoint = new Point(0, 0.5);
      myLinearGradientBrush.EndPoint = new Point(1, 0.5);
      myLinearGradientBrush.GradientStops.Add(
          new GradientStop(Colors.Red, 0.0));
      myLinearGradientBrush.GradientStops.Add(
          new GradientStop(Colors.Blue, 1));

      return myLinearGradientBrush;
    }

    public TranslateTransform AnimationTransform { get; }
    public LinearGradientBrush Brush { get; private set; }

    protected override void OnRender(DrawingContext drawingContext)
    {
      var brush = Brush;
      brush.MappingMode = BrushMappingMode.RelativeToBoundingBox;
      //brush = MakeBrush();

      Rect adornedElementRect = new Rect(this.AdornedElement.RenderSize);
      var group = new GeometryGroup();
      group.Children.Add(new RectangleGeometry(new Rect(0, 0, 100, adornedElementRect.Height)));
      group.Children.Add(new RectangleGeometry(new Rect(200, 0, 100, adornedElementRect.Height)));
      group.Children.Add(new RectangleGeometry(new Rect(400, 0, 100, adornedElementRect.Height)));
      group.Children.Add(new RectangleGeometry(new Rect(600, 0, 100, adornedElementRect.Height)));
      group.Children.Add(new RectangleGeometry(new Rect(800, 0, 100, adornedElementRect.Height)));

      drawingContext.PushClip(group);
      drawingContext.DrawRectangle(brush, new Pen(Brushes.Green, 12), adornedElementRect);
      //adornedElementRect = adornedElementRect with { Height = adornedElementRect.Height / 2};

      //var pen = new Pen(Brushes.Green, 1);

      //var oneFourth = adornedElementRect.Width / 4;
      //var smallerRect = Rect.Offset(adornedElementRect, -oneFourth*3, 0);

      //drawingContext.DrawRectangle(brush, pen, smallerRect);

      //var margin = 10;

      //drawingContext.DrawRectangle(brush, pen,
      //  new Rect(adornedElementRect.X + adornedElementRect.Width / 4 + margin,
      //  0, adornedElementRect.Width / 4 - margin * 2, adornedElementRect.Height));


      //drawingContext.DrawRectangle(brush, pen, Rect.Offset(adornedElementRect, oneFourth * 3, 0));
    }
  }
}
