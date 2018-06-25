using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public sealed class ResourceCounter
	{
		private Map map;

		private Dictionary<ThingDef, int> countedAmounts = new Dictionary<ThingDef, int>();

		private static List<ThingDef> resources = new List<ThingDef>();

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<ThingDef, ResourceCountPriority> <>f__am$cache1;

		public ResourceCounter(Map map)
		{
			this.map = map;
			this.ResetResourceCounts();
		}

		public int Silver
		{
			get
			{
				return this.GetCount(ThingDefOf.Silver);
			}
		}

		public float TotalHumanEdibleNutrition
		{
			get
			{
				float num = 0f;
				foreach (KeyValuePair<ThingDef, int> keyValuePair in this.countedAmounts)
				{
					if (keyValuePair.Key.IsNutritionGivingIngestible && keyValuePair.Key.ingestible.HumanEdible)
					{
						num += keyValuePair.Key.GetStatValueAbstract(StatDefOf.Nutrition, null) * (float)keyValuePair.Value;
					}
				}
				return num;
			}
		}

		public Dictionary<ThingDef, int> AllCountedAmounts
		{
			get
			{
				return this.countedAmounts;
			}
		}

		public static void ResetDefs()
		{
			ResourceCounter.resources.Clear();
			ResourceCounter.resources.AddRange(from def in DefDatabase<ThingDef>.AllDefs
			where def.CountAsResource
			orderby def.resourceReadoutPriority descending
			select def);
		}

		public void ResetResourceCounts()
		{
			this.countedAmounts.Clear();
			for (int i = 0; i < ResourceCounter.resources.Count; i++)
			{
				this.countedAmounts.Add(ResourceCounter.resources[i], 0);
			}
		}

		public int GetCount(ThingDef rDef)
		{
			int result;
			int num;
			if (rDef.resourceReadoutPriority == ResourceCountPriority.Uncounted)
			{
				result = 0;
			}
			else if (this.countedAmounts.TryGetValue(rDef, out num))
			{
				result = num;
			}
			else
			{
				Log.Error("Looked for nonexistent key " + rDef + " in counted resources.", false);
				this.countedAmounts.Add(rDef, 0);
				result = 0;
			}
			return result;
		}

		public int GetCountIn(ThingRequestGroup group)
		{
			int num = 0;
			foreach (KeyValuePair<ThingDef, int> keyValuePair in this.countedAmounts)
			{
				if (group.Includes(keyValuePair.Key))
				{
					num += keyValuePair.Value;
				}
			}
			return num;
		}

		public int GetCountIn(ThingCategoryDef cat)
		{
			int num = 0;
			for (int i = 0; i < cat.childThingDefs.Count; i++)
			{
				num += this.GetCount(cat.childThingDefs[i]);
			}
			for (int j = 0; j < cat.childCategories.Count; j++)
			{
				if (!cat.childCategories[j].resourceReadoutRoot)
				{
					num += this.GetCountIn(cat.childCategories[j]);
				}
			}
			return num;
		}

		public void ResourceCounterTick()
		{
			if (Find.TickManager.TicksGame % 204 == 0)
			{
				this.UpdateResourceCounts();
			}
		}

		public void UpdateResourceCounts()
		{
			this.ResetResourceCounts();
			List<SlotGroup> allGroupsListForReading = this.map.haulDestinationManager.AllGroupsListForReading;
			for (int i = 0; i < allGroupsListForReading.Count; i++)
			{
				SlotGroup slotGroup = allGroupsListForReading[i];
				foreach (Thing outerThing in slotGroup.HeldThings)
				{
					Thing innerIfMinified = outerThing.GetInnerIfMinified();
					if (innerIfMinified.def.CountAsResource && this.ShouldCount(innerIfMinified))
					{
						Dictionary<ThingDef, int> dictionary;
						ThingDef def;
						(dictionary = this.countedAmounts)[def = innerIfMinified.def] = dictionary[def] + innerIfMinified.stackCount;
					}
				}
			}
		}

		private bool ShouldCount(Thing t)
		{
			return !t.IsNotFresh();
		}

		// Note: this type is marked as 'beforefieldinit'.
		static ResourceCounter()
		{
		}

		[CompilerGenerated]
		private static bool <ResetDefs>m__0(ThingDef def)
		{
			return def.CountAsResource;
		}

		[CompilerGenerated]
		private static ResourceCountPriority <ResetDefs>m__1(ThingDef def)
		{
			return def.resourceReadoutPriority;
		}
	}
}
