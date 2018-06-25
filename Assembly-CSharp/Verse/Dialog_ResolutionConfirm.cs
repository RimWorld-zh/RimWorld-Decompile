using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EBA RID: 3770
	public class Dialog_ResolutionConfirm : Window
	{
		// Token: 0x04003B73 RID: 15219
		private float startTime;

		// Token: 0x04003B74 RID: 15220
		private IntVec2 oldRes;

		// Token: 0x04003B75 RID: 15221
		private bool oldFullscreen;

		// Token: 0x04003B76 RID: 15222
		private float oldUIScale;

		// Token: 0x04003B77 RID: 15223
		private const float RevertTime = 10f;

		// Token: 0x0600592E RID: 22830 RVA: 0x002DC3B9 File Offset: 0x002DA7B9
		private Dialog_ResolutionConfirm()
		{
			this.startTime = Time.realtimeSinceStartup;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x0600592F RID: 22831 RVA: 0x002DC3E2 File Offset: 0x002DA7E2
		public Dialog_ResolutionConfirm(bool oldFullscreen) : this()
		{
			this.oldFullscreen = oldFullscreen;
			this.oldRes = new IntVec2(Screen.width, Screen.height);
			this.oldUIScale = Prefs.UIScale;
		}

		// Token: 0x06005930 RID: 22832 RVA: 0x002DC412 File Offset: 0x002DA812
		public Dialog_ResolutionConfirm(IntVec2 oldRes) : this()
		{
			this.oldFullscreen = Screen.fullScreen;
			this.oldRes = oldRes;
			this.oldUIScale = Prefs.UIScale;
		}

		// Token: 0x06005931 RID: 22833 RVA: 0x002DC438 File Offset: 0x002DA838
		public Dialog_ResolutionConfirm(float oldUIScale) : this()
		{
			this.oldFullscreen = Screen.fullScreen;
			this.oldRes = new IntVec2(Screen.width, Screen.height);
			this.oldUIScale = oldUIScale;
		}

		// Token: 0x17000E00 RID: 3584
		// (get) Token: 0x06005932 RID: 22834 RVA: 0x002DC468 File Offset: 0x002DA868
		private float TimeUntilRevert
		{
			get
			{
				return this.startTime + 10f - Time.realtimeSinceStartup;
			}
		}

		// Token: 0x17000E01 RID: 3585
		// (get) Token: 0x06005933 RID: 22835 RVA: 0x002DC490 File Offset: 0x002DA890
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(500f, 300f);
			}
		}

		// Token: 0x06005934 RID: 22836 RVA: 0x002DC4B4 File Offset: 0x002DA8B4
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

		// Token: 0x06005935 RID: 22837 RVA: 0x002DC5C4 File Offset: 0x002DA9C4
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

		// Token: 0x06005936 RID: 22838 RVA: 0x002DC665 File Offset: 0x002DAA65
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
