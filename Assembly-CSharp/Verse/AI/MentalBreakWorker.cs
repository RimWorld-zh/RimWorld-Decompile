using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse.AI
{
	public class MentalBreakWorker
	{
		public MentalBreakDef def;

		public MentalBreakWorker()
		{
		}

		public virtual float CommonalityFor(Pawn pawn)
		{
			float num = this.def.baseCommonality;
			if (pawn.Faction == Faction.OfPlayer && this.def.commonalityFactorPerPopulationCurve != null)
			{
				num *= this.def.commonalityFactorPerPopulationCurve.Evaluate((float)PawnsFinder.AllMaps_FreeColonists.Count<Pawn>());
			}
			return num;
		}

		public virtual bool BreakCanOccur(Pawn pawn)
		{
			if (this.def.requiredTrait != null && (pawn.story == null || !pawn.story.traits.HasTrait(this.def.requiredTrait)))
			{
				return false;
			}
			if (this.def.mentalState != null && pawn.story != null && pawn.story.traits.allTraits.Any((Trait tr) => tr.CurrentData.disallowedMentalStates != null && tr.CurrentData.disallowedMentalStates.Contains(this.def.mentalState)))
			{
				return false;
			}
			if (this.def.mentalState != null && !this.def.mentalState.Worker.StateCanOccur(pawn))
			{
				return false;
			}
			if (pawn.story != null)
			{
				IEnumerable<MentalBreakDef> theOnlyAllowedMentalBreaks = pawn.story.traits.TheOnlyAllowedMentalBreaks;
				if (!theOnlyAllowedMentalBreaks.Contains(this.def) && theOnlyAllowedMentalBreaks.Any((MentalBreakDef x) => x.intensity == this.def.intensity && x.Worker.BreakCanOccur(pawn)))
				{
					return false;
				}
			}
			return !TutorSystem.TutorialMode || pawn.Faction != Faction.OfPlayer;
		}

		public virtual bool TryStart(Pawn pawn, Thought reason, bool causedByMood)
		{
			string text = (reason == null) ? null : reason.LabelCap;
			MentalStateHandler mentalStateHandler = pawn.mindState.mentalStateHandler;
			MentalStateDef mentalState = this.def.mentalState;
			string reason2 = text;
			return mentalStateHandler.TryStartMentalState(mentalState, reason2, false, causedByMood, null, false);
		}

		protected bool TrySendLetter(Pawn pawn, string textKey, Thought reason)
		{
			if (!PawnUtility.ShouldSendNotificationAbout(pawn))
			{
				return false;
			}
			string label = this.def.LabelCap + ": " + pawn.LabelShortCap;
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
			return true;
		}

		[CompilerGenerated]
		private sealed class <BreakCanOccur>c__AnonStorey0
		{
			internal Pawn pawn;

			internal MentalBreakWorker $this;

			public <BreakCanOccur>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Trait tr)
			{
				return tr.CurrentData.disallowedMentalStates != null && tr.CurrentData.disallowedMentalStates.Contains(this.$this.def.mentalState);
			}

			internal bool <>m__1(MentalBreakDef x)
			{
				return x.intensity == this.$this.def.intensity && x.Worker.BreakCanOccur(this.pawn);
			}
		}
	}
}
