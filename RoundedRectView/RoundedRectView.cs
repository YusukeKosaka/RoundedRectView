using System;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.CoreGraphics;

namespace RoundedRectView
{
	public class RoundedRectView : UIView
	{
		[Flags]
		public enum ROUND_CORNERS
		{
			/// <summary>
			/// Don't round any corners.
			/// </summary>
			None = 0,
			/// <summary>
			/// Round top left corner.
			/// </summary>
			TopLeft = 1,
			/// <summary>
			/// Round top right corner.
			/// </summary>
			TopRight = 2,
			/// <summary>
			/// Round bottom left corner.
			/// </summary>
			BottomLeft = 4,
			/// <summary>
			/// Round bottom right corner.
			/// </summary>
			BottomRight = 8,
			/// <summary>
			/// Round top corners.
			/// </summary>
			Top = TopLeft | TopRight,
			/// <summary>
			/// Round bottom corners.
			/// </summary>
			Bottom = BottomLeft | BottomRight,
			/// <summary>
			/// Round left corners.
			/// </summary>
			Left = TopLeft | BottomLeft,
			/// <summary>
			/// Round right corners.
			/// </summary>
			Right = TopRight | BottomRight,
			/// <summary>
			/// Round all corners.
			/// </summary>
			AllCorners = Left | Right | Top | Bottom
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Test.RoundedRectView"/> class.
		/// </summary>
		public RoundedRectView () : base()
		{
			this.Setup ();
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Test.RoundedRectView"/> class.
		/// </summary>
		/// <param name='rect'>rectangle of the view</param>
		public RoundedRectView (RectangleF rect) : base(rect)
		{
			this.Setup ();
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Test.RoundedRectView"/> class.
		/// </summary>
		/// <param name='rect'>Rectangle of the view</param>
		/// <param name='oBackgroundColor'>background color</param>
		public RoundedRectView (RectangleF rect, UIColor oBackgroundColor) : base(rect)
		{
			this.Setup ();
			this.BackgroundColor = oBackgroundColor;
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Test.RoundedRectView"/> class.
		/// </summary>
		/// <param name='rect'>Rectangle of the view</param>
		/// <param name='oBackgroundColor'>background color</param>
		/// <param name='eCornerFlags'>rounded corners</param>
		public RoundedRectView (RectangleF rect, UIColor oBackgroundColor, ROUND_CORNERS eCornerFlags) : base(rect)
		{
			this.Setup ();
			this.BackgroundColor = oBackgroundColor;
			this.RoundCorners = eCornerFlags;
		}
		
		/// <summary>
		/// Setup this instance with default values.
		/// </summary>
		private void Setup ()
		{
			// Call base. We override the property. It has to be FALSE.
			base.Opaque = false;
			// Call base. We override this property. Has to be clear color.
			base.BackgroundColor = UIColor.Clear;
			
			// Default settings.
			this.RoundCorners = ROUND_CORNERS.AllCorners;
			this.BackgroundColor = UIColor.White;
			this.oRectColor = UIColor.White;
			this.StrokeWidth = 1.0f;
			this.CornerRadius = 25.0f;
		}
		
		/// <summary>
		/// Setting this has no effect. It has to be FALSE.
		/// </summary>
		public override bool Opaque
		{
			get
			{
				return false;
			}
			set
			{
				// Ignore attempt to set opaque to TRUE.
			}
		}
		
		
		/// <summary>
		/// Gets or sets the color of the background. It really changes the stroke color we use to draw. The background is always clear.
		/// </summary>
		/// <value>
		/// The color of the background.
		/// </value>
		public override UIColor BackgroundColor
		{
			get
			{
				return this.oStrokeColor;
			}
			set
			{
				this.oStrokeColor = value;
				this.oRectColor = value;
			}
		}
		
		private UIColor oStrokeColor;
		private UIColor oRectColor;
		
		/// <summary>
		/// The width of the stroke. Defaults to 1.0f,
		/// </summary>
		public float StrokeWidth;
		/// <summary>
		/// The corner radius. Defaults to 25.0f.
		/// </summary>
		public float CornerRadius;
		
		/// <summary>
		/// The round corners. Default: all corners rounded.
		/// </summary>
		public ROUND_CORNERS RoundCorners;
		
		public override void Draw (RectangleF rect)
		{
			using (var oContext = UIGraphics.GetCurrentContext())
			{
				oContext.SetLineWidth (this.StrokeWidth);
				oContext.SetStrokeColor (this.oStrokeColor.CGColor);
				oContext.SetFillColor (this.oRectColor.CGColor);
    
				RectangleF oRect = this.Bounds;
    
				float fRadius = this.CornerRadius;
				float fWidth = oRect.Width;
				float fHeight = oRect.Height;
    
				// Make sure corner radius isn't larger than half the shorter side.
				if (fRadius > fWidth / 2.0f)
				{
					fRadius = fWidth / 2.0f;
				}
				if (fRadius > fHeight / 2.0f)
				{
					fRadius = fHeight / 2.0f;    
				}
				
				float fMinX = oRect.GetMinX ();
				float fMidX = oRect.GetMidX ();
				float fMaxX = oRect.GetMaxX ();
				float fMinY = oRect.GetMinY ();
				float fMidY = oRect.GetMidY ();
				float fMaxY = oRect.GetMaxY ();
				
				// Move to left middle.
				oContext.MoveTo (fMinX, fMidY);
				
				// Arc to top middle.
				oContext.AddArcToPoint (fMinX, fMinY, fMidX, fMinY, (this.RoundCorners & ROUND_CORNERS.TopLeft) == ROUND_CORNERS.TopLeft ? fRadius : 0);
				// Arc to right middle.
				oContext.AddArcToPoint (fMaxX, fMinY, fMaxX, fMidY, (this.RoundCorners & ROUND_CORNERS.TopRight) == ROUND_CORNERS.TopRight ? fRadius : 0);
				// Arc to bottom middle.
				oContext.AddArcToPoint (fMaxX, fMaxY, fMidX, fMaxY, (this.RoundCorners & ROUND_CORNERS.BottomRight) == ROUND_CORNERS.BottomRight ? fRadius : 0);
				// Arc to left middle.
				oContext.AddArcToPoint (fMinX, fMaxY, fMinX, fMidY, (this.RoundCorners & ROUND_CORNERS.BottomLeft) == ROUND_CORNERS.BottomLeft ? fRadius : 0);
				
				// Draw the path.
				oContext.ClosePath ();
				oContext.DrawPath (CGPathDrawingMode.FillStroke);
			}
		}
	}
}

