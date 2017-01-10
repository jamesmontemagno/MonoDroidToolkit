MonoDroidToolkit
================

Now available on NuGet: https://www.nuget.org/packages/MonoDroid.Toolkit

Build status: [![Build status](https://ci.appveyor.com/api/projects/status/vh63s5x6n39lp56d/branch/master?svg=true)](https://ci.appveyor.com/project/JamesMontemagno/monodroidtoolkit/branch/master)

Created by:
James Montemagno ([@JamesMontemagno](http://www.twitter.com/jamesmontemagno))

Help from:
Tomasz Cielecki ([@Cheesebaron](http://www.twitter.com/Cheesebaron))

## What is it?
MonodroidToolkit is a kit of views that I have created or port from java into Xamarin.Android (Mono for Android). I have found them very useful in my different projects.

## What is included?

### Views

* VerticalTextView : Turn you text 90 or 270!
* ScrollingTextView: Scrolls text in the view
* GridViewNoScroll: Automatically expandes the height of the gridview so you can put it inside of a list view or scroll view
* StickyViewPager: Disable view pager from moving if you desire
* SelectEndEditText: EditText view that always puts cursor at the end when focus is given. Great for remote control interfaces.
* ProgressButton: Progress Bar button ported from :[@f2prateek](https://github.com/f2prateek/progressbutton)
* [HoloCircularProgressBar](https://github.com/jamesmontemagno/MonoDroidToolkit/wiki/HoloCircularProgressBar): Holo Circular Progress Bar, Similar to 4.1 clock, ported from [@passsy](https://github.com/passsy/android-HoloCircularProgressBar) *IMPORTANT* I have 0 to 100 scale which differs from original

### Utilities

* SystemUiHider: port of auto hiding the ui in Android 
* NetworkUtils: Get IP/MAC/Hostname of android device
* ImageLoader: Simple delay image loader and caching mechanism

### Preferences

* IntEditTextPrefence: Enfoces an integer is entered on an edit text preference
* IntListPrefrences: Allows you to specify integers as the values.

### Screenshots
![Progress Button](https://raw.github.com/jamesmontemagno/MonoDroidToolkit/master/Screenshots/ProgressBarInDeviceSmall.png)
![Circular Progress Bar](https://raw.github.com/jamesmontemagno/MonoDroidToolkit/master/Screenshots/CircularDarkInDeviceSmall.png)


For more screenshots look in the [Screenshots](https://raw.github.com/jamesmontemagno/MonoDroidToolkit/master/Screenshots) folder

## Want to see something?
Simply open up an issue.

## Apps using the toolkit
* Have an app using the toolkit? Open an issue

## License
Licensed under the [Apache License, Version 2.0](http://www.apache.org/licenses/LICENSE-2.0.html)
