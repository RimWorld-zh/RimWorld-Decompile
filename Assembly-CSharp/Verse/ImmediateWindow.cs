using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000ECB RID: 3787
	public class ImmediateWindow : Window
	{
		// Token: 0x04003BD2 RID: 15314
		public Action doWindowFunc;

		// Token: 0x06005986 RID: 22918 RVA: 0x002DE164 File Offset: 0x002DC564
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

		// Token: 0x17000E15 RID: 3605
		// (get) Token: 0x06005987 RID: 22919 RVA: 0x002DE1B8 File Offset: 0x002DC5B8
		public override Vector2 InitialSize
		{
			get
			{
				return this.windowRect.size;
			}
		}

		// Token: 0x17000E16 RID: 3606
		// (get) Token: 0x06005988 RID: 22920 RVA: 0x002DE1D8 File Offset: 0x002DC5D8
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06005989 RID: 22921 RVA: 0x002DE1F2 File Offset: 0x002DC5F2
		public override void DoWindowContents(Rect inRect)
		{
			this.doWindowFunc();
		}
	}
}
