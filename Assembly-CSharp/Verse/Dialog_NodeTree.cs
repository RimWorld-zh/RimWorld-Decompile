using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EC9 RID: 3785
	public class Dialog_NodeTree : Window
	{
		// Token: 0x04003BC2 RID: 15298
		private Vector2 scrollPosition;

		// Token: 0x04003BC3 RID: 15299
		protected string title;

		// Token: 0x04003BC4 RID: 15300
		protected DiaNode curNode;

		// Token: 0x04003BC5 RID: 15301
		public Action closeAction;

		// Token: 0x04003BC6 RID: 15302
		private float makeInteractiveAtTime;

		// Token: 0x04003BC7 RID: 15303
		public Color screenFillColor = Color.clear;

		// Token: 0x04003BC8 RID: 15304
		protected float minOptionsAreaHeight;

		// Token: 0x04003BC9 RID: 15305
		private const float InteractivityDelay = 0.5f;

		// Token: 0x04003BCA RID: 15306
		private const float TitleHeight = 36f;

		// Token: 0x04003BCB RID: 15307
		protected const float OptHorMargin = 15f;

		// Token: 0x04003BCC RID: 15308
		protected const float OptVerticalSpace = 7f;

		// Token: 0x04003BCD RID: 15309
		private float optTotalHeight;

		// Token: 0x06005978 RID: 22904 RVA: 0x00182D54 File Offset: 0x00181154
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

		// Token: 0x17000E11 RID: 3601
		// (get) Token: 0x06005979 RID: 22905 RVA: 0x00182DDC File Offset: 0x001811DC
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(620f, 480f);
			}
		}

		// Token: 0x17000E12 RID: 3602
		// (get) Token: 0x0600597A RID: 22906 RVA: 0x00182E00 File Offset: 0x00181200
		private bool InteractiveNow
		{
			get
			{
				return Time.realtimeSinceStartup >= this.makeInteractiveAtTime;
			}
		}

		// Token: 0x0600597B RID: 22907 RVA: 0x00182E25 File Offset: 0x00181225
		public override void PreClose()
		{
			base.PreClose();
			this.curNode.PreClose();
		}

		// Token: 0x0600597C RID: 22908 RVA: 0x00182E39 File Offset: 0x00181239
		public override void PostClose()
		{
			base.PostClose();
			if (this.closeAction != null)
			{
				this.closeAction();
			}
		}

		// Token: 0x0600597D RID: 22909 RVA: 0x00182E58 File Offset: 0x00181258
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

		// Token: 0x0600597E RID: 22910 RVA: 0x00182EC0 File Offset: 0x001812C0
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

		// Token: 0x0600597F RID: 22911 RVA: 0x00182F48 File Offset: 0x00181348
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

		// Token: 0x06005980 RID: 22912 RVA: 0x001830A0 File Offset: 0x001814A0
		public void GotoNode(DiaNode node)
		{
			foreach (DiaOption diaOption in node.options)
			{
				diaOption.dialog = this;
			}
			this.curNode = node;
		}
	}
}
