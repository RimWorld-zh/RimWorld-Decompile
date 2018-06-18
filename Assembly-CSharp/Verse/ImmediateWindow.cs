using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EC9 RID: 3785
	public class ImmediateWindow : Window
	{
		// Token: 0x06005962 RID: 22882 RVA: 0x002DC20C File Offset: 0x002DA60C
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

		// Token: 0x17000E13 RID: 3603
		// (get) Token: 0x06005963 RID: 22883 RVA: 0x002DC260 File Offset: 0x002DA660
		public override Vector2 InitialSize
		{
			get
			{
				return this.windowRect.size;
			}
		}

		// Token: 0x17000E14 RID: 3604
		// (get) Token: 0x06005964 RID: 22884 RVA: 0x002DC280 File Offset: 0x002DA680
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06005965 RID: 22885 RVA: 0x002DC29A File Offset: 0x002DA69A
		public override void DoWindowContents(Rect inRect)
		{
			this.doWindowFunc();
		}

		// Token: 0x04003BBA RID: 15290
		public Action doWindowFunc;
	}
}
