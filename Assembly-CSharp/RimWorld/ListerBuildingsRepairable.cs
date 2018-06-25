using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000389 RID: 905
	public class ListerBuildingsRepairable
	{
		// Token: 0x04000996 RID: 2454
		private Dictionary<Faction, List<Thing>> repairables = new Dictionary<Faction, List<Thing>>();

		// Token: 0x04000997 RID: 2455
		private Dictionary<Faction, HashSet<Thing>> repairablesSet = new Dictionary<Faction, HashSet<Thing>>();

		// Token: 0x06000FA7 RID: 4007 RVA: 0x00083FC8 File Offset: 0x000823C8
		public List<Thing> RepairableBuildings(Faction fac)
		{
			return this.ListFor(fac);
		}

		// Token: 0x06000FA8 RID: 4008 RVA: 0x00083FE4 File Offset: 0x000823E4
		public bool Contains(Faction fac, Building b)
		{
			return this.HashSetFor(fac).Contains(b);
		}

		// Token: 0x06000FA9 RID: 4009 RVA: 0x00084006 File Offset: 0x00082406
		public void Notify_BuildingSpawned(Building b)
		{
			if (b.Faction != null)
			{
				this.UpdateBuilding(b);
			}
		}

		// Token: 0x06000FAA RID: 4010 RVA: 0x00084020 File Offset: 0x00082420
		public void Notify_BuildingDeSpawned(Building b)
		{
			if (b.Faction != null)
			{
				this.ListFor(b.Faction).Remove(b);
				this.HashSetFor(b.Faction).Remove(b);
			}
		}

		// Token: 0x06000FAB RID: 4011 RVA: 0x00084059 File Offset: 0x00082459
		public void Notify_BuildingTookDamage(Building b)
		{
			if (b.Faction != null)
			{
				this.UpdateBuilding(b);
			}
		}

		// Token: 0x06000FAC RID: 4012 RVA: 0x00084073 File Offset: 0x00082473
		public void Notify_BuildingRepaired(Building b)
		{
			if (b.Faction != null)
			{
				this.UpdateBuilding(b);
			}
		}

		// Token: 0x06000FAD RID: 4013 RVA: 0x00084090 File Offset: 0x00082490
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

		// Token: 0x06000FAE RID: 4014 RVA: 0x00084124 File Offset: 0x00082524
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

		// Token: 0x06000FAF RID: 4015 RVA: 0x00084164 File Offset: 0x00082564
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

		// Token: 0x06000FB0 RID: 4016 RVA: 0x000841A4 File Offset: 0x000825A4
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
	}
}
