using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class GenStep_Power : GenStep
	{
		private const int MaxDistToExistingNetForTurrets = 13;

		public bool canSpawnBatteries = true;

		public bool canSpawnPowerGenerators = true;

		public FloatRange newBatteriesInitialStoredEnergyPctRange = new FloatRange(0.2f, 0.5f);

		private List<Thing> tmpThings = new List<Thing>();

		private List<IntVec3> tmpCells = new List<IntVec3>();

		private static readonly IntRange MaxDistanceBetweenBatteryAndTransmitter = new IntRange(20, 50);

		private Dictionary<PowerNet, bool> tmpPowerNetPredicateResults = new Dictionary<PowerNet, bool>();

		private static List<IntVec3> tmpTransmitterCells = new List<IntVec3>();

		public override void Generate(Map map)
		{
			map.skyManager.ForceSetCurSkyGlow(1f);
			map.powerNetManager.UpdatePowerNetsAndConnections_First();
			this.UpdateDesiredPowerOutputForAllGenerators(map);
			this.EnsureBatteriesConnectedAndMakeSense(map);
			this.EnsurePowerUsersConnected(map);
			this.EnsureGeneratorsConnectedAndMakeSense(map);
			this.tmpThings.Clear();
		}

		private void UpdateDesiredPowerOutputForAllGenerators(Map map)
		{
			this.tmpThings.Clear();
			this.tmpThings.AddRange(map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial));
			for (int i = 0; i < this.tmpThings.Count; i++)
			{
				if (this.IsPowerGenerator(this.tmpThings[i]))
				{
					CompPowerPlant compPowerPlant = this.tmpThings[i].TryGetComp<CompPowerPlant>();
					if (compPowerPlant != null)
					{
						compPowerPlant.UpdateDesiredPowerOutput();
					}
				}
			}
		}

		private void EnsureBatteriesConnectedAndMakeSense(Map map)
		{
			this.tmpThings.Clear();
			this.tmpThings.AddRange(map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial));
			for (int i = 0; i < this.tmpThings.Count; i++)
			{
				CompPowerBattery compPowerBattery = this.tmpThings[i].TryGetComp<CompPowerBattery>();
				if (compPowerBattery != null)
				{
					PowerNet powerNet = compPowerBattery.PowerNet;
					if (powerNet == null || !this.HasAnyPowerGenerator(powerNet))
					{
						map.powerNetManager.UpdatePowerNetsAndConnections_First();
						PowerNet powerNet2 = default(PowerNet);
						IntVec3 dest = default(IntVec3);
						Building building2 = default(Building);
						if (this.TryFindClosestReachableNet(compPowerBattery.parent.Position, (Predicate<PowerNet>)((PowerNet x) => this.HasAnyPowerGenerator(x)), map, out powerNet2, out dest))
						{
							map.floodFiller.ReconstructLastFloodFillPath(dest, this.tmpCells);
							if (this.canSpawnPowerGenerators)
							{
								int count = this.tmpCells.Count;
								IntRange maxDistanceBetweenBatteryAndTransmitter = GenStep_Power.MaxDistanceBetweenBatteryAndTransmitter;
								float a = (float)maxDistanceBetweenBatteryAndTransmitter.min;
								IntRange maxDistanceBetweenBatteryAndTransmitter2 = GenStep_Power.MaxDistanceBetweenBatteryAndTransmitter;
								float chance = Mathf.InverseLerp(a, (float)maxDistanceBetweenBatteryAndTransmitter2.max, (float)count);
								Building building = default(Building);
								if (Rand.Chance(chance) && this.TrySpawnPowerGeneratorNear(compPowerBattery.parent.Position, map, compPowerBattery.parent.Faction, out building))
								{
									this.SpawnTransmitters(compPowerBattery.parent.Position, building.Position, map, compPowerBattery.parent.Faction);
									powerNet2 = null;
								}
							}
							if (powerNet2 != null)
							{
								this.SpawnTransmitters(this.tmpCells, map, compPowerBattery.parent.Faction);
							}
						}
						else if (this.canSpawnPowerGenerators && this.TrySpawnPowerGeneratorNear(compPowerBattery.parent.Position, map, compPowerBattery.parent.Faction, out building2))
						{
							this.SpawnTransmitters(compPowerBattery.parent.Position, building2.Position, map, compPowerBattery.parent.Faction);
						}
					}
				}
			}
		}

		private void EnsurePowerUsersConnected(Map map)
		{
			this.tmpThings.Clear();
			this.tmpThings.AddRange(map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial));
			for (int i = 0; i < this.tmpThings.Count; i++)
			{
				if (this.IsPowerUser(this.tmpThings[i]))
				{
					CompPowerTrader powerComp = this.tmpThings[i].TryGetComp<CompPowerTrader>();
					PowerNet powerNet = powerComp.PowerNet;
					if (powerNet != null && powerNet.hasPowerSource)
					{
						this.TryTurnOnImmediately(powerComp, map);
					}
					else
					{
						map.powerNetManager.UpdatePowerNetsAndConnections_First();
						PowerNet powerNet2 = default(PowerNet);
						IntVec3 dest = default(IntVec3);
						Building building = default(Building);
						if (this.TryFindClosestReachableNet(powerComp.parent.Position, (Predicate<PowerNet>)((PowerNet x) => x.CurrentEnergyGainRate() - powerComp.Props.basePowerConsumption * CompPower.WattsToWattDaysPerTick > 1.0000000116860974E-07), map, out powerNet2, out dest))
						{
							map.floodFiller.ReconstructLastFloodFillPath(dest, this.tmpCells);
							bool flag = false;
							if (this.canSpawnPowerGenerators && this.tmpThings[i] is Building_Turret && this.tmpCells.Count > 13)
							{
								flag = this.TrySpawnPowerGeneratorAndBatteryIfCanAndConnect(this.tmpThings[i], map);
							}
							if (!flag)
							{
								this.SpawnTransmitters(this.tmpCells, map, this.tmpThings[i].Faction);
							}
							this.TryTurnOnImmediately(powerComp, map);
						}
						else if (this.canSpawnPowerGenerators && this.TrySpawnPowerGeneratorAndBatteryIfCanAndConnect(this.tmpThings[i], map))
						{
							this.TryTurnOnImmediately(powerComp, map);
						}
						else if (this.TryFindClosestReachableNet(powerComp.parent.Position, (Predicate<PowerNet>)((PowerNet x) => x.CurrentStoredEnergy() > 1.0000000116860974E-07), map, out powerNet2, out dest))
						{
							map.floodFiller.ReconstructLastFloodFillPath(dest, this.tmpCells);
							this.SpawnTransmitters(this.tmpCells, map, this.tmpThings[i].Faction);
						}
						else if (this.canSpawnBatteries && this.TrySpawnBatteryNear(this.tmpThings[i].Position, map, this.tmpThings[i].Faction, out building))
						{
							this.SpawnTransmitters(this.tmpThings[i].Position, building.Position, map, this.tmpThings[i].Faction);
							if (building.GetComp<CompPowerBattery>().StoredEnergy > 0.0)
							{
								this.TryTurnOnImmediately(powerComp, map);
							}
						}
					}
				}
			}
		}

		private void EnsureGeneratorsConnectedAndMakeSense(Map map)
		{
			this.tmpThings.Clear();
			this.tmpThings.AddRange(map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial));
			for (int i = 0; i < this.tmpThings.Count; i++)
			{
				if (this.IsPowerGenerator(this.tmpThings[i]))
				{
					PowerNet powerNet = this.tmpThings[i].TryGetComp<CompPower>().PowerNet;
					if (powerNet == null || !this.HasAnyPowerUser(powerNet))
					{
						map.powerNetManager.UpdatePowerNetsAndConnections_First();
						PowerNet powerNet2 = default(PowerNet);
						IntVec3 dest = default(IntVec3);
						if (this.TryFindClosestReachableNet(this.tmpThings[i].Position, (Predicate<PowerNet>)((PowerNet x) => this.HasAnyPowerUser(x)), map, out powerNet2, out dest))
						{
							map.floodFiller.ReconstructLastFloodFillPath(dest, this.tmpCells);
							this.SpawnTransmitters(this.tmpCells, map, this.tmpThings[i].Faction);
						}
					}
				}
			}
		}

		private bool IsPowerUser(Thing thing)
		{
			CompPowerTrader compPowerTrader = thing.TryGetComp<CompPowerTrader>();
			return compPowerTrader != null && (compPowerTrader.PowerOutput < 0.0 || (!compPowerTrader.PowerOn && compPowerTrader.Props.basePowerConsumption > 0.0));
		}

		private bool IsPowerGenerator(Thing thing)
		{
			if (thing.TryGetComp<CompPowerPlant>() != null)
			{
				return true;
			}
			CompPowerTrader compPowerTrader = thing.TryGetComp<CompPowerTrader>();
			return compPowerTrader != null && (compPowerTrader.PowerOutput > 0.0 || (!compPowerTrader.PowerOn && compPowerTrader.Props.basePowerConsumption < 0.0));
		}

		private bool HasAnyPowerGenerator(PowerNet net)
		{
			List<CompPowerTrader> powerComps = net.powerComps;
			for (int i = 0; i < powerComps.Count; i++)
			{
				if (this.IsPowerGenerator(powerComps[i].parent))
				{
					return true;
				}
			}
			return false;
		}

		private bool HasAnyPowerUser(PowerNet net)
		{
			List<CompPowerTrader> powerComps = net.powerComps;
			for (int i = 0; i < powerComps.Count; i++)
			{
				if (this.IsPowerUser(powerComps[i].parent))
				{
					return true;
				}
			}
			return false;
		}

		private bool TryFindClosestReachableNet(IntVec3 root, Predicate<PowerNet> predicate, Map map, out PowerNet foundNet, out IntVec3 closestTransmitter)
		{
			this.tmpPowerNetPredicateResults.Clear();
			PowerNet foundNetLocal = null;
			IntVec3 closestTransmitterLocal = IntVec3.Invalid;
			map.floodFiller.FloodFill(root, (Predicate<IntVec3>)((IntVec3 x) => this.EverPossibleToTransmitPowerAt(x, map)), (Func<IntVec3, bool>)delegate(IntVec3 x)
			{
				Building transmitter = x.GetTransmitter(map);
				PowerNet powerNet = (transmitter == null) ? null : transmitter.GetComp<CompPower>().PowerNet;
				if (powerNet == null)
				{
					return false;
				}
				bool flag = default(bool);
				if (!this.tmpPowerNetPredicateResults.TryGetValue(powerNet, out flag))
				{
					flag = predicate(powerNet);
					this.tmpPowerNetPredicateResults.Add(powerNet, flag);
				}
				if (flag)
				{
					foundNetLocal = powerNet;
					closestTransmitterLocal = x;
					return true;
				}
				return false;
			}, true);
			this.tmpPowerNetPredicateResults.Clear();
			if (foundNetLocal != null)
			{
				foundNet = foundNetLocal;
				closestTransmitter = closestTransmitterLocal;
				return true;
			}
			foundNet = null;
			closestTransmitter = IntVec3.Invalid;
			return false;
		}

		private void SpawnTransmitters(List<IntVec3> cells, Map map, Faction faction)
		{
			for (int i = 0; i < cells.Count; i++)
			{
				if (cells[i].GetTransmitter(map) == null)
				{
					Thing thing = GenSpawn.Spawn(ThingDefOf.PowerConduit, cells[i], map);
					thing.SetFaction(faction, null);
				}
			}
		}

		private void SpawnTransmitters(IntVec3 start, IntVec3 end, Map map, Faction faction)
		{
			bool foundPath = false;
			map.floodFiller.FloodFill(start, (Predicate<IntVec3>)((IntVec3 x) => this.EverPossibleToTransmitPowerAt(x, map)), (Func<IntVec3, bool>)delegate(IntVec3 x)
			{
				if (x == end)
				{
					foundPath = true;
					return true;
				}
				return false;
			}, true);
			if (foundPath)
			{
				map.floodFiller.ReconstructLastFloodFillPath(end, GenStep_Power.tmpTransmitterCells);
				this.SpawnTransmitters(GenStep_Power.tmpTransmitterCells, map, faction);
			}
		}

		private bool TrySpawnPowerTransmittingBuildingNear(IntVec3 position, Map map, Faction faction, ThingDef def, out Building newBuilding)
		{
			TraverseParms traverseParams = TraverseParms.For(TraverseMode.PassAllDestroyableThings, Danger.Deadly, false);
			IntVec3 loc = default(IntVec3);
			if (RCellFinder.TryFindRandomCellNearWith(position, (Predicate<IntVec3>)delegate(IntVec3 x)
			{
				if (x.Standable(map) && !x.Roofed(map) && this.EverPossibleToTransmitPowerAt(x, map))
				{
					if (!map.reachability.CanReach(position, x, PathEndMode.OnCell, traverseParams))
					{
						return false;
					}
					CellRect.CellRectIterator iterator = GenAdj.OccupiedRect(x, Rot4.North, def.size).GetIterator();
					while (!iterator.Done())
					{
						IntVec3 current = iterator.Current;
						if (current.InBounds(map) && !current.Roofed(map) && current.GetEdifice(map) == null && current.GetFirstItem(map) == null && current.GetTransmitter(map) == null)
						{
							iterator.MoveNext();
							continue;
						}
						return false;
					}
					return true;
				}
				return false;
			}, map, out loc, 8))
			{
				newBuilding = (Building)GenSpawn.Spawn(ThingMaker.MakeThing(def, null), loc, map, Rot4.North, false);
				newBuilding.SetFaction(faction, null);
				return true;
			}
			newBuilding = null;
			return false;
		}

		private bool TrySpawnPowerGeneratorNear(IntVec3 position, Map map, Faction faction, out Building newPowerGenerator)
		{
			if (this.TrySpawnPowerTransmittingBuildingNear(position, map, faction, ThingDefOf.SolarGenerator, out newPowerGenerator))
			{
				map.powerNetManager.UpdatePowerNetsAndConnections_First();
				newPowerGenerator.GetComp<CompPowerPlant>().UpdateDesiredPowerOutput();
				return true;
			}
			return false;
		}

		private bool TrySpawnBatteryNear(IntVec3 position, Map map, Faction faction, out Building newBattery)
		{
			if (this.TrySpawnPowerTransmittingBuildingNear(position, map, faction, ThingDefOf.Battery, out newBattery))
			{
				float randomInRange = this.newBatteriesInitialStoredEnergyPctRange.RandomInRange;
				newBattery.GetComp<CompPowerBattery>().SetStoredEnergyPct(randomInRange);
				return true;
			}
			return false;
		}

		private bool TrySpawnPowerGeneratorAndBatteryIfCanAndConnect(Thing forThing, Map map)
		{
			if (!this.canSpawnPowerGenerators)
			{
				return false;
			}
			IntVec3 position = forThing.Position;
			if (this.canSpawnBatteries)
			{
				float chance = (float)((!(forThing is Building_Turret)) ? 0.10000000149011612 : 1.0);
				Building building = default(Building);
				if (Rand.Chance(chance) && this.TrySpawnBatteryNear(forThing.Position, map, forThing.Faction, out building))
				{
					this.SpawnTransmitters(forThing.Position, building.Position, map, forThing.Faction);
					position = building.Position;
				}
			}
			Building building2 = default(Building);
			if (this.TrySpawnPowerGeneratorNear(position, map, forThing.Faction, out building2))
			{
				this.SpawnTransmitters(position, building2.Position, map, forThing.Faction);
				return true;
			}
			return false;
		}

		private bool EverPossibleToTransmitPowerAt(IntVec3 c, Map map)
		{
			return c.GetTransmitter(map) != null || GenConstruct.CanBuildOnTerrain(ThingDefOf.PowerConduit, c, map, Rot4.North, null);
		}

		private void TryTurnOnImmediately(CompPowerTrader powerComp, Map map)
		{
			if (!powerComp.PowerOn)
			{
				map.powerNetManager.UpdatePowerNetsAndConnections_First();
				if (powerComp.PowerNet != null && powerComp.PowerNet.CurrentEnergyGainRate() > 1.0000000116860974E-07)
				{
					powerComp.PowerOn = true;
				}
			}
		}
	}
}
