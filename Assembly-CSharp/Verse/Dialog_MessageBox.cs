using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EBE RID: 3774
	public class Dialog_MessageBox : Window
	{
		// Token: 0x04003B83 RID: 15235
		public string text;

		// Token: 0x04003B84 RID: 15236
		public string title;

		// Token: 0x04003B85 RID: 15237
		public string buttonAText;

		// Token: 0x04003B86 RID: 15238
		public Action buttonAAction;

		// Token: 0x04003B87 RID: 15239
		public bool buttonADestructive;

		// Token: 0x04003B88 RID: 15240
		public string buttonBText;

		// Token: 0x04003B89 RID: 15241
		public Action buttonBAction;

		// Token: 0x04003B8A RID: 15242
		public string buttonCText;

		// Token: 0x04003B8B RID: 15243
		public Action buttonCAction;

		// Token: 0x04003B8C RID: 15244
		public bool buttonCClose = true;

		// Token: 0x04003B8D RID: 15245
		public float interactionDelay = 0f;

		// Token: 0x04003B8E RID: 15246
		public Action acceptAction;

		// Token: 0x04003B8F RID: 15247
		public Action cancelAction;

		// Token: 0x04003B90 RID: 15248
		private Vector2 scrollPosition = Vector2.zero;

		// Token: 0x04003B91 RID: 15249
		private float creationRealTime = -1f;

		// Token: 0x04003B92 RID: 15250
		private const float TitleHeight = 42f;

		// Token: 0x04003B93 RID: 15251
		private const float ButtonHeight = 35f;

		// Token: 0x0600594F RID: 22863 RVA: 0x002DCBC4 File Offset: 0x002DAFC4
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

		// Token: 0x17000E09 RID: 3593
		// (get) Token: 0x06005950 RID: 22864 RVA: 0x002DCCC0 File Offset: 0x002DB0C0
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(640f, 460f);
			}
		}

		// Token: 0x17000E0A RID: 3594
		// (get) Token: 0x06005951 RID: 22865 RVA: 0x002DCCE4 File Offset: 0x002DB0E4
		private float TimeUntilInteractive
		{
			get
			{
				return this.interactionDelay - (Time.realtimeSinceStartup - this.creationRealTime);
			}
		}

		// Token: 0x17000E0B RID: 3595
		// (get) Token: 0x06005952 RID: 22866 RVA: 0x002DCD0C File Offset: 0x002DB10C
		private bool InteractionDelayExpired
		{
			get
			{
				return this.TimeUntilInteractive <= 0f;
			}
		}

		// Token: 0x06005953 RID: 22867 RVA: 0x002DCD34 File Offset: 0x002DB134
		public static Dialog_MessageBox CreateConfirmation(string text, Action confirmedAct, bool destructive = false, string title = null)
		{
			string text2 = "Confirm".Translate();
			string text3 = "GoBack".Translate();
			return new Dialog_MessageBox(text, text2, confirmedAct, text3, null, title, destructive, confirmedAct, delegate()
			{
			});
		}

		// Token: 0x06005954 RID: 22868 RVA: 0x002DCD94 File Offset: 0x002DB194
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

		// Token: 0x06005955 RID: 22869 RVA: 0x002DD054 File Offset: 0x002DB454
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

		// Token: 0x06005956 RID: 22870 RVA: 0x002DD081 File Offset: 0x002DB481
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
	}
}
