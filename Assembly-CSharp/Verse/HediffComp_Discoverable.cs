using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D07 RID: 3335
	public class HediffComp_Discoverable : HediffComp
	{
		// Token: 0x040031F0 RID: 12784
		private bool discovered = false;

		// Token: 0x17000BA4 RID: 2980
		// (get) Token: 0x060049A5 RID: 18853 RVA: 0x00269514 File Offset: 0x00267914
		public HediffCompProperties_Discoverable Props
		{
			get
			{
				return (HediffCompProperties_Discoverable)this.props;
			}
		}

		// Token: 0x060049A6 RID: 18854 RVA: 0x00269534 File Offset: 0x00267934
		public override void CompExposeData()
		{
			Scribe_Values.Look<bool>(ref this.discovered, "discovered", false, false);
		}

		// Token: 0x060049A7 RID: 18855 RVA: 0x0026954C File Offset: 0x0026794C
		public override bool CompDisallowVisible()
		{
			return !this.discovered;
		}

		// Token: 0x060049A8 RID: 18856 RVA: 0x0026956A File Offset: 0x0026796A
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (Find.TickManager.TicksGame % 103 == 0)
			{
				this.CheckDiscovered();
			}
		}

		// Token: 0x060049A9 RID: 18857 RVA: 0x00269585 File Offset: 0x00267985
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			this.CheckDiscovered();
		}

		// Token: 0x060049AA RID: 18858 RVA: 0x00269590 File Offset: 0x00267990
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

		// Token: 0x060049AB RID: 18859 RVA: 0x0026990F File Offset: 0x00267D0F
		public override void Notify_PawnDied()
		{
			this.CheckDiscovered();
		}

		// Token: 0x060049AC RID: 18860 RVA: 0x00269918 File Offset: 0x00267D18
		public override string CompDebugString()
		{
			return "discovered: " + this.discovered;
		}
	}
}
