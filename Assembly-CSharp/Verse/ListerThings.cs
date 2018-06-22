using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C2C RID: 3116
	public sealed class ListerThings
	{
		// Token: 0x06004485 RID: 17541 RVA: 0x00240980 File Offset: 0x0023ED80
		public ListerThings(ListerThingsUse use)
		{
			this.use = use;
			this.listsByGroup = new List<Thing>[ThingListGroupHelper.AllGroups.Length];
			this.listsByGroup[2] = new List<Thing>();
		}

		// Token: 0x17000AB8 RID: 2744
		// (get) Token: 0x06004486 RID: 17542 RVA: 0x002409D4 File Offset: 0x0023EDD4
		public List<Thing> AllThings
		{
			get
			{
				return this.listsByGroup[2];
			}
		}

		// Token: 0x06004487 RID: 17543 RVA: 0x002409F4 File Offset: 0x0023EDF4
		public List<Thing> ThingsInGroup(ThingRequestGroup group)
		{
			return this.ThingsMatching(ThingRequest.ForGroup(group));
		}

		// Token: 0x06004488 RID: 17544 RVA: 0x00240A18 File Offset: 0x0023EE18
		public List<Thing> ThingsOfDef(ThingDef def)
		{
			return this.ThingsMatching(ThingRequest.ForDef(def));
		}

		// Token: 0x06004489 RID: 17545 RVA: 0x00240A3C File Offset: 0x0023EE3C
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

		// Token: 0x0600448A RID: 17546 RVA: 0x00240B18 File Offset: 0x0023EF18
		public bool Contains(Thing t)
		{
			return this.AllThings.Contains(t);
		}

		// Token: 0x0600448B RID: 17547 RVA: 0x00240B3C File Offset: 0x0023EF3C
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

		// Token: 0x0600448C RID: 17548 RVA: 0x00240C1C File Offset: 0x0023F01C
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

		// Token: 0x0600448D RID: 17549 RVA: 0x00240CBC File Offset: 0x0023F0BC
		public static bool EverListable(ThingDef def, ListerThingsUse use)
		{
			return (def.category != ThingCategory.Mote || (def.drawGUIOverlay && use != ListerThingsUse.Region)) && (def.category != ThingCategory.Projectile || use != ListerThingsUse.Region) && def.category != ThingCategory.Gas;
		}

		// Token: 0x0600448E RID: 17550 RVA: 0x00240D28 File Offset: 0x0023F128
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

		// Token: 0x04002E82 RID: 11906
		private Dictionary<ThingDef, List<Thing>> listsByDef = new Dictionary<ThingDef, List<Thing>>(ThingDefComparer.Instance);

		// Token: 0x04002E83 RID: 11907
		private List<Thing>[] listsByGroup;

		// Token: 0x04002E84 RID: 11908
		public ListerThingsUse use = ListerThingsUse.Undefined;

		// Token: 0x04002E85 RID: 11909
		private static readonly List<Thing> EmptyList = new List<Thing>();
	}
}
