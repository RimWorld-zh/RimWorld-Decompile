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
			int num = 0;
			bool result;
			while (true)
			{
				if (num < this.allBuildingsColonist.Count)
				{
					if (this.allBuildingsColonist[num].def == def)
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}

		public bool ColonistsHaveBuilding(Func<Thing, bool> predicate)
		{
			int num = 0;
			bool result;
			while (true)
			{
				if (num < this.allBuildingsColonist.Count)
				{
					if (predicate(this.allBuildingsColonist[num]))
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}

		public bool ColonistsHaveResearchBench()
		{
			int num = 0;
			bool result;
			while (true)
			{
				if (num < this.allBuildingsColonist.Count)
				{
					if (this.allBuildingsColonist[num] is Building_ResearchBench)
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}

		public bool ColonistsHaveBuildingWithPowerOn(ThingDef def)
		{
			int num = 0;
			bool result;
			while (true)
			{
				if (num < this.allBuildingsColonist.Count)
				{
					if (this.allBuildingsColonist[num].def == def)
					{
						CompPowerTrader compPowerTrader = this.allBuildingsColonist[num].TryGetComp<CompPowerTrader>();
						if (compPowerTrader != null && !compPowerTrader.PowerOn)
						{
							goto IL_004c;
						}
						result = true;
						break;
					}
					goto IL_004c;
				}
				result = false;
				break;
				IL_004c:
				num++;
			}
			return result;
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
