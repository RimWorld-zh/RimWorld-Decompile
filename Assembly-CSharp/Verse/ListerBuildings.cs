using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000C2C RID: 3116
	public sealed class ListerBuildings
	{
		// Token: 0x0600446E RID: 17518 RVA: 0x0023EF60 File Offset: 0x0023D360
		public void Add(Building b)
		{
			if (b.def.building == null || !b.def.building.isNaturalRock)
			{
				if (b.Faction == Faction.OfPlayer)
				{
					this.allBuildingsColonist.Add(b);
					if (b is IAttackTarget)
					{
						this.allBuildingsColonistCombatTargets.Add(b);
					}
				}
				CompProperties_Power compProperties = b.def.GetCompProperties<CompProperties_Power>();
				if (compProperties != null && compProperties.shortCircuitInRain)
				{
					this.allBuildingsColonistElecFire.Add(b);
				}
			}
		}

		// Token: 0x0600446F RID: 17519 RVA: 0x0023EFF8 File Offset: 0x0023D3F8
		public void Remove(Building b)
		{
			this.allBuildingsColonist.Remove(b);
			if (b is IAttackTarget)
			{
				this.allBuildingsColonistCombatTargets.Remove(b);
			}
			CompProperties_Power compProperties = b.def.GetCompProperties<CompProperties_Power>();
			if (compProperties != null && compProperties.shortCircuitInRain)
			{
				this.allBuildingsColonistElecFire.Remove(b);
			}
		}

		// Token: 0x06004470 RID: 17520 RVA: 0x0023F058 File Offset: 0x0023D458
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

		// Token: 0x06004471 RID: 17521 RVA: 0x0023F0AC File Offset: 0x0023D4AC
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

		// Token: 0x06004472 RID: 17522 RVA: 0x0023F100 File Offset: 0x0023D500
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

		// Token: 0x06004473 RID: 17523 RVA: 0x0023F154 File Offset: 0x0023D554
		public bool ColonistsHaveBuildingWithPowerOn(ThingDef def)
		{
			for (int i = 0; i < this.allBuildingsColonist.Count; i++)
			{
				if (this.allBuildingsColonist[i].def == def)
				{
					CompPowerTrader compPowerTrader = this.allBuildingsColonist[i].TryGetComp<CompPowerTrader>();
					if (compPowerTrader == null || compPowerTrader.PowerOn)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06004474 RID: 17524 RVA: 0x0023F1CC File Offset: 0x0023D5CC
		public IEnumerable<Building> AllBuildingsColonistOfDef(ThingDef def)
		{
			for (int i = 0; i < this.allBuildingsColonist.Count; i++)
			{
				if (this.allBuildingsColonist[i].def == def)
				{
					yield return this.allBuildingsColonist[i];
				}
			}
			yield break;
		}

		// Token: 0x06004475 RID: 17525 RVA: 0x0023F200 File Offset: 0x0023D600
		public IEnumerable<T> AllBuildingsColonistOfClass<T>() where T : Building
		{
			for (int i = 0; i < this.allBuildingsColonist.Count; i++)
			{
				T casted = this.allBuildingsColonist[i] as T;
				if (casted != null)
				{
					yield return casted;
				}
			}
			yield break;
		}

		// Token: 0x04002E71 RID: 11889
		public List<Building> allBuildingsColonist = new List<Building>();

		// Token: 0x04002E72 RID: 11890
		public HashSet<Building> allBuildingsColonistCombatTargets = new HashSet<Building>();

		// Token: 0x04002E73 RID: 11891
		public HashSet<Building> allBuildingsColonistElecFire = new HashSet<Building>();
	}
}
