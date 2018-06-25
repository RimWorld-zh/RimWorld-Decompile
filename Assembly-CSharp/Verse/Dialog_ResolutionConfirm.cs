using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EB9 RID: 3769
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

		// Token: 0x0600592E RID: 22830 RVA: 0x002DC1CD File Offset: 0x002DA5CD
		private Dialog_ResolutionConfirm()
		{
			this.startTime = Time.realtimeSinceStartup;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x0600592F RID: 22831 RVA: 0x002DC1F6 File Offset: 0x002DA5F6
		public Dialog_ResolutionConfirm(bool oldFullscreen) : this()
		{
			this.oldFullscreen = oldFullscreen;
			this.oldRes = new IntVec2(Screen.width, Screen.height);
			this.oldUIScale = Prefs.UIScale;
		}

		// Token: 0x06005930 RID: 22832 RVA: 0x002DC226 File Offset: 0x002DA626
		public Dialog_ResolutionConfirm(IntVec2 oldRes) : this()
		{
			this.oldFullscreen = Screen.fullScreen;
			this.oldRes = oldRes;
			this.oldUIScale = Prefs.UIScale;
		}

		// Token: 0x06005931 RID: 22833 RVA: 0x002DC24C File Offset: 0x002DA64C
		public Dialog_ResolutionConfirm(float oldUIScale) : this()
		{
			this.oldFullscreen = Screen.fullScreen;
			this.oldRes = new IntVec2(Screen.width, Screen.height);
			this.oldUIScale = oldUIScale;
		}

		// Token: 0x17000E00 RID: 3584
		// (get) Token: 0x06005932 RID: 22834 RVA: 0x002DC27C File Offset: 0x002DA67C
		private float TimeUntilRevert
		{
			get
			{
				return this.startTime + 10f - Time.realtimeSinceStartup;
			}
		}

		// Token: 0x17000E01 RID: 3585
		// (get) Token: 0x06005933 RID: 22835 RVA: 0x002DC2A4 File Offset: 0x002DA6A4
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(500f, 300f);
			}
		}

		// Token: 0x06005934 RID: 22836 RVA: 0x002DC2C8 File Offset: 0x002DA6C8
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

		// Token: 0x06005935 RID: 22837 RVA: 0x002DC3D8 File Offset: 0x002DA7D8
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

		// Token: 0x06005936 RID: 22838 RVA: 0x002DC479 File Offset: 0x002DA879
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
