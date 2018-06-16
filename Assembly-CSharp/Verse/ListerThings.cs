using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C30 RID: 3120
	public sealed class ListerThings
	{
		// Token: 0x0600447E RID: 17534 RVA: 0x0023F5E0 File Offset: 0x0023D9E0
		public ListerThings(ListerThingsUse use)
		{
			this.use = use;
			this.listsByGroup = new List<Thing>[ThingListGroupHelper.AllGroups.Length];
			this.listsByGroup[2] = new List<Thing>();
		}

		// Token: 0x17000AB7 RID: 2743
		// (get) Token: 0x0600447F RID: 17535 RVA: 0x0023F634 File Offset: 0x0023DA34
		public List<Thing> AllThings
		{
			get
			{
				return this.listsByGroup[2];
			}
		}

		// Token: 0x06004480 RID: 17536 RVA: 0x0023F654 File Offset: 0x0023DA54
		public List<Thing> ThingsInGroup(ThingRequestGroup group)
		{
			return this.ThingsMatching(ThingRequest.ForGroup(group));
		}

		// Token: 0x06004481 RID: 17537 RVA: 0x0023F678 File Offset: 0x0023DA78
		public List<Thing> ThingsOfDef(ThingDef def)
		{
			return this.ThingsMatching(ThingRequest.ForDef(def));
		}

		// Token: 0x06004482 RID: 17538 RVA: 0x0023F69C File Offset: 0x0023DA9C
		public List<Thing> ThingsMatching(ThingRequest req)
		{
			List<Thing> result;
			if (req.singleDef != null)
			{
				List<Thing> list;
				if (!this.listsByDef.TryGetValue(req.singleDef, out list))
				{
					result = ListerThings.EmptyList;
				}
				else
				{
					result = list;
				}
			}
			else
			{
				if (req.group == ThingRequestGroup.Undefined)
				{
					throw new InvalidOperationException("Invalid ThingRequest " + req);
				}
				if (this.use == ListerThingsUse.Region && !req.group.StoreInRegion())
				{
					Log.ErrorOnce("Tried to get things in group " + req.group + " in a region, but this group is never stored in regions. Most likely a global query should have been used.", 1968735132, false);
					result = ListerThings.EmptyList;
				}
				else
				{
					List<Thing> list2 = this.listsByGroup[(int)req.group];
					result = (list2 ?? ListerThings.EmptyList);
				}
			}
			return result;
		}

		// Token: 0x06004483 RID: 17539 RVA: 0x0023F778 File Offset: 0x0023DB78
		public bool Contains(Thing t)
		{
			return this.AllThings.Contains(t);
		}

		// Token: 0x06004484 RID: 17540 RVA: 0x0023F79C File Offset: 0x0023DB9C
		public void Add(Thing t)
		{
			if (ListerThings.EverListable(t.def, this.use))
			{
				List<Thing> list;
				if (!this.listsByDef.TryGetValue(t.def, out list))
				{
					list = new List<Thing>();
					this.listsByDef.Add(t.def, list);
				}
				list.Add(t);
				foreach (ThingRequestGroup thingRequestGroup in ThingListGroupHelper.AllGroups)
				{
					if (this.use != ListerThingsUse.Region || thingRequestGroup.StoreInRegion())
					{
						if (thingRequestGroup.Includes(t.def))
						{
							List<Thing> list2 = this.listsByGroup[(int)thingRequestGroup];
							if (list2 == null)
							{
								list2 = new List<Thing>();
								this.listsByGroup[(int)thingRequestGroup] = list2;
							}
							list2.Add(t);
						}
					}
				}
			}
		}

		// Token: 0x06004485 RID: 17541 RVA: 0x0023F87C File Offset: 0x0023DC7C
		public void Remove(Thing t)
		{
			if (ListerThings.EverListable(t.def, this.use))
			{
				this.listsByDef[t.def].Remove(t);
				ThingRequestGroup[] allGroups = ThingListGroupHelper.AllGroups;
				for (int i = 0; i < allGroups.Length; i++)
				{
					ThingRequestGroup group = allGroups[i];
					if (this.use != ListerThingsUse.Region || group.StoreInRegion())
					{
						if (group.Includes(t.def))
						{
							this.listsByGroup[i].Remove(t);
						}
					}
				}
			}
		}

		// Token: 0x06004486 RID: 17542 RVA: 0x0023F91C File Offset: 0x0023DD1C
		public static bool EverListable(ThingDef def, ListerThingsUse use)
		{
			return (def.category != ThingCategory.Mote || (def.drawGUIOverlay && use != ListerThingsUse.Region)) && (def.category != ThingCategory.Projectile || use != ListerThingsUse.Region) && def.category != ThingCategory.Gas;
		}

		// Token: 0x06004487 RID: 17543 RVA: 0x0023F988 File Offset: 0x0023DD88
		public void Clear()
		{
			this.listsByDef.Clear();
			for (int i = 0; i < this.listsByGroup.Length; i++)
			{
				if (this.listsByGroup[i] != null)
				{
					this.listsByGroup[i].Clear();
				}
			}
		}

		// Token: 0x04002E7A RID: 11898
		private Dictionary<ThingDef, List<Thing>> listsByDef = new Dictionary<ThingDef, List<Thing>>(ThingDefComparer.Instance);

		// Token: 0x04002E7B RID: 11899
		private List<Thing>[] listsByGroup;

		// Token: 0x04002E7C RID: 11900
		public ListerThingsUse use = ListerThingsUse.Undefined;

		// Token: 0x04002E7D RID: 11901
		private static readonly List<Thing> EmptyList = new List<Thing>();
	}
}
