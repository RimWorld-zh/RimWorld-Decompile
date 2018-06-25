using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public abstract class ITab_Pawn_Visitor : ITab
	{
		private const float CheckboxInterval = 30f;

		private const float CheckboxMargin = 50f;

		[CompilerGenerated]
		private static Func<PrisonerInteractionModeDef, int> <>f__am$cache0;

		public ITab_Pawn_Visitor()
		{
			this.size = new Vector2(280f, 450f);
		}

		protected override void FillTab()
		{
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.PrisonerTab, KnowledgeAmount.FrameDisplayed);
			Text.Font = GameFont.Small;
			Rect rect = new Rect(0f, 0f, this.size.x, this.size.y);
			Rect rect2 = rect.ContractedBy(10f);
			rect2.yMin += 24f;
			bool isPrisonerOfColony = base.SelPawn.IsPrisonerOfColony;
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.Begin(rect2);
			Rect rect3 = listing_Standard.GetRect(Text.LineHeight);
			rect3.width *= 0.75f;
			bool getsFood = base.SelPawn.guest.GetsFood;
			Widgets.CheckboxLabeled(rect3, "GetsFood".Translate(), ref getsFood, false, null, null, false);
			base.SelPawn.guest.GetsFood = getsFood;
			listing_Standard.Gap(12f);
			Rect rect4 = listing_Standard.GetRect(28f);
			rect4.width = 140f;
			MedicalCareUtility.MedicalCareSetter(rect4, ref base.SelPawn.playerSettings.medCare);
			listing_Standard.Gap(4f);
			if (isPrisonerOfColony)
			{
				listing_Standard.Label("RecruitmentDifficulty".Translate() + ": " + base.SelPawn.RecruitDifficulty(Faction.OfPlayer, false).ToStringPercent(), -1f, null);
				if (base.SelPawn.guilt.IsGuilty)
				{
					listing_Standard.Label("ConsideredGuilty".Translate(new object[]
					{
						base.SelPawn.guilt.TicksUntilInnocent.ToStringTicksToPeriod()
					}), -1f, null);
				}
				if (Prefs.DevMode)
				{
					listing_Standard.Label("Dev: Prison break MTB days: " + (int)PrisonBreakUtility.InitiatePrisonBreakMtbDays(base.SelPawn), -1f, null);
				}
				Rect rect5 = listing_Standard.GetRect(200f).Rounded();
				Widgets.DrawMenuSection(rect5);
				Rect position = rect5.ContractedBy(10f);
				GUI.BeginGroup(position);
				Rect rect6 = new Rect(0f, 0f, position.width, 30f);
				foreach (PrisonerInteractionModeDef prisonerInteractionModeDef in from pim in DefDatabase<PrisonerInteractionModeDef>.AllDefs
				orderby pim.listOrder
				select pim)
				{
					if (Widgets.RadioButtonLabeled(rect6, prisonerInteractionModeDef.LabelCap, base.SelPawn.guest.interactionMode == prisonerInteractionModeDef))
					{
						base.SelPawn.guest.interactionMode = prisonerInteractionModeDef;
						if (prisonerInteractionModeDef == PrisonerInteractionModeDefOf.Execution && base.SelPawn.MapHeld != null && !this.ColonyHasAnyWardenCapableOfViolence(base.SelPawn.MapHeld))
						{
							Messages.Message("MessageCantDoExecutionBecauseNoWardenCapableOfViolence".Translate(), base.SelPawn, MessageTypeDefOf.CautionInput, false);
						}
					}
					rect6.y += 28f;
				}
				GUI.EndGroup();
			}
			listing_Standard.End();
		}

		private bool ColonyHasAnyWardenCapableOfViolence(Map map)
		{
			foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
			{
				if (pawn.workSettings.WorkIsActive(WorkTypeDefOf.Warden) && (pawn.story == null || !pawn.story.WorkTagIsDisabled(WorkTags.Violent)))
				{
					return true;
				}
			}
			return false;
		}

		[CompilerGenerated]
		private static int <FillTab>m__0(PrisonerInteractionModeDef pim)
		{
			return pim.listOrder;
		}
	}
}
