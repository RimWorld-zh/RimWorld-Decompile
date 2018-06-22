using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020008CD RID: 2253
	public class Lesson_Note : Lesson
	{
		// Token: 0x0600339F RID: 13215 RVA: 0x001B99FB File Offset: 0x001B7DFB
		public Lesson_Note()
		{
		}

		// Token: 0x060033A0 RID: 13216 RVA: 0x001B9A16 File Offset: 0x001B7E16
		public Lesson_Note(ConceptDef concept)
		{
			this.def = concept;
		}

		// Token: 0x1700083F RID: 2111
		// (get) Token: 0x060033A1 RID: 13217 RVA: 0x001B9A38 File Offset: 0x001B7E38
		public bool Expiring
		{
			get
			{
				return this.expiryTime < float.MaxValue;
			}
		}

		// Token: 0x17000840 RID: 2112
		// (get) Token: 0x060033A2 RID: 13218 RVA: 0x001B9A5C File Offset: 0x001B7E5C
		public Rect MainRect
		{
			get
			{
				float num = Text.CalcHeight(this.def.HelpTextAdjusted, 432f);
				float height = num + 20f;
				return new Rect(Messages.MessagesTopLeftStandard.x, 0f, 500f, height);
			}
		}

		// Token: 0x17000841 RID: 2113
		// (get) Token: 0x060033A3 RID: 13219 RVA: 0x001B9AAC File Offset: 0x001B7EAC
		public override float MessagesYOffset
		{
			get
			{
				return this.MainRect.height;
			}
		}

		// Token: 0x060033A4 RID: 13220 RVA: 0x001B9ACF File Offset: 0x001B7ECF
		public override void ExposeData()
		{
			Scribe_Defs.Look<ConceptDef>(ref this.def, "def");
		}

		// Token: 0x060033A5 RID: 13221 RVA: 0x001B9AE2 File Offset: 0x001B7EE2
		public override void OnActivated()
		{
			base.OnActivated();
			SoundDefOf.TutorMessageAppear.PlayOneShotOnCamera(null);
		}

		// Token: 0x060033A6 RID: 13222 RVA: 0x001B9AF8 File Offset: 0x001B7EF8
		public override void LessonOnGUI()
		{
			Rect mainRect = this.MainRect;
			float alpha = 1f;
			if (this.doFadeIn)
			{
				alpha = Mathf.Clamp01(base.AgeSeconds / 0.4f);
			}
			if (this.Expiring)
			{
				float num = this.expiryTime - Time.timeSinceLevelLoad;
				if (num < 1.1f)
				{
					alpha = num / 1.1f;
				}
			}
			WindowStack windowStack = Find.WindowStack;
			int id = 134706;
			Rect mainRect2 = mainRect;
			WindowLayer layer = WindowLayer.Super;
			Action doWindowFunc = delegate()
			{
				Rect rect = mainRect.AtZero();
				Text.Font = GameFont.Small;
				if (!this.Expiring)
				{
					this.def.HighlightAllTags();
				}
				if (this.doFadeIn || this.Expiring)
				{
					GUI.color = new Color(1f, 1f, 1f, alpha);
				}
				Widgets.DrawWindowBackgroundTutor(rect);
				Rect rect2 = rect.ContractedBy(10f);
				rect2.width = 432f;
				Widgets.Label(rect2, this.def.HelpTextAdjusted);
				Rect butRect = new Rect(rect.xMax - 32f - 8f, rect.y + 8f, 32f, 32f);
				Texture2D tex;
				if (this.Expiring)
				{
					tex = Widgets.CheckboxOnTex;
				}
				else
				{
					tex = TexButton.CloseXBig;
				}
				if (Widgets.ButtonImage(butRect, tex, new Color(0.95f, 0.95f, 0.95f), new Color(0.8352941f, 0.6666667f, 0.274509817f)))
				{
					SoundDefOf.Click.PlayOneShotOnCamera(null);
					this.CloseButtonClicked();
				}
				if (Time.timeSinceLevelLoad > this.expiryTime)
				{
					this.CloseButtonClicked();
				}
				GUI.color = Color.white;
			};
			bool doBackground = false;
			float alpha2 = alpha;
			windowStack.ImmediateWindow(id, mainRect2, layer, doWindowFunc, doBackground, false, alpha2);
		}

		// Token: 0x060033A7 RID: 13223 RVA: 0x001B9BC0 File Offset: 0x001B7FC0
		private void CloseButtonClicked()
		{
			KnowledgeAmount know = (!this.def.noteTeaches) ? KnowledgeAmount.NoteClosed : KnowledgeAmount.NoteTaught;
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(this.def, know);
			Find.ActiveLesson.Deactivate();
		}

		// Token: 0x060033A8 RID: 13224 RVA: 0x001B9BFC File Offset: 0x001B7FFC
		public override void Notify_KnowledgeDemonstrated(ConceptDef conc)
		{
			if (this.def == conc && PlayerKnowledgeDatabase.GetKnowledge(conc) > 0.2f)
			{
				if (!this.Expiring)
				{
					this.expiryTime = Time.timeSinceLevelLoad + 2.1f;
				}
			}
		}

		// Token: 0x04001BAB RID: 7083
		public ConceptDef def;

		// Token: 0x04001BAC RID: 7084
		public bool doFadeIn = true;

		// Token: 0x04001BAD RID: 7085
		private float expiryTime = float.MaxValue;

		// Token: 0x04001BAE RID: 7086
		private const float RectWidth = 500f;

		// Token: 0x04001BAF RID: 7087
		private const float TextWidth = 432f;

		// Token: 0x04001BB0 RID: 7088
		private const float FadeInDuration = 0.4f;

		// Token: 0x04001BB1 RID: 7089
		private const float DoneButPad = 8f;

		// Token: 0x04001BB2 RID: 7090
		private const float DoneButSize = 32f;

		// Token: 0x04001BB3 RID: 7091
		private const float ExpiryDuration = 2.1f;

		// Token: 0x04001BB4 RID: 7092
		private const float ExpiryFadeTime = 1.1f;
	}
}
