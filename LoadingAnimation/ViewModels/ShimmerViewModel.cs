using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LoadingAnimation.ViewModels
{
  public abstract class ShimmerOptiomViewModel
  {
    public ShimmerOptiomViewModel(ObservableCollection<object> items)
    {
      Items = items;
    }

    public ObservableCollection<object> Items { get; }
  }

  public class SharedShimmerOptionViewModel : ShimmerOptiomViewModel
  {
    public SharedShimmerOptionViewModel(ObservableCollection<object> items)
      : base(items)
    {
    }
  }

  public class NoOption : ShimmerOptiomViewModel
  {
    public NoOption(ObservableCollection<object> items) : base(items)
    {
    }
  }

  public partial class ShimmerViewModel : ObservableObject
  {
    private Dictionary<Type, Func<ShimmerOptiomViewModel>> _shimmerOptionsMap;

    [ObservableProperty]
    private ShimmerOptiomViewModel _option;

    public ShimmerViewModel()
    {
      _option = new SharedShimmerOptionViewModel(Items);

      _shimmerOptionsMap = new Dictionary<Type, Func<ShimmerOptiomViewModel>>
      {
        {typeof(SharedShimmerOptionViewModel), () => new NoOption(Items) },
        {typeof(NoOption), () => new SharedShimmerOptionViewModel(Items) },
      };
    }

    [RelayCommand]
    private void NextOption()
    {
      Option = _shimmerOptionsMap[Option.GetType()]();
    }

    [RelayCommand]
    private void SetItems(int targetItemsCount)
    {
      var diff = targetItemsCount - Items.Count;
      if (diff > 0)
      {
        for (int i = 0; i < diff; i++)
        {
          Items.Add(new object());
        }
      } 
      else
      {
        while (Items.Count > targetItemsCount)
        {
          Items.RemoveAt(Items.Count - 1);
        }
      }
    }

    public ObservableCollection<object> Items { get; } = new ObservableCollection<object>();


  }
}
