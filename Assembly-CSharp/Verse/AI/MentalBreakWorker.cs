using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A5B RID: 2651
	public class MentalBreakWorker
	{
		// Token: 0x04002556 RID: 9558
		public MentalBreakDef def;

		// Token: 0x06003AFB RID: 15099 RVA: 0x001F5074 File Offset: 0x001F3474
		public virtual float CommonalityFor(Pawn pawn)
		{
			float num = this.def.baseCommonality;
			if (pawn.Faction == Faction.OfPlayer && this.def.commonalityFactorPerPopulationCurve != null)
			{
				num *= this.def.commonalityFactorPerPopulationCurve.Evaluate((float)PawnsFinder.AllMaps_FreeColonists.Count<Pawn>());
			}
			return num;
		}

		// Token: 0x06003AFC RID: 15100 RVA: 0x001F50D4 File Offset: 0x001F34D4
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

		// Token: 0x06003AFD RID: 15101 RVA: 0x001F5254 File Offset: 0x001F3654
		public virtual bool TryStart(Pawn pawn, Thought reason, bool causedByMood)
		{
			string text = (reason == null) ? null : reason.LabelCap;
			MentalStateHandler mentalStateHandler = pawn.mindState.mentalStateHandler;
			MentalStateDef mentalState = this.def.mentalState;
			string reason2 = text;
			return mentalStateHandler.TryStartMentalState(mentalState, reason2, false, causedByMood, null, false);
		}

		// Token: 0x06003AFE RID: 15102 RVA: 0x001F52A4 File Offset: 0x001F36A4
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
	}
}
