using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MonoDroidToolkit;

namespace Samples
{
  [Activity(Label = "Network Utils")]
  public class NetworkActivity : Activity
  {
    protected override void OnCreate(Bundle bundle)
    {
      base.OnCreate(bundle);
      SetContentView(Resource.Layout.Network);

      Task.Run(() =>
      {
        FindViewById<TextView>(Resource.Id.textView1).Text = "Host Name: " + NetworkUtils.GetHostName();
      });
      FindViewById<TextView>(Resource.Id.textView2).Text = "IP: " + NetworkUtils.GetIPAddress();
      FindViewById<TextView>(Resource.Id.textView3).Text = "Mac eth: " + NetworkUtils.GetMacAddress("eth0");
      FindViewById<TextView>(Resource.Id.textView4).Text = "Mac wlan: " + NetworkUtils.GetMacAddress("wlan0");
      FindViewById<TextView>(Resource.Id.textView5).Text = "Mac null: " + NetworkUtils.GetMacAddress();
      var interfaces = string.Empty;
      foreach (var inter in NetworkUtils.GetAllNetworkInterfaces())
      {
        interfaces += inter.NetworkInterface + ", ";
      }
      FindViewById<TextView>(Resource.Id.textView6).Text = "Interfaces: " + interfaces;
      
    }
  }
}