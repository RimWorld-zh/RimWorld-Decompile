using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

		[CompilerGenerated]
		private static Predicate<PowerNet> <>f__am$cache0;

		public GenStep_Power()
		{
		}

		public override int SeedPart
		{
			get
			{
				return 1186199651;
			}
		}

		public override void Generate(Map map, GenStepParams parms)
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
						PowerNet powerNet2;
						IntVec3 dest;
						Building building2;
						if (this.TryFindClosestReachableNet(compPowerBattery.parent.Position, (PowerNet x) => this.HasAnyPowerGenerator(x), map, out powerNet2, out dest))
						{
							map.floodFiller.ReconstructLastFloodFillPath(dest, this.tmpCells);
							if (this.canSpawnPowerGenerators)
							{
								int count = this.tmpCells.Count;
								float chance = Mathf.InverseLerp((float)GenStep_Power.MaxDistanceBetweenBatteryAndTransmitter.min, (float)GenStep_Power.MaxDistanceBetweenBatteryAndTransmitter.max, (float)count);
								Building building;
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
						PowerNet powerNet2;
						IntVec3 dest;
						Building building;
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
						else if (this.canSpawnBatteries && this.TrySpawnBatteryNear(this.tmpThings[i].Position, map, this.tmpThings[i].Faction, out building))
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

		private bool IsPowerUser(Thing thing)
		{
			CompPowerTrader compPowerTrader = thing.TryGetComp<CompPowerTrader>();
			return compPowerTrader != null && (compPowerTrader.PowerOutput < 0f || (!compPowerTrader.PowerOn && compPowerTrader.Props.basePowerConsumption > 0f));
		}

		private bool IsPowerGenerator(Thing thing)
		{
			if (thing.TryGetComp<CompPowerPlant>() != null)
			{
				return true;
			}
			CompPowerTrader compPowerTrader = thing.TryGetComp<CompPowerTrader>();
			return compPowerTrader != null && (compPowerTrader.PowerOutput > 0f || (!compPowerTrader.PowerOn && compPowerTrader.Props.basePowerConsumption < 0f));
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
			map.floodFiller.FloodFill(root, (IntVec3 x) => this.EverPossibleToTransmitPowerAt(x, map), delegate(IntVec3 x)
			{
				Building transmitter = x.GetTransmitter(map);
				PowerNet powerNet = (transmitter == null) ? null : transmitter.GetComp<CompPower>().PowerNet;
				if (powerNet == null)
				{
					return false;
				}
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
					return true;
				}
				return false;
			}, int.MaxValue, true, null);
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
					Thing thing = GenSpawn.Spawn(ThingDefOf.PowerConduit, cells[i], map, WipeMode.Vanish);
					thing.SetFaction(faction, null);
				}
			}
		}

		private void SpawnTransmitters(IntVec3 start, IntVec3 end, Map map, Faction faction)
		{
			bool foundPath = false;
			map.floodFiller.FloodFill(start, (IntVec3 x) => this.EverPossibleToTransmitPowerAt(x, map), delegate(IntVec3 x)
			{
				if (x == end)
				{
					foundPath = true;
					return true;
				}
				return false;
			}, int.MaxValue, true, null);
			if (foundPath)
			{
				map.floodFiller.ReconstructLastFloodFillPath(end, GenStep_Power.tmpTransmitterCells);
				this.SpawnTransmitters(GenStep_Power.tmpTransmitterCells, map, faction);
			}
		}

		private bool TrySpawnPowerTransmittingBuildingNear(IntVec3 position, Map map, Faction faction, ThingDef def, out Building newBuilding, Predicate<IntVec3> extraValidator = null)
		{
			TraverseParms traverseParams = TraverseParms.For(TraverseMode.PassAllDestroyableThings, Danger.Deadly, false);
			IntVec3 loc;
			if (RCellFinder.TryFindRandomCellNearWith(position, delegate(IntVec3 x)
			{
				if (!x.Standable(map) || x.Roofed(map) || !this.EverPossibleToTransmitPowerAt(x, map))
				{
					return false;
				}
				if (!map.reachability.CanReach(position, x, PathEndMode.OnCell, traverseParams))
				{
					return false;
				}
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
				return extraValidator == null || extraValidator(x);
			}, map, out loc, 8, 2147483647))
			{
				newBuilding = (Building)GenSpawn.Spawn(ThingMaker.MakeThing(def, null), loc, map, Rot4.North, WipeMode.Vanish, false);
				newBuilding.SetFaction(faction, null);
				return true;
			}
			newBuilding = null;
			return false;
		}

		private bool TrySpawnPowerGeneratorNear(IntVec3 position, Map map, Faction faction, out Building newPowerGenerator)
		{
			if (this.TrySpawnPowerTransmittingBuildingNear(position, map, faction, ThingDefOf.SolarGenerator, out newPowerGenerator, null))
			{
				map.powerNetManager.UpdatePowerNetsAndConnections_First();
				newPowerGenerator.GetComp<CompPowerPlant>().UpdateDesiredPowerOutput();
				return true;
			}
			return false;
		}

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
			if (this.TrySpawnPowerTransmittingBuildingNear(position, map, faction, ThingDefOf.Battery, out newBattery, extraValidator))
			{
				float randomInRange = this.newBatteriesInitialStoredEnergyPctRange.RandomInRange;
				newBattery.GetComp<CompPowerBattery>().SetStoredEnergyPct(randomInRange);
				if (this.spawnRoofOverNewBatteries)
				{
					this.SpawnRoofOver(newBattery);
				}
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
				float chance = (!(forThing is Building_Turret)) ? 0.1f : 1f;
				Building building;
				if (Rand.Chance(chance) && this.TrySpawnBatteryNear(forThing.Position, map, forThing.Faction, out building))
				{
					this.SpawnTransmitters(forThing.Position, building.Position, map, forThing.Faction);
					position = building.Position;
				}
			}
			Building building2;
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
			if (powerComp.PowerOn)
			{
				return;
			}
			map.powerNetManager.UpdatePowerNetsAndConnections_First();
			if (powerComp.PowerNet != null && powerComp.PowerNet.CurrentEnergyGainRate() > 1E-07f)
			{
				powerComp.PowerOn = true;
			}
		}

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
			if (flag)
			{
				return;
			}
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

		// Note: this type is marked as 'beforefieldinit'.
		static GenStep_Power()
		{
		}

		[CompilerGenerated]
		private bool <EnsureBatteriesConnectedAndMakeSense>m__0(PowerNet x)
		{
			return this.HasAnyPowerGenerator(x);
		}

		[CompilerGenerated]
		private static bool <EnsurePowerUsersConnected>m__1(PowerNet x)
		{
			return x.CurrentStoredEnergy() > 1E-07f;
		}

		[CompilerGenerated]
		private bool <EnsureGeneratorsConnectedAndMakeSense>m__2(PowerNet x)
		{
			return this.HasAnyPowerUser(x);
		}

		[CompilerGenerated]
		private sealed class <EnsurePowerUsersConnected>c__AnonStorey0
		{
			internal CompPowerTrader powerComp;

			public <EnsurePowerUsersConnected>c__AnonStorey0()
			{
			}

			internal bool <>m__0(PowerNet x)
			{
				return x.CurrentEnergyGainRate() - this.powerComp.Props.basePowerConsumption * CompPower.WattsToWattDaysPerTick > 1E-07f;
			}
		}

		[CompilerGenerated]
		private sealed class <TryFindClosestReachableNet>c__AnonStorey1
		{
			internal Map map;

			internal Predicate<PowerNet> predicate;

			internal PowerNet foundNetLocal;

			internal IntVec3 closestTransmitterLocal;

			internal GenStep_Power $this;

			public <TryFindClosestReachableNet>c__AnonStorey1()
			{
			}

			internal bool <>m__0(IntVec3 x)
			{
				return this.$this.EverPossibleToTransmitPowerAt(x, this.map);
			}

			internal bool <>m__1(IntVec3 x)
			{
				Building transmitter = x.GetTransmitter(this.map);
				PowerNet powerNet = (transmitter == null) ? null : transmitter.GetComp<CompPower>().PowerNet;
				if (powerNet == null)
				{
					return false;
				}
				bool flag;
				if (!this.$this.tmpPowerNetPredicateResults.TryGetValue(powerNet, out flag))
				{
					flag = this.predicate(powerNet);
					this.$this.tmpPowerNetPredicateResults.Add(powerNet, flag);
				}
				if (flag)
				{
					this.foundNetLocal = powerNet;
					this.closestTransmitterLocal = x;
					return true;
				}
				return false;
			}
		}

		[CompilerGenerated]
		private sealed class <SpawnTransmitters>c__AnonStorey2
		{
			internal Map map;

			internal IntVec3 end;

			internal bool foundPath;

			internal GenStep_Power $this;

			public <SpawnTransmitters>c__AnonStorey2()
			{
			}

			internal bool <>m__0(IntVec3 x)
			{
				return this.$this.EverPossibleToTransmitPowerAt(x, this.map);
			}

			internal bool <>m__1(IntVec3 x)
			{
				if (x == this.end)
				{
					this.foundPath = true;
					return true;
				}
				return false;
			}
		}

		[CompilerGenerated]
		private sealed class <TrySpawnPowerTransmittingBuildingNear>c__AnonStorey3
		{
			internal Map map;

			internal IntVec3 position;

			internal TraverseParms traverseParams;

			internal ThingDef def;

			internal Predicate<IntVec3> extraValidator;

			internal GenStep_Power $this;

			public <TrySpawnPowerTransmittingBuildingNear>c__AnonStorey3()
			{
			}

			internal bool <>m__0(IntVec3 x)
			{
				if (!x.Standable(this.map) || x.Roofed(this.map) || !this.$this.EverPossibleToTransmitPowerAt(x, this.map))
				{
					return false;
				}
				if (!this.map.reachability.CanReach(this.position, x, PathEndMode.OnCell, this.traverseParams))
				{
					return false;
				}
				CellRect.CellRectIterator iterator = GenAdj.OccupiedRect(x, Rot4.North, this.def.size).GetIterator();
				while (!iterator.Done())
				{
					IntVec3 c = iterator.Current;
					if (!c.InBounds(this.map) || c.Roofed(this.map) || c.GetEdifice(this.map) != null || c.GetFirstItem(this.map) != null || c.GetTransmitter(this.map) != null)
					{
						return false;
					}
					iterator.MoveNext();
				}
				return this.extraValidator == null || this.extraValidator(x);
			}
		}

		[CompilerGenerated]
		private sealed class <TrySpawnBatteryNear>c__AnonStorey4
		{
			internal Map map;

			private static Predicate<PlaceWorker> <>f__am$cache0;

			public <TrySpawnBatteryNear>c__AnonStorey4()
			{
			}

			internal bool <>m__0(IntVec3 x)
			{
				CellRect.CellRectIterator iterator = GenAdj.OccupiedRect(x, Rot4.North, ThingDefOf.Battery.size).ExpandedBy(3).GetIterator();
				while (!iterator.Done())
				{
					IntVec3 c = iterator.Current;
					if (c.InBounds(this.map))
					{
						List<Thing> thingList = c.GetThingList(this.map);
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
			}

			private static bool <>m__1(PlaceWorker y)
			{
				return y is PlaceWorker_NotUnderRoof;
			}
		}

		[CompilerGenerated]
		private sealed class <SpawnRoofOver>c__AnonStorey5
		{
			internal Thing thing;

			public <SpawnRoofOver>c__AnonStorey5()
			{
			}

			internal bool <>m__0(IntVec3 x)
			{
				return !x.InBounds(this.thing.Map) || !x.Walkable(this.thing.Map);
			}
		}
	}
}
