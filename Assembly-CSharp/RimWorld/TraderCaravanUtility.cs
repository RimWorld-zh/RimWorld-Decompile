using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000785 RID: 1925
	public static class TraderCaravanUtility
	{
		// Token: 0x06002AB2 RID: 10930 RVA: 0x0016988C File Offset: 0x00167C8C
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

		// Token: 0x06002AB3 RID: 10931 RVA: 0x00169918 File Offset: 0x00167D18
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

		// Token: 0x06002AB4 RID: 10932 RVA: 0x00169978 File Offset: 0x00167D78
		public static float GenerateGuardPoints()
		{
			return (float)Rand.Range(550, 1000);
		}
	}
}
