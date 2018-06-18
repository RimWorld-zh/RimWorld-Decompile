using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EC7 RID: 3783
	public class Dialog_NodeTree : Window
	{
		// Token: 0x06005954 RID: 22868 RVA: 0x001827C8 File Offset: 0x00180BC8
		public Dialog_NodeTree(DiaNode nodeRoot, bool delayInteractivity = false, bool radioMode = false, string title = null)
		{
			this.title = title;
			this.GotoNode(nodeRoot);
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			if (delayInteractivity)
			{
				this.makeInteractiveAtTime = Time.realtimeSinceStartup + 0.5f;
			}
			this.soundAppear = SoundDefOf.CommsWindow_Open;
			this.soundClose = SoundDefOf.CommsWindow_Close;
			if (radioMode)
			{
				this.soundAmbient = SoundDefOf.RadioComms_Ambience;
			}
		}

		// Token: 0x17000E0F RID: 3599
		// (get) Token: 0x06005955 RID: 22869 RVA: 0x00182850 File Offset: 0x00180C50
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(620f, 480f);
			}
		}

		// Token: 0x17000E10 RID: 3600
		// (get) Token: 0x06005956 RID: 22870 RVA: 0x00182874 File Offset: 0x00180C74
		private bool InteractiveNow
		{
			get
			{
				return Time.realtimeSinceStartup >= this.makeInteractiveAtTime;
			}
		}

		// Token: 0x06005957 RID: 22871 RVA: 0x00182899 File Offset: 0x00180C99
		public override void PreClose()
		{
			base.PreClose();
			this.curNode.PreClose();
		}

		// Token: 0x06005958 RID: 22872 RVA: 0x001828AD File Offset: 0x00180CAD
		public override void PostClose()
		{
			base.PostClose();
			if (this.closeAction != null)
			{
				this.closeAction();
			}
		}

		// Token: 0x06005959 RID: 22873 RVA: 0x001828CC File Offset: 0x00180CCC
		public override void WindowOnGUI()
		{
			if (this.screenFillColor != Color.clear)
			{
				GUI.color = this.screenFillColor;
				GUI.DrawTexture(new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight), BaseContent.WhiteTex);
				GUI.color = Color.white;
			}
			base.WindowOnGUI();
		}

		// Token: 0x0600595A RID: 22874 RVA: 0x00182934 File Offset: 0x00180D34
		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = inRect.AtZero();
			if (this.title != null)
			{
				Text.Font = GameFont.Small;
				Rect rect2 = rect;
				rect2.height = 36f;
				rect.yMin += 53f;
				Widgets.DrawTitleBG(rect2);
				rect2.xMin += 9f;
				rect2.yMin += 5f;
				Widgets.Label(rect2, this.title);
			}
			this.DrawNode(rect);
		}

		// Token: 0x0600595B RID: 22875 RVA: 0x001829BC File Offset: 0x00180DBC
		protected void DrawNode(Rect rect)
		{
			GUI.BeginGroup(rect);
			Text.Font = GameFont.Small;
			Rect outRect = new Rect(0f, 0f, rect.width, rect.height - Mathf.Max(this.optTotalHeight, this.minOptionsAreaHeight));
			float width = rect.width - 16f;
			Rect rect2 = new Rect(0f, 0f, width, Text.CalcHeight(this.curNode.text, width));
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, rect2, true);
			Widgets.Label(rect2, this.curNode.text);
			Widgets.EndScrollView();
			float num = rect.height - this.optTotalHeight;
			float num2 = 0f;
			for (int i = 0; i < this.curNode.options.Count; i++)
			{
				Rect rect3 = new Rect(15f, num, rect.width - 30f, 999f);
				float num3 = this.curNode.options[i].OptOnGUI(rect3, this.InteractiveNow);
				num += num3 + 7f;
				num2 += num3 + 7f;
			}
			if (Event.current.type == EventType.Layout)
			{
				this.optTotalHeight = num2;
			}
			GUI.EndGroup();
		}

		// Token: 0x0600595C RID: 22876 RVA: 0x00182B14 File Offset: 0x00180F14
		public void GotoNode(DiaNode node)
		{
			foreach (DiaOption diaOption in node.options)
			{
				diaOption.dialog = this;
			}
			this.curNode = node;
		}

		// Token: 0x04003BAA RID: 15274
		private Vector2 scrollPosition;

		// Token: 0x04003BAB RID: 15275
		protected string title;

		// Token: 0x04003BAC RID: 15276
		protected DiaNode curNode;

		// Token: 0x04003BAD RID: 15277
		public Action closeAction;

		// Token: 0x04003BAE RID: 15278
		private float makeInteractiveAtTime;

		// Token: 0x04003BAF RID: 15279
		public Color screenFillColor = Color.clear;

		// Token: 0x04003BB0 RID: 15280
		protected float minOptionsAreaHeight;

		// Token: 0x04003BB1 RID: 15281
		private const float InteractivityDelay = 0.5f;

		// Token: 0x04003BB2 RID: 15282
		private const float TitleHeight = 36f;

		// Token: 0x04003BB3 RID: 15283
		protected const float OptHorMargin = 15f;

		// Token: 0x04003BB4 RID: 15284
		protected const float OptVerticalSpace = 7f;

		// Token: 0x04003BB5 RID: 15285
		private float optTotalHeight;
	}
}
