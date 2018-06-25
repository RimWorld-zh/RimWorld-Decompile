using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld
{
	public class Alert_AwaitingMedicalOperation : Alert
	{
		[CompilerGenerated]
		private static Func<Pawn, bool> <>f__am$cache0;

		public Alert_AwaitingMedicalOperation()
		{
		}

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
			return string.Format("PatientsAwaitingMedicalOperation".Translate(), this.AwaitingMedicalOperation.Count<Pawn>().ToStringCached());
		}

		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.AwaitingMedicalOperation)
			{
				stringBuilder.AppendLine("    " + pawn.LabelShort.CapitalizeFirst());
			}
			return string.Format("PatientsAwaitingMedicalOperationDesc".Translate(), stringBuilder.ToString());
		}

		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.AwaitingMedicalOperation);
		}

		[CompilerGenerated]
		private static bool <get_AwaitingMedicalOperation>m__0(Pawn p)
		{
			return HealthAIUtility.ShouldHaveSurgeryDoneNow(p) && p.InBed();
		}
	}
}
