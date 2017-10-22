using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse.AI
{
	public class MentalBreakWorker
	{
		public MentalBreakDef def;

		public virtual float CommonalityFor(Pawn pawn)
		{
			return this.def.baseCommonality;
		}

		public virtual bool BreakCanOccur(Pawn pawn)
		{
			bool result;
			if (this.def.requiredTrait != null && (pawn.story == null || !pawn.story.traits.HasTrait(this.def.requiredTrait)))
			{
				result = false;
			}
			else if (this.def.mentalState != null && pawn.story != null && pawn.story.traits.allTraits.Any((Predicate<Trait>)((Trait tr) => tr.CurrentData.disallowedMentalStates != null && tr.CurrentData.disallowedMentalStates.Contains(this.def.mentalState))))
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
					if (!theOnlyAllowedMentalBreaks.Contains(this.def) && theOnlyAllowedMentalBreaks.Any((Func<MentalBreakDef, bool>)((MentalBreakDef x) => x.intensity == this.def.intensity && x.Worker.BreakCanOccur(pawn))))
					{
						result = false;
						goto IL_0171;
					}
				}
				result = ((byte)((!TutorSystem.TutorialMode || pawn.Faction != Faction.OfPlayer) ? 1 : 0) != 0);
			}
			goto IL_0171;
			IL_0171:
			return result;
		}

		public virtual bool TryStart(Pawn pawn, Thought reason, bool causedByMood)
		{
			string text = (reason == null) ? null : reason.LabelCap;
			MentalStateHandler mentalStateHandler = pawn.mindState.mentalStateHandler;
			MentalStateDef mentalState = this.def.mentalState;
			string reason2 = text;
			return mentalStateHandler.TryStartMentalState(mentalState, reason2, false, causedByMood, null);
		}

		protected bool TrySendLetter(Pawn pawn, string textKey, Thought reason)
		{
			bool result;
			if (!PawnUtility.ShouldSendNotificationAbout(pawn))
			{
				result = false;
			}
			else
			{
				string label = "MentalBreakLetterLabel".Translate() + ": " + this.def.label;
				string text = textKey.Translate(pawn.Label).CapitalizeFirst();
				if (reason != null)
				{
					text = text + "\n\n" + "FinalStraw".Translate(reason.LabelCap);
				}
				Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NegativeEvent, (Thing)pawn, (string)null);
				result = true;
			}
			return result;
		}
	}
}
