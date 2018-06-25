using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000830 RID: 2096
	public class Page_ConfigureStartingPawns : Page
	{
		// Token: 0x04001980 RID: 6528
		private Pawn curPawn;

		// Token: 0x04001981 RID: 6529
		private const float TabAreaWidth = 140f;

		// Token: 0x04001982 RID: 6530
		private const float RightRectLeftPadding = 5f;

		// Token: 0x04001983 RID: 6531
		private const float PawnEntryHeight = 60f;

		// Token: 0x04001984 RID: 6532
		private const float SkillSummaryHeight = 141f;

		// Token: 0x04001985 RID: 6533
		private const int SkillSummaryColumns = 4;

		// Token: 0x04001986 RID: 6534
		private const int TeamSkillExtraInset = 10;

		// Token: 0x04001987 RID: 6535
		private static readonly Vector2 PawnPortraitSize = new Vector2(100f, 140f);

		// Token: 0x04001988 RID: 6536
		private static readonly Vector2 PawnSelectorPortraitSize = new Vector2(70f, 110f);

		// Token: 0x04001989 RID: 6537
		private int SkillsPerColumn = -1;

		// Token: 0x17000782 RID: 1922
		// (get) Token: 0x06002F3F RID: 12095 RVA: 0x00193FF8 File Offset: 0x001923F8
		public override string PageTitle
		{
			get
			{
				return "CreateCharacters".Translate();
			}
		}

		// Token: 0x06002F40 RID: 12096 RVA: 0x00194017 File Offset: 0x00192417
		public override void PreOpen()
		{
			base.PreOpen();
			if (Find.GameInitData.startingAndOptionalPawns.Count > 0)
			{
				this.curPawn = Find.GameInitData.startingAndOptionalPawns[0];
			}
		}

		// Token: 0x06002F41 RID: 12097 RVA: 0x0019404B File Offset: 0x0019244B
		public override void PostOpen()
		{
			base.PostOpen();
			TutorSystem.Notify_Event("PageStart-ConfigureStartingPawns");
		}

		// Token: 0x06002F42 RID: 12098 RVA: 0x00194064 File Offset: 0x00192464
		public override void DoWindowContents(Rect rect)
		{
			base.DrawPageTitle(rect);
			rect.yMin += 45f;
			base.DoBottomButtons(rect, "Start".Translate(), null, null, true);
			rect.yMax -= 38f;
			Rect rect2 = rect;
			rect2.width = 140f;
			this.DrawPawnList(rect2);
			UIHighlighter.HighlightOpportunity(rect2, "ReorderPawn");
			Rect rect3 = rect;
			rect3.xMin += 140f;
			Rect rect4 = rect3.BottomPartPixels(141f);
			rect3.yMax = rect4.yMin;
			rect3 = rect3.ContractedBy(4f);
			rect4 = rect4.ContractedBy(4f);
			this.DrawPortraitArea(rect3);
			this.DrawSkillSummaries(rect4);
		}

		// Token: 0x06002F43 RID: 12099 RVA: 0x00194128 File Offset: 0x00192528
		private void DrawPawnList(Rect rect)
		{
			Rect rect2 = rect;
			rect2.height = 60f;
			rect2 = rect2.ContractedBy(4f);
			int groupID = ReorderableWidget.NewGroup(delegate(int from, int to)
			{
				if (TutorSystem.AllowAction("ReorderPawn"))
				{
					Pawn item = Find.GameInitData.startingAndOptionalPawns[from];
					Find.GameInitData.startingAndOptionalPawns.Insert(to, item);
					Find.GameInitData.startingAndOptionalPawns.RemoveAt((from >= to) ? (from + 1) : from);
					TutorSystem.Notify_Event("ReorderPawn");
				}
			}, ReorderableDirection.Vertical, -1f, null);
			rect2.y += 15f;
			this.DrawPawnListLabelAbove(rect2, "StartingPawnsSelected".Translate());
			for (int i = 0; i < Find.GameInitData.startingAndOptionalPawns.Count; i++)
			{
				if (i == Find.GameInitData.startingPawnCount)
				{
					rect2.y += 30f;
					this.DrawPawnListLabelAbove(rect2, "StartingPawnsLeftBehind".Translate());
				}
				Pawn pawn = Find.GameInitData.startingAndOptionalPawns[i];
				GUI.BeginGroup(rect2);
				Rect rect3 = new Rect(Vector2.zero, rect2.size);
				Widgets.DrawOptionBackground(rect3, this.curPawn == pawn);
				MouseoverSounds.DoRegion(rect3);
				GUI.color = new Color(1f, 1f, 1f, 0.2f);
				GUI.DrawTexture(new Rect(110f - Page_ConfigureStartingPawns.PawnSelectorPortraitSize.x / 2f, 40f - Page_ConfigureStartingPawns.PawnSelectorPortraitSize.y / 2f, Page_ConfigureStartingPawns.PawnSelectorPortraitSize.x, Page_ConfigureStartingPawns.PawnSelectorPortraitSize.y), PortraitsCache.Get(pawn, Page_ConfigureStartingPawns.PawnSelectorPortraitSize, default(Vector3), 1f));
				GUI.color = Color.white;
				Rect rect4 = rect3.ContractedBy(4f).Rounded();
				NameTriple nameTriple = pawn.Name as NameTriple;
				string label;
				if (nameTriple != null)
				{
					label = ((!string.IsNullOrEmpty(nameTriple.Nick)) ? nameTriple.Nick : nameTriple.First);
				}
				else
				{
					label = pawn.LabelShort;
				}
				Widgets.Label(rect4.TopPart(0.5f).Rounded(), label);
				if (Text.CalcSize(pawn.story.TitleCap).x > rect4.width)
				{
					Widgets.Label(rect4.BottomPart(0.5f).Rounded(), pawn.story.TitleShortCap);
				}
				else
				{
					Widgets.Label(rect4.BottomPart(0.5f).Rounded(), pawn.story.TitleCap);
				}
				if (Event.current.type == EventType.MouseDown && Mouse.IsOver(rect3))
				{
					this.curPawn = pawn;
					SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
				}
				GUI.EndGroup();
				if (ReorderableWidget.Reorderable(groupID, rect2.ExpandedBy(4f), false))
				{
					Widgets.DrawRectFast(rect2, Widgets.WindowBGFillColor * new Color(1f, 1f, 1f, 0.5f), null);
				}
				TooltipHandler.TipRegion(rect2, new TipSignal("DragToReorder".Translate(), pawn.GetHashCode() * 3499));
				rect2.y += 60f;
			}
		}

		// Token: 0x06002F44 RID: 12100 RVA: 0x0019445C File Offset: 0x0019285C
		private void DrawPawnListLabelAbove(Rect rect, string label)
		{
			rect.yMax = rect.yMin;
			rect.yMin -= 30f;
			rect.xMin -= 4f;
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.LowerLeft;
			Widgets.Label(rect, label);
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Small;
		}

		// Token: 0x06002F45 RID: 12101 RVA: 0x001944C0 File Offset: 0x001928C0
		private void DrawPortraitArea(Rect rect)
		{
			Widgets.DrawMenuSection(rect);
			rect = rect.ContractedBy(17f);
			GUI.DrawTexture(new Rect(rect.center.x - Page_ConfigureStartingPawns.PawnPortraitSize.x / 2f, rect.yMin - 20f, Page_ConfigureStartingPawns.PawnPortraitSize.x, Page_ConfigureStartingPawns.PawnPortraitSize.y), PortraitsCache.Get(this.curPawn, Page_ConfigureStartingPawns.PawnPortraitSize, default(Vector3), 1f));
			Rect rect2 = rect;
			rect2.width = 500f;
			CharacterCardUtility.DrawCharacterCard(rect2, this.curPawn, new Action(this.RandomizeCurPawn), rect);
			Rect rect3 = rect;
			rect3.yMin += 100f;
			rect3.xMin = rect2.xMax + 5f;
			rect3.height = 200f;
			Text.Font = GameFont.Medium;
			Widgets.Label(rect3, "Health".Translate());
			Text.Font = GameFont.Small;
			rect3.yMin += 35f;
			HealthCardUtility.DrawHediffListing(rect3, this.curPawn, true);
			Rect rect4 = new Rect(rect3.x, rect3.yMax, rect3.width, 200f);
			Text.Font = GameFont.Medium;
			Widgets.Label(rect4, "Relations".Translate());
			Text.Font = GameFont.Small;
			rect4.yMin += 35f;
			SocialCardUtility.DrawRelationsAndOpinions(rect4, this.curPawn);
		}

		// Token: 0x06002F46 RID: 12102 RVA: 0x00194650 File Offset: 0x00192A50
		private void DrawSkillSummaries(Rect rect)
		{
			rect.xMin += 10f;
			rect.xMax -= 10f;
			Widgets.DrawMenuSection(rect);
			rect = rect.ContractedBy(17f);
			Text.Font = GameFont.Medium;
			Widgets.Label(new Rect(rect.min, new Vector2(rect.width, 45f)), "TeamSkills".Translate());
			Text.Font = GameFont.Small;
			rect.yMin += 45f;
			rect = rect.LeftPart(0.25f);
			rect.height = 27f;
			List<SkillDef> allDefsListForReading = DefDatabase<SkillDef>.AllDefsListForReading;
			if (this.SkillsPerColumn < 0)
			{
				this.SkillsPerColumn = Mathf.CeilToInt((float)(from sd in allDefsListForReading
				where sd.pawnCreatorSummaryVisible
				select sd).Count<SkillDef>() / 4f);
			}
			int num = 0;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				SkillDef skillDef = allDefsListForReading[i];
				if (skillDef.pawnCreatorSummaryVisible)
				{
					Rect r = rect;
					r.x = rect.x + rect.width * (float)(num / this.SkillsPerColumn);
					r.y = rect.y + rect.height * (float)(num % this.SkillsPerColumn);
					r.height = 24f;
					r.width -= 4f;
					Pawn pawn = this.FindBestSkillOwner(skillDef);
					SkillUI.DrawSkill(pawn.skills.GetSkill(skillDef), r.Rounded(), SkillUI.SkillDrawMode.Menu, pawn.Name.ToString());
					num++;
				}
			}
		}

		// Token: 0x06002F47 RID: 12103 RVA: 0x00194810 File Offset: 0x00192C10
		private Pawn FindBestSkillOwner(SkillDef skill)
		{
			Pawn pawn = Find.GameInitData.startingAndOptionalPawns[0];
			SkillRecord skillRecord = pawn.skills.GetSkill(skill);
			for (int i = 1; i < Find.GameInitData.startingPawnCount; i++)
			{
				SkillRecord skill2 = Find.GameInitData.startingAndOptionalPawns[i].skills.GetSkill(skill);
				if (skillRecord.TotallyDisabled || skill2.Level > skillRecord.Level || (skill2.Level == skillRecord.Level && skill2.passion > skillRecord.passion))
				{
					pawn = Find.GameInitData.startingAndOptionalPawns[i];
					skillRecord = skill2;
				}
			}
			return pawn;
		}

		// Token: 0x06002F48 RID: 12104 RVA: 0x001948D4 File Offset: 0x00192CD4
		private void RandomizeCurPawn()
		{
			if (TutorSystem.AllowAction("RandomizePawn"))
			{
				int num = 0;
				do
				{
					this.curPawn = StartingPawnUtility.RandomizeInPlace(this.curPawn);
					num++;
					if (num > 20)
					{
						break;
					}
				}
				while (!StartingPawnUtility.WorkTypeRequirementsSatisfied());
				TutorSystem.Notify_Event("RandomizePawn");
			}
		}

		// Token: 0x06002F49 RID: 12105 RVA: 0x0019493C File Offset: 0x00192D3C
		protected override bool CanDoNext()
		{
			bool result;
			if (!base.CanDoNext())
			{
				result = false;
			}
			else
			{
				if (TutorSystem.TutorialMode)
				{
					WorkTypeDef workTypeDef = StartingPawnUtility.RequiredWorkTypesDisabledForEveryone().FirstOrDefault<WorkTypeDef>();
					if (workTypeDef != null)
					{
						Messages.Message("RequiredWorkTypeDisabledForEveryone".Translate() + ": " + workTypeDef.gerundLabel.CapitalizeFirst() + ".", MessageTypeDefOf.RejectInput, false);
						return false;
					}
				}
				foreach (Pawn pawn in Find.GameInitData.startingAndOptionalPawns)
				{
					if (!pawn.Name.IsValid)
					{
						Messages.Message("EveryoneNeedsValidName".Translate(), MessageTypeDefOf.RejectInput, false);
						return false;
					}
				}
				PortraitsCache.Clear();
				result = true;
			}
			return result;
		}

		// Token: 0x06002F4A RID: 12106 RVA: 0x00194A3C File Offset: 0x00192E3C
		protected override void DoNext()
		{
			this.CheckWarnRequiredWorkTypesDisabledForEveryone(delegate
			{
				foreach (Pawn pawn in Find.GameInitData.startingAndOptionalPawns)
				{
					NameTriple nameTriple = pawn.Name as NameTriple;
					if (nameTriple != null && string.IsNullOrEmpty(nameTriple.Nick))
					{
						pawn.Name = new NameTriple(nameTriple.First, nameTriple.First, nameTriple.Last);
					}
				}
				this.<DoNext>__BaseCallProxy0();
			});
		}

		// Token: 0x06002F4B RID: 12107 RVA: 0x00194A54 File Offset: 0x00192E54
		private void CheckWarnRequiredWorkTypesDisabledForEveryone(Action nextAction)
		{
			IEnumerable<WorkTypeDef> enumerable = StartingPawnUtility.RequiredWorkTypesDisabledForEveryone();
			if (enumerable.Any<WorkTypeDef>())
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (WorkTypeDef workTypeDef in enumerable)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.Append("  - " + workTypeDef.gerundLabel.CapitalizeFirst());
				}
				string text = "ConfirmRequiredWorkTypeDisabledForEveryone".Translate(new object[]
				{
					stringBuilder.ToString()
				});
				Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(text, nextAction, false, null));
			}
			else
			{
				nextAction();
			}
		}

		// Token: 0x06002F4C RID: 12108 RVA: 0x00194B28 File Offset: 0x00192F28
		public void SelectPawn(Pawn c)
		{
			if (c != this.curPawn)
			{
				this.curPawn = c;
			}
		}
	}
}
