using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D05 RID: 3333
	public class HediffComp_Discoverable : HediffComp
	{
		// Token: 0x17000BA5 RID: 2981
		// (get) Token: 0x060049A2 RID: 18850 RVA: 0x00269438 File Offset: 0x00267838
		public HediffCompProperties_Discoverable Props
		{
			get
			{
				return (HediffCompProperties_Discoverable)this.props;
			}
		}

		// Token: 0x060049A3 RID: 18851 RVA: 0x00269458 File Offset: 0x00267858
		public override void CompExposeData()
		{
			Scribe_Values.Look<bool>(ref this.discovered, "discovered", false, false);
		}

		// Token: 0x060049A4 RID: 18852 RVA: 0x00269470 File Offset: 0x00267870
		public override bool CompDisallowVisible()
		{
			return !this.discovered;
		}

		// Token: 0x060049A5 RID: 18853 RVA: 0x0026948E File Offset: 0x0026788E
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (Find.TickManager.TicksGame % 103 == 0)
			{
				this.CheckDiscovered();
			}
		}

		// Token: 0x060049A6 RID: 18854 RVA: 0x002694A9 File Offset: 0x002678A9
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			this.CheckDiscovered();
		}

		// Token: 0x060049A7 RID: 18855 RVA: 0x002694B4 File Offset: 0x002678B4
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

		// Token: 0x060049A8 RID: 18856 RVA: 0x00269833 File Offset: 0x00267C33
		public override void Notify_PawnDied()
		{
			this.CheckDiscovered();
		}

		// Token: 0x060049A9 RID: 18857 RVA: 0x0026983C File Offset: 0x00267C3C
		public override string CompDebugString()
		{
			return "discovered: " + this.discovered;
		}

		// Token: 0x040031F0 RID: 12784
		private bool discovered = false;
	}
}
