using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000C2B RID: 3115
	public sealed class ListerBuildings
	{
		// Token: 0x04002E80 RID: 11904
		public List<Building> allBuildingsColonist = new List<Building>();

		// Token: 0x04002E81 RID: 11905
		public HashSet<Building> allBuildingsColonistCombatTargets = new HashSet<Building>();

		// Token: 0x04002E82 RID: 11906
		public HashSet<Building> allBuildingsColonistElecFire = new HashSet<Building>();

		// Token: 0x06004478 RID: 17528 RVA: 0x002406BC File Offset: 0x0023EABC
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

		// Token: 0x06004479 RID: 17529 RVA: 0x00240754 File Offset: 0x0023EB54
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

		// Token: 0x0600447A RID: 17530 RVA: 0x002407B4 File Offset: 0x0023EBB4
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

		// Token: 0x0600447B RID: 17531 RVA: 0x00240808 File Offset: 0x0023EC08
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

		// Token: 0x0600447C RID: 17532 RVA: 0x0024085C File Offset: 0x0023EC5C
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

		// Token: 0x0600447D RID: 17533 RVA: 0x002408B0 File Offset: 0x0023ECB0
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

		// Token: 0x0600447E RID: 17534 RVA: 0x00240928 File Offset: 0x0023ED28
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

		// Token: 0x0600447F RID: 17535 RVA: 0x0024095C File Offset: 0x0023ED5C
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
