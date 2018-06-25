using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020002B0 RID: 688
	public abstract class MainTabWindow : Window
	{
		// Token: 0x04000688 RID: 1672
		public MainButtonDef def;

		// Token: 0x06000B7F RID: 2943 RVA: 0x00067F3C File Offset: 0x0006633C
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
		// (get) Token: 0x06000B80 RID: 2944 RVA: 0x00067F70 File Offset: 0x00066370
		public virtual Vector2 RequestedTabSize
		{
			get
			{
				return new Vector2(1010f, 684f);
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000B81 RID: 2945 RVA: 0x00067F94 File Offset: 0x00066394
		public virtual MainTabWindowAnchor Anchor
		{
			get
			{
				return MainTabWindowAnchor.Left;
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000B82 RID: 2946 RVA: 0x00067FAC File Offset: 0x000663AC
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

		// Token: 0x06000B83 RID: 2947 RVA: 0x0006800D File Offset: 0x0006640D
		public override void DoWindowContents(Rect inRect)
		{
			this.SetInitialSizeAndPosition();
		}

		// Token: 0x06000B84 RID: 2948 RVA: 0x00068018 File Offset: 0x00066418
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
