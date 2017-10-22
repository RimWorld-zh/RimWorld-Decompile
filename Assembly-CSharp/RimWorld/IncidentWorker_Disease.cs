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
			return (map == null) ? (from x in ((Caravan)target).PawnsListForReading
			where x.IsFreeColonist || x.IsPrisonerOfColony
			select x) : map.mapPawns.FreeColonistsAndPrisoners;
		}

		private IEnumerable<Pawn> PotentialVictims(IIncidentTarget target)
		{
			return this.PotentialVictimCandidates(target).Where((Func<Pawn, bool>)delegate(Pawn p)
			{
				bool result;
				if (p.ParentHolder is Building_CryptosleepCasket)
				{
					result = false;
				}
				else
				{
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
							result = false;
							goto IL_00b9;
						}
					}
					result = (p.health.immunity.DiseaseContractChanceFactor(base.def.diseaseIncident, null) > 0.0);
				}
				goto IL_00b9;
				IL_00b9:
				return result;
			});
		}

		private static bool CanAddHediffToAnyPartOfDef(Pawn pawn, HediffDef hediffDef, BodyPartDef partDef)
		{
			List<BodyPartRecord> allParts = pawn.def.race.body.AllParts;
			int num = 0;
			bool result;
			while (true)
			{
				if (num < allParts.Count)
				{
					BodyPartRecord bodyPartRecord = allParts[num];
					if (bodyPartRecord.def == partDef && !pawn.health.hediffSet.PartIsMissing(bodyPartRecord) && !pawn.health.hediffSet.HasHediff(hediffDef, bodyPartRecord, false))
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}

		protected override bool CanFireNowSub(IIncidentTarget target)
		{
			return (byte)(this.PotentialVictims(target).Any() ? 1 : 0) != 0;
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
