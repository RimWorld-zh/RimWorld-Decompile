using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D09 RID: 3337
	public class HediffComp_Discoverable : HediffComp
	{
		// Token: 0x17000BA4 RID: 2980
		// (get) Token: 0x06004993 RID: 18835 RVA: 0x00268048 File Offset: 0x00266448
		public HediffCompProperties_Discoverable Props
		{
			get
			{
				return (HediffCompProperties_Discoverable)this.props;
			}
		}

		// Token: 0x06004994 RID: 18836 RVA: 0x00268068 File Offset: 0x00266468
		public override void CompExposeData()
		{
			Scribe_Values.Look<bool>(ref this.discovered, "discovered", false, false);
		}

		// Token: 0x06004995 RID: 18837 RVA: 0x00268080 File Offset: 0x00266480
		public override bool CompDisallowVisible()
		{
			return !this.discovered;
		}

		// Token: 0x06004996 RID: 18838 RVA: 0x0026809E File Offset: 0x0026649E
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (Find.TickManager.TicksGame % 103 == 0)
			{
				this.CheckDiscovered();
			}
		}

		// Token: 0x06004997 RID: 18839 RVA: 0x002680B9 File Offset: 0x002664B9
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			this.CheckDiscovered();
		}

		// Token: 0x06004998 RID: 18840 RVA: 0x002680C4 File Offset: 0x002664C4
		private void CheckDiscovered()
		{
			if (!this.discovered)
			{
				if (this.parent.CurStage.becomeVisible)
				{
					this.discovered = true;
					if (this.Props.sendLetterWhenDiscovered && PawnUtility.ShouldSendNotificationAbout(base.Pawn))
					{
						if (base.Pawn.RaceProps.Humanlike)
						{
							string label;
							if (!this.Props.discoverLetterLabel.NullOrEmpty())
							{
								label = string.Format(this.Props.discoverLetterLabel, base.Pawn.LabelShort.CapitalizeFirst()).CapitalizeFirst();
							}
							else
							{
								label = "LetterLabelNewDisease".Translate() + " (" + base.Def.label + ")";
							}
							string text;
							if (!this.Props.discoverLetterText.NullOrEmpty())
							{
								text = string.Format(this.Props.discoverLetterText, base.Pawn.LabelIndefinite()).AdjustedFor(base.Pawn).CapitalizeFirst();
							}
							else if (this.parent.Part == null)
							{
								text = "NewDisease".Translate(new object[]
								{
									base.Pawn.LabelIndefinite(),
									base.Def.label,
									base.Pawn.LabelDefinite()
								}).AdjustedFor(base.Pawn).CapitalizeFirst();
							}
							else
							{
								text = "NewPartDisease".Translate(new object[]
								{
									base.Pawn.LabelIndefinite(),
									this.parent.Part.Label,
									base.Pawn.LabelDefinite(),
									base.Def.LabelCap
								}).AdjustedFor(base.Pawn).CapitalizeFirst();
							}
							Find.LetterStack.ReceiveLetter(label, text, (this.Props.letterType == null) ? LetterDefOf.NegativeEvent : this.Props.letterType, base.Pawn, null, null);
						}
						else
						{
							string text2;
							if (!this.Props.discoverLetterText.NullOrEmpty())
							{
								text2 = string.Format(this.Props.discoverLetterText, base.Pawn.LabelIndefinite()).AdjustedFor(base.Pawn).CapitalizeFirst();
							}
							else if (this.parent.Part == null)
							{
								text2 = "NewDiseaseAnimal".Translate(new object[]
								{
									base.Pawn.LabelShort,
									base.Def.LabelCap,
									base.Pawn.LabelDefinite()
								}).AdjustedFor(base.Pawn).CapitalizeFirst();
							}
							else
							{
								text2 = "NewPartDiseaseAnimal".Translate(new object[]
								{
									base.Pawn.LabelShort,
									this.parent.Part.Label,
									base.Pawn.LabelDefinite(),
									base.Def.LabelCap
								}).AdjustedFor(base.Pawn).CapitalizeFirst();
							}
							Messages.Message(text2, base.Pawn, (this.Props.messageType == null) ? MessageTypeDefOf.NegativeHealthEvent : this.Props.messageType, true);
						}
					}
				}
			}
		}

		// Token: 0x06004999 RID: 18841 RVA: 0x00268425 File Offset: 0x00266825
		public override void Notify_PawnDied()
		{
			this.CheckDiscovered();
		}

		// Token: 0x0600499A RID: 18842 RVA: 0x00268430 File Offset: 0x00266830
		public override string CompDebugString()
		{
			return "discovered: " + this.discovered;
		}

		// Token: 0x040031E7 RID: 12775
		private bool discovered = false;
	}
}
