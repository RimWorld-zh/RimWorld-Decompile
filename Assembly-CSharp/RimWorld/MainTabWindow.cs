using UnityEngine;
using Verse;

namespace RimWorld
{
	public abstract class MainTabWindow : Window
	{
		public MainButtonDef def;

		public virtual Vector2 RequestedTabSize
		{
			get
			{
				return new Vector2(1010f, 684f);
			}
		}

		public virtual MainTabWindowAnchor Anchor
		{
			get
			{
				return MainTabWindowAnchor.Left;
			}
		}

		public override Vector2 InitialSize
		{
			get
			{
				Vector2 requestedTabSize = this.RequestedTabSize;
				if (requestedTabSize.y > (float)(UI.screenHeight - 35))
				{
					requestedTabSize.y = (float)(UI.screenHeight - 35);
				}
				if (requestedTabSize.x > (float)UI.screenWidth)
				{
					requestedTabSize.x = (float)UI.screenWidth;
				}
				return requestedTabSize;
			}
		}

		public MainTabWindow()
		{
			base.layer = WindowLayer.GameUI;
			base.soundAppear = null;
			base.soundClose = null;
			base.doCloseButton = false;
			base.doCloseX = false;
			base.closeOnEscapeKey = true;
			base.preventCameraMotion = false;
		}

		public override void DoWindowContents(Rect inRect)
		{
			this.SetInitialSizeAndPosition();
		}

		protected override void SetInitialSizeAndPosition()
		{
			base.SetInitialSizeAndPosition();
			if (this.Anchor == MainTabWindowAnchor.Left)
			{
				base.windowRect.x = 0f;
			}
			else
			{
				base.windowRect.x = (float)UI.screenWidth - base.windowRect.width;
			}
			base.windowRect.y = (float)(UI.screenHeight - 35) - base.windowRect.height;
		}
	}
}
