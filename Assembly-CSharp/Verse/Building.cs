using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000DC5 RID: 3525
	public class Building : ThingWithComps
	{
		// Token: 0x17000CBA RID: 3258
		// (get) Token: 0x06004EC0 RID: 20160 RVA: 0x00129570 File Offset: 0x00127970
		public CompPower PowerComp
		{
			get
			{
				return base.GetComp<CompPower>();
			}
		}

		// Token: 0x17000CBB RID: 3259
		// (get) Token: 0x06004EC1 RID: 20161 RVA: 0x0012958C File Offset: 0x0012798C
		public virtual bool TransmitsPowerNow
		{
			get
			{
				CompPower powerComp = this.PowerComp;
				return powerComp != null && powerComp.Props.transmitsPower;
			}
		}

		// Token: 0x17000CBC RID: 3260
		// (set) Token: 0x06004EC2 RID: 20162 RVA: 0x001295BC File Offset: 0x001279BC
		public override int HitPoints
		{
			set
			{
				int hitPoints = this.HitPoints;
				base.HitPoints = value;
				BuildingsDamageSectionLayerUtility.Notify_BuildingHitPointsChanged(this, hitPoints);
			}
		}

		// Token: 0x06004EC3 RID: 20163 RVA: 0x001295DF File Offset: 0x001279DF
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.canChangeTerrainOnDestroyed, "canChangeTerrainOnDestroyed", true, false);
		}

		// Token: 0x06004EC4 RID: 20164 RVA: 0x001295FC File Offset: 0x001279FC
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (this.def.IsEdifice())
			{
				map.edificeGrid.Register(this);
			}
			base.SpawnSetup(map, respawningAfterLoad);
			base.Map.listerBuildings.Add(this);
			if (this.def.coversFloor)
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
					if (!SnowGrid.CanCoexistWithSnow(this.def))
					{
						base.Map.snowGrid.SetDepth(intVec, 0f);
					}
				}
			}
			if (base.Faction == Faction.OfPlayer)
			{
				if (this.def.building != null && this.def.building.spawnedConceptLearnOpportunity != null)
				{
					LessonAutoActivator.TeachOpportunity(this.def.building.spawnedConceptLearnOpportunity, OpportunityType.GoodToKnow);
				}
			}
			AutoHomeAreaMaker.Notify_BuildingSpawned(this);
			if (this.def.building != null && !this.def.building.soundAmbient.NullOrUndefined())
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					SoundInfo info = SoundInfo.InMap(this, MaintenanceType.None);
					this.sustainerAmbient = this.def.building.soundAmbient.TrySpawnSustainer(info);
				});
			}
			base.Map.listerBuildingsRepairable.Notify_BuildingSpawned(this);
			if (!this.CanBeSeenOver())
			{
				base.Map.exitMapGrid.Notify_LOSBlockerSpawned();
			}
			SmoothSurfaceDesignatorUtility.Notify_BuildingSpawned(this);
		}

		// Token: 0x06004EC5 RID: 20165 RVA: 0x001297C8 File Offset: 0x00127BC8
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.DeSpawn(mode);
			if (this.def.IsEdifice())
			{
				map.edificeGrid.DeRegister(this);
			}
			if (mode != DestroyMode.WillReplace)
			{
				if (this.def.MakeFog)
				{
					map.fogGrid.Notify_FogBlockerRemoved(base.Position);
				}
				if (this.def.holdsRoof)
				{
					RoofCollapseCellsFinder.Notify_RoofHolderDespawned(this, map);
				}
				if (this.def.IsSmoothable)
				{
					SmoothSurfaceDesignatorUtility.Notify_BuildingDespawned(this, map);
				}
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
					if (this.def.coversFloor)
					{
						mapMeshFlag |= MapMeshFlag.Terrain;
					}
					if (this.def.Fillage == FillCategory.Full)
					{
						mapMeshFlag |= MapMeshFlag.Roofs;
						mapMeshFlag |= MapMeshFlag.Snow;
					}
					map.mapDrawer.MapMeshDirty(loc, mapMeshFlag);
					map.glowGrid.MarkGlowGridDirty(loc);
				}
			}
			map.listerBuildings.Remove(this);
			map.listerBuildingsRepairable.Notify_BuildingDeSpawned(this);
			if (this.def.building.leaveTerrain != null && Current.ProgramState == ProgramState.Playing && this.canChangeTerrainOnDestroyed)
			{
				CellRect.CellRectIterator iterator = this.OccupiedRect().GetIterator();
				while (!iterator.Done())
				{
					map.terrainGrid.SetTerrain(iterator.Current, this.def.building.leaveTerrain);
					iterator.MoveNext();
				}
			}
			map.designationManager.Notify_BuildingDespawned(this);
			if (!this.CanBeSeenOver())
			{
				map.exitMapGrid.Notify_LOSBlockerDespawned();
			}
			if (this.def.building.hasFuelingPort)
			{
				IntVec3 fuelingPortCell = FuelingPortUtility.GetFuelingPortCell(base.Position, base.Rotation);
				CompLaunchable compLaunchable = FuelingPortUtility.LaunchableAt(fuelingPortCell, map);
				if (compLaunchable != null)
				{
					compLaunchable.Notify_FuelingPortSourceDeSpawned();
				}
			}
			if (this.def.building.ai_combatDangerous)
			{
				AvoidGridMaker.Notify_CombatDangerousBuildingDespawned(this, map);
			}
		}

		// Token: 0x06004EC6 RID: 20166 RVA: 0x00129A28 File Offset: 0x00127E28
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			SmoothableWallUtility.Notify_BuildingDestroying(this, mode);
			base.Destroy(mode);
			InstallBlueprintUtility.CancelBlueprintsFor(this);
			if (mode == DestroyMode.Deconstruct)
			{
				SoundDefOf.Building_Deconstructed.PlayOneShot(new TargetInfo(base.Position, map, false));
			}
			if (Find.PlaySettings.autoRebuild && mode == DestroyMode.KillFinalize && base.Faction == Faction.OfPlayer && this.def != null && this.def.blueprintDef != null && this.def.IsResearchFinished)
			{
				GenConstruct.PlaceBlueprintForBuild(this.def, base.Position, map, base.Rotation, Faction.OfPlayer, base.Stuff);
			}
		}

		// Token: 0x06004EC7 RID: 20167 RVA: 0x00129AE9 File Offset: 0x00127EE9
		public override void Draw()
		{
			if (this.def.drawerType == DrawerType.RealtimeOnly)
			{
				base.Draw();
			}
			else
			{
				base.Comps_PostDraw();
			}
		}

		// Token: 0x06004EC8 RID: 20168 RVA: 0x00129B10 File Offset: 0x00127F10
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
				base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.PowerGrid, true, false);
				if (newFaction == Faction.OfPlayer)
				{
					AutoHomeAreaMaker.Notify_BuildingClaimed(this);
				}
			}
		}

		// Token: 0x06004EC9 RID: 20169 RVA: 0x00129BB4 File Offset: 0x00127FB4
		public override void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
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
			base.PreApplyDamage(ref dinfo, out absorbed);
		}

		// Token: 0x06004ECA RID: 20170 RVA: 0x00129C4F File Offset: 0x0012804F
		public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			base.PostApplyDamage(dinfo, totalDamageDealt);
			if (base.Spawned)
			{
				base.Map.listerBuildingsRepairable.Notify_BuildingTookDamage(this);
			}
		}

		// Token: 0x06004ECB RID: 20171 RVA: 0x00129C78 File Offset: 0x00128078
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			Blueprint_Install blueprint_Install = InstallBlueprintUtility.ExistingBlueprintFor(this);
			if (blueprint_Install != null)
			{
				GenDraw.DrawLineBetween(this.TrueCenter(), blueprint_Install.TrueCenter());
			}
		}

		// Token: 0x06004ECC RID: 20172 RVA: 0x00129CAC File Offset: 0x001280AC
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo c in this.<GetGizmos>__BaseCallProxy0())
			{
				yield return c;
			}
			if (this.def.Minifiable && base.Faction == Faction.OfPlayer)
			{
				yield return InstallationDesignatorDatabase.DesignatorFor(this.def);
			}
			Command buildCopy = BuildCopyCommandUtility.BuildCopyCommand(this.def, base.Stuff);
			if (buildCopy != null)
			{
				yield return buildCopy;
			}
			if (base.Faction == Faction.OfPlayer)
			{
				foreach (Command facility in BuildFacilityCommandUtility.BuildFacilityCommands(this.def))
				{
					yield return facility;
				}
			}
			yield break;
		}

		// Token: 0x06004ECD RID: 20173 RVA: 0x00129CD8 File Offset: 0x001280D8
		public virtual bool ClaimableBy(Faction by)
		{
			bool result;
			if (!this.def.Claimable)
			{
				result = false;
			}
			else
			{
				if (base.Faction != null)
				{
					if (base.Faction == by)
					{
						return false;
					}
					if (by == Faction.OfPlayer)
					{
						if (base.Spawned)
						{
							List<Pawn> list = base.Map.mapPawns.SpawnedPawnsInFaction(base.Faction);
							for (int i = 0; i < list.Count; i++)
							{
								if (list[i].RaceProps.Humanlike && GenHostility.IsActiveThreatToPlayer(list[i]))
								{
									return false;
								}
							}
						}
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06004ECE RID: 20174 RVA: 0x00129DA0 File Offset: 0x001281A0
		public virtual bool DeconstructibleBy(Faction faction)
		{
			return DebugSettings.godMode || (this.def.building.IsDeconstructible && (base.Faction == faction || this.ClaimableBy(faction) || this.def.building.alwaysDeconstructible));
		}

		// Token: 0x06004ECF RID: 20175 RVA: 0x00129E10 File Offset: 0x00128210
		public virtual ushort PathWalkCostFor(Pawn p)
		{
			return 0;
		}

		// Token: 0x06004ED0 RID: 20176 RVA: 0x00129E28 File Offset: 0x00128228
		public virtual bool IsDangerousFor(Pawn p)
		{
			return false;
		}

		// Token: 0x0400346C RID: 13420
		private Sustainer sustainerAmbient = null;

		// Token: 0x0400346D RID: 13421
		public bool canChangeTerrainOnDestroyed = true;
	}
}
