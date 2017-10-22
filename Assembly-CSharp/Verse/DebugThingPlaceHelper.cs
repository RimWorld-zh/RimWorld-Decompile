using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	public static class DebugThingPlaceHelper
	{
		public static bool IsDebugSpawnable(ThingDef def)
		{
			return (byte)(def.forceDebugSpawnable ? 1 : ((def.thingClass != typeof(Corpse) && !def.IsBlueprint && !def.IsFrame && def != ThingDefOf.ActiveDropPod && def.thingClass != typeof(MinifiedThing) && def.thingClass != typeof(UnfinishedThing) && !def.destroyOnDrop) ? ((def.category == ThingCategory.Filth || def.category == ThingCategory.Item || def.category == ThingCategory.Plant || def.category == ThingCategory.Ethereal) ? 1 : ((def.category == ThingCategory.Building && def.building.isNaturalRock) ? 1 : ((def.category == ThingCategory.Building && def.designationCategory == null) ? 1 : 0))) : 0)) != 0;
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
				compQuality.SetQuality(QualityUtility.RandomQuality(), ArtGenerationContext.Colony);
			}
			if (thing.def.Minifiable)
			{
				thing = thing.MakeMinified();
			}
			thing.stackCount = stackCount;
			if (direct)
			{
				GenPlace.TryPlaceThing(thing, c, Find.VisibleMap, ThingPlaceMode.Direct, null);
			}
			else
			{
				GenPlace.TryPlaceThing(thing, c, Find.VisibleMap, ThingPlaceMode.Near, null);
			}
		}

		public static List<DebugMenuOption> TryPlaceOptionsForStackCount(int stackCount, bool direct)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			IEnumerable<ThingDef> enumerable = from def in DefDatabase<ThingDef>.AllDefs
			where DebugThingPlaceHelper.IsDebugSpawnable(def) && def.stackLimit >= stackCount
			select def;
			foreach (ThingDef item in enumerable)
			{
				ThingDef localDef = item;
				list.Add(new DebugMenuOption(localDef.LabelCap, DebugMenuOptionMode.Tool, (Action)delegate()
				{
					DebugThingPlaceHelper.DebugSpawn(localDef, UI.MouseCell(), stackCount, direct);
				}));
			}
			if (stackCount == 1)
			{
				foreach (ThingDef item2 in from def in DefDatabase<ThingDef>.AllDefs
				where def.Minifiable
				select def)
				{
					ThingDef localDef2 = item2;
					list.Add(new DebugMenuOption(localDef2.LabelCap + " (minified)", DebugMenuOptionMode.Tool, (Action)delegate()
					{
						DebugThingPlaceHelper.DebugSpawn(localDef2, UI.MouseCell(), stackCount, direct);
					}));
				}
			}
			return list;
		}
	}
}
