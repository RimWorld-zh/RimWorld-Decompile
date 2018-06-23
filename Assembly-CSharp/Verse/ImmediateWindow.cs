using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EC8 RID: 3784
	public class ImmediateWindow : Window
	{
		// Token: 0x04003BCA RID: 15306
		public Action doWindowFunc;

		// Token: 0x06005983 RID: 22915 RVA: 0x002DDE58 File Offset: 0x002DC258
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

		// Token: 0x17000E16 RID: 3606
		// (get) Token: 0x06005984 RID: 22916 RVA: 0x002DDEAC File Offset: 0x002DC2AC
		public override Vector2 InitialSize
		{
			get
			{
				return this.windowRect.size;
			}
		}

		// Token: 0x17000E17 RID: 3607
		// (get) Token: 0x06005985 RID: 22917 RVA: 0x002DDECC File Offset: 0x002DC2CC
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06005986 RID: 22918 RVA: 0x002DDEE6 File Offset: 0x002DC2E6
		public override void DoWindowContents(Rect inRect)
		{
			this.doWindowFunc();
		}
	}
}
