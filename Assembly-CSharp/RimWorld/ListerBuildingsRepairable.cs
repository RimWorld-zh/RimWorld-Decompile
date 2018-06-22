using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000387 RID: 903
	public class ListerBuildingsRepairable
	{
		// Token: 0x06000FA3 RID: 4003 RVA: 0x00083E78 File Offset: 0x00082278
		public List<Thing> RepairableBuildings(Faction fac)
		{
			return this.ListFor(fac);
		}

		// Token: 0x06000FA4 RID: 4004 RVA: 0x00083E94 File Offset: 0x00082294
		public bool Contains(Faction fac, Building b)
		{
			return this.HashSetFor(fac).Contains(b);
		}

		// Token: 0x06000FA5 RID: 4005 RVA: 0x00083EB6 File Offset: 0x000822B6
		public void Notify_BuildingSpawned(Building b)
		{
			if (b.Faction != null)
			{
				this.UpdateBuilding(b);
			}
		}

		// Token: 0x06000FA6 RID: 4006 RVA: 0x00083ED0 File Offset: 0x000822D0
		public void Notify_BuildingDeSpawned(Building b)
		{
			if (b.Faction != null)
			{
				this.ListFor(b.Faction).Remove(b);
				this.HashSetFor(b.Faction).Remove(b);
			}
		}

		// Token: 0x06000FA7 RID: 4007 RVA: 0x00083F09 File Offset: 0x00082309
		public void Notify_BuildingTookDamage(Building b)
		{
			if (b.Faction != null)
			{
				this.UpdateBuilding(b);
			}
		}

		// Token: 0x06000FA8 RID: 4008 RVA: 0x00083F23 File Offset: 0x00082323
		public void Notify_BuildingRepaired(Building b)
		{
			if (b.Faction != null)
			{
				this.UpdateBuilding(b);
			}
		}

		// Token: 0x06000FA9 RID: 4009 RVA: 0x00083F40 File Offset: 0x00082340
		private void UpdateBuilding(Building b)
		{
			if (b.Faction != null && b.def.building.repairable)
			{
				List<Thing> list = this.ListFor(b.Faction);
				HashSet<Thing> hashSet = this.HashSetFor(b.Faction);
				if (b.HitPoints < b.MaxHitPoints)
				{
					if (!list.Contains(b))
					{
						list.Add(b);
					}
					hashSet.Add(b);
				}
				else
				{
					list.Remove(b);
					hashSet.Remove(b);
				}
			}
		}

		// Token: 0x06000FAA RID: 4010 RVA: 0x00083FD4 File Offset: 0x000823D4
		private List<Thing> ListFor(Faction fac)
		{
			List<Thing> list;
			if (!this.repairables.TryGetValue(fac, out list))
			{
				list = new List<Thing>();
				this.repairables.Add(fac, list);
			}
			return list;
		}

		// Token: 0x06000FAB RID: 4011 RVA: 0x00084014 File Offset: 0x00082414
		private HashSet<Thing> HashSetFor(Faction fac)
		{
			HashSet<Thing> hashSet;
			if (!this.repairablesSet.TryGetValue(fac, out hashSet))
			{
				hashSet = new HashSet<Thing>();
				this.repairablesSet.Add(fac, hashSet);
			}
			return hashSet;
		}

		// Token: 0x06000FAC RID: 4012 RVA: 0x00084054 File Offset: 0x00082454
		internal string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				List<Thing> list = this.ListFor(faction);
				if (!list.NullOrEmpty<Thing>())
				{
					stringBuilder.AppendLine(string.Concat(new object[]
					{
						"=======",
						faction.Name,
						" (",
						faction.def,
						")"
					}));
					foreach (Thing thing in list)
					{
						stringBuilder.AppendLine(thing.ThingID);
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04000996 RID: 2454
		private Dictionary<Faction, List<Thing>> repairables = new Dictionary<Faction, List<Thing>>();

		// Token: 0x04000997 RID: 2455
		private Dictionary<Faction, HashSet<Thing>> repairablesSet = new Dictionary<Faction, HashSet<Thing>>();
	}
}
