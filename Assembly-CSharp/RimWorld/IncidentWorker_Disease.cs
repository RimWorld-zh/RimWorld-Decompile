using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_Disease : IncidentWorker
	{
		private IEnumerable<Pawn> PotentialVictims(Map map)
		{
			return map.mapPawns.FreeColonistsAndPrisoners.Where((Func<Pawn, bool>)delegate(Pawn p)
			{
				if (p.ParentHolder is Building_CryptosleepCasket)
				{
					return false;
				}
				if (!base.def.diseasePartsToAffect.NullOrEmpty())
				{
					int num = 0;
					while (true)
					{
						if (num < base.def.diseasePartsToAffect.Count)
						{
							if (!IncidentWorker_Disease.CanAddHediffToAnyPartOfDef(p, base.def.diseaseIncident, base.def.diseasePartsToAffect[num]))
							{
								num++;
								continue;
							}
							break;
						}
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
				if (bodyPartRecord.def == partDef && !pawn.health.hediffSet.PartIsMissing(bodyPartRecord) && !pawn.health.hediffSet.HasHediff(hediffDef, bodyPartRecord))
				{
					return true;
				}
			}
			return false;
		}

		protected override bool CanFireNowSub(IIncidentTarget target)
		{
			if (!this.PotentialVictims((Map)target).Any())
			{
				return false;
			}
			return true;
		}

		public override bool TryExecute(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			int num = map.mapPawns.FreeColonistsAndPrisoners.Count();
			int randomInRange = new IntRange(Mathf.RoundToInt((float)num * base.def.diseaseVictimFractionRange.min), Mathf.RoundToInt((float)num * base.def.diseaseVictimFractionRange.max)).RandomInRange;
			randomInRange = Mathf.Clamp(randomInRange, 1, base.def.diseaseMaxVictims);
			int num2 = 0;
			while (num2 < randomInRange && this.PotentialVictims(map).Any())
			{
				Pawn pawn = this.PotentialVictims(map).RandomElementByWeight((Func<Pawn, float>)((Pawn x) => x.health.immunity.DiseaseContractChanceFactor(base.def.diseaseIncident, null)));
				HediffGiveUtility.TryApply(pawn, base.def.diseaseIncident, base.def.diseasePartsToAffect, false, 1, null);
				num2++;
			}
			return true;
		}
	}
}
