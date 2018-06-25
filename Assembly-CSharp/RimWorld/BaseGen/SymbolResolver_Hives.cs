using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.BaseGen
{
	public class SymbolResolver_Hives : SymbolResolver
	{
		private static readonly IntRange DefaultHivesCountRange = new IntRange(1, 3);

		public SymbolResolver_Hives()
		{
		}

		public override bool CanResolve(ResolveParams rp)
		{
			IntVec3 intVec;
			return base.CanResolve(rp) && this.TryFindFirstHivePos(rp.rect, out intVec);
		}

		public override void Resolve(ResolveParams rp)
		{
			IntVec3 loc;
			if (this.TryFindFirstHivePos(rp.rect, out loc))
			{
				int? hivesCount = rp.hivesCount;
				int num = (hivesCount == null) ? SymbolResolver_Hives.DefaultHivesCountRange.RandomInRange : hivesCount.Value;
				Hive hive = (Hive)ThingMaker.MakeThing(ThingDefOf.Hive, null);
				hive.SetFaction(Faction.OfInsects, null);
				if (rp.disableHives != null && rp.disableHives.Value)
				{
					hive.active = false;
				}
				hive = (Hive)GenSpawn.Spawn(hive, loc, BaseGen.globalSettings.map, WipeMode.Vanish);
				for (int i = 0; i < num - 1; i++)
				{
					Hive hive2;
					if (hive.GetComp<CompSpawnerHives>().TrySpawnChildHive(true, out hive2))
					{
						hive = hive2;
					}
				}
			}
		}

		private bool TryFindFirstHivePos(CellRect rect, out IntVec3 pos)
		{
			Map map = BaseGen.globalSettings.map;
			return (from mc in rect.Cells
			where mc.Standable(map)
			select mc).TryRandomElement(out pos);
		}

		// Note: this type is marked as 'beforefieldinit'.
		static SymbolResolver_Hives()
		{
		}

		[CompilerGenerated]
		private sealed class <TryFindFirstHivePos>c__AnonStorey0
		{
			internal Map map;

			public <TryFindFirstHivePos>c__AnonStorey0()
			{
			}

			internal bool <>m__0(IntVec3 mc)
			{
				return mc.Standable(this.map);
			}
		}
	}
}
