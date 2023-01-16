using Android.Content;
using Android.Graphics.Drawables;
using Android.Views;
using NewsApp.CustomRenderers;
using NewsApp.Droid.CustomRenderers;
using Plugin.FirebaseCrashlytics;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(GradientButton), typeof(GradientButtonRenderer))]

namespace NewsApp.Droid.CustomRenderers
{
	public class GradientButtonRenderer : ButtonRenderer
	{
		#region instances

		private GradientDrawable _gradient;

		#endregion instances

		#region constructor

		public GradientButtonRenderer(Context context) : base(context)
		{
		}

		#endregion constructor

		#region overridable

		protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null || Element == null)
			{
				Control.Touch -= ButtonTouched;
			}

			if (Control != null)
			{
				try
				{
					Control.Touch += ButtonTouched;
					Control.StateListAnimator = new Android.Animation.StateListAnimator();
					Control.SetBackground(DrawGradient(e));
				}
				catch (Exception ex)
				{
					CrossFirebaseCrashlytics.Current.RecordException(ex);
				}
			}
		}

		#endregion overridable

		#region EventHandler

		/// <summary>
		/// Draw the gradient with the correct oppacity
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ButtonTouched(object sender, TouchEventArgs e)
		{
			e.Handled = false;

			if (e.Event.Action == MotionEventActions.Down)
			{
				_gradient.Alpha = 200;
				Control.SetBackground(_gradient);
			}
			else if (e.Event.Action == MotionEventActions.Up)
			{
				_gradient.Alpha = 255;
				Control.SetBackground(_gradient);
			}
		}

		#endregion EventHandler

		#region privates

		/// <summary>
		/// Create the gradient for the button background
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
		private GradientDrawable DrawGradient(ElementChangedEventArgs<Xamarin.Forms.Button> e)
		{
			var button = e.NewElement as GradientButton;
			var orientation = button.GradientOrientation == GradientButton.GradientOrientationStates.Horizontal ?
				GradientDrawable.Orientation.LeftRight : GradientDrawable.Orientation.TopBottom;

			_gradient = new GradientDrawable(orientation, new[] {
				button.StartColor.ToAndroid().ToArgb(),
				button.EndColor.ToAndroid().ToArgb(),
			});

			_gradient.SetCornerRadius(button.CornerRadius * 10);
			_gradient.SetStroke(0, button.StartColor.ToAndroid());

			return _gradient;
		}

		#endregion privates
	}
}