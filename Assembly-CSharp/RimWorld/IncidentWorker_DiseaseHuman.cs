using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200032B RID: 811
	public class IncidentWorker_DiseaseHuman : IncidentWorker_Disease
	{
		// Token: 0x06000DDA RID: 3546 RVA: 0x00076618 File Offset: 0x00074A18
		protected override IEnumerable<Pawn> PotentialVictimCandidates(IIncidentTarget target)
		{
			Map map = target as Map;
			IEnumerable<Pawn> result;
			if (map != null)
			{
				result = map.mapPawns.FreeColonistsAndPrisoners;
			}
			else
			{
				result = from x in ((Caravan)target).PawnsListForReading
				where x.IsFreeColonist || x.IsPrisonerOfColony
				select x;
			}
			return result;
		}

		// Token: 0x06000DDB RID: 3547 RVA: 0x00076678 File Offset: 0x00074A78
		protected override IEnumerable<Pawn> ActualVictims(IncidentParms parms)
		{
			int num = this.PotentialVictimCandidates(parms.target).Count<Pawn>();
			IntRange intRange = new IntRange(Mathf.RoundToInt((float)num * this.def.diseaseVictimFractionRange.min), Mathf.RoundToInt((float)num * this.def.diseaseVictimFractionRange.max));
			int num2 = intRange.RandomInRange;
			num2 = Mathf.Clamp(num2, 1, this.def.diseaseMaxVictims);
			return base.PotentialVictims(parms.target).InRandomOrder(null).Take(num2);
		}
	}
}
