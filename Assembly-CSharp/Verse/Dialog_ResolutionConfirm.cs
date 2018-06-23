using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EB7 RID: 3767
	public class Dialog_ResolutionConfirm : Window
	{
		// Token: 0x04003B6B RID: 15211
		private float startTime;

		// Token: 0x04003B6C RID: 15212
		private IntVec2 oldRes;

		// Token: 0x04003B6D RID: 15213
		private bool oldFullscreen;

		// Token: 0x04003B6E RID: 15214
		private float oldUIScale;

		// Token: 0x04003B6F RID: 15215
		private const float RevertTime = 10f;

		// Token: 0x0600592A RID: 22826 RVA: 0x002DC0A1 File Offset: 0x002DA4A1
		private Dialog_ResolutionConfirm()
		{
			this.startTime = Time.realtimeSinceStartup;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x0600592B RID: 22827 RVA: 0x002DC0CA File Offset: 0x002DA4CA
		public Dialog_ResolutionConfirm(bool oldFullscreen) : this()
		{
			this.oldFullscreen = oldFullscreen;
			this.oldRes = new IntVec2(Screen.width, Screen.height);
			this.oldUIScale = Prefs.UIScale;
		}

		// Token: 0x0600592C RID: 22828 RVA: 0x002DC0FA File Offset: 0x002DA4FA
		public Dialog_ResolutionConfirm(IntVec2 oldRes) : this()
		{
			this.oldFullscreen = Screen.fullScreen;
			this.oldRes = oldRes;
			this.oldUIScale = Prefs.UIScale;
		}

		// Token: 0x0600592D RID: 22829 RVA: 0x002DC120 File Offset: 0x002DA520
		public Dialog_ResolutionConfirm(float oldUIScale) : this()
		{
			this.oldFullscreen = Screen.fullScreen;
			this.oldRes = new IntVec2(Screen.width, Screen.height);
			this.oldUIScale = oldUIScale;
		}

		// Token: 0x17000E01 RID: 3585
		// (get) Token: 0x0600592E RID: 22830 RVA: 0x002DC150 File Offset: 0x002DA550
		private float TimeUntilRevert
		{
			get
			{
				return this.startTime + 10f - Time.realtimeSinceStartup;
			}
		}

		// Token: 0x17000E02 RID: 3586
		// (get) Token: 0x0600592F RID: 22831 RVA: 0x002DC178 File Offset: 0x002DA578
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(500f, 300f);
			}
		}

		// Token: 0x06005930 RID: 22832 RVA: 0x002DC19C File Offset: 0x002DA59C
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

		// Token: 0x06005931 RID: 22833 RVA: 0x002DC2AC File Offset: 0x002DA6AC
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

		// Token: 0x06005932 RID: 22834 RVA: 0x002DC34D File Offset: 0x002DA74D
		public override void WindowUpdate()
		{
			if (this.TimeUntilRevert <= 0f)
			{
				this.Revert();
				this.Close(true);
			}
		}
	}
}
