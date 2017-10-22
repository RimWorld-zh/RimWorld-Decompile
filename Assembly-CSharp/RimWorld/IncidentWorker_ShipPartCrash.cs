using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	internal abstract class IncidentWorker_ShipPartCrash : IncidentWorker
	{
		private const float ShipPointsFactor = 0.9f;

		private const int IncidentMinimumPoints = 300;

		protected virtual int CountToSpawn
		{
			get
			{
				return 1;
			}
		}

		protected override bool CanFireNowSub(IIncidentTarget target)
		{
			Map map = (Map)target;
			return (byte)((map.listerThings.ThingsOfDef(base.def.shipPart).Count <= 0) ? 1 : 0) != 0;
		}

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			int num = 0;
			int countToSpawn = this.CountToSpawn;
			IntVec3 cell = IntVec3.Invalid;
			float shrapnelDirection = Rand.Range(0f, 360f);
			int num2 = 0;
			IntVec3 intVec = default(IntVec3);
			while (num2 < countToSpawn && CellFinderLoose.TryFindSkyfallerCell(ThingDefOf.CrashedShipPartIncoming, map, out intVec, 14, default(IntVec3), -1, false, true, true, true, (Predicate<IntVec3>)null))
			{
				Building_CrashedShipPart building_CrashedShipPart = (Building_CrashedShipPart)ThingMaker.MakeThing(base.def.shipPart, null);
				building_CrashedShipPart.SetFaction(Faction.OfMechanoids, null);
				building_CrashedShipPart.GetComp<CompSpawnerMechanoidsOnDamaged>().pointsLeft = Mathf.Max((float)(parms.points * 0.89999997615814209), 300f);
				Skyfaller skyfaller = SkyfallerMaker.MakeSkyfaller(ThingDefOf.CrashedShipPartIncoming, building_CrashedShipPart);
				skyfaller.shrapnelDirection = shrapnelDirection;
				GenSpawn.Spawn(skyfaller, intVec, map);
				num++;
				cell = intVec;
				num2++;
			}
			if (num > 0)
			{
				base.SendStandardLetter(new TargetInfo(cell, map, false));
			}
			return num > 0;
		}
	}
}
