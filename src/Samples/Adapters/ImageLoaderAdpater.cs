using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MonoDroidToolkit;
using Samples.ViewModels;

namespace Samples.Adapters
{
  public class ImageLoaderWrapper : Java.Lang.Object
  {
    public TextView Name { get; set; }
    public ImageView Image { get; set; }
  }
  public class ImageLoaderAdapter : BaseAdapter<FriendViewModel>
  {
    private Activity context;
    private ImageLoader imageLoader;
    private List<FriendViewModel> friends; 
    public ImageLoaderAdapter(Activity context, ImageLoader imageLoader, List<FriendViewModel> friends)
    {
      this.imageLoader = imageLoader;
      this.context = context;
      this.friends = friends;
    }
    public override View GetView(int position, View convertView, ViewGroup parent)
    {
      ImageLoaderWrapper wrapper = null;
      var view = convertView;
      if (convertView == null)
      {
        view = context.LayoutInflater.Inflate(Resource.Layout.Friend, null);
        wrapper = new ImageLoaderWrapper();
        wrapper.Name = view.FindViewById<TextView>(Resource.Id.name);
        wrapper.Image = view.FindViewById<ImageView>(Resource.Id.image);
        view.Tag = wrapper;
      }
      else
      {
        wrapper = convertView.Tag as ImageLoaderWrapper;
      }

      var friend = friends[position];
      wrapper.Name.Text = friend.Title;
      imageLoader.DisplayImage(friend.Image, wrapper.Image, -1);

      return view;
    }

    public override FriendViewModel this[int position]
    {
      get { return friends[position]; }
    }

    public override int Count
    {
      get { return friends.Count; }
    }

    public override long GetItemId(int position)
    {
      return position;
    }
  }
}