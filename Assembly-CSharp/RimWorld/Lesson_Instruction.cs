using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020008CC RID: 2252
	public abstract class Lesson_Instruction : Lesson
	{
		// Token: 0x1700083A RID: 2106
		// (get) Token: 0x06003394 RID: 13204 RVA: 0x001B7174 File Offset: 0x001B5574
		protected Map Map
		{
			get
			{
				return Find.AnyPlayerHomeMap;
			}
		}

		// Token: 0x1700083B RID: 2107
		// (get) Token: 0x06003395 RID: 13205 RVA: 0x001B7190 File Offset: 0x001B5590
		protected virtual float ProgressPercent
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x1700083C RID: 2108
		// (get) Token: 0x06003396 RID: 13206 RVA: 0x001B71AC File Offset: 0x001B55AC
		protected virtual bool ShowProgressBar
		{
			get
			{
				return this.ProgressPercent >= 0f;
			}
		}

		// Token: 0x1700083D RID: 2109
		// (get) Token: 0x06003397 RID: 13207 RVA: 0x001B71D4 File Offset: 0x001B55D4
		public override string DefaultRejectInputMessage
		{
			get
			{
				return this.def.rejectInputMessage;
			}
		}

		// Token: 0x1700083E RID: 2110
		// (get) Token: 0x06003398 RID: 13208 RVA: 0x001B71F4 File Offset: 0x001B55F4
		public override InstructionDef Instruction
		{
			get
			{
				return this.def;
			}
		}

		// Token: 0x06003399 RID: 13209 RVA: 0x001B720F File Offset: 0x001B560F
		public override void ExposeData()
		{
			Scribe_Defs.Look<InstructionDef>(ref this.def, "def");
			base.ExposeData();
		}

		// Token: 0x0600339A RID: 13210 RVA: 0x001B7228 File Offset: 0x001B5628
		public override void OnActivated()
		{
			base.OnActivated();
			if (this.def.giveOnActivateCount > 0)
			{
				Thing thing = ThingMaker.MakeThing(this.def.giveOnActivateDef, null);
				thing.stackCount = this.def.giveOnActivateCount;
				GenSpawn.Spawn(thing, TutorUtility.FindUsableRect(2, 2, this.Map, 0f, false).CenterCell, this.Map, WipeMode.Vanish);
			}
			if (this.def.resetBuildDesignatorStuffs)
			{
				foreach (DesignationCategoryDef designationCategoryDef in DefDatabase<DesignationCategoryDef>.AllDefs)
				{
					foreach (Designator designator in designationCategoryDef.ResolvedAllowedDesignators)
					{
						Designator_Build designator_Build = designator as Designator_Build;
						if (designator_Build != null)
						{
							designator_Build.ResetStuffToDefault();
						}
					}
				}
			}
		}

		// Token: 0x0600339B RID: 13211 RVA: 0x001B7358 File Offset: 0x001B5758
		public override void LessonOnGUI()
		{
			Text.Font = GameFont.Small;
			string textAdj = this.def.text.AdjustedForKeys(null, true);
			float num = Text.CalcHeight(textAdj, 290f);
			float num2 = num + 20f;
			if (this.ShowProgressBar)
			{
				num2 += 47f;
			}
			Vector2 b = new Vector2((float)UI.screenWidth - 17f - 155f, 17f + num2 / 2f);
			if (!Find.TutorialState.introDone)
			{
				float screenOverlayAlpha = 0f;
				if (this.def.startCentered)
				{
					Vector2 vector = new Vector2((float)(UI.screenWidth / 2), (float)(UI.screenHeight / 2));
					if (base.AgeSeconds < 4f)
					{
						b = vector;
						screenOverlayAlpha = 0.9f;
					}
					else if (base.AgeSeconds < 5f)
					{
						float t = (base.AgeSeconds - 4f) / 1f;
						b = Vector2.Lerp(vector, b, t);
						screenOverlayAlpha = Mathf.Lerp(0.9f, 0f, t);
					}
				}
				if (screenOverlayAlpha > 0f)
				{
					Rect fullScreenRect = new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight);
					Find.WindowStack.ImmediateWindow(972651, fullScreenRect, WindowLayer.SubSuper, delegate
					{
						GUI.color = new Color(1f, 1f, 1f, screenOverlayAlpha);
						GUI.DrawTexture(fullScreenRect, BaseContent.BlackTex);
						GUI.color = Color.white;
					}, false, true, 0f);
				}
				else
				{
					Find.TutorialState.introDone = true;
				}
			}
			Rect mainRect = new Rect(b.x - 155f, b.y - num2 / 2f - 10f, 310f, num2);
			Find.WindowStack.ImmediateWindow(177706, mainRect, WindowLayer.Super, delegate
			{
				Rect rect = mainRect.AtZero();
				Widgets.DrawWindowBackgroundTutor(rect);
				Rect rect2 = rect.ContractedBy(10f);
				Text.Font = GameFont.Small;
				Rect rect3 = rect2;
				if (this.ShowProgressBar)
				{
					rect3.height -= 47f;
				}
				Widgets.Label(rect3, textAdj);
				if (this.ShowProgressBar)
				{
					Rect rect4 = new Rect(rect2.x, rect2.yMax - 30f, rect2.width, 30f);
					Widgets.FillableBar(rect4, this.ProgressPercent, LearningReadout.ProgressBarFillTex);
				}
				if (this.AgeSeconds < 0.5f)
				{
					GUI.color = new Color(1f, 1f, 1f, Mathf.Lerp(1f, 0f, this.AgeSeconds / 0.5f));
					GUI.DrawTexture(rect, BaseContent.WhiteTex);
					GUI.color = Color.white;
				}
			}, false, false, 1f);
			if (this.def.highlightTags != null)
			{
				for (int i = 0; i < this.def.highlightTags.Count; i++)
				{
					UIHighlighter.HighlightTag(this.def.highlightTags[i]);
				}
			}
		}

		// Token: 0x0600339C RID: 13212 RVA: 0x001B75CB File Offset: 0x001B59CB
		public override void Notify_Event(EventPack ep)
		{
			if (this.def.eventTagsEnd != null && this.def.eventTagsEnd.Contains(ep.Tag))
			{
				Find.ActiveLesson.Deactivate();
			}
		}

		// Token: 0x0600339D RID: 13213 RVA: 0x001B7604 File Offset: 0x001B5A04
		public override AcceptanceReport AllowAction(EventPack ep)
		{
			return this.def.actionTagsAllowed != null && this.def.actionTagsAllowed.Contains(ep.Tag);
		}

		// Token: 0x0600339E RID: 13214 RVA: 0x001B7648 File Offset: 0x001B5A48
		public override void PostDeactivated()
		{
			SoundDefOf.CommsWindow_Close.PlayOneShotOnCamera(null);
			TutorSystem.Notify_Event("InstructionDeactivated-" + this.def.defName);
			if (this.def.endTutorial)
			{
				Find.ActiveLesson.Deactivate();
				Find.TutorialState.Notify_TutorialEnding();
				LessonAutoActivator.Notify_TutorialEnding();
			}
		}

		// Token: 0x04001BA8 RID: 7080
		public InstructionDef def;

		// Token: 0x04001BA9 RID: 7081
		private const float RectWidth = 310f;

		// Token: 0x04001BAA RID: 7082
		private const float BarHeight = 30f;
	}
}
