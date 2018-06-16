using System;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D29 RID: 3369
	public class Hediff_AddedPart : HediffWithComps
	{
		// Token: 0x17000BCE RID: 3022
		// (get) Token: 0x06004A26 RID: 18982 RVA: 0x0026A9E8 File Offset: 0x00268DE8
		public override bool ShouldRemove
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000BCF RID: 3023
		// (get) Token: 0x06004A27 RID: 18983 RVA: 0x0026AA00 File Offset: 0x00268E00
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

		// Token: 0x06004A28 RID: 18984 RVA: 0x0026AA60 File Offset: 0x00268E60
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
