using RimWorld;
using System;
using System.Collections.Generic;
using Verse.AI;

namespace Verse
{
	public sealed class ListerBuildings
	{
		public List<Building> allBuildingsColonist = new List<Building>();

		public HashSet<Building> allBuildingsColonistCombatTargets = new HashSet<Building>();

		public HashSet<Building> allBuildingsColonistElecFire = new HashSet<Building>();

		public void Add(Building b)
		{
			if (b.def.building != null && b.def.building.isNaturalRock)
				return;
			if (b.Faction == Faction.OfPlayer)
			{
				this.allBuildingsColonist.Add(b);
				if (b is IAttackTarget)
				{
					this.allBuildingsColonistCombatTargets.Add(b);
				}
			}
			CompProperties_Power compProperties = b.def.GetCompProperties<CompProperties_Power>();
			if (compProperties != null && compProperties.startElectricalFires)
			{
				this.allBuildingsColonistElecFire.Add(b);
			}
		}

		public void Remove(Building b)
		{
			this.allBuildingsColonist.Remove(b);
			if (b is IAttackTarget)
			{
				this.allBuildingsColonistCombatTargets.Remove(b);
			}
			CompProperties_Power compProperties = b.def.GetCompProperties<CompProperties_Power>();
			if (compProperties != null && compProperties.startElectricalFires)
			{
				this.allBuildingsColonistElecFire.Remove(b);
			}
		}

		public bool ColonistsHaveBuilding(ThingDef def)
		{
			for (int i = 0; i < this.allBuildingsColonist.Count; i++)
			{
				if (this.allBuildingsColonist[i].def == def)
				{
					return true;
				}
			}
			return false;
		}

		public bool ColonistsHaveBuilding(Func<Thing, bool> predicate)
		{
			for (int i = 0; i < this.allBuildingsColonist.Count; i++)
			{
				if (predicate(this.allBuildingsColonist[i]))
				{
					return true;
				}
			}
			return false;
		}

		public bool ColonistsHaveResearchBench()
		{
			for (int i = 0; i < this.allBuildingsColonist.Count; i++)
			{
				if (this.allBuildingsColonist[i] is Building_ResearchBench)
				{
					return true;
				}
			}
			return false;
		}

		public bool ColonistsHaveBuildingWithPowerOn(ThingDef def)
		{
			for (int i = 0; i < this.allBuildingsColonist.Count; i++)
			{
				if (this.allBuildingsColonist[i].def == def)
				{
					CompPowerTrader compPowerTrader = this.allBuildingsColonist[i].TryGetComp<CompPowerTrader>();
					if (compPowerTrader != null && !compPowerTrader.PowerOn)
					{
						continue;
					}
					return true;
				}
			}
			return false;
		}

		public IEnumerable<Building> AllBuildingsColonistOfDef(ThingDef def)
		{
			int i = 0;
			while (true)
			{
				if (i < this.allBuildingsColonist.Count)
				{
					if (this.allBuildingsColonist[i].def != def)
					{
						i++;
						continue;
					}
					break;
				}
				yield break;
			}
			yield return this.allBuildingsColonist[i];
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public IEnumerable<T> AllBuildingsColonistOfClass<T>() where T : Building
		{
			int i = 0;
			T casted;
			while (true)
			{
				if (i < this.allBuildingsColonist.Count)
				{
					casted = (T)(this.allBuildingsColonist[i] as T);
					if (casted == null)
					{
						i++;
						continue;
					}
					break;
				}
				yield break;
			}
			yield return casted;
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
