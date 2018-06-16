using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020003EB RID: 1003
	public class GenStep_Power : GenStep
	{
		// Token: 0x1700024D RID: 589
		// (get) Token: 0x0600113B RID: 4411 RVA: 0x00093DC4 File Offset: 0x000921C4
		public override int SeedPart
		{
			get
			{
				return 1186199651;
			}
		}

		// Token: 0x0600113C RID: 4412 RVA: 0x00093DE0 File Offset: 0x000921E0
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

		// Token: 0x0600113D RID: 4413 RVA: 0x00093E30 File Offset: 0x00092230
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

		// Token: 0x0600113E RID: 4414 RVA: 0x00093EBC File Offset: 0x000922BC
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
						PowerNet powerNet2;
						IntVec3 dest;
						if (this.TryFindClosestReachableNet(compPowerBattery.parent.Position, (PowerNet x) => this.HasAnyPowerGenerator(x), map, out powerNet2, out dest))
						{
							map.floodFiller.ReconstructLastFloodFillPath(dest, this.tmpCells);
							if (this.canSpawnPowerGenerators)
							{
								int count = this.tmpCells.Count;
								float chance = Mathf.InverseLerp((float)GenStep_Power.MaxDistanceBetweenBatteryAndTransmitter.min, (float)GenStep_Power.MaxDistanceBetweenBatteryAndTransmitter.max, (float)count);
								if (Rand.Chance(chance))
								{
									Building building;
									if (this.TrySpawnPowerGeneratorNear(compPowerBattery.parent.Position, map, compPowerBattery.parent.Faction, out building))
									{
										this.SpawnTransmitters(compPowerBattery.parent.Position, building.Position, map, compPowerBattery.parent.Faction);
										powerNet2 = null;
									}
								}
							}
							if (powerNet2 != null)
							{
								this.SpawnTransmitters(this.tmpCells, map, compPowerBattery.parent.Faction);
							}
						}
						else if (this.canSpawnPowerGenerators)
						{
							Building building2;
							if (this.TrySpawnPowerGeneratorNear(compPowerBattery.parent.Position, map, compPowerBattery.parent.Faction, out building2))
							{
								this.SpawnTransmitters(compPowerBattery.parent.Position, building2.Position, map, compPowerBattery.parent.Faction);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600113F RID: 4415 RVA: 0x000940A4 File Offset: 0x000924A4
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
						PowerNet powerNet2;
						IntVec3 dest;
						if (this.TryFindClosestReachableNet(powerComp.parent.Position, (PowerNet x) => x.CurrentEnergyGainRate() - powerComp.Props.basePowerConsumption * CompPower.WattsToWattDaysPerTick > 1E-07f, map, out powerNet2, out dest))
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
						else if (this.TryFindClosestReachableNet(powerComp.parent.Position, (PowerNet x) => x.CurrentStoredEnergy() > 1E-07f, map, out powerNet2, out dest))
						{
							map.floodFiller.ReconstructLastFloodFillPath(dest, this.tmpCells);
							this.SpawnTransmitters(this.tmpCells, map, this.tmpThings[i].Faction);
						}
						else if (this.canSpawnBatteries)
						{
							Building building;
							if (this.TrySpawnBatteryNear(this.tmpThings[i].Position, map, this.tmpThings[i].Faction, out building))
							{
								this.SpawnTransmitters(this.tmpThings[i].Position, building.Position, map, this.tmpThings[i].Faction);
								if (building.GetComp<CompPowerBattery>().StoredEnergy > 0f)
								{
									this.TryTurnOnImmediately(powerComp, map);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06001140 RID: 4416 RVA: 0x00094368 File Offset: 0x00092768
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
						PowerNet powerNet2;
						IntVec3 dest;
						if (this.TryFindClosestReachableNet(this.tmpThings[i].Position, (PowerNet x) => this.HasAnyPowerUser(x), map, out powerNet2, out dest))
						{
							map.floodFiller.ReconstructLastFloodFillPath(dest, this.tmpCells);
							this.SpawnTransmitters(this.tmpCells, map, this.tmpThings[i].Faction);
						}
					}
				}
			}
		}

		// Token: 0x06001141 RID: 4417 RVA: 0x0009446C File Offset: 0x0009286C
		private bool IsPowerUser(Thing thing)
		{
			CompPowerTrader compPowerTrader = thing.TryGetComp<CompPowerTrader>();
			return compPowerTrader != null && (compPowerTrader.PowerOutput < 0f || (!compPowerTrader.PowerOn && compPowerTrader.Props.basePowerConsumption > 0f));
		}

		// Token: 0x06001142 RID: 4418 RVA: 0x000944C4 File Offset: 0x000928C4
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
				result = (compPowerTrader != null && (compPowerTrader.PowerOutput > 0f || (!compPowerTrader.PowerOn && compPowerTrader.Props.basePowerConsumption < 0f)));
			}
			return result;
		}

		// Token: 0x06001143 RID: 4419 RVA: 0x00094530 File Offset: 0x00092930
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

		// Token: 0x06001144 RID: 4420 RVA: 0x00094584 File Offset: 0x00092984
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

		// Token: 0x06001145 RID: 4421 RVA: 0x000945D8 File Offset: 0x000929D8
		private bool TryFindClosestReachableNet(IntVec3 root, Predicate<PowerNet> predicate, Map map, out PowerNet foundNet, out IntVec3 closestTransmitter)
		{
			this.tmpPowerNetPredicateResults.Clear();
			PowerNet foundNetLocal = null;
			IntVec3 closestTransmitterLocal = IntVec3.Invalid;
			map.floodFiller.FloodFill(root, (IntVec3 x) => this.EverPossibleToTransmitPowerAt(x, map), delegate(IntVec3 x)
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
					bool flag;
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
			}, int.MaxValue, true, null);
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

		// Token: 0x06001146 RID: 4422 RVA: 0x0009469C File Offset: 0x00092A9C
		private void SpawnTransmitters(List<IntVec3> cells, Map map, Faction faction)
		{
			for (int i = 0; i < cells.Count; i++)
			{
				if (cells[i].GetTransmitter(map) == null)
				{
					Thing thing = GenSpawn.Spawn(ThingDefOf.PowerConduit, cells[i], map, WipeMode.Vanish);
					thing.SetFaction(faction, null);
				}
			}
		}

		// Token: 0x06001147 RID: 4423 RVA: 0x000946F8 File Offset: 0x00092AF8
		private void SpawnTransmitters(IntVec3 start, IntVec3 end, Map map, Faction faction)
		{
			bool foundPath = false;
			map.floodFiller.FloodFill(start, (IntVec3 x) => this.EverPossibleToTransmitPowerAt(x, map), delegate(IntVec3 x)
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
			}, int.MaxValue, true, null);
			if (foundPath)
			{
				map.floodFiller.ReconstructLastFloodFillPath(end, GenStep_Power.tmpTransmitterCells);
				this.SpawnTransmitters(GenStep_Power.tmpTransmitterCells, map, faction);
			}
		}

		// Token: 0x06001148 RID: 4424 RVA: 0x00094794 File Offset: 0x00092B94
		private bool TrySpawnPowerTransmittingBuildingNear(IntVec3 position, Map map, Faction faction, ThingDef def, out Building newBuilding, Predicate<IntVec3> extraValidator = null)
		{
			TraverseParms traverseParams = TraverseParms.For(TraverseMode.PassAllDestroyableThings, Danger.Deadly, false);
			IntVec3 loc;
			bool result;
			if (RCellFinder.TryFindRandomCellNearWith(position, delegate(IntVec3 x)
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
						IntVec3 c = iterator.Current;
						if (!c.InBounds(map) || c.Roofed(map) || c.GetEdifice(map) != null || c.GetFirstItem(map) != null || c.GetTransmitter(map) != null)
						{
							return false;
						}
						iterator.MoveNext();
					}
					result2 = (extraValidator == null || extraValidator(x));
				}
				return result2;
			}, map, out loc, 8, 2147483647))
			{
				newBuilding = (Building)GenSpawn.Spawn(ThingMaker.MakeThing(def, null), loc, map, Rot4.North, WipeMode.Vanish, false);
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

		// Token: 0x06001149 RID: 4425 RVA: 0x0009484C File Offset: 0x00092C4C
		private bool TrySpawnPowerGeneratorNear(IntVec3 position, Map map, Faction faction, out Building newPowerGenerator)
		{
			bool result;
			if (this.TrySpawnPowerTransmittingBuildingNear(position, map, faction, ThingDefOf.SolarGenerator, out newPowerGenerator, null))
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

		// Token: 0x0600114A RID: 4426 RVA: 0x00094898 File Offset: 0x00092C98
		private bool TrySpawnBatteryNear(IntVec3 position, Map map, Faction faction, out Building newBattery)
		{
			Predicate<IntVec3> extraValidator = null;
			if (this.spawnRoofOverNewBatteries)
			{
				extraValidator = delegate(IntVec3 x)
				{
					CellRect.CellRectIterator iterator = GenAdj.OccupiedRect(x, Rot4.North, ThingDefOf.Battery.size).ExpandedBy(3).GetIterator();
					while (!iterator.Done())
					{
						IntVec3 c = iterator.Current;
						if (c.InBounds(map))
						{
							List<Thing> thingList = c.GetThingList(map);
							for (int i = 0; i < thingList.Count; i++)
							{
								if (thingList[i].def.PlaceWorkers != null)
								{
									if (thingList[i].def.PlaceWorkers.Any((PlaceWorker y) => y is PlaceWorker_NotUnderRoof))
									{
										return false;
									}
								}
							}
						}
						iterator.MoveNext();
					}
					return true;
				};
			}
			bool result;
			if (this.TrySpawnPowerTransmittingBuildingNear(position, map, faction, ThingDefOf.Battery, out newBattery, extraValidator))
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

		// Token: 0x0600114B RID: 4427 RVA: 0x00094928 File Offset: 0x00092D28
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
					float chance = (!(forThing is Building_Turret)) ? 0.1f : 1f;
					if (Rand.Chance(chance))
					{
						Building building;
						if (this.TrySpawnBatteryNear(forThing.Position, map, forThing.Faction, out building))
						{
							this.SpawnTransmitters(forThing.Position, building.Position, map, forThing.Faction);
							position = building.Position;
						}
					}
				}
				Building building2;
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

		// Token: 0x0600114C RID: 4428 RVA: 0x000949FC File Offset: 0x00092DFC
		private bool EverPossibleToTransmitPowerAt(IntVec3 c, Map map)
		{
			return c.GetTransmitter(map) != null || GenConstruct.CanBuildOnTerrain(ThingDefOf.PowerConduit, c, map, Rot4.North, null);
		}

		// Token: 0x0600114D RID: 4429 RVA: 0x00094A34 File Offset: 0x00092E34
		private void TryTurnOnImmediately(CompPowerTrader powerComp, Map map)
		{
			if (!powerComp.PowerOn)
			{
				map.powerNetManager.UpdatePowerNetsAndConnections_First();
				if (powerComp.PowerNet != null && powerComp.PowerNet.CurrentEnergyGainRate() > 1E-07f)
				{
					powerComp.PowerOn = true;
				}
			}
		}

		// Token: 0x0600114E RID: 4430 RVA: 0x00094A84 File Offset: 0x00092E84
		private void SpawnRoofOver(Thing thing)
		{
			CellRect cellRect = thing.OccupiedRect();
			bool flag = true;
			CellRect.CellRectIterator iterator = cellRect.GetIterator();
			while (!iterator.Done())
			{
				if (!iterator.Current.Roofed(thing.Map))
				{
					flag = false;
					break;
				}
				iterator.MoveNext();
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
					ThingDef stuff = Rand.Element<ThingDef>(ThingDefOf.WoodLog, ThingDefOf.Steel);
					foreach (IntVec3 intVec in cellRect2.Corners)
					{
						if (intVec.InBounds(thing.Map))
						{
							if (intVec.Standable(thing.Map))
							{
								if (intVec.GetFirstItem(thing.Map) == null && intVec.GetFirstBuilding(thing.Map) == null && intVec.GetFirstPawn(thing.Map) == null)
								{
									if (!GenAdj.CellsAdjacent8Way(new TargetInfo(intVec, thing.Map, false)).Any((IntVec3 x) => !x.InBounds(thing.Map) || !x.Walkable(thing.Map)))
									{
										if (intVec.SupportsStructureType(thing.Map, ThingDefOf.Wall.terrainAffordanceNeeded))
										{
											Thing thing2 = ThingMaker.MakeThing(ThingDefOf.Wall, stuff);
											GenSpawn.Spawn(thing2, intVec, thing.Map, WipeMode.Vanish);
											thing2.SetFaction(thing.Faction, null);
											num++;
										}
									}
								}
							}
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

		// Token: 0x04000A7C RID: 2684
		public bool canSpawnBatteries = true;

		// Token: 0x04000A7D RID: 2685
		public bool canSpawnPowerGenerators = true;

		// Token: 0x04000A7E RID: 2686
		public bool spawnRoofOverNewBatteries = true;

		// Token: 0x04000A7F RID: 2687
		public FloatRange newBatteriesInitialStoredEnergyPctRange = new FloatRange(0.2f, 0.5f);

		// Token: 0x04000A80 RID: 2688
		private List<Thing> tmpThings = new List<Thing>();

		// Token: 0x04000A81 RID: 2689
		private List<IntVec3> tmpCells = new List<IntVec3>();

		// Token: 0x04000A82 RID: 2690
		private const int MaxDistToExistingNetForTurrets = 13;

		// Token: 0x04000A83 RID: 2691
		private const int RoofPadding = 2;

		// Token: 0x04000A84 RID: 2692
		private static readonly IntRange MaxDistanceBetweenBatteryAndTransmitter = new IntRange(20, 50);

		// Token: 0x04000A85 RID: 2693
		private Dictionary<PowerNet, bool> tmpPowerNetPredicateResults = new Dictionary<PowerNet, bool>();

		// Token: 0x04000A86 RID: 2694
		private static List<IntVec3> tmpTransmitterCells = new List<IntVec3>();
	}
}
