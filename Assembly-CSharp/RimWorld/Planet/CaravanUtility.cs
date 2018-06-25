using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005E6 RID: 1510
	public static class CaravanUtility
	{
		// Token: 0x06001DD9 RID: 7641 RVA: 0x0010175C File Offset: 0x000FFB5C
		public static bool IsOwner(Pawn pawn, Faction caravanFaction)
		{
			bool result;
			if (caravanFaction == null)
			{
				Log.Warning("Called IsOwner with null faction.", false);
				result = false;
			}
			else
			{
				result = (!pawn.NonHumanlikeOrWildMan() && pawn.Faction == caravanFaction && pawn.HostFaction == null);
			}
			return result;
		}

		// Token: 0x06001DDA RID: 7642 RVA: 0x001017B0 File Offset: 0x000FFBB0
		public static Caravan GetCaravan(this Pawn pawn)
		{
			return pawn.ParentHolder as Caravan;
		}

		// Token: 0x06001DDB RID: 7643 RVA: 0x001017D0 File Offset: 0x000FFBD0
		public static bool IsCaravanMember(this Pawn pawn)
		{
			return pawn.GetCaravan() != null;
		}

		// Token: 0x06001DDC RID: 7644 RVA: 0x001017F4 File Offset: 0x000FFBF4
		public static bool IsPlayerControlledCaravanMember(this Pawn pawn)
		{
			Caravan caravan = pawn.GetCaravan();
			return caravan != null && caravan.IsPlayerControlled;
		}

		// Token: 0x06001DDD RID: 7645 RVA: 0x00101820 File Offset: 0x000FFC20
		public static int BestGotoDestNear(int tile, Caravan c)
		{
			Predicate<int> predicate = (int t) => !Find.World.Impassable(t) && c.CanReach(t);
			int result;
			if (predicate(tile))
			{
				result = tile;
			}
			else
			{
				int num;
				GenWorldClosest.TryFindClosestTile(tile, predicate, out num, 50, true);
				result = num;
			}
			return result;
		}

		// Token: 0x06001DDE RID: 7646 RVA: 0x00101870 File Offset: 0x000FFC70
		public static bool PlayerHasAnyCaravan()
		{
			List<Caravan> caravans = Find.WorldObjects.Caravans;
			for (int i = 0; i < caravans.Count; i++)
			{
				if (caravans[i].IsPlayerControlled)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001DDF RID: 7647 RVA: 0x001018C4 File Offset: 0x000FFCC4
		public static Pawn RandomOwner(this Caravan caravan)
		{
			return (from p in caravan.PawnsListForReading
			where caravan.IsOwner(p)
			select p).RandomElement<Pawn>();
		}

		// Token: 0x06001DE0 RID: 7648 RVA: 0x00101908 File Offset: 0x000FFD08
		public static bool ShouldAutoCapture(Pawn p, Faction caravanFaction)
		{
			return p.RaceProps.Humanlike && !p.Dead && p.Faction != caravanFaction && (!p.IsPrisoner || p.HostFaction != caravanFaction);
		}
	}
}
