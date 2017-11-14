using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public sealed class ResourceCounter
	{
		private Map map;

		private Dictionary<ThingDef, int> countedAmounts = new Dictionary<ThingDef, int>();

		private static List<ThingDef> resources = new List<ThingDef>();

		public int Silver
		{
			get
			{
				return this.GetCount(ThingDefOf.Silver);
			}
		}

		public int Metal
		{
			get
			{
				return this.GetCount(ThingDefOf.Steel);
			}
		}

		public float TotalHumanEdibleNutrition
		{
			get
			{
				float num = 0f;
				foreach (KeyValuePair<ThingDef, int> countedAmount in this.countedAmounts)
				{
					if (countedAmount.Key.IsNutritionGivingIngestible && countedAmount.Key.ingestible.HumanEdible)
					{
						num += countedAmount.Key.ingestible.nutrition * (float)countedAmount.Value;
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

		public ResourceCounter(Map map)
		{
			this.map = map;
			this.ResetResourceCounts();
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
			if (rDef.resourceReadoutPriority == ResourceCountPriority.Uncounted)
			{
				return 0;
			}
			int result = default(int);
			if (this.countedAmounts.TryGetValue(rDef, out result))
			{
				return result;
			}
			Log.Error("Looked for nonexistent key " + rDef + " in counted resources.");
			this.countedAmounts.Add(rDef, 0);
			return 0;
		}

		public int GetCountIn(ThingRequestGroup group)
		{
			int num = 0;
			foreach (KeyValuePair<ThingDef, int> countedAmount in this.countedAmounts)
			{
				if (group.Includes(countedAmount.Key))
				{
					num += countedAmount.Value;
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
			List<SlotGroup> allGroupsListForReading = this.map.slotGroupManager.AllGroupsListForReading;
			for (int i = 0; i < allGroupsListForReading.Count; i++)
			{
				SlotGroup slotGroup = allGroupsListForReading[i];
				foreach (Thing heldThing in slotGroup.HeldThings)
				{
					if (heldThing.def.CountAsResource && this.ShouldCount(heldThing))
					{
						Dictionary<ThingDef, int> dictionary;
						ThingDef def;
						(dictionary = this.countedAmounts)[def = heldThing.def] = dictionary[def] + heldThing.stackCount;
					}
				}
			}
		}

		private bool ShouldCount(Thing t)
		{
			if (t.IsNotFresh())
			{
				return false;
			}
			return true;
		}
	}
}
