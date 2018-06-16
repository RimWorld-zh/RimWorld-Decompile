using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020002AE RID: 686
	public abstract class MainTabWindow : Window
	{
		// Token: 0x06000B7E RID: 2942 RVA: 0x00067D88 File Offset: 0x00066188
		public MainTabWindow()
		{
			this.layer = WindowLayer.GameUI;
			this.soundAppear = null;
			this.soundClose = null;
			this.doCloseButton = false;
			this.doCloseX = false;
			this.preventCameraMotion = false;
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000B7F RID: 2943 RVA: 0x00067DBC File Offset: 0x000661BC
		public virtual Vector2 RequestedTabSize
		{
			get
			{
				return new Vector2(1010f, 684f);
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000B80 RID: 2944 RVA: 0x00067DE0 File Offset: 0x000661E0
		public virtual MainTabWindowAnchor Anchor
		{
			get
			{
				return MainTabWindowAnchor.Left;
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000B81 RID: 2945 RVA: 0x00067DF8 File Offset: 0x000661F8
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

		// Token: 0x06000B82 RID: 2946 RVA: 0x00067E59 File Offset: 0x00066259
		public override void DoWindowContents(Rect inRect)
		{
			this.SetInitialSizeAndPosition();
		}

		// Token: 0x06000B83 RID: 2947 RVA: 0x00067E64 File Offset: 0x00066264
		protected override void SetInitialSizeAndPosition()
		{
			base.SetInitialSizeAndPosition();
			if (this.Anchor == MainTabWindowAnchor.Left)
			{
				this.windowRect.x = 0f;
			}
			else
			{
				this.windowRect.x = (float)UI.screenWidth - this.windowRect.width;
			}
			this.windowRect.y = (float)(UI.screenHeight - 35) - this.windowRect.height;
		}

		// Token: 0x04000687 RID: 1671
		public MainButtonDef def;
	}
}
