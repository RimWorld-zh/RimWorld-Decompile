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
		// (get) Token: 0x06002B4C RID: 11084 RVA: 0x0016E2C8 File Offset: 0x0016C6C8
		private IEnumerable<Pawn> AwaitingMedicalOperation
		{
			get
			{
				return from p in PawnsFinder.AllMaps_SpawnedPawnsInFaction(Faction.OfPlayer).Concat(PawnsFinder.AllMaps_PrisonersOfColonySpawned)
				where HealthAIUtility.ShouldHaveSurgeryDoneNow(p) && p.InBed()
				select p;
			}
		}

		// Token: 0x06002B4D RID: 11085 RVA: 0x0016E314 File Offset: 0x0016C714
		public override string GetLabel()
		{
			return string.Format("PatientsAwaitingMedicalOperation".Translate(), this.AwaitingMedicalOperation.Count<Pawn>().ToStringCached());
		}

		// Token: 0x06002B4E RID: 11086 RVA: 0x0016E348 File Offset: 0x0016C748
		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.AwaitingMedicalOperation)
			{
				stringBuilder.AppendLine("    " + pawn.LabelShort.CapitalizeFirst());
			}
			return string.Format("PatientsAwaitingMedicalOperationDesc".Translate(), stringBuilder.ToString());
		}

		// Token: 0x06002B4F RID: 11087 RVA: 0x0016E3DC File Offset: 0x0016C7DC
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.AwaitingMedicalOperation);
		}
	}
}
