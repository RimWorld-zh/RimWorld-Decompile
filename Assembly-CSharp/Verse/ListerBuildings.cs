using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000C2A RID: 3114
	public sealed class ListerBuildings
	{
		// Token: 0x04002E79 RID: 11897
		public List<Building> allBuildingsColonist = new List<Building>();

		// Token: 0x04002E7A RID: 11898
		public HashSet<Building> allBuildingsColonistCombatTargets = new HashSet<Building>();

		// Token: 0x04002E7B RID: 11899
		public HashSet<Building> allBuildingsColonistElecFire = new HashSet<Building>();

		// Token: 0x06004478 RID: 17528 RVA: 0x002403DC File Offset: 0x0023E7DC
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

		// Token: 0x06004479 RID: 17529 RVA: 0x00240474 File Offset: 0x0023E874
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

		// Token: 0x0600447A RID: 17530 RVA: 0x002404D4 File Offset: 0x0023E8D4
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

		// Token: 0x0600447B RID: 17531 RVA: 0x00240528 File Offset: 0x0023E928
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

		// Token: 0x0600447C RID: 17532 RVA: 0x0024057C File Offset: 0x0023E97C
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

		// Token: 0x0600447D RID: 17533 RVA: 0x002405D0 File Offset: 0x0023E9D0
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

		// Token: 0x0600447E RID: 17534 RVA: 0x00240648 File Offset: 0x0023EA48
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

		// Token: 0x0600447F RID: 17535 RVA: 0x0024067C File Offset: 0x0023EA7C
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
	}
}
