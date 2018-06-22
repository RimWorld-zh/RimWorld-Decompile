using System;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D25 RID: 3365
	public class Hediff_AddedPart : HediffWithComps
	{
		// Token: 0x17000BCF RID: 3023
		// (get) Token: 0x06004A35 RID: 18997 RVA: 0x0026BDF4 File Offset: 0x0026A1F4
		public override bool ShouldRemove
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000BD0 RID: 3024
		// (get) Token: 0x06004A36 RID: 18998 RVA: 0x0026BE0C File Offset: 0x0026A20C
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

		// Token: 0x06004A37 RID: 18999 RVA: 0x0026BE6C File Offset: 0x0026A26C
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

		// Token: 0x06004A38 RID: 19000 RVA: 0x0026BF44 File Offset: 0x0026A344
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit && base.Part == null)
			{
				Log.Error("Hediff_AddedPart has null part after loading.", false);
				this.pawn.health.hediffSet.hediffs.Remove(this);
			}
		}
	}
}
