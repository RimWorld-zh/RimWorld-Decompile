using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D08 RID: 3336
	public class HediffComp_Discoverable : HediffComp
	{
		// Token: 0x040031F7 RID: 12791
		private bool discovered = false;

		// Token: 0x17000BA4 RID: 2980
		// (get) Token: 0x060049A5 RID: 18853 RVA: 0x002697F4 File Offset: 0x00267BF4
		public HediffCompProperties_Discoverable Props
		{
			get
			{
				return (HediffCompProperties_Discoverable)this.props;
			}
		}

		// Token: 0x060049A6 RID: 18854 RVA: 0x00269814 File Offset: 0x00267C14
		public override void CompExposeData()
		{
			Scribe_Values.Look<bool>(ref this.discovered, "discovered", false, false);
		}

		// Token: 0x060049A7 RID: 18855 RVA: 0x0026982C File Offset: 0x00267C2C
		public override bool CompDisallowVisible()
		{
			return !this.discovered;
		}

		// Token: 0x060049A8 RID: 18856 RVA: 0x0026984A File Offset: 0x00267C4A
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (Find.TickManager.TicksGame % 103 == 0)
			{
				this.CheckDiscovered();
			}
		}

		// Token: 0x060049A9 RID: 18857 RVA: 0x00269865 File Offset: 0x00267C65
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			this.CheckDiscovered();
		}

		// Token: 0x060049AA RID: 18858 RVA: 0x00269870 File Offset: 0x00267C70
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
								text = string.Format(this.Props.discoverLetterText, base.Pawn.LabelIndefinite()).AdjustedFor(base.Pawn, "PAWN").CapitalizeFirst();
							}
							else if (this.parent.Part == null)
							{
								text = "NewDisease".Translate(new object[]
								{
									base.Pawn.LabelIndefinite(),
									base.Def.label,
									base.Pawn.LabelDefinite()
								}).AdjustedFor(base.Pawn, "PAWN").CapitalizeFirst();
							}
							else
							{
								text = "NewPartDisease".Translate(new object[]
								{
									base.Pawn.LabelIndefinite(),
									this.parent.Part.Label,
									base.Pawn.LabelDefinite(),
									base.Def.LabelCap
								}).AdjustedFor(base.Pawn, "PAWN").CapitalizeFirst();
							}
							Find.LetterStack.ReceiveLetter(label, text, (this.Props.letterType == null) ? LetterDefOf.NegativeEvent : this.Props.letterType, base.Pawn, null, null);
						}
						else
						{
							string text2;
							if (!this.Props.discoverLetterText.NullOrEmpty())
							{
								text2 = string.Format(this.Props.discoverLetterText, base.Pawn.LabelIndefinite()).AdjustedFor(base.Pawn, "PAWN").CapitalizeFirst();
							}
							else if (this.parent.Part == null)
							{
								text2 = "NewDiseaseAnimal".Translate(new object[]
								{
									base.Pawn.LabelShort,
									base.Def.LabelCap,
									base.Pawn.LabelDefinite()
								}).AdjustedFor(base.Pawn, "PAWN").CapitalizeFirst();
							}
							else
							{
								text2 = "NewPartDiseaseAnimal".Translate(new object[]
								{
									base.Pawn.LabelShort,
									this.parent.Part.Label,
									base.Pawn.LabelDefinite(),
									base.Def.LabelCap
								}).AdjustedFor(base.Pawn, "PAWN").CapitalizeFirst();
							}
							Messages.Message(text2, base.Pawn, (this.Props.messageType == null) ? MessageTypeDefOf.NegativeHealthEvent : this.Props.messageType, true);
						}
					}
				}
			}
		}

		// Token: 0x060049AB RID: 18859 RVA: 0x00269BEF File Offset: 0x00267FEF
		public override void Notify_PawnDied()
		{
			this.CheckDiscovered();
		}

		// Token: 0x060049AC RID: 18860 RVA: 0x00269BF8 File Offset: 0x00267FF8
		public override string CompDebugString()
		{
			return "discovered: " + this.discovered;
		}
	}
}
