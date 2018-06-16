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
		// (get) Token: 0x06002B4E RID: 11086 RVA: 0x0016DCA8 File Offset: 0x0016C0A8
		private IEnumerable<Pawn> AwaitingMedicalOperation
		{
			get
			{
				return from p in PawnsFinder.AllMaps_SpawnedPawnsInFaction(Faction.OfPlayer).Concat(PawnsFinder.AllMaps_PrisonersOfColonySpawned)
				where HealthAIUtility.ShouldHaveSurgeryDoneNow(p) && p.InBed()
				select p;
			}
		}

		// Token: 0x06002B4F RID: 11087 RVA: 0x0016DCF4 File Offset: 0x0016C0F4
		public override string GetLabel()
		{
			return string.Format("PatientsAwaitingMedicalOperation".Translate(), this.AwaitingMedicalOperation.Count<Pawn>().ToStringCached());
		}

		// Token: 0x06002B50 RID: 11088 RVA: 0x0016DD28 File Offset: 0x0016C128
		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.AwaitingMedicalOperation)
			{
				stringBuilder.AppendLine("    " + pawn.LabelShort.CapitalizeFirst());
			}
			return string.Format("PatientsAwaitingMedicalOperationDesc".Translate(), stringBuilder.ToString());
		}

		// Token: 0x06002B51 RID: 11089 RVA: 0x0016DDBC File Offset: 0x0016C1BC
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.AwaitingMedicalOperation);
		}
	}
}
