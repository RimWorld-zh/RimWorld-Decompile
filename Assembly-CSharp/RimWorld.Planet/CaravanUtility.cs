using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	public static class CaravanUtility
	{
		public static bool IsOwner(Pawn pawn, Faction caravanFaction)
		{
			bool result;
			if (caravanFaction == null)
			{
				Log.Warning("Called IsOwner with null faction.");
				result = false;
			}
			else
			{
				result = (!pawn.NonHumanlikeOrWildMan() && pawn.Faction == caravanFaction && pawn.HostFaction == null);
			}
			return result;
		}

		public static Caravan GetCaravan(this Pawn pawn)
		{
			return pawn.ParentHolder as Caravan;
		}

		public static bool IsCaravanMember(this Pawn pawn)
		{
			return pawn.GetCaravan() != null;
		}

		public static bool IsPlayerControlledCaravanMember(this Pawn pawn)
		{
			Caravan caravan = pawn.GetCaravan();
			return caravan != null && caravan.IsPlayerControlled;
		}

		public static int BestGotoDestNear(int tile, Caravan c)
		{
			Predicate<int> predicate = (Predicate<int>)((int t) => (byte)((!Find.World.Impassable(t)) ? (c.CanReach(t) ? 1 : 0) : 0) != 0);
			int result;
			if (predicate(tile))
			{
				result = tile;
			}
			else
			{
				int num = default(int);
				GenWorldClosest.TryFindClosestTile(tile, predicate, out num, 50, true);
				result = num;
			}
			return result;
		}

		public static bool PlayerHasAnyCaravan()
		{
			List<Caravan> caravans = Find.WorldObjects.Caravans;
			int num = 0;
			bool result;
			while (true)
			{
				if (num < caravans.Count)
				{
					if (caravans[num].IsPlayerControlled)
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}

		public static Pawn RandomOwner(this Caravan caravan)
		{
			return (from p in caravan.PawnsListForReading
			where caravan.IsOwner(p)
			select p).RandomElement();
		}
	}
}
