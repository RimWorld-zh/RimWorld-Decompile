using RimWorld;

namespace Verse
{
	public class HediffComp_Discoverable : HediffComp
	{
		private bool discovered;

		public HediffCompProperties_Discoverable Props
		{
			get
			{
				return (HediffCompProperties_Discoverable)base.props;
			}
		}

		public override void CompExposeData()
		{
			Scribe_Values.Look<bool>(ref this.discovered, "discovered", false, false);
		}

		public override bool CompDisallowVisible()
		{
			return !this.discovered;
		}

		public override void CompPostTick(ref float severityAdjustment)
		{
			if (Find.TickManager.TicksGame % 103 == 0)
			{
				this.CheckDiscovered();
			}
		}

		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			this.CheckDiscovered();
		}

		private void CheckDiscovered()
		{
			if (!this.discovered && base.parent.CurStage.becomeVisible)
			{
				this.discovered = true;
				if (this.Props.sendLetterWhenDiscovered && PawnUtility.ShouldSendNotificationAbout(base.Pawn))
				{
					string label = this.Props.discoverLetterLabel.NullOrEmpty() ? ("LetterLabelNewDisease".Translate() + " (" + base.Def.label + ")") : string.Format(this.Props.discoverLetterLabel, base.Pawn.LabelShort).CapitalizeFirst();
					string text = this.Props.discoverLetterText.NullOrEmpty() ? ((base.parent.Part != null) ? "NewPartDisease".Translate(base.Pawn.LabelIndefinite(), base.parent.Part.def.label, base.Pawn.LabelDefinite(), base.Def.LabelCap).AdjustedFor(base.Pawn).CapitalizeFirst() : "NewDisease".Translate(base.Pawn.LabelIndefinite(), base.Def.label, base.Pawn.LabelDefinite()).AdjustedFor(base.Pawn).CapitalizeFirst()) : string.Format(this.Props.discoverLetterText, base.Pawn.LabelIndefinite()).AdjustedFor(base.Pawn).CapitalizeFirst();
					if (base.Pawn.RaceProps.Humanlike)
					{
						Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NegativeEvent, base.Pawn, null);
					}
					else
					{
						Messages.Message(text, base.Pawn, MessageTypeDefOf.NeutralEvent);
					}
				}
			}
		}

		public override void Notify_PawnDied()
		{
			this.CheckDiscovered();
		}

		public override string CompDebugString()
		{
			return "discovered: " + this.discovered;
		}
	}
}
