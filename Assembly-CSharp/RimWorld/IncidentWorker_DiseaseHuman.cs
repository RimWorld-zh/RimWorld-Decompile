using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_DiseaseHuman : IncidentWorker_Disease
	{
		[CompilerGenerated]
		private static Func<Pawn, bool> <>f__am$cache0;

		public IncidentWorker_DiseaseHuman()
		{
		}

		protected override IEnumerable<Pawn> PotentialVictimCandidates(IIncidentTarget target)
		{
			Map map = target as Map;
			if (map != null)
			{
				return map.mapPawns.FreeColonistsAndPrisoners;
			}
			return from x in ((Caravan)target).PawnsListForReading
			where x.IsFreeColonist || x.IsPrisonerOfColony
			select x;
		}

		protected override IEnumerable<Pawn> ActualVictims(IncidentParms parms)
		{
			int num = this.PotentialVictimCandidates(parms.target).Count<Pawn>();
			IntRange intRange = new IntRange(Mathf.RoundToInt((float)num * this.def.diseaseVictimFractionRange.min), Mathf.RoundToInt((float)num * this.def.diseaseVictimFractionRange.max));
			int num2 = intRange.RandomInRange;
			num2 = Mathf.Clamp(num2, 1, this.def.diseaseMaxVictims);
			return base.PotentialVictims(parms.target).InRandomOrder(null).Take(num2);
		}

		[CompilerGenerated]
		private static bool <PotentialVictimCandidates>m__0(Pawn x)
		{
			return x.IsFreeColonist || x.IsPrisonerOfColony;
		}
	}
}
