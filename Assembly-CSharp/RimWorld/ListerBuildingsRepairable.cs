using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000387 RID: 903
	public class ListerBuildingsRepairable
	{
		// Token: 0x06000FA3 RID: 4003 RVA: 0x00083C8C File Offset: 0x0008208C
		public List<Thing> RepairableBuildings(Faction fac)
		{
			return this.ListFor(fac);
		}

		// Token: 0x06000FA4 RID: 4004 RVA: 0x00083CA8 File Offset: 0x000820A8
		public bool Contains(Faction fac, Building b)
		{
			return this.HashSetFor(fac).Contains(b);
		}

		// Token: 0x06000FA5 RID: 4005 RVA: 0x00083CCA File Offset: 0x000820CA
		public void Notify_BuildingSpawned(Building b)
		{
			if (b.Faction != null)
			{
				this.UpdateBuilding(b);
			}
		}

		// Token: 0x06000FA6 RID: 4006 RVA: 0x00083CE4 File Offset: 0x000820E4
		public void Notify_BuildingDeSpawned(Building b)
		{
			if (b.Faction != null)
			{
				this.ListFor(b.Faction).Remove(b);
				this.HashSetFor(b.Faction).Remove(b);
			}
		}

		// Token: 0x06000FA7 RID: 4007 RVA: 0x00083D1D File Offset: 0x0008211D
		public void Notify_BuildingTookDamage(Building b)
		{
			if (b.Faction != null)
			{
				this.UpdateBuilding(b);
			}
		}

		// Token: 0x06000FA8 RID: 4008 RVA: 0x00083D37 File Offset: 0x00082137
		public void Notify_BuildingRepaired(Building b)
		{
			if (b.Faction != null)
			{
				this.UpdateBuilding(b);
			}
		}

		// Token: 0x06000FA9 RID: 4009 RVA: 0x00083D54 File Offset: 0x00082154
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

		// Token: 0x06000FAA RID: 4010 RVA: 0x00083DE8 File Offset: 0x000821E8
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

		// Token: 0x06000FAB RID: 4011 RVA: 0x00083E28 File Offset: 0x00082228
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

		// Token: 0x06000FAC RID: 4012 RVA: 0x00083E68 File Offset: 0x00082268
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

		// Token: 0x04000994 RID: 2452
		private Dictionary<Faction, List<Thing>> repairables = new Dictionary<Faction, List<Thing>>();

		// Token: 0x04000995 RID: 2453
		private Dictionary<Faction, HashSet<Thing>> repairablesSet = new Dictionary<Faction, HashSet<Thing>>();
	}
}
