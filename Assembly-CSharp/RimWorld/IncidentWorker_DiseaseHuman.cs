using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000329 RID: 809
	public class IncidentWorker_DiseaseHuman : IncidentWorker_Disease
	{
		// Token: 0x06000DD6 RID: 3542 RVA: 0x000764C8 File Offset: 0x000748C8
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

		// Token: 0x06000DD7 RID: 3543 RVA: 0x00076528 File Offset: 0x00074928
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
