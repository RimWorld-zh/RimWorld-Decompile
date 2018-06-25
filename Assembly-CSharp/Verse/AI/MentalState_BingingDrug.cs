using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A79 RID: 2681
	public class MentalState_BingingDrug : MentalState_Binging
	{
		// Token: 0x04002585 RID: 9605
		public ChemicalDef chemical;

		// Token: 0x04002586 RID: 9606
		public DrugCategory drugCategory;

		// Token: 0x04002587 RID: 9607
		private static List<ChemicalDef> addictions = new List<ChemicalDef>();

		// Token: 0x17000917 RID: 2327
		// (get) Token: 0x06003B93 RID: 15251 RVA: 0x001F7FF8 File Offset: 0x001F63F8
		public override string InspectLine
		{
			get
			{
				return string.Format(base.InspectLine, this.chemical.label);
			}
		}

		// Token: 0x06003B94 RID: 15252 RVA: 0x001F8023 File Offset: 0x001F6423
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ChemicalDef>(ref this.chemical, "chemical");
			Scribe_Values.Look<DrugCategory>(ref this.drugCategory, "drugCategory", DrugCategory.None, false);
		}

		// Token: 0x06003B95 RID: 15253 RVA: 0x001F8050 File Offset: 0x001F6450
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			this.ChooseRandomChemical();
			if (PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				string label = "MentalBreakLetterLabel".Translate() + ": " + "LetterLabelDrugBinge".Translate(new object[]
				{
					this.chemical.label
				}).CapitalizeFirst();
				string text = "LetterDrugBinge".Translate(new object[]
				{
					this.pawn.Label,
					this.chemical.label
				}).CapitalizeFirst();
				if (reason != null)
				{
					text = text + "\n\n" + "FinalStraw".Translate(new object[]
					{
						reason.CapitalizeFirst()
					});
				}
				Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.ThreatSmall, this.pawn, null, null);
			}
		}

		// Token: 0x06003B96 RID: 15254 RVA: 0x001F8130 File Offset: 0x001F6530
		public override void PostEnd()
		{
			base.PostEnd();
			if (PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				Messages.Message("MessageNoLongerBingingOnDrug".Translate(new object[]
				{
					this.pawn.LabelShort,
					this.chemical.label
				}), this.pawn, MessageTypeDefOf.SituationResolved, true);
			}
		}

		// Token: 0x06003B97 RID: 15255 RVA: 0x001F8198 File Offset: 0x001F6598
		private void ChooseRandomChemical()
		{
			MentalState_BingingDrug.addictions.Clear();
			List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				Hediff_Addiction hediff_Addiction = hediffs[i] as Hediff_Addiction;
				if (hediff_Addiction != null && AddictionUtility.CanBingeOnNow(this.pawn, hediff_Addiction.Chemical, DrugCategory.Any))
				{
					MentalState_BingingDrug.addictions.Add(hediff_Addiction.Chemical);
				}
			}
			if (MentalState_BingingDrug.addictions.Count > 0)
			{
				this.chemical = MentalState_BingingDrug.addictions.RandomElement<ChemicalDef>();
				this.drugCategory = DrugCategory.Any;
				MentalState_BingingDrug.addictions.Clear();
			}
			else
			{
				this.chemical = (from x in DefDatabase<ChemicalDef>.AllDefsListForReading
				where AddictionUtility.CanBingeOnNow(this.pawn, x, this.def.drugCategory)
				select x).RandomElementWithFallback(null);
				if (this.chemical != null)
				{
					this.drugCategory = this.def.drugCategory;
				}
				else
				{
					this.chemical = (from x in DefDatabase<ChemicalDef>.AllDefsListForReading
					where AddictionUtility.CanBingeOnNow(this.pawn, x, DrugCategory.Any)
					select x).RandomElementWithFallback(null);
					if (this.chemical != null)
					{
						this.drugCategory = DrugCategory.Any;
					}
					else
					{
						this.chemical = DefDatabase<ChemicalDef>.AllDefsListForReading.RandomElement<ChemicalDef>();
						this.drugCategory = DrugCategory.Any;
					}
				}
			}
		}
	}
}
