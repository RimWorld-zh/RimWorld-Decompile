using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020007A3 RID: 1955
	public class Alert_AwaitingMedicalOperation : Alert
	{
		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x06002B49 RID: 11081 RVA: 0x0016DF14 File Offset: 0x0016C314
		private IEnumerable<Pawn> AwaitingMedicalOperation
		{
			get
			{
				return from p in PawnsFinder.AllMaps_SpawnedPawnsInFaction(Faction.OfPlayer).Concat(PawnsFinder.AllMaps_PrisonersOfColonySpawned)
				where HealthAIUtility.ShouldHaveSurgeryDoneNow(p) && p.InBed()
				select p;
			}
		}

		// Token: 0x06002B4A RID: 11082 RVA: 0x0016DF60 File Offset: 0x0016C360
		public override string GetLabel()
		{
			return string.Format("PatientsAwaitingMedicalOperation".Translate(), this.AwaitingMedicalOperation.Count<Pawn>().ToStringCached());
		}

		// Token: 0x06002B4B RID: 11083 RVA: 0x0016DF94 File Offset: 0x0016C394
		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.AwaitingMedicalOperation)
			{
				stringBuilder.AppendLine("    " + pawn.LabelShort.CapitalizeFirst());
			}
			return string.Format("PatientsAwaitingMedicalOperationDesc".Translate(), stringBuilder.ToString());
		}

		// Token: 0x06002B4C RID: 11084 RVA: 0x0016E028 File Offset: 0x0016C428
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.AwaitingMedicalOperation);
		}
	}
}
