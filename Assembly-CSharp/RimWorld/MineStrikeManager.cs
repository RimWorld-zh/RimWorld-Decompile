using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000414 RID: 1044
	public class MineStrikeManager : IExposable
	{
		// Token: 0x060011F2 RID: 4594 RVA: 0x0009BCE8 File Offset: 0x0009A0E8
		public void ExposeData()
		{
			Scribe_Collections.Look<StrikeRecord>(ref this.strikeRecords, "strikeRecords", LookMode.Deep, new object[0]);
		}

		// Token: 0x060011F3 RID: 4595 RVA: 0x0009BD04 File Offset: 0x0009A104
		public void CheckStruckOre(IntVec3 justMinedPos, ThingDef justMinedDef, Thing miner)
		{
			if (miner.Faction == Faction.OfPlayer)
			{
				for (int i = 0; i < 4; i++)
				{
					IntVec3 intVec = justMinedPos + GenAdj.CardinalDirections[i];
					if (intVec.InBounds(miner.Map))
					{
						Building edifice = intVec.GetEdifice(miner.Map);
						if (edifice != null && edifice.def != justMinedDef && MineStrikeManager.MineableIsValuable(edifice.def) && !this.AlreadyVisibleNearby(intVec, miner.Map, edifice.def) && !this.RecentlyStruck(intVec, edifice.def))
						{
							StrikeRecord item = default(StrikeRecord);
							item.cell = intVec;
							item.def = edifice.def;
							item.ticksGame = Find.TickManager.TicksGame;
							this.strikeRecords.Add(item);
							Messages.Message("StruckMineable".Translate(new object[]
							{
								edifice.def.label
							}), edifice, MessageTypeDefOf.PositiveEvent, true);
							TaleRecorder.RecordTale(TaleDefOf.StruckMineable, new object[]
							{
								miner,
								edifice
							});
						}
					}
				}
			}
		}

		// Token: 0x060011F4 RID: 4596 RVA: 0x0009BE44 File Offset: 0x0009A244
		public bool AlreadyVisibleNearby(IntVec3 center, Map map, ThingDef mineableDef)
		{
			CellRect cellRect = CellRect.CenteredOn(center, 1);
			for (int i = 1; i < MineStrikeManager.RadialVisibleCells; i++)
			{
				IntVec3 c = center + GenRadial.RadialPattern[i];
				if (c.InBounds(map) && !c.Fogged(map) && !cellRect.Contains(c))
				{
					Building edifice = c.GetEdifice(map);
					if (edifice != null && edifice.def == mineableDef)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060011F5 RID: 4597 RVA: 0x0009BEE0 File Offset: 0x0009A2E0
		private bool RecentlyStruck(IntVec3 cell, ThingDef def)
		{
			for (int i = this.strikeRecords.Count - 1; i >= 0; i--)
			{
				if (this.strikeRecords[i].Expired)
				{
					this.strikeRecords.RemoveAt(i);
				}
				else if (this.strikeRecords[i].def == def && this.strikeRecords[i].cell.InHorDistOf(cell, 12f))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060011F6 RID: 4598 RVA: 0x0009BF8C File Offset: 0x0009A38C
		public static bool MineableIsValuable(ThingDef mineableDef)
		{
			return mineableDef.mineable && mineableDef.building.mineableThing != null && mineableDef.building.mineableThing.GetStatValueAbstract(StatDefOf.MarketValue, null) * (float)mineableDef.building.mineableYield > 10f;
		}

		// Token: 0x060011F7 RID: 4599 RVA: 0x0009BFF0 File Offset: 0x0009A3F0
		public static bool MineableIsVeryValuable(ThingDef mineableDef)
		{
			return mineableDef.mineable && mineableDef.building.mineableThing != null && mineableDef.building.mineableThing.GetStatValueAbstract(StatDefOf.MarketValue, null) * (float)mineableDef.building.mineableYield > 100f;
		}

		// Token: 0x060011F8 RID: 4600 RVA: 0x0009C054 File Offset: 0x0009A454
		public string DebugStrikeRecords()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (StrikeRecord strikeRecord in this.strikeRecords)
			{
				stringBuilder.AppendLine(strikeRecord.ToString());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04000AF3 RID: 2803
		private List<StrikeRecord> strikeRecords = new List<StrikeRecord>();

		// Token: 0x04000AF4 RID: 2804
		private const int RecentStrikeIgnoreRadius = 12;

		// Token: 0x04000AF5 RID: 2805
		private static readonly int RadialVisibleCells = GenRadial.NumCellsInRadius(5.9f);
	}
}
