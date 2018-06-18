using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D08 RID: 3336
	public class HediffComp_Discoverable : HediffComp
	{
		// Token: 0x17000BA3 RID: 2979
		// (get) Token: 0x06004991 RID: 18833 RVA: 0x00268020 File Offset: 0x00266420
		public HediffCompProperties_Discoverable Props
		{
			get
			{
				return (HediffCompProperties_Discoverable)this.props;
			}
		}

		// Token: 0x06004992 RID: 18834 RVA: 0x00268040 File Offset: 0x00266440
		public override void CompExposeData()
		{
			Scribe_Values.Look<bool>(ref this.discovered, "discovered", false, false);
		}

		// Token: 0x06004993 RID: 18835 RVA: 0x00268058 File Offset: 0x00266458
		public override bool CompDisallowVisible()
		{
			return !this.discovered;
		}

		// Token: 0x06004994 RID: 18836 RVA: 0x00268076 File Offset: 0x00266476
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (Find.TickManager.TicksGame % 103 == 0)
			{
				this.CheckDiscovered();
			}
		}

		// Token: 0x06004995 RID: 18837 RVA: 0x00268091 File Offset: 0x00266491
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			this.CheckDiscovered();
		}

		// Token: 0x06004996 RID: 18838 RVA: 0x0026809C File Offset: 0x0026649C
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

		// Token: 0x06004997 RID: 18839 RVA: 0x002683FD File Offset: 0x002667FD
		public override void Notify_PawnDied()
		{
			this.CheckDiscovered();
		}

		// Token: 0x06004998 RID: 18840 RVA: 0x00268408 File Offset: 0x00266808
		public override string CompDebugString()
		{
			return "discovered: " + this.discovered;
		}

		// Token: 0x040031E5 RID: 12773
		private bool discovered = false;
	}
}
