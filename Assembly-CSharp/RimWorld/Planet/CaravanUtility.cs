using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005E6 RID: 1510
	public static class CaravanUtility
	{
		// Token: 0x06001DDA RID: 7642 RVA: 0x001014F4 File Offset: 0x000FF8F4
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

		// Token: 0x06001DDB RID: 7643 RVA: 0x00101548 File Offset: 0x000FF948
		public static Caravan GetCaravan(this Pawn pawn)
		{
			return pawn.ParentHolder as Caravan;
		}

		// Token: 0x06001DDC RID: 7644 RVA: 0x00101568 File Offset: 0x000FF968
		public static bool IsCaravanMember(this Pawn pawn)
		{
			return pawn.GetCaravan() != null;
		}

		// Token: 0x06001DDD RID: 7645 RVA: 0x0010158C File Offset: 0x000FF98C
		public static bool IsPlayerControlledCaravanMember(this Pawn pawn)
		{
			Caravan caravan = pawn.GetCaravan();
			return caravan != null && caravan.IsPlayerControlled;
		}

		// Token: 0x06001DDE RID: 7646 RVA: 0x001015B8 File Offset: 0x000FF9B8
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

		// Token: 0x06001DDF RID: 7647 RVA: 0x00101608 File Offset: 0x000FFA08
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

		// Token: 0x06001DE0 RID: 7648 RVA: 0x0010165C File Offset: 0x000FFA5C
		public static Pawn RandomOwner(this Caravan caravan)
		{
			return (from p in caravan.PawnsListForReading
			where caravan.IsOwner(p)
			select p).RandomElement<Pawn>();
		}

		// Token: 0x06001DE1 RID: 7649 RVA: 0x001016A0 File Offset: 0x000FFAA0
		public static bool ShouldAutoCapture(Pawn p, Faction caravanFaction)
		{
			return p.RaceProps.Humanlike && !p.Dead && p.Faction != caravanFaction && (!p.IsPrisoner || p.HostFaction != caravanFaction);
		}
	}
}
