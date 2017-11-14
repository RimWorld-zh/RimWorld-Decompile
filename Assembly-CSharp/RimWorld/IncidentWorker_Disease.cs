using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_Disease : IncidentWorker
	{
		private IEnumerable<Pawn> PotentialVictimCandidates(IIncidentTarget target)
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

		private IEnumerable<Pawn> PotentialVictims(IIncidentTarget target)
		{
			return this.PotentialVictimCandidates(target).Where(delegate(Pawn p)
			{
				if (p.ParentHolder is Building_CryptosleepCasket)
				{
					return false;
				}
				if (!base.def.diseasePartsToAffect.NullOrEmpty())
				{
					bool flag = false;
					int num = 0;
					while (num < base.def.diseasePartsToAffect.Count)
					{
						if (!IncidentWorker_Disease.CanAddHediffToAnyPartOfDef(p, base.def.diseaseIncident, base.def.diseasePartsToAffect[num]))
						{
							num++;
							continue;
						}
						flag = true;
						break;
					}
					if (!flag)
					{
						return false;
					}
				}
				return p.health.immunity.DiseaseContractChanceFactor(base.def.diseaseIncident, null) > 0.0;
			});
		}

		private static bool CanAddHediffToAnyPartOfDef(Pawn pawn, HediffDef hediffDef, BodyPartDef partDef)
		{
			List<BodyPartRecord> allParts = pawn.def.race.body.AllParts;
			for (int i = 0; i < allParts.Count; i++)
			{
				BodyPartRecord bodyPartRecord = allParts[i];
				if (bodyPartRecord.def == partDef && !pawn.health.hediffSet.PartIsMissing(bodyPartRecord) && !pawn.health.hediffSet.HasHediff(hediffDef, bodyPartRecord, false))
				{
					return true;
				}
			}
			return false;
		}

		protected override bool CanFireNowSub(IIncidentTarget target)
		{
			if (!this.PotentialVictims(target).Any())
			{
				return false;
			}
			return true;
		}

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			int num = this.PotentialVictimCandidates(parms.target).Count();
			int randomInRange = new IntRange(Mathf.RoundToInt((float)num * base.def.diseaseVictimFractionRange.min), Mathf.RoundToInt((float)num * base.def.diseaseVictimFractionRange.max)).RandomInRange;
			randomInRange = Mathf.Clamp(randomInRange, 1, base.def.diseaseMaxVictims);
			int num2 = 0;
			Pawn pawn = default(Pawn);
			while (num2 < randomInRange && this.PotentialVictims(parms.target).TryRandomElementByWeight<Pawn>((Func<Pawn, float>)((Pawn x) => x.health.immunity.DiseaseContractChanceFactor(base.def.diseaseIncident, null)), out pawn))
			{
				HediffGiveUtility.TryApply(pawn, base.def.diseaseIncident, base.def.diseasePartsToAffect, false, 1, null);
				num2++;
			}
			return true;
		}
	}
}
