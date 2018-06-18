using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EBD RID: 3773
	public class Dialog_MessageBox : Window
	{
		// Token: 0x0600592B RID: 22827 RVA: 0x002DAE58 File Offset: 0x002D9258
		public Dialog_MessageBox(string text, string buttonAText = null, Action buttonAAction = null, string buttonBText = null, Action buttonBAction = null, string title = null, bool buttonADestructive = false, Action acceptAction = null, Action cancelAction = null)
		{
			this.text = text;
			this.buttonAText = buttonAText;
			this.buttonAAction = buttonAAction;
			this.buttonADestructive = buttonADestructive;
			this.buttonBText = buttonBText;
			this.buttonBAction = buttonBAction;
			this.title = title;
			this.acceptAction = acceptAction;
			this.cancelAction = cancelAction;
			if (buttonAText.NullOrEmpty())
			{
				this.buttonAText = "OK".Translate();
			}
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
			this.creationRealTime = RealTime.LastRealTime;
			this.onlyOneOfTypeAllowed = false;
			bool flag = buttonAAction == null && buttonBAction == null && this.buttonCAction == null;
			this.forceCatchAcceptAndCancelEventEvenIfUnfocused = (acceptAction != null || cancelAction != null || flag);
			this.closeOnAccept = flag;
			this.closeOnCancel = flag;
		}

		// Token: 0x17000E07 RID: 3591
		// (get) Token: 0x0600592C RID: 22828 RVA: 0x002DAF54 File Offset: 0x002D9354
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(640f, 460f);
			}
		}

		// Token: 0x17000E08 RID: 3592
		// (get) Token: 0x0600592D RID: 22829 RVA: 0x002DAF78 File Offset: 0x002D9378
		private float TimeUntilInteractive
		{
			get
			{
				return this.interactionDelay - (Time.realtimeSinceStartup - this.creationRealTime);
			}
		}

		// Token: 0x17000E09 RID: 3593
		// (get) Token: 0x0600592E RID: 22830 RVA: 0x002DAFA0 File Offset: 0x002D93A0
		private bool InteractionDelayExpired
		{
			get
			{
				return this.TimeUntilInteractive <= 0f;
			}
		}

		// Token: 0x0600592F RID: 22831 RVA: 0x002DAFC8 File Offset: 0x002D93C8
		public static Dialog_MessageBox CreateConfirmation(string text, Action confirmedAct, bool destructive = false, string title = null)
		{
			string text2 = "Confirm".Translate();
			string text3 = "GoBack".Translate();
			return new Dialog_MessageBox(text, text2, confirmedAct, text3, null, title, destructive, confirmedAct, delegate()
			{
			});
		}

		// Token: 0x06005930 RID: 22832 RVA: 0x002DB028 File Offset: 0x002D9428
		public override void DoWindowContents(Rect inRect)
		{
			float num = inRect.y;
			if (!this.title.NullOrEmpty())
			{
				Text.Font = GameFont.Medium;
				Widgets.Label(new Rect(0f, num, inRect.width, 42f), this.title);
				num += 42f;
			}
			Text.Font = GameFont.Small;
			Rect outRect = new Rect(inRect.x, num, inRect.width, inRect.height - 35f - 5f - num);
			float width = outRect.width - 16f;
			Rect viewRect = new Rect(0f, 0f, width, Text.CalcHeight(this.text, width));
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, viewRect, true);
			Widgets.Label(new Rect(0f, 0f, viewRect.width, viewRect.height), this.text);
			Widgets.EndScrollView();
			int num2 = (!this.buttonCText.NullOrEmpty()) ? 3 : 2;
			float num3 = inRect.width / (float)num2;
			float width2 = num3 - 20f;
			if (this.buttonADestructive)
			{
				GUI.color = new Color(1f, 0.3f, 0.35f);
			}
			string label = (!this.InteractionDelayExpired) ? (this.buttonAText + "(" + Mathf.Ceil(this.TimeUntilInteractive).ToString("F0") + ")") : this.buttonAText;
			if (Widgets.ButtonText(new Rect(num3 * (float)(num2 - 1) + 10f, inRect.height - 35f, width2, 35f), label, true, false, true))
			{
				if (this.InteractionDelayExpired)
				{
					if (this.buttonAAction != null)
					{
						this.buttonAAction();
					}
					this.Close(true);
				}
			}
			GUI.color = Color.white;
			if (this.buttonBText != null)
			{
				if (Widgets.ButtonText(new Rect(0f, inRect.height - 35f, width2, 35f), this.buttonBText, true, false, true))
				{
					if (this.buttonBAction != null)
					{
						this.buttonBAction();
					}
					this.Close(true);
				}
			}
			if (this.buttonCText != null)
			{
				if (Widgets.ButtonText(new Rect(num3 + 10f, inRect.height - 35f, width2, 35f), this.buttonCText, true, false, true))
				{
					if (this.buttonCAction != null)
					{
						this.buttonCAction();
					}
					if (this.buttonCClose)
					{
						this.Close(true);
					}
				}
			}
		}

		// Token: 0x06005931 RID: 22833 RVA: 0x002DB2E8 File Offset: 0x002D96E8
		public override void OnCancelKeyPressed()
		{
			if (this.cancelAction != null)
			{
				this.cancelAction();
				this.Close(true);
			}
			else
			{
				base.OnCancelKeyPressed();
			}
		}

		// Token: 0x06005932 RID: 22834 RVA: 0x002DB315 File Offset: 0x002D9715
		public override void OnAcceptKeyPressed()
		{
			if (this.acceptAction != null)
			{
				this.acceptAction();
				this.Close(true);
			}
			else
			{
				base.OnAcceptKeyPressed();
			}
		}

		// Token: 0x04003B73 RID: 15219
		public string text;

		// Token: 0x04003B74 RID: 15220
		public string title;

		// Token: 0x04003B75 RID: 15221
		public string buttonAText;

		// Token: 0x04003B76 RID: 15222
		public Action buttonAAction;

		// Token: 0x04003B77 RID: 15223
		public bool buttonADestructive;

		// Token: 0x04003B78 RID: 15224
		public string buttonBText;

		// Token: 0x04003B79 RID: 15225
		public Action buttonBAction;

		// Token: 0x04003B7A RID: 15226
		public string buttonCText;

		// Token: 0x04003B7B RID: 15227
		public Action buttonCAction;

		// Token: 0x04003B7C RID: 15228
		public bool buttonCClose = true;

		// Token: 0x04003B7D RID: 15229
		public float interactionDelay = 0f;

		// Token: 0x04003B7E RID: 15230
		public Action acceptAction;

		// Token: 0x04003B7F RID: 15231
		public Action cancelAction;

		// Token: 0x04003B80 RID: 15232
		private Vector2 scrollPosition = Vector2.zero;

		// Token: 0x04003B81 RID: 15233
		private float creationRealTime = -1f;

		// Token: 0x04003B82 RID: 15234
		private const float TitleHeight = 42f;

		// Token: 0x04003B83 RID: 15235
		private const float ButtonHeight = 35f;
	}
}
