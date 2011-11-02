using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;

namespace RoundedRectView
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		UIWindow window;

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			// create a new window instance based on the screen size
			window = new UIWindow (UIScreen.MainScreen.Bounds);
			window.BackgroundColor = UIColor.ScrollViewTexturedBackgroundColor;
			
			// All corners rounded.
			var v = new RoundedRectView (new RectangleF (100, 100, 400, 200), UIColor.Green);
			window.AddSubview (v);
			
			// Bottom corners rounded.
			v = new RoundedRectView (new RectangleF (100, 400, 200, 200), UIColor.Red, RoundedRectView.ROUND_CORNERS.Bottom);
			window.AddSubview (v);
			
			// Top left and bottom right rounded.
			v = new RoundedRectView (new RectangleF (400, 500, 100, 100), UIColor.Blue, RoundedRectView.ROUND_CORNERS.TopLeft | RoundedRectView.ROUND_CORNERS.BottomRight);
			window.AddSubview (v);
			
			// Texture background.
			v = new RoundedRectView (new RectangleF (480, 630, 190, 190), UIColor.Black);
			window.AddSubview (v);
			v = new RoundedRectView (new RectangleF (500, 650, 150, 150), UIColor.ScrollViewTexturedBackgroundColor);
			window.AddSubview (v);
			
			// make the window visible
			window.MakeKeyAndVisible ();
			
			return true;
		}
	}
}

