using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000443 RID: 1091
	public class WealthWatcher
	{
		// Token: 0x060012EC RID: 4844 RVA: 0x000A3729 File Offset: 0x000A1B29
		public WealthWatcher(Map map)
		{
			this.map = map;
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x060012ED RID: 4845 RVA: 0x000A3750 File Offset: 0x000A1B50
		public int HealthTotal
		{
			get
			{
				this.RecountIfNeeded();
				return this.totalHealth;
			}
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x060012EE RID: 4846 RVA: 0x000A3774 File Offset: 0x000A1B74
		public float WealthTotal
		{
			get
			{
				this.RecountIfNeeded();
				return this.wealthItems + this.wealthBuildings + this.wealthTameAnimals;
			}
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x060012EF RID: 4847 RVA: 0x000A37A4 File Offset: 0x000A1BA4
		public float WealthItems
		{
			get
			{
				this.RecountIfNeeded();
				return this.wealthItems;
			}
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x060012F0 RID: 4848 RVA: 0x000A37C8 File Offset: 0x000A1BC8
		public float WealthBuildings
		{
			get
			{
				this.RecountIfNeeded();
				return this.wealthBuildings;
			}
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x060012F1 RID: 4849 RVA: 0x000A37EC File Offset: 0x000A1BEC
		public float WealthTameAnimals
		{
			get
			{
				this.RecountIfNeeded();
				return this.wealthTameAnimals;
			}
		}

		// Token: 0x060012F2 RID: 4850 RVA: 0x000A380D File Offset: 0x000A1C0D
		private void RecountIfNeeded()
		{
			if ((float)Find.TickManager.TicksGame - this.lastCountTick > 5000f)
			{
				this.ForceRecount(false);
			}
		}

		// Token: 0x060012F3 RID: 4851 RVA: 0x000A3834 File Offset: 0x000A1C34
		public void ForceRecount(bool allowDuringInit = false)
		{
			if (!allowDuringInit && Current.ProgramState != ProgramState.Playing)
			{
				Log.Error("WealthWatcher recount in game mode " + Current.ProgramState, false);
			}
			else
			{
				this.wealthItems = this.CalculateWealthItems();
				this.wealthBuildings = 0f;
				this.wealthTameAnimals = 0f;
				this.totalHealth = 0;
				List<Thing> list = this.map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial);
				for (int i = 0; i < list.Count; i++)
				{
					Thing thing = list[i];
					if (thing.Faction == Faction.OfPlayer)
					{
						this.wealthBuildings += thing.MarketValue;
						this.totalHealth += thing.HitPoints;
					}
				}
				foreach (Pawn pawn in this.map.mapPawns.PawnsInFaction(Faction.OfPlayer))
				{
					if (pawn.RaceProps.Animal)
					{
						this.wealthTameAnimals += pawn.MarketValue;
					}
					if (pawn.IsFreeColonist)
					{
						this.totalHealth += Mathf.RoundToInt(pawn.health.summaryHealth.SummaryHealthPercent * 100f);
					}
				}
				this.lastCountTick = (float)Find.TickManager.TicksGame;
			}
		}

		// Token: 0x060012F4 RID: 4852 RVA: 0x000A39CC File Offset: 0x000A1DCC
		public static float GetEquipmentApparelAndInventoryWealth(Pawn p)
		{
			float num = 0f;
			if (p.equipment != null)
			{
				List<ThingWithComps> allEquipmentListForReading = p.equipment.AllEquipmentListForReading;
				for (int i = 0; i < allEquipmentListForReading.Count; i++)
				{
					num += allEquipmentListForReading[i].MarketValue * (float)allEquipmentListForReading[i].stackCount;
				}
			}
			if (p.apparel != null)
			{
				List<Apparel> wornApparel = p.apparel.WornApparel;
				for (int j = 0; j < wornApparel.Count; j++)
				{
					num += wornApparel[j].MarketValue * (float)wornApparel[j].stackCount;
				}
			}
			if (p.inventory != null)
			{
				ThingOwner<Thing> innerContainer = p.inventory.innerContainer;
				for (int k = 0; k < innerContainer.Count; k++)
				{
					num += innerContainer[k].MarketValue * (float)innerContainer[k].stackCount;
				}
			}
			return num;
		}

		// Token: 0x060012F5 RID: 4853 RVA: 0x000A3AE8 File Offset: 0x000A1EE8
		private float CalculateWealthItems()
		{
			this.tmpThings.Clear();
			ThingOwnerUtility.GetAllThingsRecursively<Thing>(this.map, ThingRequest.ForGroup(ThingRequestGroup.HaulableEver), this.tmpThings, false, delegate(IThingHolder x)
			{
				bool result;
				if (x is PassingShip || x is MapComponent)
				{
					result = false;
				}
				else
				{
					Pawn pawn = x as Pawn;
					result = (pawn == null || pawn.Faction == Faction.OfPlayer);
				}
				return result;
			}, true);
			float num = 0f;
			for (int i = 0; i < this.tmpThings.Count; i++)
			{
				if (this.tmpThings[i].SpawnedOrAnyParentSpawned && !this.tmpThings[i].PositionHeld.Fogged(this.map))
				{
					num += this.tmpThings[i].MarketValue * (float)this.tmpThings[i].stackCount;
				}
			}
			this.tmpThings.Clear();
			return num;
		}

		// Token: 0x04000B7A RID: 2938
		private Map map;

		// Token: 0x04000B7B RID: 2939
		private float wealthItems;

		// Token: 0x04000B7C RID: 2940
		private float wealthBuildings;

		// Token: 0x04000B7D RID: 2941
		private float wealthTameAnimals;

		// Token: 0x04000B7E RID: 2942
		private int totalHealth;

		// Token: 0x04000B7F RID: 2943
		private float lastCountTick = -99999f;

		// Token: 0x04000B80 RID: 2944
		private const int MinCountInterval = 5000;

		// Token: 0x04000B81 RID: 2945
		private List<Thing> tmpThings = new List<Thing>();
	}
}
