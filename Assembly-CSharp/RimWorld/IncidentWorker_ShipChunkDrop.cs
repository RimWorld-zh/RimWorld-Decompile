using System;
using System.Linq;
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
				return IncidentWorker_ShipChunkDrop.CountChance.RandomElementByWeight((Func<Pair<int, float>, float>)delegate(Pair<int, float> x)
				{
					if (x.First == 1)
					{
						return x.Second;
					}
					return x.Second * timePassedFactor;
				}).First;
			}
		}

		public override bool TryExecute(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 intVec = default(IntVec3);
			if (!ShipChunkDropCellFinder.TryFindShipChunkDropCell(map.Center, map, 999999, out intVec))
			{
				return false;
			}
			this.SpawnShipChunks(intVec, map, this.RandomCountToDrop);
			Messages.Message("MessageShipChunkDrop".Translate(), new TargetInfo(intVec, map, false), MessageSound.Standard);
			return true;
		}

		private void SpawnShipChunks(IntVec3 firstChunkPos, Map map, int count)
		{
			this.SpawnChunk(firstChunkPos, map);
			for (int i = 0; i < count - 1; i++)
			{
				IntVec3 pos = default(IntVec3);
				if (ShipChunkDropCellFinder.TryFindShipChunkDropCell(firstChunkPos, map, 5, out pos))
				{
					this.SpawnChunk(pos, map);
				}
			}
		}

		private void SpawnChunk(IntVec3 pos, Map map)
		{
			CellRect cr = CellRect.SingleCell(pos);
			cr.Width++;
			cr.Height++;
			RoofCollapserImmediate.DropRoofInCells(from c in cr.ExpandedBy(1).ClipInsideMap(map).Cells
			where cr.Contains(c) || !map.thingGrid.CellContains(c, ThingCategory.Pawn)
			select c, map);
			GenSpawn.Spawn(ThingDefOf.ShipChunk, pos, map);
		}
	}
}
