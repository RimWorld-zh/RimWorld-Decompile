using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020002B0 RID: 688
	public abstract class MainTabWindow : Window
	{
		// Token: 0x04000686 RID: 1670
		public MainButtonDef def;

		// Token: 0x06000B80 RID: 2944 RVA: 0x00067F40 File Offset: 0x00066340
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
		// (get) Token: 0x06000B81 RID: 2945 RVA: 0x00067F74 File Offset: 0x00066374
		public virtual Vector2 RequestedTabSize
		{
			get
			{
				return new Vector2(1010f, 684f);
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000B82 RID: 2946 RVA: 0x00067F98 File Offset: 0x00066398
		public virtual MainTabWindowAnchor Anchor
		{
			get
			{
				return MainTabWindowAnchor.Left;
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000B83 RID: 2947 RVA: 0x00067FB0 File Offset: 0x000663B0
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

		// Token: 0x06000B84 RID: 2948 RVA: 0x00068011 File Offset: 0x00066411
		public override void DoWindowContents(Rect inRect)
		{
			this.SetInitialSizeAndPosition();
		}

		// Token: 0x06000B85 RID: 2949 RVA: 0x0006801C File Offset: 0x0006641C
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
	}
}
