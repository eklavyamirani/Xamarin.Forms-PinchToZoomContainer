using System;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using System.Linq;
using Xamarin.Forms;
using TestPinchToZoom;
using TestPinchToZoom.iOS;

[assembly:ExportRenderer(typeof(PinchToZoomContainer), typeof(PinchToZoomRenderer))]
namespace TestPinchToZoom.iOS
{
	public class PinchToZoomRenderer : ViewRenderer<PinchToZoomContainer, UIScrollView>
	{
		private View Content;
		private bool childrenCreated = false;

		protected override void OnElementChanged (ElementChangedEventArgs<PinchToZoomContainer> e)
		{
			base.OnElementChanged (e);

			if (e.NewElement == null) {
				return;
			}

			if (Control == null) {
				SetNativeControl (new UIScrollView ());
			}

			Control.MaximumZoomScale = 3f;
			Control.MinimumZoomScale = 0.1f;
			Control.ViewForZoomingInScrollView += GetViewForZoomingInScrollView;
			this.Content = e.NewElement.Content;

		}

		private UIView GetViewForZoomingInScrollView(UIScrollView scrollView)
		{
			return scrollView.Subviews.FirstOrDefault ();
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			if (Control == null || Control.Subviews == null) 
			{
				return;
			}

			if (!childrenCreated) 
			{
				// render the child Content of the view
				var childRenderer = RendererFactory.GetRenderer (this.Content);
				// This is important. The renderer requires the size of the element to render.
				childRenderer.SetElementSize (new Size (Control.Frame.Width, Control.Frame.Height));
				var nativeChildView = childRenderer.NativeView;
				// Again, we need to provide a frame, since we are laying out the view
				nativeChildView.Frame = Control.Frame;
				Control.AddSubview (nativeChildView);
				this.childrenCreated = true;
			}

		}
	}
}

