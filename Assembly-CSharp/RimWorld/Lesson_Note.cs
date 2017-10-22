using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	public class Lesson_Note : Lesson
	{
		public ConceptDef def;

		public bool doFadeIn = true;

		private float expiryTime = 3.40282347E+38f;

		private const float RectWidth = 500f;

		private const float TextWidth = 432f;

		private const float FadeInDuration = 0.4f;

		private const float DoneButPad = 8f;

		private const float DoneButSize = 32f;

		private const float ExpiryDuration = 2.1f;

		private const float ExpiryFadeTime = 1.1f;

		public bool Expiring
		{
			get
			{
				return this.expiryTime < 3.4028234663852886E+38;
			}
		}

		public Rect MainRect
		{
			get
			{
				float num = Text.CalcHeight(this.def.HelpTextAdjusted, 432f);
				float height = (float)(num + 20.0);
				Vector2 messagesTopLeftStandard = Messages.MessagesTopLeftStandard;
				return new Rect(messagesTopLeftStandard.x, 0f, 500f, height);
			}
		}

		public override float MessagesYOffset
		{
			get
			{
				return this.MainRect.height;
			}
		}

		public Lesson_Note()
		{
		}

		public Lesson_Note(ConceptDef concept)
		{
			this.def = concept;
		}

		public override void ExposeData()
		{
			Scribe_Defs.Look<ConceptDef>(ref this.def, "def");
		}

		public override void OnActivated()
		{
			base.OnActivated();
			SoundDefOf.TutorMessageAppear.PlayOneShotOnCamera(null);
		}

		public override void LessonOnGUI()
		{
			Rect mainRect = this.MainRect;
			float alpha = 1f;
			if (this.doFadeIn)
			{
				alpha = Mathf.Clamp01((float)(base.AgeSeconds / 0.40000000596046448));
			}
			if (this.Expiring)
			{
				float num = this.expiryTime - Time.timeSinceLevelLoad;
				if (num < 1.1000000238418579)
				{
					alpha = (float)(num / 1.1000000238418579);
				}
			}
			WindowStack windowStack = Find.WindowStack;
			int iD = 134706;
			Rect rect = mainRect;
			WindowLayer layer = WindowLayer.Super;
			Action doWindowFunc = (Action)delegate
			{
				Rect rect2 = mainRect.AtZero();
				Text.Font = GameFont.Small;
				if (!this.Expiring)
				{
					this.def.HighlightAllTags();
				}
				if (this.doFadeIn || this.Expiring)
				{
					GUI.color = new Color(1f, 1f, 1f, alpha);
				}
				Widgets.DrawWindowBackgroundTutor(rect2);
				Rect rect3 = rect2.ContractedBy(10f);
				rect3.width = 432f;
				Widgets.Label(rect3, this.def.HelpTextAdjusted);
				Rect butRect = new Rect((float)(rect2.xMax - 32.0 - 8.0), (float)(rect2.y + 8.0), 32f, 32f);
				Texture2D tex = (!this.Expiring) ? TexButton.CloseXBig : Widgets.CheckboxOnTex;
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
			float shadowAlpha = alpha;
			windowStack.ImmediateWindow(iD, rect, layer, doWindowFunc, doBackground, false, shadowAlpha);
		}

		private void CloseButtonClicked()
		{
			KnowledgeAmount know = (KnowledgeAmount)((!this.def.noteTeaches) ? 7 : 8);
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(this.def, know);
			Find.ActiveLesson.Deactivate();
		}

		public override void Notify_KnowledgeDemonstrated(ConceptDef conc)
		{
			if (this.def == conc && PlayerKnowledgeDatabase.GetKnowledge(conc) > 0.20000000298023224 && !this.Expiring)
			{
				this.expiryTime = (float)(Time.timeSinceLevelLoad + 2.0999999046325684);
			}
		}
	}
}
