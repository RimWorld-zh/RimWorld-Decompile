using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class WealthWatcher
	{
		private Map map;

		private float wealthItems;

		private float wealthBuildings;

		private float wealthTameAnimals;

		private int totalHealth;

		private float lastCountTick = -99999f;

		private const int MinCountInterval = 5000;

		private List<Thing> tmpThings = new List<Thing>();

		[CompilerGenerated]
		private static Predicate<IThingHolder> <>f__am$cache0;

		public WealthWatcher(Map map)
		{
			this.map = map;
		}

		public int HealthTotal
		{
			get
			{
				this.RecountIfNeeded();
				return this.totalHealth;
			}
		}

		public float WealthTotal
		{
			get
			{
				this.RecountIfNeeded();
				return this.wealthItems + this.wealthBuildings + this.wealthTameAnimals;
			}
		}

		public float WealthItems
		{
			get
			{
				this.RecountIfNeeded();
				return this.wealthItems;
			}
		}

		public float WealthBuildings
		{
			get
			{
				this.RecountIfNeeded();
				return this.wealthBuildings;
			}
		}

		public float WealthTameAnimals
		{
			get
			{
				this.RecountIfNeeded();
				return this.wealthTameAnimals;
			}
		}

		private void RecountIfNeeded()
		{
			if ((float)Find.TickManager.TicksGame - this.lastCountTick > 5000f)
			{
				this.ForceRecount(false);
			}
		}

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

		[CompilerGenerated]
		private static bool <CalculateWealthItems>m__0(IThingHolder x)
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
		}
	}
}
