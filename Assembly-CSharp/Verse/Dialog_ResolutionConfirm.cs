using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EB8 RID: 3768
	public class Dialog_ResolutionConfirm : Window
	{
		// Token: 0x06005909 RID: 22793 RVA: 0x002DA455 File Offset: 0x002D8855
		private Dialog_ResolutionConfirm()
		{
			this.startTime = Time.realtimeSinceStartup;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x0600590A RID: 22794 RVA: 0x002DA47E File Offset: 0x002D887E
		public Dialog_ResolutionConfirm(bool oldFullscreen) : this()
		{
			this.oldFullscreen = oldFullscreen;
			this.oldRes = new IntVec2(Screen.width, Screen.height);
			this.oldUIScale = Prefs.UIScale;
		}

		// Token: 0x0600590B RID: 22795 RVA: 0x002DA4AE File Offset: 0x002D88AE
		public Dialog_ResolutionConfirm(IntVec2 oldRes) : this()
		{
			this.oldFullscreen = Screen.fullScreen;
			this.oldRes = oldRes;
			this.oldUIScale = Prefs.UIScale;
		}

		// Token: 0x0600590C RID: 22796 RVA: 0x002DA4D4 File Offset: 0x002D88D4
		public Dialog_ResolutionConfirm(float oldUIScale) : this()
		{
			this.oldFullscreen = Screen.fullScreen;
			this.oldRes = new IntVec2(Screen.width, Screen.height);
			this.oldUIScale = oldUIScale;
		}

		// Token: 0x17000DFE RID: 3582
		// (get) Token: 0x0600590D RID: 22797 RVA: 0x002DA504 File Offset: 0x002D8904
		private float TimeUntilRevert
		{
			get
			{
				return this.startTime + 10f - Time.realtimeSinceStartup;
			}
		}

		// Token: 0x17000DFF RID: 3583
		// (get) Token: 0x0600590E RID: 22798 RVA: 0x002DA52C File Offset: 0x002D892C
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(500f, 300f);
			}
		}

		// Token: 0x0600590F RID: 22799 RVA: 0x002DA550 File Offset: 0x002D8950
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

		// Token: 0x06005910 RID: 22800 RVA: 0x002DA660 File Offset: 0x002D8A60
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

		// Token: 0x06005911 RID: 22801 RVA: 0x002DA701 File Offset: 0x002D8B01
		public override void WindowUpdate()
		{
			if (this.TimeUntilRevert <= 0f)
			{
				this.Revert();
				this.Close(true);
			}
		}

		// Token: 0x04003B5B RID: 15195
		private float startTime;

		// Token: 0x04003B5C RID: 15196
		private IntVec2 oldRes;

		// Token: 0x04003B5D RID: 15197
		private bool oldFullscreen;

		// Token: 0x04003B5E RID: 15198
		private float oldUIScale;

		// Token: 0x04003B5F RID: 15199
		private const float RevertTime = 10f;
	}
}
