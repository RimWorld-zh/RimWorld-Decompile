using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_ShipChunkDrop : IncidentWorker
	{
		private static readonly Pair<int, float>[] CountChance = new Pair<int, float>[4]
		{
			new Pair<int, float>(1, 1f),
			new Pair<int, float>(2, 0.95f),
			new Pair<int, float>(3, 0.7f),
			new Pair<int, float>(4, 0.4f)
		};

		private int RandomCountToDrop
		{
			get
			{
				float x2 = (float)((float)Find.TickManager.TicksGame / 3600000.0);
				float timePassedFactor = Mathf.Clamp(GenMath.LerpDouble(0f, 1.2f, 1f, 0.1f, x2), 0.1f, 1f);
				return IncidentWorker_ShipChunkDrop.CountChance.RandomElementByWeight((Func<Pair<int, float>, float>)((Pair<int, float> x) => (x.First != 1) ? (x.Second * timePassedFactor) : x.Second)).First;
			}
		}

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 intVec = default(IntVec3);
			bool result;
			if (!this.TryFindShipChunkDropCell(map.Center, map, 999999, out intVec))
			{
				result = false;
			}
			else
			{
				this.SpawnShipChunks(intVec, map, this.RandomCountToDrop);
				Messages.Message("MessageShipChunkDrop".Translate(), new TargetInfo(intVec, map, false), MessageTypeDefOf.NeutralEvent);
				result = true;
			}
			return result;
		}

		private void SpawnShipChunks(IntVec3 firstChunkPos, Map map, int count)
		{
			this.SpawnChunk(firstChunkPos, map);
			for (int i = 0; i < count - 1; i++)
			{
				IntVec3 pos = default(IntVec3);
				if (this.TryFindShipChunkDropCell(firstChunkPos, map, 5, out pos))
				{
					this.SpawnChunk(pos, map);
				}
			}
		}

		private void SpawnChunk(IntVec3 pos, Map map)
		{
			SkyfallerMaker.SpawnSkyfaller(ThingDefOf.ShipChunkIncoming, ThingDefOf.ShipChunk, pos, map);
		}

		private bool TryFindShipChunkDropCell(IntVec3 nearLoc, Map map, int maxDist, out IntVec3 pos)
		{
			ThingDef shipChunkIncoming = ThingDefOf.ShipChunkIncoming;
			return CellFinderLoose.TryFindSkyfallerCell(shipChunkIncoming, map, out pos, 10, nearLoc, maxDist, true, false, false, false, (Predicate<IntVec3>)null);
		}
	}
}
