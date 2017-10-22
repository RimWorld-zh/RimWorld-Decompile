using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	public static class TraderCaravanUtility
	{
		public static TraderCaravanRole GetTraderCaravanRole(this Pawn p)
		{
			return (TraderCaravanRole)((p.kindDef != PawnKindDefOf.Slave) ? (p.kindDef.trader ? 1 : ((!p.kindDef.RaceProps.packAnimal) ? ((!p.RaceProps.Animal) ? 3 : 4) : 2)) : 4);
		}

		public static Pawn FindTrader(Lord lord)
		{
			int num = 0;
			Pawn result;
			while (true)
			{
				if (num < lord.ownedPawns.Count)
				{
					if (lord.ownedPawns[num].GetTraderCaravanRole() == TraderCaravanRole.Trader)
					{
						result = lord.ownedPawns[num];
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}

		public static float GenerateGuardPoints()
		{
			return (float)Rand.Range(550, 1000);
		}
	}
}
