using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005E4 RID: 1508
	public static class CaravanUtility
	{
		// Token: 0x06001DD6 RID: 7638 RVA: 0x001013A4 File Offset: 0x000FF7A4
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

		// Token: 0x06001DD7 RID: 7639 RVA: 0x001013F8 File Offset: 0x000FF7F8
		public static Caravan GetCaravan(this Pawn pawn)
		{
			return pawn.ParentHolder as Caravan;
		}

		// Token: 0x06001DD8 RID: 7640 RVA: 0x00101418 File Offset: 0x000FF818
		public static bool IsCaravanMember(this Pawn pawn)
		{
			return pawn.GetCaravan() != null;
		}

		// Token: 0x06001DD9 RID: 7641 RVA: 0x0010143C File Offset: 0x000FF83C
		public static bool IsPlayerControlledCaravanMember(this Pawn pawn)
		{
			Caravan caravan = pawn.GetCaravan();
			return caravan != null && caravan.IsPlayerControlled;
		}

		// Token: 0x06001DDA RID: 7642 RVA: 0x00101468 File Offset: 0x000FF868
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

		// Token: 0x06001DDB RID: 7643 RVA: 0x001014B8 File Offset: 0x000FF8B8
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

		// Token: 0x06001DDC RID: 7644 RVA: 0x0010150C File Offset: 0x000FF90C
		public static Pawn RandomOwner(this Caravan caravan)
		{
			return (from p in caravan.PawnsListForReading
			where caravan.IsOwner(p)
			select p).RandomElement<Pawn>();
		}

		// Token: 0x06001DDD RID: 7645 RVA: 0x00101550 File Offset: 0x000FF950
		public static bool ShouldAutoCapture(Pawn p, Faction caravanFaction)
		{
			return p.RaceProps.Humanlike && !p.Dead && p.Faction != caravanFaction && (!p.IsPrisoner || p.HostFaction != caravanFaction);
		}
	}
}
