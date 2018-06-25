using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000416 RID: 1046
	public class MineStrikeManager : IExposable
	{
		// Token: 0x04000AF3 RID: 2803
		private List<StrikeRecord> strikeRecords = new List<StrikeRecord>();

		// Token: 0x04000AF4 RID: 2804
		private const int RecentStrikeIgnoreRadius = 12;

		// Token: 0x04000AF5 RID: 2805
		private static readonly int RadialVisibleCells = GenRadial.NumCellsInRadius(5.9f);

		// Token: 0x060011F6 RID: 4598 RVA: 0x0009BE38 File Offset: 0x0009A238
		public void ExposeData()
		{
			Scribe_Collections.Look<StrikeRecord>(ref this.strikeRecords, "strikeRecords", LookMode.Deep, new object[0]);
		}

		// Token: 0x060011F7 RID: 4599 RVA: 0x0009BE54 File Offset: 0x0009A254
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

		// Token: 0x060011F8 RID: 4600 RVA: 0x0009BF94 File Offset: 0x0009A394
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

		// Token: 0x060011F9 RID: 4601 RVA: 0x0009C030 File Offset: 0x0009A430
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

		// Token: 0x060011FA RID: 4602 RVA: 0x0009C0DC File Offset: 0x0009A4DC
		public static bool MineableIsValuable(ThingDef mineableDef)
		{
			return mineableDef.mineable && mineableDef.building.mineableThing != null && mineableDef.building.mineableThing.GetStatValueAbstract(StatDefOf.MarketValue, null) * (float)mineableDef.building.mineableYield > 10f;
		}

		// Token: 0x060011FB RID: 4603 RVA: 0x0009C140 File Offset: 0x0009A540
		public static bool MineableIsVeryValuable(ThingDef mineableDef)
		{
			return mineableDef.mineable && mineableDef.building.mineableThing != null && mineableDef.building.mineableThing.GetStatValueAbstract(StatDefOf.MarketValue, null) * (float)mineableDef.building.mineableYield > 100f;
		}

		// Token: 0x060011FC RID: 4604 RVA: 0x0009C1A4 File Offset: 0x0009A5A4
		public string DebugStrikeRecords()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (StrikeRecord strikeRecord in this.strikeRecords)
			{
				stringBuilder.AppendLine(strikeRecord.ToString());
			}
			return stringBuilder.ToString();
		}
	}
}
