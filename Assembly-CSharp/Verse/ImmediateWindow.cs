using System;
using UnityEngine;

namespace Verse
{
	public class ImmediateWindow : Window
	{
		public Action doWindowFunc;

		public override Vector2 InitialSize
		{
			get
			{
				return base.windowRect.size;
			}
		}

		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		public ImmediateWindow()
		{
			base.doCloseButton = false;
			base.doCloseX = false;
			base.soundAppear = null;
			base.soundClose = null;
			base.closeOnClickedOutside = false;
			base.closeOnEscapeKey = false;
			base.focusWhenOpened = false;
			base.preventCameraMotion = false;
		}

		public override void DoWindowContents(Rect inRect)
		{
			this.doWindowFunc();
		}
	}
}
