using System;
using System.Collections.Generic;

namespace Verse
{
	public sealed class ListerThings
	{
		private Dictionary<ThingDef, List<Thing>> listsByDef = new Dictionary<ThingDef, List<Thing>>(ThingDefComparer.Instance);

		private List<Thing>[] listsByGroup;

		public ListerThingsUse use;

		private static readonly List<Thing> EmptyList = new List<Thing>();

		public List<Thing> AllThings
		{
			get
			{
				return this.listsByGroup[2];
			}
		}

		public ListerThings(ListerThingsUse use)
		{
			this.use = use;
			this.listsByGroup = new List<Thing>[ThingListGroupHelper.AllGroups.Length];
			this.listsByGroup[2] = new List<Thing>();
		}

		public List<Thing> ThingsInGroup(ThingRequestGroup group)
		{
			return this.ThingsMatching(ThingRequest.ForGroup(group));
		}

		public List<Thing> ThingsOfDef(ThingDef def)
		{
			return this.ThingsMatching(ThingRequest.ForDef(def));
		}

		public List<Thing> ThingsMatching(ThingRequest req)
		{
			if (req.singleDef != null)
			{
				List<Thing> result = default(List<Thing>);
				if (!this.listsByDef.TryGetValue(req.singleDef, out result))
				{
					return ListerThings.EmptyList;
				}
				return result;
			}
			if (req.group != 0)
			{
				List<Thing> list = this.listsByGroup[(uint)req.group];
				return list ?? ListerThings.EmptyList;
			}
			throw new InvalidOperationException("Invalid ThingRequest " + req);
		}

		public bool Contains(Thing t)
		{
			return this.AllThings.Contains(t);
		}

		public void Add(Thing t)
		{
			if (ListerThings.EverListable(t.def, this.use))
			{
				List<Thing> list = default(List<Thing>);
				if (!this.listsByDef.TryGetValue(t.def, out list))
				{
					list = new List<Thing>();
					this.listsByDef.Add(t.def, list);
				}
				list.Add(t);
				ThingRequestGroup[] allGroups = ThingListGroupHelper.AllGroups;
				foreach (ThingRequestGroup thingRequestGroup in allGroups)
				{
					if ((this.use != ListerThingsUse.Region || thingRequestGroup.StoreInRegion()) && thingRequestGroup.Includes(t.def))
					{
						List<Thing> list2 = this.listsByGroup[(uint)thingRequestGroup];
						if (list2 == null)
						{
							list2 = new List<Thing>();
							this.listsByGroup[(uint)thingRequestGroup] = list2;
						}
						list2.Add(t);
					}
				}
			}
		}

		public void Remove(Thing t)
		{
			if (ListerThings.EverListable(t.def, this.use))
			{
				this.listsByDef[t.def].Remove(t);
				ThingRequestGroup[] allGroups = ThingListGroupHelper.AllGroups;
				for (int i = 0; i < allGroups.Length; i++)
				{
					ThingRequestGroup group = allGroups[i];
					if ((this.use != ListerThingsUse.Region || group.StoreInRegion()) && group.Includes(t.def))
					{
						this.listsByGroup[i].Remove(t);
					}
				}
			}
		}

		public static bool EverListable(ThingDef def, ListerThingsUse use)
		{
			if (def.category == ThingCategory.Mote && (!def.drawGUIOverlay || use == ListerThingsUse.Region))
			{
				return false;
			}
			if (def.category == ThingCategory.Projectile && use == ListerThingsUse.Region)
			{
				return false;
			}
			if (def.category == ThingCategory.Gas)
			{
				return false;
			}
			return true;
		}
	}
}
