using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200042D RID: 1069
	public sealed class ResourceCounter
	{
		// Token: 0x04000B65 RID: 2917
		private Map map;

		// Token: 0x04000B66 RID: 2918
		private Dictionary<ThingDef, int> countedAmounts = new Dictionary<ThingDef, int>();

		// Token: 0x04000B67 RID: 2919
		private static List<ThingDef> resources = new List<ThingDef>();

		// Token: 0x060012A9 RID: 4777 RVA: 0x000A1F77 File Offset: 0x000A0377
		public ResourceCounter(Map map)
		{
			this.map = map;
			this.ResetResourceCounts();
		}

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x060012AA RID: 4778 RVA: 0x000A1F98 File Offset: 0x000A0398
		public int Silver
		{
			get
			{
				return this.GetCount(ThingDefOf.Silver);
			}
		}

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x060012AB RID: 4779 RVA: 0x000A1FB8 File Offset: 0x000A03B8
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

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x060012AC RID: 4780 RVA: 0x000A2064 File Offset: 0x000A0464
		public Dictionary<ThingDef, int> AllCountedAmounts
		{
			get
			{
				return this.countedAmounts;
			}
		}

		// Token: 0x060012AD RID: 4781 RVA: 0x000A2080 File Offset: 0x000A0480
		public static void ResetDefs()
		{
			ResourceCounter.resources.Clear();
			ResourceCounter.resources.AddRange(from def in DefDatabase<ThingDef>.AllDefs
			where def.CountAsResource
			orderby def.resourceReadoutPriority descending
			select def);
		}

		// Token: 0x060012AE RID: 4782 RVA: 0x000A20EC File Offset: 0x000A04EC
		public void ResetResourceCounts()
		{
			this.countedAmounts.Clear();
			for (int i = 0; i < ResourceCounter.resources.Count; i++)
			{
				this.countedAmounts.Add(ResourceCounter.resources[i], 0);
			}
		}

		// Token: 0x060012AF RID: 4783 RVA: 0x000A213C File Offset: 0x000A053C
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

		// Token: 0x060012B0 RID: 4784 RVA: 0x000A21A4 File Offset: 0x000A05A4
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

		// Token: 0x060012B1 RID: 4785 RVA: 0x000A2224 File Offset: 0x000A0624
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

		// Token: 0x060012B2 RID: 4786 RVA: 0x000A22B8 File Offset: 0x000A06B8
		public void ResourceCounterTick()
		{
			if (Find.TickManager.TicksGame % 204 == 0)
			{
				this.UpdateResourceCounts();
			}
		}

		// Token: 0x060012B3 RID: 4787 RVA: 0x000A22D8 File Offset: 0x000A06D8
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

		// Token: 0x060012B4 RID: 4788 RVA: 0x000A23BC File Offset: 0x000A07BC
		private bool ShouldCount(Thing t)
		{
			return !t.IsNotFresh();
		}
	}
}
