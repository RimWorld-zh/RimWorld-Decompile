using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_Tornado : IncidentWorker
	{
		private const int MinDistanceFromMapEdge = 30;

		private const float MinWind = 1f;

		protected override bool CanFireNowSub(IIncidentTarget target)
		{
			Map map = (Map)target;
			return map.weatherManager.CurWindSpeedFactor >= 1.0;
		}

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			CellRect cellRect = CellRect.WholeMap(map).ContractedBy(30);
			if (cellRect.IsEmpty)
			{
				cellRect = CellRect.WholeMap(map);
			}
			IntVec3 loc = default(IntVec3);
			bool result;
			if (!CellFinder.TryFindRandomCellInsideWith(cellRect, (Predicate<IntVec3>)((IntVec3 x) => this.CanSpawnTornadoAt(x, map)), out loc))
			{
				result = false;
			}
			else
			{
				Tornado t = (Tornado)GenSpawn.Spawn(ThingDefOf.Tornado, loc, map);
				base.SendStandardLetter((Thing)t);
				result = true;
			}
			return result;
		}

		private bool CanSpawnTornadoAt(IntVec3 c, Map map)
		{
			bool result;
			if (c.Fogged(map))
			{
				result = false;
			}
			else
			{
				int num = GenRadial.NumCellsInRadius(7f);
				for (int num2 = 0; num2 < num; num2++)
				{
					IntVec3 c2 = c + GenRadial.RadialPattern[num2];
					if (c2.InBounds(map) && this.AnyPawnOfPlayerFactionAt(c2, map))
						goto IL_005c;
				}
				result = true;
			}
			goto IL_0076;
			IL_0076:
			return result;
			IL_005c:
			result = false;
			goto IL_0076;
		}

		private bool AnyPawnOfPlayerFactionAt(IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			int num = 0;
			bool result;
			while (true)
			{
				if (num < thingList.Count)
				{
					Pawn pawn = thingList[num] as Pawn;
					if (pawn != null && pawn.Faction == Faction.OfPlayer)
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
	}
}
