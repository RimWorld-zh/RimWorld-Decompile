using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000ECA RID: 3786
	public class ImmediateWindow : Window
	{
		// Token: 0x06005964 RID: 22884 RVA: 0x002DC1D4 File Offset: 0x002DA5D4
		public ImmediateWindow()
		{
			this.doCloseButton = false;
			this.doCloseX = false;
			this.soundAppear = null;
			this.soundClose = null;
			this.closeOnClickedOutside = false;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.focusWhenOpened = false;
			this.preventCameraMotion = false;
		}

		// Token: 0x17000E14 RID: 3604
		// (get) Token: 0x06005965 RID: 22885 RVA: 0x002DC228 File Offset: 0x002DA628
		public override Vector2 InitialSize
		{
			get
			{
				return this.windowRect.size;
			}
		}

		// Token: 0x17000E15 RID: 3605
		// (get) Token: 0x06005966 RID: 22886 RVA: 0x002DC248 File Offset: 0x002DA648
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06005967 RID: 22887 RVA: 0x002DC262 File Offset: 0x002DA662
		public override void DoWindowContents(Rect inRect)
		{
			this.doWindowFunc();
		}

		// Token: 0x04003BBB RID: 15291
		public Action doWindowFunc;
	}
}
