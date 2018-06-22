using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000E25 RID: 3621
	public static class DebugThingPlaceHelper
	{
		// Token: 0x0600550A RID: 21770 RVA: 0x002BA5AC File Offset: 0x002B89AC
		public static bool IsDebugSpawnable(ThingDef def, bool allowPlayerBuildable = false)
		{
			return def.forceDebugSpawnable || (def.thingClass != typeof(Corpse) && !def.IsBlueprint && !def.IsFrame && def != ThingDefOf.ActiveDropPod && def.thingClass != typeof(MinifiedThing) && def.thingClass != typeof(UnfinishedThing) && !def.destroyOnDrop && (def.category == ThingCategory.Filth || def.category == ThingCategory.Item || def.category == ThingCategory.Plant || def.category == ThingCategory.Ethereal || (def.category == ThingCategory.Building && def.building.isNaturalRock) || (def.category == ThingCategory.Building && !def.BuildableByPlayer) || (def.category == ThingCategory.Building && def.BuildableByPlayer && allowPlayerBuildable)));
		}

		// Token: 0x0600550B RID: 21771 RVA: 0x002BA6E4 File Offset: 0x002B8AE4
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

		// Token: 0x0600550C RID: 21772 RVA: 0x002BA774 File Offset: 0x002B8B74
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

		// Token: 0x0600550D RID: 21773 RVA: 0x002BA904 File Offset: 0x002B8D04
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
	}
}
