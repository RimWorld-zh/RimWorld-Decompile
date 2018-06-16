using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000787 RID: 1927
	public static class TraderCaravanUtility
	{
		// Token: 0x06002AB3 RID: 10931 RVA: 0x001694D0 File Offset: 0x001678D0
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

		// Token: 0x06002AB4 RID: 10932 RVA: 0x0016955C File Offset: 0x0016795C
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

		// Token: 0x06002AB5 RID: 10933 RVA: 0x001695BC File Offset: 0x001679BC
		public static float GenerateGuardPoints()
		{
			return (float)Rand.Range(550, 1000);
		}
	}
}
