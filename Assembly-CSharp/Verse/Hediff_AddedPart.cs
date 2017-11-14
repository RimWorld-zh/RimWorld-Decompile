using RimWorld;
using System.Text;

namespace Verse
{
	public class Hediff_AddedPart : HediffWithComps
	{
		public override bool ShouldRemove
		{
			get
			{
				return false;
			}
		}

		public override string TipStringExtra
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(base.TipStringExtra);
				stringBuilder.AppendLine("Efficiency".Translate() + ": " + base.def.addedPartProps.partEfficiency.ToStringPercent());
				return stringBuilder.ToString();
			}
		}

		public override void PostAdd(DamageInfo? dinfo)
		{
			if (base.Part == null)
			{
				Log.Error("Part is null. It should be set before PostAdd for " + base.def + ".");
			}
			else
			{
				base.pawn.health.RestorePart(base.Part, this, false);
				for (int i = 0; i < base.Part.parts.Count; i++)
				{
					Hediff_MissingPart hediff_MissingPart = (Hediff_MissingPart)HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, base.pawn, null);
					hediff_MissingPart.IsFresh = true;
					hediff_MissingPart.lastInjury = HediffDefOf.SurgicalCut;
					hediff_MissingPart.Part = base.Part.parts[i];
					base.pawn.health.hediffSet.AddDirect(hediff_MissingPart, null);
				}
			}
		}
	}
}
