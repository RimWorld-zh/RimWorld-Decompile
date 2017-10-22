using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	public class Alert_AwaitingMedicalOperation : Alert
	{
		private IEnumerable<Pawn> AwaitingMedicalOperation
		{
			get
			{
				return from p in PawnsFinder.AllMaps_SpawnedPawnsInFaction(Faction.OfPlayer).Concat(PawnsFinder.AllMaps_PrisonersOfColonySpawned)
				where HealthAIUtility.ShouldHaveSurgeryDoneNow(p) && p.InBed()
				select p;
			}
		}

		public override string GetLabel()
		{
			return string.Format("PatientsAwaitingMedicalOperation".Translate(), this.AwaitingMedicalOperation.Count().ToStringCached());
		}

		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn item in this.AwaitingMedicalOperation)
			{
				stringBuilder.AppendLine("    " + item.NameStringShort);
			}
			return string.Format("PatientsAwaitingMedicalOperationDesc".Translate(), stringBuilder.ToString());
		}

		public override AlertReport GetReport()
		{
			return (Thing)this.AwaitingMedicalOperation.FirstOrDefault();
		}
	}
}
