using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020002AE RID: 686
	public abstract class MainTabWindow : Window
	{
		// Token: 0x06000B7C RID: 2940 RVA: 0x00067DF0 File Offset: 0x000661F0
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
		// (get) Token: 0x06000B7D RID: 2941 RVA: 0x00067E24 File Offset: 0x00066224
		public virtual Vector2 RequestedTabSize
		{
			get
			{
				return new Vector2(1010f, 684f);
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000B7E RID: 2942 RVA: 0x00067E48 File Offset: 0x00066248
		public virtual MainTabWindowAnchor Anchor
		{
			get
			{
				return MainTabWindowAnchor.Left;
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000B7F RID: 2943 RVA: 0x00067E60 File Offset: 0x00066260
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

		// Token: 0x06000B80 RID: 2944 RVA: 0x00067EC1 File Offset: 0x000662C1
		public override void DoWindowContents(Rect inRect)
		{
			this.SetInitialSizeAndPosition();
		}

		// Token: 0x06000B81 RID: 2945 RVA: 0x00067ECC File Offset: 0x000662CC
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

		// Token: 0x04000686 RID: 1670
		public MainButtonDef def;
	}
}
