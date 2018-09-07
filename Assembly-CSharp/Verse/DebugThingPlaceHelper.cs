using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse
{
	public static class DebugThingPlaceHelper
	{
		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache1;

		public static bool IsDebugSpawnable(ThingDef def, bool allowPlayerBuildable = false)
		{
			return def.forceDebugSpawnable || (def.thingClass != typeof(Corpse) && !def.IsBlueprint && !def.IsFrame && def != ThingDefOf.ActiveDropPod && def.thingClass != typeof(MinifiedThing) && def.thingClass != typeof(UnfinishedThing) && !def.destroyOnDrop && (def.category == ThingCategory.Filth || def.category == ThingCategory.Item || def.category == ThingCategory.Plant || def.category == ThingCategory.Ethereal || (def.category == ThingCategory.Building && def.building.isNaturalRock) || (def.category == ThingCategory.Building && !def.BuildableByPlayer) || (def.category == ThingCategory.Building && def.BuildableByPlayer && allowPlayerBuildable)));
		}

		public static void DebugSpawn(ThingDef def, IntVec3 c, int stackCount = -1, bool direct = false)
		{
			if (stackCount <= 0)
			{
				stackCount = def.stackLimit;
			}
			ThingDef stuff = GenStuff.RandomStuffFor(def);
			Thing thing = ThingMaker.MakeThing(def, stuff);
			CompQuality compQuality = thing.TryGetComp<CompQuality>();
			if (compQuality != null)
			{
				compQuality.SetQuality(QualityUtility.GenerateQualityRandomEqualChance(), ArtGenerationContext.Colony);
			}
			if (thing.def.Minifiable)
			{
				thing = thing.MakeMinified();
			}
			thing.stackCount = stackCount;
			if (direct)
			{
				GenPlace.TryPlaceThing(thing, c, Find.CurrentMap, ThingPlaceMode.Direct, null, null);
			}
			else
			{
				GenPlace.TryPlaceThing(thing, c, Find.CurrentMap, ThingPlaceMode.Near, null, null);
			}
		}

		public static List<DebugMenuOption> TryPlaceOptionsForStackCount(int stackCount, bool direct)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			IEnumerable<ThingDef> enumerable = from def in DefDatabase<ThingDef>.AllDefs
			where DebugThingPlaceHelper.IsDebugSpawnable(def, false) && def.stackLimit >= stackCount
			select def;
			foreach (ThingDef localDef3 in enumerable)
			{
				ThingDef localDef = localDef3;
				list.Add(new DebugMenuOption(localDef.LabelCap, DebugMenuOptionMode.Tool, delegate()
				{
					DebugThingPlaceHelper.DebugSpawn(localDef, UI.MouseCell(), stackCount, direct);
				}));
			}
			if (stackCount == 1)
			{
				foreach (ThingDef localDef2 in from def in DefDatabase<ThingDef>.AllDefs
				where def.Minifiable
				select def)
				{
					ThingDef localDef = localDef2;
					list.Add(new DebugMenuOption(localDef.LabelCap + " (minified)", DebugMenuOptionMode.Tool, delegate()
					{
						DebugThingPlaceHelper.DebugSpawn(localDef, UI.MouseCell(), stackCount, direct);
					}));
				}
			}
			return list;
		}

		public static List<DebugMenuOption> SpawnOptions(WipeMode wipeMode)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			IEnumerable<ThingDef> enumerable = from def in DefDatabase<ThingDef>.AllDefs
			where DebugThingPlaceHelper.IsDebugSpawnable(def, true)
			select def;
			foreach (ThingDef localDef2 in enumerable)
			{
				ThingDef localDef = localDef2;
				list.Add(new DebugMenuOption(localDef.LabelCap, DebugMenuOptionMode.Tool, delegate()
				{
					Thing thing = ThingMaker.MakeThing(localDef, GenStuff.RandomStuffFor(localDef));
					CompQuality compQuality = thing.TryGetComp<CompQuality>();
					if (compQuality != null)
					{
						compQuality.SetQuality(QualityUtility.GenerateQualityRandomEqualChance(), ArtGenerationContext.Colony);
					}
					GenSpawn.Spawn(thing, UI.MouseCell(), Find.CurrentMap, wipeMode);
				}));
			}
			return list;
		}

		[CompilerGenerated]
		private static bool <TryPlaceOptionsForStackCount>m__0(ThingDef def)
		{
			return def.Minifiable;
		}

		[CompilerGenerated]
		private static bool <SpawnOptions>m__1(ThingDef def)
		{
			return DebugThingPlaceHelper.IsDebugSpawnable(def, true);
		}

		[CompilerGenerated]
		private sealed class <TryPlaceOptionsForStackCount>c__AnonStorey0
		{
			internal int stackCount;

			internal bool direct;

			public <TryPlaceOptionsForStackCount>c__AnonStorey0()
			{
			}

			internal bool <>m__0(ThingDef def)
			{
				return DebugThingPlaceHelper.IsDebugSpawnable(def, false) && def.stackLimit >= this.stackCount;
			}
		}

		[CompilerGenerated]
		private sealed class <TryPlaceOptionsForStackCount>c__AnonStorey1
		{
			internal ThingDef localDef;

			internal DebugThingPlaceHelper.<TryPlaceOptionsForStackCount>c__AnonStorey0 <>f__ref$0;

			public <TryPlaceOptionsForStackCount>c__AnonStorey1()
			{
			}

			internal void <>m__0()
			{
				DebugThingPlaceHelper.DebugSpawn(this.localDef, UI.MouseCell(), this.<>f__ref$0.stackCount, this.<>f__ref$0.direct);
			}
		}

		[CompilerGenerated]
		private sealed class <TryPlaceOptionsForStackCount>c__AnonStorey2
		{
			internal ThingDef localDef;

			internal DebugThingPlaceHelper.<TryPlaceOptionsForStackCount>c__AnonStorey0 <>f__ref$0;

			public <TryPlaceOptionsForStackCount>c__AnonStorey2()
			{
			}

			internal void <>m__0()
			{
				DebugThingPlaceHelper.DebugSpawn(this.localDef, UI.MouseCell(), this.<>f__ref$0.stackCount, this.<>f__ref$0.direct);
			}
		}

		[CompilerGenerated]
		private sealed class <SpawnOptions>c__AnonStorey4
		{
			internal WipeMode wipeMode;

			public <SpawnOptions>c__AnonStorey4()
			{
			}
		}

		[CompilerGenerated]
		private sealed class <SpawnOptions>c__AnonStorey3
		{
			internal ThingDef localDef;

			internal DebugThingPlaceHelper.<SpawnOptions>c__AnonStorey4 <>f__ref$4;

			public <SpawnOptions>c__AnonStorey3()
			{
			}

			internal void <>m__0()
			{
				Thing thing = ThingMaker.MakeThing(this.localDef, GenStuff.RandomStuffFor(this.localDef));
				CompQuality compQuality = thing.TryGetComp<CompQuality>();
				if (compQuality != null)
				{
					compQuality.SetQuality(QualityUtility.GenerateQualityRandomEqualChance(), ArtGenerationContext.Colony);
				}
				GenSpawn.Spawn(thing, UI.MouseCell(), Find.CurrentMap, this.<>f__ref$4.wipeMode);
			}
		}
	}
}
