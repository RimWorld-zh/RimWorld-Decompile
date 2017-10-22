using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	public class ListerBuildingsRepairable
	{
		private Dictionary<Faction, List<Thing>> repairables = new Dictionary<Faction, List<Thing>>();

		private Dictionary<Faction, HashSet<Thing>> repairablesSet = new Dictionary<Faction, HashSet<Thing>>();

		public List<Thing> RepairableBuildings(Faction fac)
		{
			return this.ListFor(fac);
		}

		public bool Contains(Faction fac, Building b)
		{
			return this.HashSetFor(fac).Contains(b);
		}

		public void Notify_BuildingSpawned(Building b)
		{
			if (b.Faction != null)
			{
				this.UpdateBuilding(b);
			}
		}

		public void Notify_BuildingDeSpawned(Building b)
		{
			if (b.Faction != null)
			{
				this.ListFor(b.Faction).Remove(b);
				this.HashSetFor(b.Faction).Remove(b);
			}
		}

		public void Notify_BuildingTookDamage(Building b)
		{
			if (b.Faction != null)
			{
				this.UpdateBuilding(b);
			}
		}

		public void Notify_BuildingRepaired(Building b)
		{
			if (b.Faction != null)
			{
				this.UpdateBuilding(b);
			}
		}

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

		private List<Thing> ListFor(Faction fac)
		{
			List<Thing> list = default(List<Thing>);
			if (!this.repairables.TryGetValue(fac, out list))
			{
				list = new List<Thing>();
				this.repairables.Add(fac, list);
			}
			return list;
		}

		private HashSet<Thing> HashSetFor(Faction fac)
		{
			HashSet<Thing> hashSet = default(HashSet<Thing>);
			if (!this.repairablesSet.TryGetValue(fac, out hashSet))
			{
				hashSet = new HashSet<Thing>();
				this.repairablesSet.Add(fac, hashSet);
			}
			return hashSet;
		}

		internal string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Faction allFaction in Find.FactionManager.AllFactions)
			{
				List<Thing> list = this.ListFor(allFaction);
				if (!list.NullOrEmpty())
				{
					stringBuilder.AppendLine("=======" + allFaction.Name + " (" + allFaction.def + ")");
					foreach (Thing item in list)
					{
						stringBuilder.AppendLine(item.ThingID);
					}
				}
			}
			return stringBuilder.ToString();
		}
	}
}
