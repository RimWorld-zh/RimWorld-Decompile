using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	public class MineStrikeManager : IExposable
	{
		private List<StrikeRecord> strikeRecords = new List<StrikeRecord>();

		private const int RecentStrikeIgnoreRadius = 12;

		private static readonly int RadialVisibleCells = GenRadial.NumCellsInRadius(5.9f);

		public void ExposeData()
		{
			Scribe_Collections.Look<StrikeRecord>(ref this.strikeRecords, "strikeRecords", LookMode.Deep, new object[0]);
		}

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
							StrikeRecord item = new StrikeRecord
							{
								cell = intVec,
								def = edifice.def,
								ticksGame = Find.TickManager.TicksGame
							};
							this.strikeRecords.Add(item);
							Messages.Message("StruckMineable".Translate(edifice.def.label), (Thing)edifice, MessageTypeDefOf.PositiveEvent);
							TaleRecorder.RecordTale(TaleDefOf.StruckMineable, miner, edifice);
						}
					}
				}
			}
		}

		public bool AlreadyVisibleNearby(IntVec3 center, Map map, ThingDef mineableDef)
		{
			CellRect cellRect = CellRect.CenteredOn(center, 1);
			int num = 1;
			bool result;
			while (true)
			{
				if (num < MineStrikeManager.RadialVisibleCells)
				{
					IntVec3 c = center + GenRadial.RadialPattern[num];
					if (c.InBounds(map) && !c.Fogged(map) && !cellRect.Contains(c))
					{
						Building edifice = c.GetEdifice(map);
						if (edifice != null && edifice.def == mineableDef)
						{
							result = true;
							break;
						}
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}

		private bool RecentlyStruck(IntVec3 cell, ThingDef def)
		{
			int num = this.strikeRecords.Count - 1;
			bool result;
			while (true)
			{
				if (num >= 0)
				{
					if (this.strikeRecords[num].Expired)
					{
						this.strikeRecords.RemoveAt(num);
					}
					else
					{
						StrikeRecord strikeRecord = this.strikeRecords[num];
						if (strikeRecord.def == def)
						{
							StrikeRecord strikeRecord2 = this.strikeRecords[num];
							if (strikeRecord2.cell.InHorDistOf(cell, 12f))
							{
								result = true;
								break;
							}
						}
					}
					num--;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}

		public static bool MineableIsValuable(ThingDef mineableDef)
		{
			return mineableDef.mineable && mineableDef.building.mineableThing.GetStatValueAbstract(StatDefOf.MarketValue, null) * (float)mineableDef.building.mineableYield > 10.0;
		}

		public static bool MineableIsVeryValuable(ThingDef mineableDef)
		{
			return mineableDef.mineable && mineableDef.building.mineableThing.GetStatValueAbstract(StatDefOf.MarketValue, null) * (float)mineableDef.building.mineableYield > 100.0;
		}

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
