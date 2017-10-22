using RimWorld;
using System;
using System.Collections.Generic;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace Verse
{
	public class Building : ThingWithComps
	{
		private Sustainer sustainerAmbient;

		public CompPower PowerComp
		{
			get
			{
				return base.GetComp<CompPower>();
			}
		}

		public virtual bool TransmitsPowerNow
		{
			get
			{
				CompPower powerComp = this.PowerComp;
				return powerComp != null && powerComp.Props.transmitsPower;
			}
		}

		public override int HitPoints
		{
			set
			{
				int hitPoints = this.HitPoints;
				base.HitPoints = value;
				BuildingsDamageSectionLayerUtility.Notify_BuildingHitPointsChanged(this, hitPoints);
			}
		}

		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (base.def.IsEdifice())
			{
				map.edificeGrid.Register(this);
			}
			base.SpawnSetup(map, respawningAfterLoad);
			base.Map.listerBuildings.Add(this);
			if (base.def.coversFloor)
			{
				base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Terrain, true, false);
			}
			CellRect cellRect = this.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					IntVec3 intVec = new IntVec3(j, 0, i);
					base.Map.mapDrawer.MapMeshDirty(intVec, MapMeshFlag.Buildings);
					base.Map.glowGrid.MarkGlowGridDirty(intVec);
					if (!SnowGrid.CanCoexistWithSnow(base.def))
					{
						base.Map.snowGrid.SetDepth(intVec, 0f);
					}
				}
			}
			if (base.Faction == Faction.OfPlayer && base.def.building != null && base.def.building.spawnedConceptLearnOpportunity != null)
			{
				LessonAutoActivator.TeachOpportunity(base.def.building.spawnedConceptLearnOpportunity, OpportunityType.GoodToKnow);
			}
			AutoHomeAreaMaker.Notify_BuildingSpawned(this);
			if (base.def.building != null && !base.def.building.soundAmbient.NullOrUndefined())
			{
				LongEventHandler.ExecuteWhenFinished((Action)delegate
				{
					SoundInfo info = SoundInfo.InMap((Thing)this, MaintenanceType.None);
					this.sustainerAmbient = base.def.building.soundAmbient.TrySpawnSustainer(info);
				});
			}
			base.Map.listerBuildingsRepairable.Notify_BuildingSpawned(this);
			if (!this.CanBeSeenOver())
			{
				base.Map.exitMapGrid.Notify_LOSBlockerSpawned();
			}
			SmoothFloorDesignatorUtility.Notify_BuildingSpawned(this);
		}

		public override void DeSpawn()
		{
			Map map = base.Map;
			base.DeSpawn();
			if (base.def.IsEdifice())
			{
				map.edificeGrid.DeRegister(this);
			}
			if (base.def.MakeFog)
			{
				map.fogGrid.Notify_FogBlockerRemoved(base.Position);
			}
			if (base.def.holdsRoof)
			{
				RoofCollapseCellsFinder.Notify_RoofHolderDespawned(this, map);
			}
			if (this.sustainerAmbient != null)
			{
				this.sustainerAmbient.End();
			}
			CellRect cellRect = this.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					IntVec3 loc = new IntVec3(j, 0, i);
					MapMeshFlag mapMeshFlag = MapMeshFlag.Buildings;
					if (base.def.coversFloor)
					{
						mapMeshFlag = (MapMeshFlag)((int)mapMeshFlag | 16);
					}
					if (base.def.Fillage == FillCategory.Full)
					{
						mapMeshFlag = (MapMeshFlag)((int)mapMeshFlag | 32);
						mapMeshFlag = (MapMeshFlag)((int)mapMeshFlag | 64);
					}
					map.mapDrawer.MapMeshDirty(loc, mapMeshFlag);
					map.glowGrid.MarkGlowGridDirty(loc);
				}
			}
			map.listerBuildings.Remove(this);
			map.listerBuildingsRepairable.Notify_BuildingDeSpawned(this);
			if (base.def.leaveTerrain != null && Current.ProgramState == ProgramState.Playing)
			{
				CellRect.CellRectIterator iterator = this.OccupiedRect().GetIterator();
				while (!iterator.Done())
				{
					map.terrainGrid.SetTerrain(iterator.Current, base.def.leaveTerrain);
					iterator.MoveNext();
				}
			}
			map.designationManager.Notify_BuildingDespawned(this);
			if (!this.CanBeSeenOver())
			{
				map.exitMapGrid.Notify_LOSBlockerDespawned();
			}
			if (base.def.building.hasFuelingPort)
			{
				IntVec3 fuelingPortCell = FuelingPortUtility.GetFuelingPortCell(base.Position, base.Rotation);
				CompLaunchable compLaunchable = FuelingPortUtility.LaunchableAt(fuelingPortCell, map);
				if (compLaunchable != null)
				{
					compLaunchable.Notify_FuelingPortSourceDeSpawned();
				}
			}
			if (base.def.building.ai_combatDangerous)
			{
				AvoidGridMaker.Notify_CombatDangerousBuildingDespawned(this, map);
			}
		}

		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.Destroy(mode);
			InstallBlueprintUtility.CancelBlueprintsFor(this);
			if (mode == DestroyMode.Deconstruct)
			{
				SoundDef.Named("BuildingDeconstructed").PlayOneShot(new TargetInfo(base.Position, map, false));
			}
		}

		public override void Draw()
		{
			if (base.def.drawerType == DrawerType.RealtimeOnly)
			{
				base.Draw();
			}
			base.Comps_PostDraw();
		}

		public override void SetFaction(Faction newFaction, Pawn recruiter = null)
		{
			if (base.Spawned)
			{
				base.Map.listerBuildingsRepairable.Notify_BuildingDeSpawned(this);
				base.Map.listerBuildings.Remove(this);
			}
			base.SetFaction(newFaction, recruiter);
			if (base.Spawned)
			{
				base.Map.listerBuildingsRepairable.Notify_BuildingSpawned(this);
				base.Map.listerBuildings.Add(this);
			}
		}

		public override void PreApplyDamage(DamageInfo dinfo, out bool absorbed)
		{
			if (base.Faction != null && base.Spawned && base.Faction != Faction.OfPlayer)
			{
				for (int i = 0; i < base.Map.lordManager.lords.Count; i++)
				{
					Lord lord = base.Map.lordManager.lords[i];
					if (lord.faction == base.Faction)
					{
						lord.Notify_BuildingDamaged(this, dinfo);
					}
				}
			}
			base.PreApplyDamage(dinfo, out absorbed);
		}

		public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			base.PostApplyDamage(dinfo, totalDamageDealt);
			if (base.Spawned)
			{
				base.Map.listerBuildingsRepairable.Notify_BuildingTookDamage(this);
			}
		}

		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			Blueprint_Install blueprint_Install = InstallBlueprintUtility.ExistingBlueprintFor(this);
			if (blueprint_Install != null)
			{
				GenDraw.DrawLineBetween(this.TrueCenter(), blueprint_Install.TrueCenter());
			}
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			if (base.def.Minifiable && base.Faction == Faction.OfPlayer)
			{
				yield return (Gizmo)InstallationDesignatorDatabase.DesignatorFor(base.def);
			}
			Command buildCopy = BuildCopyCommandUtility.BuildCopyCommand(base.def, base.Stuff);
			if (buildCopy != null)
			{
				yield return (Gizmo)buildCopy;
			}
		}

		public virtual bool ClaimableBy(Faction by)
		{
			if (!base.def.building.isNaturalRock && base.def.Claimable)
			{
				if (base.Faction != null)
				{
					if (base.Faction == by)
					{
						return false;
					}
					List<Pawn> list = base.Map.mapPawns.SpawnedPawnsInFaction(base.Faction);
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].RaceProps.Humanlike && GenHostility.IsActiveThreat(list[i]))
						{
							return false;
						}
					}
				}
				return true;
			}
			return false;
		}

		public virtual ushort PathFindCostFor(Pawn p)
		{
			return (ushort)0;
		}

		public virtual ushort PathWalkCostFor(Pawn p)
		{
			return (ushort)0;
		}

		public virtual bool IsDangerousFor(Pawn p)
		{
			return false;
		}
	}
}
