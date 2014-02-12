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
using com.refractored.monodroidtoolkit.imageloader;
using Samples.Adapters;
using Samples.ViewModels;

namespace Samples
{
  [Activity(Label = "Image Loading", Theme = "@style/MyTheme")]
  public class ImageLoaderActivity : ListActivity
  {
    private List<FriendViewModel> friends;
    private ImageLoader imageLoader;
    protected override void OnCreate(Bundle bundle)
    {
      base.OnCreate(bundle);
      SetContentView(Resource.Layout.ImageLoader);
      // Create your application here
      friends = Util.GenerateFriends();
      imageLoader = new ImageLoader(this, 64, 40);
      ListAdapter = new ImageLoaderAdapter(this, imageLoader, friends);
    }

    protected override void OnStop()
    {
      base.OnStop();
      imageLoader.ClearCache();
    }
  }
}