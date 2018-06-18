using System;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D28 RID: 3368
	public class Hediff_AddedPart : HediffWithComps
	{
		// Token: 0x17000BCD RID: 3021
		// (get) Token: 0x06004A24 RID: 18980 RVA: 0x0026A9C0 File Offset: 0x00268DC0
		public override bool ShouldRemove
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000BCE RID: 3022
		// (get) Token: 0x06004A25 RID: 18981 RVA: 0x0026A9D8 File Offset: 0x00268DD8
		public override string TipStringExtra
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(base.TipStringExtra);
				stringBuilder.AppendLine("Efficiency".Translate() + ": " + this.def.addedPartProps.partEfficiency.ToStringPercent());
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06004A26 RID: 18982 RVA: 0x0026AA38 File Offset: 0x00268E38
		public override void PostAdd(DamageInfo? dinfo)
		{
			if (base.Part == null)
			{
				Log.Error("Part is null. It should be set before PostAdd for " + this.def + ".", false);
			}
			else
			{
				this.pawn.health.RestorePart(base.Part, this, false);
				for (int i = 0; i < base.Part.parts.Count; i++)
				{
					Hediff_MissingPart hediff_MissingPart = (Hediff_MissingPart)HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, this.pawn, null);
					hediff_MissingPart.IsFresh = true;
					hediff_MissingPart.lastInjury = HediffDefOf.SurgicalCut;
					hediff_MissingPart.Part = base.Part.parts[i];
					this.pawn.health.hediffSet.AddDirect(hediff_MissingPart, null, null);
				}
			}
		}
	}
}
