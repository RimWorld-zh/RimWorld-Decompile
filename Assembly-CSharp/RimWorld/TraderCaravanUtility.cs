using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000783 RID: 1923
	public static class TraderCaravanUtility
	{
		// Token: 0x06002AAE RID: 10926 RVA: 0x0016973C File Offset: 0x00167B3C
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

		// Token: 0x06002AAF RID: 10927 RVA: 0x001697C8 File Offset: 0x00167BC8
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

		// Token: 0x06002AB0 RID: 10928 RVA: 0x00169828 File Offset: 0x00167C28
		public static float GenerateGuardPoints()
		{
			return (float)Rand.Range(550, 1000);
		}
	}
}
