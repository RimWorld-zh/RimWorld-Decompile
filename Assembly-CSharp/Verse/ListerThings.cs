using System;
using System.Collections.Generic;

namespace Verse
{
	public sealed class ListerThings
	{
		private Dictionary<ThingDef, List<Thing>> listsByDef = new Dictionary<ThingDef, List<Thing>>(ThingDefComparer.Instance);

		private List<Thing>[] listsByGroup;

		public ListerThingsUse use = ListerThingsUse.Undefined;

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
			List<Thing> result;
			if (req.singleDef != null)
			{
				List<Thing> list = default(List<Thing>);
				result = (this.listsByDef.TryGetValue(req.singleDef, out list) ? list : ListerThings.EmptyList);
				goto IL_007e;
			}
			if (req.group != 0)
			{
				List<Thing> list2 = this.listsByGroup[(uint)req.group];
				result = (list2 ?? ListerThings.EmptyList);
				goto IL_007e;
			}
			throw new InvalidOperationException("Invalid ThingRequest " + req);
			IL_007e:
			return result;
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
				for (int i = 0; i < allGroups.Length; i++)
				{
					ThingRequestGroup thingRequestGroup = allGroups[i];
					if ((this.use != ListerThingsUse.Region || thingRequestGroup.StoreInRegion()) && thingRequestGroup.Includes(t.def))
					{
						List<Thing> list2 = this.listsByGroup[(uint)thingRequestGroup];
						if (list2 == null)
						{
							list2 = (this.listsByGroup[(uint)thingRequestGroup] = new List<Thing>());
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
			return (byte)((def.category != ThingCategory.Mote || (def.drawGUIOverlay && use != ListerThingsUse.Region)) ? ((def.category != ThingCategory.Projectile || use != ListerThingsUse.Region) ? ((def.category != ThingCategory.Gas) ? 1 : 0) : 0) : 0) != 0;
		}
	}
}
