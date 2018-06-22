using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000340 RID: 832
	internal abstract class IncidentWorker_ShipPartCrash : IncidentWorker
	{
		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000E39 RID: 3641 RVA: 0x00077CD8 File Offset: 0x000760D8
		protected virtual int CountToSpawn
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06000E3A RID: 3642 RVA: 0x00077CF0 File Offset: 0x000760F0
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			return map.listerThings.ThingsOfDef(this.def.shipPart).Count <= 0;
		}

		// Token: 0x06000E3B RID: 3643 RVA: 0x00077D3C File Offset: 0x0007613C
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			int num = 0;
			int countToSpawn = this.CountToSpawn;
			IntVec3 cell = IntVec3.Invalid;
			float shrapnelDirection = Rand.Range(0f, 360f);
			for (int i = 0; i < countToSpawn; i++)
			{
				IntVec3 intVec;
				if (!CellFinderLoose.TryFindSkyfallerCell(ThingDefOf.CrashedShipPartIncoming, map, out intVec, 14, default(IntVec3), -1, false, true, true, true, true, false, null))
				{
					break;
				}
				Building_CrashedShipPart building_CrashedShipPart = (Building_CrashedShipPart)ThingMaker.MakeThing(this.def.shipPart, null);
				building_CrashedShipPart.SetFaction(Faction.OfMechanoids, null);
				building_CrashedShipPart.GetComp<CompSpawnerMechanoidsOnDamaged>().pointsLeft = Mathf.Max(parms.points * 0.9f, 300f);
				Skyfaller skyfaller = SkyfallerMaker.MakeSkyfaller(ThingDefOf.CrashedShipPartIncoming, building_CrashedShipPart);
				skyfaller.shrapnelDirection = shrapnelDirection;
				GenSpawn.Spawn(skyfaller, intVec, map, WipeMode.Vanish);
				num++;
				cell = intVec;
			}
			if (num > 0)
			{
				base.SendStandardLetter(new TargetInfo(cell, map, false), null, new string[0]);
			}
			return num > 0;
		}

		// Token: 0x040008E9 RID: 2281
		private const float ShipPointsFactor = 0.9f;

		// Token: 0x040008EA RID: 2282
		private const int IncidentMinimumPoints = 300;
	}
}
