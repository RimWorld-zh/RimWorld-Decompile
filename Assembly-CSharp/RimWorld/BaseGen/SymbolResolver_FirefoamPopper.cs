using System;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.BaseGen
{
	public class SymbolResolver_FirefoamPopper : SymbolResolver
	{
		public SymbolResolver_FirefoamPopper()
		{
		}

		public override bool CanResolve(ResolveParams rp)
		{
			IntVec3 intVec;
			return base.CanResolve(rp) && this.TryFindSpawnCell(rp.rect, out intVec);
		}

		public override void Resolve(ResolveParams rp)
		{
			IntVec3 loc;
			if (!this.TryFindSpawnCell(rp.rect, out loc))
			{
				return;
			}
			Thing thing = ThingMaker.MakeThing(ThingDefOf.FirefoamPopper, null);
			thing.SetFaction(rp.faction, null);
			GenSpawn.Spawn(thing, loc, BaseGen.globalSettings.map, WipeMode.Vanish);
		}

		private bool TryFindSpawnCell(CellRect rect, out IntVec3 result)
		{
			Map map = BaseGen.globalSettings.map;
			return CellFinder.TryFindRandomCellInsideWith(rect, delegate(IntVec3 c)
			{
				bool result2;
				if (c.Standable(map) && !BaseGenUtility.AnyDoorAdjacentCardinalTo(c, map) && c.GetFirstItem(map) == null)
				{
					result2 = !GenSpawn.WouldWipeAnythingWith(c, Rot4.North, ThingDefOf.FirefoamPopper, map, (Thing x) => x.def.category == ThingCategory.Building);
				}
				else
				{
					result2 = false;
				}
				return result2;
			}, out result);
		}

		[CompilerGenerated]
		private sealed class <TryFindSpawnCell>c__AnonStorey0
		{
			internal Map map;

			private static Predicate<Thing> <>f__am$cache0;

			public <TryFindSpawnCell>c__AnonStorey0()
			{
			}

			internal bool <>m__0(IntVec3 c)
			{
				bool result;
				if (c.Standable(this.map) && !BaseGenUtility.AnyDoorAdjacentCardinalTo(c, this.map) && c.GetFirstItem(this.map) == null)
				{
					result = !GenSpawn.WouldWipeAnythingWith(c, Rot4.North, ThingDefOf.FirefoamPopper, this.map, (Thing x) => x.def.category == ThingCategory.Building);
				}
				else
				{
					result = false;
				}
				return result;
			}

			private static bool <>m__1(Thing x)
			{
				return x.def.category == ThingCategory.Building;
			}
		}
	}
}
