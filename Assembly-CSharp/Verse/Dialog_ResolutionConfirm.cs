using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EB9 RID: 3769
	public class Dialog_ResolutionConfirm : Window
	{
		// Token: 0x0600590B RID: 22795 RVA: 0x002DA41D File Offset: 0x002D881D
		private Dialog_ResolutionConfirm()
		{
			this.startTime = Time.realtimeSinceStartup;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x0600590C RID: 22796 RVA: 0x002DA446 File Offset: 0x002D8846
		public Dialog_ResolutionConfirm(bool oldFullscreen) : this()
		{
			this.oldFullscreen = oldFullscreen;
			this.oldRes = new IntVec2(Screen.width, Screen.height);
			this.oldUIScale = Prefs.UIScale;
		}

		// Token: 0x0600590D RID: 22797 RVA: 0x002DA476 File Offset: 0x002D8876
		public Dialog_ResolutionConfirm(IntVec2 oldRes) : this()
		{
			this.oldFullscreen = Screen.fullScreen;
			this.oldRes = oldRes;
			this.oldUIScale = Prefs.UIScale;
		}

		// Token: 0x0600590E RID: 22798 RVA: 0x002DA49C File Offset: 0x002D889C
		public Dialog_ResolutionConfirm(float oldUIScale) : this()
		{
			this.oldFullscreen = Screen.fullScreen;
			this.oldRes = new IntVec2(Screen.width, Screen.height);
			this.oldUIScale = oldUIScale;
		}

		// Token: 0x17000DFF RID: 3583
		// (get) Token: 0x0600590F RID: 22799 RVA: 0x002DA4CC File Offset: 0x002D88CC
		private float TimeUntilRevert
		{
			get
			{
				return this.startTime + 10f - Time.realtimeSinceStartup;
			}
		}

		// Token: 0x17000E00 RID: 3584
		// (get) Token: 0x06005910 RID: 22800 RVA: 0x002DA4F4 File Offset: 0x002D88F4
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(500f, 300f);
			}
		}

		// Token: 0x06005911 RID: 22801 RVA: 0x002DA518 File Offset: 0x002D8918
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Small;
			string label = "ConfirmResolutionChange".Translate(new object[]
			{
				Mathf.CeilToInt(this.TimeUntilRevert)
			});
			Widgets.Label(new Rect(0f, 0f, inRect.width, inRect.height), label);
			if (Widgets.ButtonText(new Rect(0f, inRect.height - 35f, inRect.width / 2f - 20f, 35f), "ResolutionKeep".Translate(), true, false, true))
			{
				this.Close(true);
			}
			if (Widgets.ButtonText(new Rect(inRect.width / 2f + 20f, inRect.height - 35f, inRect.width / 2f - 20f, 35f), "ResolutionRevert".Translate(), true, false, true))
			{
				this.Revert();
				this.Close(true);
			}
		}

		// Token: 0x06005912 RID: 22802 RVA: 0x002DA628 File Offset: 0x002D8A28
		private void Revert()
		{
			if (Prefs.LogVerbose)
			{
				Log.Message(string.Concat(new object[]
				{
					"Reverting screen settings to ",
					this.oldRes.x,
					"x",
					this.oldRes.z,
					", fs=",
					this.oldFullscreen
				}), false);
			}
			Screen.SetResolution(this.oldRes.x, this.oldRes.z, this.oldFullscreen);
			Prefs.UIScale = this.oldUIScale;
		}

		// Token: 0x06005913 RID: 22803 RVA: 0x002DA6C9 File Offset: 0x002D8AC9
		public override void WindowUpdate()
		{
			if (this.TimeUntilRevert <= 0f)
			{
				this.Revert();
				this.Close(true);
			}
		}

		// Token: 0x04003B5C RID: 15196
		private float startTime;

		// Token: 0x04003B5D RID: 15197
		private IntVec2 oldRes;

		// Token: 0x04003B5E RID: 15198
		private bool oldFullscreen;

		// Token: 0x04003B5F RID: 15199
		private float oldUIScale;

		// Token: 0x04003B60 RID: 15200
		private const float RevertTime = 10f;
	}
}
