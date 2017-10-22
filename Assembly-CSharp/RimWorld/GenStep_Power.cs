using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class GenStep_Power : GenStep
	{
		public bool canSpawnBatteries = true;

		public bool canSpawnPowerGenerators = true;

		public bool spawnRoofOverNewBatteries = true;

		public FloatRange newBatteriesInitialStoredEnergyPctRange = new FloatRange(0.2f, 0.5f);

		private List<Thing> tmpThings = new List<Thing>();

		private List<IntVec3> tmpCells = new List<IntVec3>();

		private const int MaxDistToExistingNetForTurrets = 13;

		private const int RoofPadding = 2;

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
			bool result;
			if (thing.TryGetComp<CompPowerPlant>() != null)
			{
				result = true;
			}
			else
			{
				CompPowerTrader compPowerTrader = thing.TryGetComp<CompPowerTrader>();
				result = (compPowerTrader != null && (compPowerTrader.PowerOutput > 0.0 || (!compPowerTrader.PowerOn && compPowerTrader.Props.basePowerConsumption < 0.0)));
			}
			return result;
		}

		private bool HasAnyPowerGenerator(PowerNet net)
		{
			List<CompPowerTrader> powerComps = net.powerComps;
			int num = 0;
			bool result;
			while (true)
			{
				if (num < powerComps.Count)
				{
					if (this.IsPowerGenerator(powerComps[num].parent))
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

		private bool HasAnyPowerUser(PowerNet net)
		{
			List<CompPowerTrader> powerComps = net.powerComps;
			int num = 0;
			bool result;
			while (true)
			{
				if (num < powerComps.Count)
				{
					if (this.IsPowerUser(powerComps[num].parent))
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

		private bool TryFindClosestReachableNet(IntVec3 root, Predicate<PowerNet> predicate, Map map, out PowerNet foundNet, out IntVec3 closestTransmitter)
		{
			this.tmpPowerNetPredicateResults.Clear();
			PowerNet foundNetLocal = null;
			IntVec3 closestTransmitterLocal = IntVec3.Invalid;
			map.floodFiller.FloodFill(root, (Predicate<IntVec3>)((IntVec3 x) => this.EverPossibleToTransmitPowerAt(x, map)), (Func<IntVec3, bool>)delegate(IntVec3 x)
			{
				Building transmitter = x.GetTransmitter(map);
				PowerNet powerNet = (transmitter == null) ? null : transmitter.GetComp<CompPower>().PowerNet;
				bool result2;
				if (powerNet == null)
				{
					result2 = false;
				}
				else
				{
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
						result2 = true;
					}
					else
					{
						result2 = false;
					}
				}
				return result2;
			}, 2147483647, true, null);
			this.tmpPowerNetPredicateResults.Clear();
			bool result;
			if (foundNetLocal != null)
			{
				foundNet = foundNetLocal;
				closestTransmitter = closestTransmitterLocal;
				result = true;
			}
			else
			{
				foundNet = null;
				closestTransmitter = IntVec3.Invalid;
				result = false;
			}
			return result;
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
				bool result;
				if (x == end)
				{
					foundPath = true;
					result = true;
				}
				else
				{
					result = false;
				}
				return result;
			}, 2147483647, true, null);
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
			bool result;
			if (RCellFinder.TryFindRandomCellNearWith(position, (Predicate<IntVec3>)delegate(IntVec3 x)
			{
				bool result2;
				if (!x.Standable(map) || x.Roofed(map) || !this.EverPossibleToTransmitPowerAt(x, map))
				{
					result2 = false;
				}
				else if (!map.reachability.CanReach(position, x, PathEndMode.OnCell, traverseParams))
				{
					result2 = false;
				}
				else
				{
					CellRect.CellRectIterator iterator = GenAdj.OccupiedRect(x, Rot4.North, def.size).GetIterator();
					while (!iterator.Done())
					{
						IntVec3 current = iterator.Current;
						if (current.InBounds(map) && !current.Roofed(map) && current.GetEdifice(map) == null && current.GetFirstItem(map) == null && current.GetTransmitter(map) == null)
						{
							iterator.MoveNext();
							continue;
						}
						goto IL_00f2;
					}
					result2 = true;
				}
				goto IL_0115;
				IL_0115:
				return result2;
				IL_00f2:
				result2 = false;
				goto IL_0115;
			}, map, out loc, 8, 2147483647))
			{
				newBuilding = (Building)GenSpawn.Spawn(ThingMaker.MakeThing(def, null), loc, map, Rot4.North, false);
				newBuilding.SetFaction(faction, null);
				result = true;
			}
			else
			{
				newBuilding = null;
				result = false;
			}
			return result;
		}

		private bool TrySpawnPowerGeneratorNear(IntVec3 position, Map map, Faction faction, out Building newPowerGenerator)
		{
			bool result;
			if (this.TrySpawnPowerTransmittingBuildingNear(position, map, faction, ThingDefOf.SolarGenerator, out newPowerGenerator))
			{
				map.powerNetManager.UpdatePowerNetsAndConnections_First();
				newPowerGenerator.GetComp<CompPowerPlant>().UpdateDesiredPowerOutput();
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		private bool TrySpawnBatteryNear(IntVec3 position, Map map, Faction faction, out Building newBattery)
		{
			bool result;
			if (this.TrySpawnPowerTransmittingBuildingNear(position, map, faction, ThingDefOf.Battery, out newBattery))
			{
				float randomInRange = this.newBatteriesInitialStoredEnergyPctRange.RandomInRange;
				newBattery.GetComp<CompPowerBattery>().SetStoredEnergyPct(randomInRange);
				if (this.spawnRoofOverNewBatteries)
				{
					this.SpawnRoofOver(newBattery);
				}
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		private bool TrySpawnPowerGeneratorAndBatteryIfCanAndConnect(Thing forThing, Map map)
		{
			bool result;
			if (!this.canSpawnPowerGenerators)
			{
				result = false;
			}
			else
			{
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
					result = true;
				}
				else
				{
					result = false;
				}
			}
			return result;
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

		private void SpawnRoofOver(Thing thing)
		{
			CellRect cellRect = thing.OccupiedRect();
			bool flag = true;
			CellRect.CellRectIterator iterator = cellRect.GetIterator();
			while (!iterator.Done())
			{
				if (iterator.Current.Roofed(thing.Map))
				{
					iterator.MoveNext();
					continue;
				}
				flag = false;
				break;
			}
			if (!flag)
			{
				int num = 0;
				CellRect cellRect2 = cellRect.ExpandedBy(2);
				CellRect.CellRectIterator iterator2 = cellRect2.GetIterator();
				while (!iterator2.Done())
				{
					if (iterator2.Current.InBounds(thing.Map) && iterator2.Current.GetRoofHolderOrImpassable(thing.Map) != null)
					{
						num++;
					}
					iterator2.MoveNext();
				}
				if (num < 2)
				{
					ThingDef stuff = Rand.Element(ThingDefOf.WoodLog, ThingDefOf.Steel);
					foreach (IntVec3 corner in cellRect2.Corners)
					{
						if (corner.InBounds(thing.Map) && corner.Standable(thing.Map) && corner.GetFirstItem(thing.Map) == null && corner.GetFirstBuilding(thing.Map) == null && corner.GetFirstPawn(thing.Map) == null && !GenAdj.CellsAdjacent8Way(new TargetInfo(corner, thing.Map, false)).Any((Func<IntVec3, bool>)((IntVec3 x) => !x.InBounds(thing.Map) || !x.Walkable(thing.Map))) && corner.SupportsStructureType(thing.Map, ThingDefOf.Wall.terrainAffordanceNeeded))
						{
							Thing thing2 = ThingMaker.MakeThing(ThingDefOf.Wall, stuff);
							GenSpawn.Spawn(thing2, corner, thing.Map);
							thing2.SetFaction(thing.Faction, null);
							num++;
						}
					}
				}
				if (num > 0)
				{
					CellRect.CellRectIterator iterator3 = cellRect2.GetIterator();
					while (!iterator3.Done())
					{
						if (iterator3.Current.InBounds(thing.Map) && !iterator3.Current.Roofed(thing.Map))
						{
							thing.Map.roofGrid.SetRoof(iterator3.Current, RoofDefOf.RoofConstructed);
						}
						iterator3.MoveNext();
					}
				}
			}
		}
	}
}
