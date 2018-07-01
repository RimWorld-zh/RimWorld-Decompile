using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class GenStep_PreciousLump : GenStep_ScatterLumpsMineable
	{
		public List<ThingOption> mineables;

		public FloatRange totalValueRange = new FloatRange(1000f, 2000f);

		[CompilerGenerated]
		private static Func<ThingOption, float> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<IntVec3, int> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<IntVec3, int> <>f__am$cache2;

		[CompilerGenerated]
		private static Func<IntVec3, int> <>f__am$cache3;

		[CompilerGenerated]
		private static Func<IntVec3, int> <>f__am$cache4;

		public GenStep_PreciousLump()
		{
		}

		public override int SeedPart
		{
			get
			{
				return 1634184421;
			}
		}

		public override void Generate(Map map, GenStepParams parms)
		{
			if (parms.siteCoreOrPart != null && parms.siteCoreOrPart.parms.preciousLumpResources != null)
			{
				this.forcedDefToScatter = parms.siteCoreOrPart.parms.preciousLumpResources;
			}
			else
			{
				this.forcedDefToScatter = this.mineables.RandomElementByWeight((ThingOption x) => x.weight).thingDef;
			}
			this.count = 1;
			float randomInRange = this.totalValueRange.RandomInRange;
			float baseMarketValue = this.forcedDefToScatter.building.mineableThing.BaseMarketValue;
			this.forcedLumpSize = Mathf.Max(Mathf.RoundToInt(randomInRange / ((float)this.forcedDefToScatter.building.mineableYield * baseMarketValue)), 1);
			base.Generate(map, parms);
		}

		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			return map.reachability.CanReachMapEdge(c, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false));
		}

		protected override void ScatterAt(IntVec3 c, Map map, int stackCount = 1)
		{
			base.ScatterAt(c, map, stackCount);
			int minX = this.recentLumpCells.Min((IntVec3 x) => x.x);
			int minZ = this.recentLumpCells.Min((IntVec3 x) => x.z);
			int maxX = this.recentLumpCells.Max((IntVec3 x) => x.x);
			int maxZ = this.recentLumpCells.Max((IntVec3 x) => x.z);
			CellRect var = CellRect.FromLimits(minX, minZ, maxX, maxZ);
			MapGenerator.SetVar<CellRect>("RectOfInterest", var);
		}

		[CompilerGenerated]
		private static float <Generate>m__0(ThingOption x)
		{
			return x.weight;
		}

		[CompilerGenerated]
		private static int <ScatterAt>m__1(IntVec3 x)
		{
			return x.x;
		}

		[CompilerGenerated]
		private static int <ScatterAt>m__2(IntVec3 x)
		{
			return x.z;
		}

		[CompilerGenerated]
		private static int <ScatterAt>m__3(IntVec3 x)
		{
			return x.x;
		}

		[CompilerGenerated]
		private static int <ScatterAt>m__4(IntVec3 x)
		{
			return x.z;
		}
	}
}
