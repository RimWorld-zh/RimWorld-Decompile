using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	public static class TraderCaravanUtility
	{
		public static TraderCaravanRole GetTraderCaravanRole(this Pawn p)
		{
			TraderCaravanRole result;
			if (p.kindDef == PawnKindDefOf.Slave)
			{
				result = TraderCaravanRole.Chattel;
			}
			else if (p.kindDef.trader)
			{
				result = TraderCaravanRole.Trader;
			}
			else if (p.kindDef.RaceProps.packAnimal && p.inventory.innerContainer.Any)
			{
				result = TraderCaravanRole.Carrier;
			}
			else if (p.RaceProps.Animal)
			{
				result = TraderCaravanRole.Chattel;
			}
			else
			{
				result = TraderCaravanRole.Guard;
			}
			return result;
		}

		public static Pawn FindTrader(Lord lord)
		{
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				if (lord.ownedPawns[i].GetTraderCaravanRole() == TraderCaravanRole.Trader)
				{
					return lord.ownedPawns[i];
				}
			}
			return null;
		}

		public static float GenerateGuardPoints()
		{
			return (float)Rand.Range(550, 1000);
		}
	}
}
