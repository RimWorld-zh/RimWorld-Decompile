using RimWorld;
using System.Collections.Generic;
using System.Linq;

namespace Verse.AI
{
	public class MentalState_BingingDrug : MentalState_Binging
	{
		public ChemicalDef chemical;

		public DrugCategory drugCategory;

		private static List<ChemicalDef> addictions = new List<ChemicalDef>();

		public override string InspectLine
		{
			get
			{
				return string.Format(base.InspectLine, this.chemical.label);
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ChemicalDef>(ref this.chemical, "chemical");
			Scribe_Values.Look<DrugCategory>(ref this.drugCategory, "drugCategory", DrugCategory.None, false);
		}

		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			this.ChooseRandomChemical();
			if (PawnUtility.ShouldSendNotificationAbout(base.pawn))
			{
				string label = "MentalBreakLetterLabel".Translate() + ": " + "LetterLabelDrugBinge".Translate(this.chemical.label);
				string text = "LetterDrugBinge".Translate(base.pawn.Label, this.chemical.label).CapitalizeFirst();
				if (reason != null)
				{
					text = text + "\n\n" + "FinalStraw".Translate(reason);
				}
				Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.ThreatSmall, base.pawn, null);
			}
		}

		public override void PostEnd()
		{
			base.PostEnd();
			if (PawnUtility.ShouldSendNotificationAbout(base.pawn))
			{
				Messages.Message("MessageNoLongerBingingOnDrug".Translate(base.pawn.NameStringShort, this.chemical.label), base.pawn, MessageTypeDefOf.SituationResolved);
			}
		}

		private void ChooseRandomChemical()
		{
			MentalState_BingingDrug.addictions.Clear();
			List<Hediff> hediffs = base.pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				Hediff_Addiction hediff_Addiction = hediffs[i] as Hediff_Addiction;
				if (hediff_Addiction != null && AddictionUtility.CanBingeOnNow(base.pawn, hediff_Addiction.Chemical, DrugCategory.Any))
				{
					MentalState_BingingDrug.addictions.Add(hediff_Addiction.Chemical);
				}
			}
			if (MentalState_BingingDrug.addictions.Count > 0)
			{
				this.chemical = MentalState_BingingDrug.addictions.RandomElement();
				this.drugCategory = DrugCategory.Any;
				MentalState_BingingDrug.addictions.Clear();
			}
			else
			{
				this.chemical = (from x in DefDatabase<ChemicalDef>.AllDefsListForReading
				where AddictionUtility.CanBingeOnNow(base.pawn, x, base.def.drugCategory)
				select x).RandomElementWithFallback(null);
				if (this.chemical != null)
				{
					this.drugCategory = base.def.drugCategory;
				}
				else
				{
					this.chemical = (from x in DefDatabase<ChemicalDef>.AllDefsListForReading
					where AddictionUtility.CanBingeOnNow(base.pawn, x, DrugCategory.Any)
					select x).RandomElementWithFallback(null);
					if (this.chemical != null)
					{
						this.drugCategory = DrugCategory.Any;
					}
					else
					{
						this.chemical = DefDatabase<ChemicalDef>.AllDefsListForReading.RandomElement();
						this.drugCategory = DrugCategory.Any;
					}
				}
			}
		}
	}
}
