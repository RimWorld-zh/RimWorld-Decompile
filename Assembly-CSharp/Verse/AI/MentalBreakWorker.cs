using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A58 RID: 2648
	public class MentalBreakWorker
	{
		// Token: 0x06003AF6 RID: 15094 RVA: 0x001F4C1C File Offset: 0x001F301C
		public virtual float CommonalityFor(Pawn pawn)
		{
			float num = this.def.baseCommonality;
			if (pawn.Faction == Faction.OfPlayer && this.def.commonalityFactorPerPopulationCurve != null)
			{
				num *= this.def.commonalityFactorPerPopulationCurve.Evaluate((float)PawnsFinder.AllMaps_FreeColonists.Count<Pawn>());
			}
			return num;
		}

		// Token: 0x06003AF7 RID: 15095 RVA: 0x001F4C7C File Offset: 0x001F307C
		public virtual bool BreakCanOccur(Pawn pawn)
		{
			bool result;
			if (this.def.requiredTrait != null && (pawn.story == null || !pawn.story.traits.HasTrait(this.def.requiredTrait)))
			{
				result = false;
			}
			else if (this.def.mentalState != null && pawn.story != null && pawn.story.traits.allTraits.Any((Trait tr) => tr.CurrentData.disallowedMentalStates != null && tr.CurrentData.disallowedMentalStates.Contains(this.def.mentalState)))
			{
				result = false;
			}
			else if (this.def.mentalState != null && !this.def.mentalState.Worker.StateCanOccur(pawn))
			{
				result = false;
			}
			else
			{
				if (pawn.story != null)
				{
					IEnumerable<MentalBreakDef> theOnlyAllowedMentalBreaks = pawn.story.traits.TheOnlyAllowedMentalBreaks;
					if (!theOnlyAllowedMentalBreaks.Contains(this.def) && theOnlyAllowedMentalBreaks.Any((MentalBreakDef x) => x.intensity == this.def.intensity && x.Worker.BreakCanOccur(pawn)))
					{
						return false;
					}
				}
				result = (!TutorSystem.TutorialMode || pawn.Faction != Faction.OfPlayer);
			}
			return result;
		}

		// Token: 0x06003AF8 RID: 15096 RVA: 0x001F4DFC File Offset: 0x001F31FC
		public virtual bool TryStart(Pawn pawn, Thought reason, bool causedByMood)
		{
			string text = (reason == null) ? null : reason.LabelCap;
			MentalStateHandler mentalStateHandler = pawn.mindState.mentalStateHandler;
			MentalStateDef mentalState = this.def.mentalState;
			string reason2 = text;
			return mentalStateHandler.TryStartMentalState(mentalState, reason2, false, causedByMood, null, false);
		}

		// Token: 0x06003AF9 RID: 15097 RVA: 0x001F4E4C File Offset: 0x001F324C
		protected bool TrySendLetter(Pawn pawn, string textKey, Thought reason)
		{
			bool result;
			if (!PawnUtility.ShouldSendNotificationAbout(pawn))
			{
				result = false;
			}
			else
			{
				string label = "MentalBreakLetterLabel".Translate() + ": " + this.def.LabelCap;
				string text = textKey.Translate(new object[]
				{
					pawn.Label
				}).CapitalizeFirst();
				if (reason != null)
				{
					text = text + "\n\n" + "FinalStraw".Translate(new object[]
					{
						reason.LabelCap
					});
				}
				text = text.AdjustedFor(pawn, "PAWN");
				Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NegativeEvent, pawn, null, null);
				result = true;
			}
			return result;
		}

		// Token: 0x04002545 RID: 9541
		public MentalBreakDef def;
	}
}
