using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C2F RID: 3119
	public sealed class ListerThings
	{
		// Token: 0x04002E89 RID: 11913
		private Dictionary<ThingDef, List<Thing>> listsByDef = new Dictionary<ThingDef, List<Thing>>(ThingDefComparer.Instance);

		// Token: 0x04002E8A RID: 11914
		private List<Thing>[] listsByGroup;

		// Token: 0x04002E8B RID: 11915
		public ListerThingsUse use = ListerThingsUse.Undefined;

		// Token: 0x04002E8C RID: 11916
		private static readonly List<Thing> EmptyList = new List<Thing>();

		// Token: 0x06004488 RID: 17544 RVA: 0x00240D3C File Offset: 0x0023F13C
		public ListerThings(ListerThingsUse use)
		{
			this.use = use;
			this.listsByGroup = new List<Thing>[ThingListGroupHelper.AllGroups.Length];
			this.listsByGroup[2] = new List<Thing>();
		}

		// Token: 0x17000AB7 RID: 2743
		// (get) Token: 0x06004489 RID: 17545 RVA: 0x00240D90 File Offset: 0x0023F190
		public List<Thing> AllThings
		{
			get
			{
				return this.listsByGroup[2];
			}
		}

		// Token: 0x0600448A RID: 17546 RVA: 0x00240DB0 File Offset: 0x0023F1B0
		public List<Thing> ThingsInGroup(ThingRequestGroup group)
		{
			return this.ThingsMatching(ThingRequest.ForGroup(group));
		}

		// Token: 0x0600448B RID: 17547 RVA: 0x00240DD4 File Offset: 0x0023F1D4
		public List<Thing> ThingsOfDef(ThingDef def)
		{
			return this.ThingsMatching(ThingRequest.ForDef(def));
		}

		// Token: 0x0600448C RID: 17548 RVA: 0x00240DF8 File Offset: 0x0023F1F8
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

		// Token: 0x0600448D RID: 17549 RVA: 0x00240ED4 File Offset: 0x0023F2D4
		public bool Contains(Thing t)
		{
			return this.AllThings.Contains(t);
		}

		// Token: 0x0600448E RID: 17550 RVA: 0x00240EF8 File Offset: 0x0023F2F8
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

		// Token: 0x0600448F RID: 17551 RVA: 0x00240FD8 File Offset: 0x0023F3D8
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

		// Token: 0x06004490 RID: 17552 RVA: 0x00241078 File Offset: 0x0023F478
		public static bool EverListable(ThingDef def, ListerThingsUse use)
		{
			return (def.category != ThingCategory.Mote || (def.drawGUIOverlay && use != ListerThingsUse.Region)) && (def.category != ThingCategory.Projectile || use != ListerThingsUse.Region) && def.category != ThingCategory.Gas;
		}

		// Token: 0x06004491 RID: 17553 RVA: 0x002410E4 File Offset: 0x0023F4E4
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
	}
}
