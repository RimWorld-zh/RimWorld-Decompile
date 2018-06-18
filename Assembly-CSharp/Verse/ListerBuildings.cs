using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000C2B RID: 3115
	public sealed class ListerBuildings
	{
		// Token: 0x0600446C RID: 17516 RVA: 0x0023EF38 File Offset: 0x0023D338
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

		// Token: 0x0600446D RID: 17517 RVA: 0x0023EFD0 File Offset: 0x0023D3D0
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

		// Token: 0x0600446E RID: 17518 RVA: 0x0023F030 File Offset: 0x0023D430
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

		// Token: 0x0600446F RID: 17519 RVA: 0x0023F084 File Offset: 0x0023D484
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

		// Token: 0x06004470 RID: 17520 RVA: 0x0023F0D8 File Offset: 0x0023D4D8
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

		// Token: 0x06004471 RID: 17521 RVA: 0x0023F12C File Offset: 0x0023D52C
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

		// Token: 0x06004472 RID: 17522 RVA: 0x0023F1A4 File Offset: 0x0023D5A4
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

		// Token: 0x06004473 RID: 17523 RVA: 0x0023F1D8 File Offset: 0x0023D5D8
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

		// Token: 0x04002E6F RID: 11887
		public List<Building> allBuildingsColonist = new List<Building>();

		// Token: 0x04002E70 RID: 11888
		public HashSet<Building> allBuildingsColonistCombatTargets = new HashSet<Building>();

		// Token: 0x04002E71 RID: 11889
		public HashSet<Building> allBuildingsColonistElecFire = new HashSet<Building>();
	}
}
