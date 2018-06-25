using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
				Log.Warning("Called IsOwner with null faction.", false);
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

		public static Pawn RandomOwner(this Caravan caravan)
		{
			return (from p in caravan.PawnsListForReading
			where caravan.IsOwner(p)
			select p).RandomElement<Pawn>();
		}

		public static bool ShouldAutoCapture(Pawn p, Faction caravanFaction)
		{
			return p.RaceProps.Humanlike && !p.Dead && p.Faction != caravanFaction && (!p.IsPrisoner || p.HostFaction != caravanFaction);
		}

		[CompilerGenerated]
		private sealed class <BestGotoDestNear>c__AnonStorey0
		{
			internal Caravan c;

			public <BestGotoDestNear>c__AnonStorey0()
			{
			}

			internal bool <>m__0(int t)
			{
				return !Find.World.Impassable(t) && this.c.CanReach(t);
			}
		}

		[CompilerGenerated]
		private sealed class <RandomOwner>c__AnonStorey1
		{
			internal Caravan caravan;

			public <RandomOwner>c__AnonStorey1()
			{
			}

			internal bool <>m__0(Pawn p)
			{
				return this.caravan.IsOwner(p);
			}
		}
	}
}
