using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000341 RID: 833
	public class IncidentWorker_ShipChunkDrop : IncidentWorker
	{
		// Token: 0x040008E8 RID: 2280
		private static readonly Pair<int, float>[] CountChance = new Pair<int, float>[]
		{
			new Pair<int, float>(1, 1f),
			new Pair<int, float>(2, 0.95f),
			new Pair<int, float>(3, 0.7f),
			new Pair<int, float>(4, 0.4f)
		};

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000E35 RID: 3637 RVA: 0x00078E68 File Offset: 0x00077268
		private int RandomCountToDrop
		{
			get
			{
				float x2 = (float)Find.TickManager.TicksGame / 3600000f;
				float timePassedFactor = Mathf.Clamp(GenMath.LerpDouble(0f, 1.2f, 1f, 0.1f, x2), 0.1f, 1f);
				return IncidentWorker_ShipChunkDrop.CountChance.RandomElementByWeight(delegate(Pair<int, float> x)
				{
					float result;
					if (x.First == 1)
					{
						result = x.Second;
					}
					else
					{
						result = x.Second * timePassedFactor;
					}
					return result;
				}).First;
			}
		}

		// Token: 0x06000E36 RID: 3638 RVA: 0x00078EE4 File Offset: 0x000772E4
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			bool result;
			if (!base.CanFireNowSub(parms))
			{
				result = false;
			}
			else
			{
				Map map = (Map)parms.target;
				IntVec3 intVec;
				result = this.TryFindShipChunkDropCell(map.Center, map, 999999, out intVec);
			}
			return result;
		}

		// Token: 0x06000E37 RID: 3639 RVA: 0x00078F2C File Offset: 0x0007732C
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 intVec;
			bool result;
			if (!this.TryFindShipChunkDropCell(map.Center, map, 999999, out intVec))
			{
				result = false;
			}
			else
			{
				this.SpawnShipChunks(intVec, map, this.RandomCountToDrop);
				Messages.Message("MessageShipChunkDrop".Translate(), new TargetInfo(intVec, map, false), MessageTypeDefOf.NeutralEvent, true);
				result = true;
			}
			return result;
		}

		// Token: 0x06000E38 RID: 3640 RVA: 0x00078FA0 File Offset: 0x000773A0
		private void SpawnShipChunks(IntVec3 firstChunkPos, Map map, int count)
		{
			this.SpawnChunk(firstChunkPos, map);
			for (int i = 0; i < count - 1; i++)
			{
				IntVec3 pos;
				if (this.TryFindShipChunkDropCell(firstChunkPos, map, 5, out pos))
				{
					this.SpawnChunk(pos, map);
				}
			}
		}

		// Token: 0x06000E39 RID: 3641 RVA: 0x00078FE4 File Offset: 0x000773E4
		private void SpawnChunk(IntVec3 pos, Map map)
		{
			SkyfallerMaker.SpawnSkyfaller(ThingDefOf.ShipChunkIncoming, ThingDefOf.ShipChunk, pos, map);
		}

		// Token: 0x06000E3A RID: 3642 RVA: 0x00078FFC File Offset: 0x000773FC
		private bool TryFindShipChunkDropCell(IntVec3 nearLoc, Map map, int maxDist, out IntVec3 pos)
		{
			ThingDef shipChunkIncoming = ThingDefOf.ShipChunkIncoming;
			ref IntVec3 cell = ref pos;
			return CellFinderLoose.TryFindSkyfallerCell(shipChunkIncoming, map, out cell, 10, nearLoc, maxDist, true, false, false, false, true, false, null);
		}
	}
}
