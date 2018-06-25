using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020007A5 RID: 1957
	public class Alert_AwaitingMedicalOperation : Alert
	{
		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x06002B4D RID: 11085 RVA: 0x0016E064 File Offset: 0x0016C464
		private IEnumerable<Pawn> AwaitingMedicalOperation
		{
			get
			{
				return from p in PawnsFinder.AllMaps_SpawnedPawnsInFaction(Faction.OfPlayer).Concat(PawnsFinder.AllMaps_PrisonersOfColonySpawned)
				where HealthAIUtility.ShouldHaveSurgeryDoneNow(p) && p.InBed()
				select p;
			}
		}

		// Token: 0x06002B4E RID: 11086 RVA: 0x0016E0B0 File Offset: 0x0016C4B0
		public override string GetLabel()
		{
			return string.Format("PatientsAwaitingMedicalOperation".Translate(), this.AwaitingMedicalOperation.Count<Pawn>().ToStringCached());
		}

		// Token: 0x06002B4F RID: 11087 RVA: 0x0016E0E4 File Offset: 0x0016C4E4
		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.AwaitingMedicalOperation)
			{
				stringBuilder.AppendLine("    " + pawn.LabelShort.CapitalizeFirst());
			}
			return string.Format("PatientsAwaitingMedicalOperationDesc".Translate(), stringBuilder.ToString());
		}

		// Token: 0x06002B50 RID: 11088 RVA: 0x0016E178 File Offset: 0x0016C578
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.AwaitingMedicalOperation);
		}
	}
}
