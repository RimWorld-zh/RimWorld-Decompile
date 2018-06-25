using System;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D28 RID: 3368
	public class Hediff_AddedPart : HediffWithComps
	{
		// Token: 0x17000BCE RID: 3022
		// (get) Token: 0x06004A39 RID: 19001 RVA: 0x0026C200 File Offset: 0x0026A600
		public override bool ShouldRemove
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000BCF RID: 3023
		// (get) Token: 0x06004A3A RID: 19002 RVA: 0x0026C218 File Offset: 0x0026A618
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

		// Token: 0x06004A3B RID: 19003 RVA: 0x0026C278 File Offset: 0x0026A678
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

		// Token: 0x06004A3C RID: 19004 RVA: 0x0026C350 File Offset: 0x0026A750
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
