using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020008D1 RID: 2257
	public class Lesson_Note : Lesson
	{
		// Token: 0x060033A6 RID: 13222 RVA: 0x001B9813 File Offset: 0x001B7C13
		public Lesson_Note()
		{
		}

		// Token: 0x060033A7 RID: 13223 RVA: 0x001B982E File Offset: 0x001B7C2E
		public Lesson_Note(ConceptDef concept)
		{
			this.def = concept;
		}

		// Token: 0x1700083E RID: 2110
		// (get) Token: 0x060033A8 RID: 13224 RVA: 0x001B9850 File Offset: 0x001B7C50
		public bool Expiring
		{
			get
			{
				return this.expiryTime < float.MaxValue;
			}
		}

		// Token: 0x1700083F RID: 2111
		// (get) Token: 0x060033A9 RID: 13225 RVA: 0x001B9874 File Offset: 0x001B7C74
		public Rect MainRect
		{
			get
			{
				float num = Text.CalcHeight(this.def.HelpTextAdjusted, 432f);
				float height = num + 20f;
				return new Rect(Messages.MessagesTopLeftStandard.x, 0f, 500f, height);
			}
		}

		// Token: 0x17000840 RID: 2112
		// (get) Token: 0x060033AA RID: 13226 RVA: 0x001B98C4 File Offset: 0x001B7CC4
		public override float MessagesYOffset
		{
			get
			{
				return this.MainRect.height;
			}
		}

		// Token: 0x060033AB RID: 13227 RVA: 0x001B98E7 File Offset: 0x001B7CE7
		public override void ExposeData()
		{
			Scribe_Defs.Look<ConceptDef>(ref this.def, "def");
		}

		// Token: 0x060033AC RID: 13228 RVA: 0x001B98FA File Offset: 0x001B7CFA
		public override void OnActivated()
		{
			base.OnActivated();
			SoundDefOf.TutorMessageAppear.PlayOneShotOnCamera(null);
		}

		// Token: 0x060033AD RID: 13229 RVA: 0x001B9910 File Offset: 0x001B7D10
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

		// Token: 0x060033AE RID: 13230 RVA: 0x001B99D8 File Offset: 0x001B7DD8
		private void CloseButtonClicked()
		{
			KnowledgeAmount know = (!this.def.noteTeaches) ? KnowledgeAmount.NoteClosed : KnowledgeAmount.NoteTaught;
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(this.def, know);
			Find.ActiveLesson.Deactivate();
		}

		// Token: 0x060033AF RID: 13231 RVA: 0x001B9A14 File Offset: 0x001B7E14
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

		// Token: 0x04001BAD RID: 7085
		public ConceptDef def;

		// Token: 0x04001BAE RID: 7086
		public bool doFadeIn = true;

		// Token: 0x04001BAF RID: 7087
		private float expiryTime = float.MaxValue;

		// Token: 0x04001BB0 RID: 7088
		private const float RectWidth = 500f;

		// Token: 0x04001BB1 RID: 7089
		private const float TextWidth = 432f;

		// Token: 0x04001BB2 RID: 7090
		private const float FadeInDuration = 0.4f;

		// Token: 0x04001BB3 RID: 7091
		private const float DoneButPad = 8f;

		// Token: 0x04001BB4 RID: 7092
		private const float DoneButSize = 32f;

		// Token: 0x04001BB5 RID: 7093
		private const float ExpiryDuration = 2.1f;

		// Token: 0x04001BB6 RID: 7094
		private const float ExpiryFadeTime = 1.1f;
	}
}
