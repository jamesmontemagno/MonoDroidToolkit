using System;
using System.Linq;
using System.Reflection;
using Android.Runtime;

namespace MonoDroidToolkit
{
	//From http://forums.xamarin.com/discussion/comment/5816/#Comment_5816
	public static class ResourceIdManager
	{
		static bool _idInitialized;
		public static void UpdateIdValues ()
		{
			if (_idInitialized)
				return;
			var eass = Assembly.GetExecutingAssembly ();
			Func<Assembly,Type> f = ass =>
				ass.GetCustomAttributes (typeof (ResourceDesignerAttribute), true)
					.Select (ca => ca as ResourceDesignerAttribute)
					.Where (ca => ca != null && ca.IsApplication)
					.Select (ca => ass.GetType (ca.FullName))
					.Where (ty => ty != null)
					.FirstOrDefault ();
			var t = f (eass);
			if (t == null)
				t = AppDomain.CurrentDomain.GetAssemblies ().Select (ass => f (ass)).Where (ty => ty != null).FirstOrDefault ();
			if (t != null)
				t.GetMethod ("UpdateIdValues").Invoke (null, new object [0]);
			_idInitialized = true;
		}
	}
}

