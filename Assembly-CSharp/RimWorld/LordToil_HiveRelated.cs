using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public abstract class LordToil_HiveRelated : LordToil
	{
		[CompilerGenerated]
		private static Predicate<KeyValuePair<Pawn, Hive>> <>f__am$cache0;

		public LordToil_HiveRelated()
		{
			this.data = new LordToil_HiveRelatedData();
		}

		private LordToil_HiveRelatedData Data
		{
			get
			{
				return (LordToil_HiveRelatedData)this.data;
			}
		}

		protected void FilterOutUnspawnedHives()
		{
			this.Data.assignedHives.RemoveAll((KeyValuePair<Pawn, Hive> x) => x.Value == null || !x.Value.Spawned);
		}

		protected Hive GetHiveFor(Pawn pawn)
		{
			Hive hive;
			Hive result;
			if (this.Data.assignedHives.TryGetValue(pawn, out hive))
			{
				result = hive;
			}
			else
			{
				hive = this.FindClosestHive(pawn);
				if (hive != null)
				{
					this.Data.assignedHives.Add(pawn, hive);
				}
				result = hive;
			}
			return result;
		}

		private Hive FindClosestHive(Pawn pawn)
		{
			return (Hive)GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(ThingDefOf.Hive), PathEndMode.Touch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 30f, (Thing x) => x.Faction == pawn.Faction, null, 0, 30, false, RegionType.Set_Passable, false);
		}

		[CompilerGenerated]
		private static bool <FilterOutUnspawnedHives>m__0(KeyValuePair<Pawn, Hive> x)
		{
			return x.Value == null || !x.Value.Spawned;
		}

		[CompilerGenerated]
		private sealed class <FindClosestHive>c__AnonStorey0
		{
			internal Pawn pawn;

			public <FindClosestHive>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Thing x)
			{
				return x.Faction == this.pawn.Faction;
			}
		}
	}
}
