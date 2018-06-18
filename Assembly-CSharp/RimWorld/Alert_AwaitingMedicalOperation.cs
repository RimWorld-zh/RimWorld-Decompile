using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020007A7 RID: 1959
	public class Alert_AwaitingMedicalOperation : Alert
	{
		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x06002B50 RID: 11088 RVA: 0x0016DD3C File Offset: 0x0016C13C
		private IEnumerable<Pawn> AwaitingMedicalOperation
		{
			get
			{
				return from p in PawnsFinder.AllMaps_SpawnedPawnsInFaction(Faction.OfPlayer).Concat(PawnsFinder.AllMaps_PrisonersOfColonySpawned)
				where HealthAIUtility.ShouldHaveSurgeryDoneNow(p) && p.InBed()
				select p;
			}
		}

		// Token: 0x06002B51 RID: 11089 RVA: 0x0016DD88 File Offset: 0x0016C188
		public override string GetLabel()
		{
			return string.Format("PatientsAwaitingMedicalOperation".Translate(), this.AwaitingMedicalOperation.Count<Pawn>().ToStringCached());
		}

		// Token: 0x06002B52 RID: 11090 RVA: 0x0016DDBC File Offset: 0x0016C1BC
		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.AwaitingMedicalOperation)
			{
				stringBuilder.AppendLine("    " + pawn.LabelShort.CapitalizeFirst());
			}
			return string.Format("PatientsAwaitingMedicalOperationDesc".Translate(), stringBuilder.ToString());
		}

		// Token: 0x06002B53 RID: 11091 RVA: 0x0016DE50 File Offset: 0x0016C250
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.AwaitingMedicalOperation);
		}
	}
}
