using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005E8 RID: 1512
	public static class CaravanUtility
	{
		// Token: 0x06001DDD RID: 7645 RVA: 0x001012D8 File Offset: 0x000FF6D8
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

		// Token: 0x06001DDE RID: 7646 RVA: 0x0010132C File Offset: 0x000FF72C
		public static Caravan GetCaravan(this Pawn pawn)
		{
			return pawn.ParentHolder as Caravan;
		}

		// Token: 0x06001DDF RID: 7647 RVA: 0x0010134C File Offset: 0x000FF74C
		public static bool IsCaravanMember(this Pawn pawn)
		{
			return pawn.GetCaravan() != null;
		}

		// Token: 0x06001DE0 RID: 7648 RVA: 0x00101370 File Offset: 0x000FF770
		public static bool IsPlayerControlledCaravanMember(this Pawn pawn)
		{
			Caravan caravan = pawn.GetCaravan();
			return caravan != null && caravan.IsPlayerControlled;
		}

		// Token: 0x06001DE1 RID: 7649 RVA: 0x0010139C File Offset: 0x000FF79C
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

		// Token: 0x06001DE2 RID: 7650 RVA: 0x001013EC File Offset: 0x000FF7EC
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

		// Token: 0x06001DE3 RID: 7651 RVA: 0x00101440 File Offset: 0x000FF840
		public static Pawn RandomOwner(this Caravan caravan)
		{
			return (from p in caravan.PawnsListForReading
			where caravan.IsOwner(p)
			select p).RandomElement<Pawn>();
		}

		// Token: 0x06001DE4 RID: 7652 RVA: 0x00101484 File Offset: 0x000FF884
		public static bool ShouldAutoCapture(Pawn p, Faction caravanFaction)
		{
			return p.RaceProps.Humanlike && !p.Dead && p.Faction != caravanFaction && (!p.IsPrisoner || p.HostFaction != caravanFaction);
		}
	}
}
