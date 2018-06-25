using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000785 RID: 1925
	public static class TraderCaravanUtility
	{
		// Token: 0x06002AB1 RID: 10929 RVA: 0x00169AF0 File Offset: 0x00167EF0
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

		// Token: 0x06002AB2 RID: 10930 RVA: 0x00169B7C File Offset: 0x00167F7C
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

		// Token: 0x06002AB3 RID: 10931 RVA: 0x00169BDC File Offset: 0x00167FDC
		public static float GenerateGuardPoints()
		{
			return (float)Rand.Range(550, 1000);
		}
	}
}
