using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	public class Thing : Entity, IExposable, ISelectable, ILoadReferenceable, ISignalReceiver
	{
		public ThingDef def = null;

		public int thingIDNumber = -1;

		private sbyte mapIndexOrState = (sbyte)(-1);

		private IntVec3 positionInt = IntVec3.Invalid;

		private Rot4 rotationInt = Rot4.North;

		public int stackCount = 1;

		protected Faction factionInt = null;

		private ThingDef stuffInt = null;

		private Graphic graphicInt = null;

		private int hitPointsInt = -1;

		public ThingOwner holdingOwner = null;

		protected const sbyte UnspawnedState = -1;

		private const sbyte MemoryState = -2;

		private const sbyte DiscardedState = -3;

		public static bool allowDestroyNonDestroyable = false;

		public virtual int HitPoints
		{
			get
			{
				return this.hitPointsInt;
			}
			set
			{
				this.hitPointsInt = value;
			}
		}

		public int MaxHitPoints
		{
			get
			{
				return Mathf.RoundToInt(this.GetStatValue(StatDefOf.MaxHitPoints, true));
			}
		}

		public float MarketValue
		{
			get
			{
				return this.GetStatValue(StatDefOf.MarketValue, true);
			}
		}

		public bool FlammableNow
		{
			get
			{
				bool result;
				if (this.GetStatValue(StatDefOf.Flammability, true) < 0.0099999997764825821)
				{
					result = false;
				}
				else
				{
					if (this.Spawned && !this.FireBulwark)
					{
						List<Thing> thingList = this.Position.GetThingList(this.Map);
						if (thingList != null)
						{
							for (int i = 0; i < thingList.Count; i++)
							{
								if (thingList[i].FireBulwark)
									goto IL_0067;
							}
						}
					}
					result = true;
				}
				goto IL_0088;
				IL_0088:
				return result;
				IL_0067:
				result = false;
				goto IL_0088;
			}
		}

		public virtual bool FireBulwark
		{
			get
			{
				return this.def.Fillage == FillCategory.Full;
			}
		}

		public bool Destroyed
		{
			get
			{
				return this.mapIndexOrState == -2 || this.mapIndexOrState == -3;
			}
		}

		public bool Discarded
		{
			get
			{
				return this.mapIndexOrState == -3;
			}
		}

		public bool Spawned
		{
			get
			{
				return this.mapIndexOrState >= 0;
			}
		}

		public bool SpawnedOrAnyParentSpawned
		{
			get
			{
				return this.Spawned || (this.ParentHolder != null && ThingOwnerUtility.SpawnedOrAnyParentSpawned(this.ParentHolder));
			}
		}

		public Map Map
		{
			get
			{
				return (this.mapIndexOrState < 0) ? null : Find.Maps[this.mapIndexOrState];
			}
		}

		public Map MapHeld
		{
			get
			{
				return (!this.Spawned) ? ((this.ParentHolder != null) ? ThingOwnerUtility.GetRootMap(this.ParentHolder) : null) : this.Map;
			}
		}

		public IntVec3 Position
		{
			get
			{
				return this.positionInt;
			}
			set
			{
				if (!(value == this.positionInt))
				{
					if (this.Spawned)
					{
						if (this.def.AffectsRegions)
						{
							Log.Warning("Changed position of a spawned thing which affects regions. This is not supported.");
						}
						this.DirtyMapMesh(this.Map);
						RegionListersUpdater.DeregisterInRegions(this, this.Map);
						this.Map.thingGrid.Deregister(this, false);
					}
					this.positionInt = value;
					if (this.Spawned)
					{
						this.Map.thingGrid.Register(this);
						RegionListersUpdater.RegisterInRegions(this, this.Map);
						this.DirtyMapMesh(this.Map);
						if (this.def.AffectsReachability)
						{
							this.Map.reachability.ClearCache();
						}
					}
				}
			}
		}

		public IntVec3 PositionHeld
		{
			get
			{
				IntVec3 result;
				if (this.Spawned)
				{
					result = this.Position;
				}
				else
				{
					IntVec3 rootPosition = ThingOwnerUtility.GetRootPosition(this.ParentHolder);
					result = ((!rootPosition.IsValid) ? this.Position : rootPosition);
				}
				return result;
			}
		}

		public Rot4 Rotation
		{
			get
			{
				return this.rotationInt;
			}
			set
			{
				if (!(value == this.rotationInt))
				{
					if (this.Spawned && (this.def.size.x != 1 || this.def.size.z != 1))
					{
						if (this.def.AffectsRegions)
						{
							Log.Warning("Changed rotation of a spawned non-single-cell thing which affects regions. This is not supported.");
						}
						RegionListersUpdater.DeregisterInRegions(this, this.Map);
						this.Map.thingGrid.Deregister(this, false);
					}
					this.rotationInt = value;
					if (this.Spawned)
					{
						if (this.def.size.x == 1 && this.def.size.z == 1)
							return;
						this.Map.thingGrid.Register(this);
						RegionListersUpdater.RegisterInRegions(this, this.Map);
						if (this.def.AffectsReachability)
						{
							this.Map.reachability.ClearCache();
						}
					}
				}
			}
		}

		public bool Smeltable
		{
			get
			{
				return this.def.smeltable && (!this.def.MadeFromStuff || this.Stuff.stuffProps.smeltable);
			}
		}

		public IThingHolder ParentHolder
		{
			get
			{
				return (this.holdingOwner == null) ? null : this.holdingOwner.Owner;
			}
		}

		public Faction Faction
		{
			get
			{
				return this.factionInt;
			}
		}

		public string ThingID
		{
			get
			{
				return (!this.def.HasThingIDNumber) ? this.def.defName : (this.def.defName + this.thingIDNumber.ToString());
			}
			set
			{
				this.thingIDNumber = Thing.IDNumberFromThingID(value);
			}
		}

		public IntVec2 RotatedSize
		{
			get
			{
				return this.rotationInt.IsHorizontal ? new IntVec2(this.def.size.z, this.def.size.x) : this.def.size;
			}
		}

		public override string Label
		{
			get
			{
				return (this.stackCount <= 1) ? this.LabelNoCount : (this.LabelNoCount + " x" + this.stackCount.ToStringCached());
			}
		}

		public virtual string LabelNoCount
		{
			get
			{
				return GenLabel.ThingLabel(this.def, this.Stuff, 1);
			}
		}

		public override string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		public virtual string LabelCapNoCount
		{
			get
			{
				return this.LabelNoCount.CapitalizeFirst();
			}
		}

		public override string LabelShort
		{
			get
			{
				return this.LabelNoCount;
			}
		}

		public virtual bool IngestibleNow
		{
			get
			{
				return !this.IsBurning() && this.def.IsIngestible;
			}
		}

		public ThingDef Stuff
		{
			get
			{
				return this.stuffInt;
			}
		}

		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats
		{
			get
			{
				yield break;
			}
		}

		public Graphic DefaultGraphic
		{
			get
			{
				Graphic badGraphic;
				if (this.graphicInt == null)
				{
					if (this.def.graphicData == null)
					{
						Log.ErrorOnce(this.def + " has no graphicData but we are trying to access it.", 764532);
						badGraphic = BaseContent.BadGraphic;
						goto IL_0067;
					}
					this.graphicInt = this.def.graphicData.GraphicColoredFor(this);
				}
				badGraphic = this.graphicInt;
				goto IL_0067;
				IL_0067:
				return badGraphic;
			}
		}

		public virtual Graphic Graphic
		{
			get
			{
				return this.DefaultGraphic;
			}
		}

		public virtual IntVec3 InteractionCell
		{
			get
			{
				return ThingUtility.InteractionCellWhenAt(this.def, this.Position, this.Rotation, this.Map);
			}
		}

		public float AmbientTemperature
		{
			get
			{
				float num = default(float);
				return (float)((!this.Spawned) ? ((this.ParentHolder == null || !ThingOwnerUtility.TryGetFixedTemperature(this.ParentHolder, out num)) ? ((!this.SpawnedOrAnyParentSpawned) ? ((this.Tile < 0) ? 21.0 : GenTemperature.GetTemperatureAtTile(this.Tile)) : GenTemperature.GetTemperatureForCell(this.PositionHeld, this.MapHeld)) : num) : GenTemperature.GetTemperatureForCell(this.Position, this.Map));
			}
		}

		public int Tile
		{
			get
			{
				return (!this.Spawned) ? ((this.ParentHolder != null) ? ThingOwnerUtility.GetRootTile(this.ParentHolder) : (-1)) : this.Map.Tile;
			}
		}

		public virtual Vector3 DrawPos
		{
			get
			{
				return this.TrueCenter();
			}
		}

		public virtual Color DrawColor
		{
			get
			{
				return (this.Stuff == null) ? ((this.def.graphicData == null) ? Color.white : this.def.graphicData.color) : this.Stuff.stuffProps.color;
			}
			set
			{
				Log.Error("Cannot set instance color on non-ThingWithComps " + this.LabelCap + " at " + this.Position + ".");
			}
		}

		public virtual Color DrawColorTwo
		{
			get
			{
				return (this.def.graphicData == null) ? Color.white : this.def.graphicData.colorTwo;
			}
		}

		public string UniqueVerbOwnerID()
		{
			return this.ThingID;
		}

		public static int IDNumberFromThingID(string thingID)
		{
			string value = Regex.Match(thingID, "\\d+$").Value;
			int result = 0;
			try
			{
				result = Convert.ToInt32(value);
			}
			catch (Exception ex)
			{
				Log.Error("Could not convert id number from thingID=" + thingID + ", numString=" + value + " Exception=" + ex.ToString());
			}
			return result;
		}

		public virtual void PostMake()
		{
			ThingIDMaker.GiveIDTo(this);
			if (this.def.useHitPoints)
			{
				this.HitPoints = Mathf.RoundToInt((float)this.MaxHitPoints * Mathf.Clamp01(this.def.startingHpRange.RandomInRange));
			}
		}

		public string GetUniqueLoadID()
		{
			return "Thing_" + this.ThingID;
		}

		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (this.Destroyed)
			{
				Log.Error("Spawning destroyed thing " + this + " at " + this.Position + ". Correcting.");
				this.mapIndexOrState = (sbyte)(-1);
				if (this.HitPoints <= 0 && this.def.useHitPoints)
				{
					this.HitPoints = 1;
				}
			}
			if (this.Spawned)
			{
				Log.Error("Tried to spawn already-spawned thing " + this + " at " + this.Position);
			}
			else
			{
				int num = Find.Maps.IndexOf(map);
				if (num < 0)
				{
					Log.Error("Tried to spawn thing " + this + ", but the map provided does not exist.");
				}
				else
				{
					if (this.stackCount > this.def.stackLimit)
					{
						Log.Error("Spawned " + this + " with stackCount " + this.stackCount + " but stackLimit is " + this.def.stackLimit + ". Truncating.");
						this.stackCount = this.def.stackLimit;
					}
					this.mapIndexOrState = (sbyte)num;
					RegionListersUpdater.RegisterInRegions(this, map);
					if (!map.spawnedThings.TryAdd(this, false))
					{
						Log.Error("Couldn't add thing " + this + " to spawned things.");
					}
					map.listerThings.Add(this);
					map.thingGrid.Register(this);
					if (Find.TickManager != null)
					{
						Find.TickManager.RegisterAllTickabilityFor(this);
					}
					this.DirtyMapMesh(map);
					if (this.def.drawerType != DrawerType.MapMeshOnly)
					{
						map.dynamicDrawManager.RegisterDrawable(this);
					}
					map.tooltipGiverList.Notify_ThingSpawned(this);
					if (this.def.graphicData != null && this.def.graphicData.Linked)
					{
						map.linkGrid.Notify_LinkerCreatedOrDestroyed(this);
						map.mapDrawer.MapMeshDirty(this.Position, MapMeshFlag.Things, true, false);
					}
					if (!this.def.CanOverlapZones)
					{
						map.zoneManager.Notify_NoZoneOverlapThingSpawned(this);
					}
					if (this.def.AffectsRegions)
					{
						map.regionDirtyer.Notify_ThingAffectingRegionsSpawned(this);
					}
					if (this.def.pathCost != 0 || this.def.passability == Traversability.Impassable)
					{
						map.pathGrid.RecalculatePerceivedPathCostUnderThing(this);
					}
					if (this.def.AffectsReachability)
					{
						map.reachability.ClearCache();
					}
					map.coverGrid.Register(this);
					if (this.def.category == ThingCategory.Item)
					{
						map.listerHaulables.Notify_Spawned(this);
					}
					map.attackTargetsCache.Notify_ThingSpawned(this);
					Region validRegionAt_NoRebuild = map.regionGrid.GetValidRegionAt_NoRebuild(this.Position);
					Room room = (validRegionAt_NoRebuild != null) ? validRegionAt_NoRebuild.Room : null;
					if (room != null)
					{
						room.Notify_ContainedThingSpawnedOrDespawned(this);
					}
					StealAIDebugDrawer.Notify_ThingChanged(this);
					if (this is IThingHolder && Find.ColonistBar != null)
					{
						Find.ColonistBar.MarkColonistsDirty();
					}
					if (this.def.category == ThingCategory.Item)
					{
						SlotGroup slotGroup = this.Position.GetSlotGroup(map);
						if (slotGroup != null && slotGroup.parent != null)
						{
							slotGroup.parent.Notify_ReceivedThing(this);
						}
					}
					if (this.def.receivesSignals)
					{
						Find.SignalManager.RegisterReceiver(this);
					}
				}
			}
		}

		public override void DeSpawn()
		{
			if (this.Destroyed)
			{
				Log.Error("Tried to despawn " + this.ToStringSafe() + " which is already destroyed.");
			}
			else if (!this.Spawned)
			{
				Log.Error("Tried to despawn " + this.ToStringSafe() + " which is not spawned.");
			}
			else
			{
				Map map = this.Map;
				RegionListersUpdater.DeregisterInRegions(this, map);
				map.spawnedThings.Remove(this);
				map.listerThings.Remove(this);
				map.thingGrid.Deregister(this, false);
				map.coverGrid.DeRegister(this);
				if (this.def.receivesSignals)
				{
					Find.SignalManager.DeregisterReceiver(this);
				}
				map.tooltipGiverList.Notify_ThingDespawned(this);
				if (this.def.graphicData != null && this.def.graphicData.Linked)
				{
					map.linkGrid.Notify_LinkerCreatedOrDestroyed(this);
					map.mapDrawer.MapMeshDirty(this.Position, MapMeshFlag.Things, true, false);
				}
				Find.Selector.Deselect(this);
				this.DirtyMapMesh(map);
				if (this.def.drawerType != DrawerType.MapMeshOnly)
				{
					map.dynamicDrawManager.DeRegisterDrawable(this);
				}
				Region validRegionAt_NoRebuild = map.regionGrid.GetValidRegionAt_NoRebuild(this.Position);
				Room room = (validRegionAt_NoRebuild != null) ? validRegionAt_NoRebuild.Room : null;
				if (room != null)
				{
					room.Notify_ContainedThingSpawnedOrDespawned(this);
				}
				if (this.def.AffectsRegions)
				{
					map.regionDirtyer.Notify_ThingAffectingRegionsDespawned(this);
				}
				if (this.def.pathCost != 0 || this.def.passability == Traversability.Impassable)
				{
					map.pathGrid.RecalculatePerceivedPathCostUnderThing(this);
				}
				if (this.def.AffectsReachability)
				{
					map.reachability.ClearCache();
				}
				Find.TickManager.DeRegisterAllTickabilityFor(this);
				this.mapIndexOrState = (sbyte)(-1);
				if (this.def.category == ThingCategory.Item)
				{
					map.listerHaulables.Notify_DeSpawned(this);
				}
				map.attackTargetsCache.Notify_ThingDespawned(this);
				StealAIDebugDrawer.Notify_ThingChanged(this);
				if (this is IThingHolder && Find.ColonistBar != null)
				{
					Find.ColonistBar.MarkColonistsDirty();
				}
				if (this.def.category == ThingCategory.Item)
				{
					SlotGroup slotGroup = this.Position.GetSlotGroup(map);
					if (slotGroup != null && slotGroup.parent != null)
					{
						slotGroup.parent.Notify_LostThing(this);
					}
				}
			}
		}

		public virtual void Kill(DamageInfo? dinfo = default(DamageInfo?), Hediff exactCulprit = null)
		{
			this.Destroy(DestroyMode.KillFinalize);
		}

		public virtual void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (!Thing.allowDestroyNonDestroyable && !this.def.destroyable)
			{
				Log.Error("Tried to destroy non-destroyable thing " + this);
			}
			else if (this.Destroyed)
			{
				Log.Error("Tried to destroy already-destroyed thing " + this);
			}
			else
			{
				bool spawned = this.Spawned;
				Map map = this.Map;
				if (this.Spawned)
				{
					this.DeSpawn();
				}
				this.mapIndexOrState = (sbyte)(-2);
				if (this.def.DiscardOnDestroyed)
				{
					this.Discard(false);
				}
				CompExplosive compExplosive = this.TryGetComp<CompExplosive>();
				bool flag = compExplosive != null && compExplosive.destroyedThroughDetonation;
				if (spawned && !flag)
				{
					GenLeaving.DoLeavingsFor(this, map, mode);
				}
				if (this.holdingOwner != null)
				{
					this.holdingOwner.Notify_ContainedItemDestroyed(this);
				}
				this.RemoveAllReservationsAndDesignationsOnThis();
				if (!(this is Pawn))
				{
					this.stackCount = 0;
				}
			}
		}

		public virtual void PreTraded(TradeAction action, Pawn playerNegotiator, ITrader trader)
		{
		}

		public virtual void PostGeneratedForTrader(TraderKindDef trader, int forTile, Faction forFaction)
		{
			if (this.def.colorGeneratorInTraderStock != null)
			{
				this.SetColor(this.def.colorGeneratorInTraderStock.NewRandomizedColor(), true);
			}
		}

		public virtual void Notify_MyMapRemoved()
		{
			if (this.def.receivesSignals)
			{
				Find.SignalManager.DeregisterReceiver(this);
			}
			if (!ThingOwnerUtility.AnyParentIs<Pawn>(this))
			{
				this.mapIndexOrState = (sbyte)(-3);
			}
			this.RemoveAllReservationsAndDesignationsOnThis();
		}

		public void ForceSetStateToUnspawned()
		{
			this.mapIndexOrState = (sbyte)(-1);
		}

		public void DecrementMapIndex()
		{
			if (this.mapIndexOrState <= 0)
			{
				Log.Warning("Tried to decrement map index for " + this + ", but mapIndexOrState=" + this.mapIndexOrState);
			}
			else
			{
				this.mapIndexOrState = (sbyte)(this.mapIndexOrState - 1);
			}
		}

		private void RemoveAllReservationsAndDesignationsOnThis()
		{
			if (this.def.category != ThingCategory.Mote)
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					maps[i].reservationManager.ReleaseAllForTarget(this);
					maps[i].physicalInteractionReservationManager.ReleaseAllForTarget(this);
					IAttackTarget attackTarget = this as IAttackTarget;
					if (attackTarget != null)
					{
						maps[i].attackTargetReservationManager.ReleaseAllForTarget(attackTarget);
					}
					maps[i].designationManager.RemoveAllDesignationsOn(this, false);
				}
			}
		}

		public virtual void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.def, "def");
			if (this.def.HasThingIDNumber)
			{
				string thingID = this.ThingID;
				Scribe_Values.Look(ref thingID, "id", (string)null, false);
				this.ThingID = thingID;
			}
			Scribe_Values.Look<sbyte>(ref this.mapIndexOrState, "map", (sbyte)(-1), false);
			if (Scribe.mode == LoadSaveMode.LoadingVars && this.mapIndexOrState >= 0)
			{
				this.mapIndexOrState = (sbyte)(-1);
			}
			Scribe_Values.Look<IntVec3>(ref this.positionInt, "pos", IntVec3.Invalid, false);
			Scribe_Values.Look<Rot4>(ref this.rotationInt, "rot", Rot4.North, false);
			if (this.def.useHitPoints)
			{
				Scribe_Values.Look<int>(ref this.hitPointsInt, "health", -1, false);
			}
			bool flag = this.def.tradeability != 0 && this.def.category == ThingCategory.Item;
			if (this.def.stackLimit > 1 || flag)
			{
				Scribe_Values.Look<int>(ref this.stackCount, "stackCount", 0, true);
			}
			Scribe_Defs.Look<ThingDef>(ref this.stuffInt, "stuff");
			string facID = (this.factionInt == null) ? "null" : this.factionInt.GetUniqueLoadID();
			Scribe_Values.Look(ref facID, "faction", "null", false);
			if (Scribe.mode != LoadSaveMode.LoadingVars && Scribe.mode != LoadSaveMode.ResolvingCrossRefs && Scribe.mode != LoadSaveMode.PostLoadInit)
				return;
			if (facID == "null")
			{
				this.factionInt = null;
			}
			else if (Find.World != null && Find.FactionManager != null)
			{
				this.factionInt = Find.FactionManager.AllFactions.FirstOrDefault((Func<Faction, bool>)((Faction fa) => fa.GetUniqueLoadID() == facID));
			}
		}

		public virtual void PostMapInit()
		{
		}

		public virtual void Draw()
		{
			this.DrawAt(this.DrawPos, false);
		}

		public virtual void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			this.Graphic.Draw(drawLoc, (!flip) ? this.Rotation : this.Rotation.Opposite, this, 0f);
		}

		public virtual void Print(SectionLayer layer)
		{
			this.Graphic.Print(layer, this);
		}

		public void DirtyMapMesh(Map map)
		{
			if (this.def.drawerType != DrawerType.RealtimeOnly)
			{
				CellRect.CellRectIterator iterator = this.OccupiedRect().GetIterator();
				while (!iterator.Done())
				{
					map.mapDrawer.MapMeshDirty(iterator.Current, MapMeshFlag.Things);
					iterator.MoveNext();
				}
			}
		}

		public virtual void DrawGUIOverlay()
		{
			if (Find.CameraDriver.CurrentZoom == CameraZoomRange.Closest)
			{
				QualityCategory cat = default(QualityCategory);
				if (this.def.stackLimit > 1)
				{
					GenMapUI.DrawThingLabel(this, this.stackCount.ToStringCached());
				}
				else if (this.TryGetQuality(out cat))
				{
					GenMapUI.DrawThingLabel(this, cat.GetLabelShort());
				}
			}
		}

		public virtual void DrawExtraSelectionOverlays()
		{
			if (this.def.specialDisplayRadius > 0.10000000149011612)
			{
				GenDraw.DrawRadiusRing(this.Position, this.def.specialDisplayRadius);
			}
			if (this.def.drawPlaceWorkersWhileSelected && this.def.PlaceWorkers != null)
			{
				for (int i = 0; i < this.def.PlaceWorkers.Count; i++)
				{
					this.def.PlaceWorkers[i].DrawGhost(this.def, this.Position, this.Rotation);
				}
			}
			if (this.def.hasInteractionCell)
			{
				GenDraw.DrawInteractionCell(this.def, this.Position, this.rotationInt);
			}
		}

		public virtual string GetInspectString()
		{
			return "";
		}

		public virtual string GetInspectStringLowPriority()
		{
			return (!SteadyAtmosphereEffects.InDeterioratingPosition(this) || !(SteadyAtmosphereEffects.FinalDeteriorationRate(this) > 0.0010000000474974513)) ? null : "DeterioratingDueToBeingUnroofed".Translate();
		}

		public virtual IEnumerable<Gizmo> GetGizmos()
		{
			yield break;
		}

		public virtual IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
		{
			yield break;
		}

		public virtual IEnumerable<InspectTabBase> GetInspectTabs()
		{
			return this.def.inspectorTabsResolved;
		}

		public DamageWorker.DamageResult TakeDamage(DamageInfo dinfo)
		{
			DamageWorker.DamageResult result;
			if (this.Destroyed)
			{
				result = DamageWorker.DamageResult.MakeNew();
			}
			else if (dinfo.Amount == 0)
			{
				result = DamageWorker.DamageResult.MakeNew();
			}
			else
			{
				if (this.def.damageMultipliers != null)
				{
					for (int i = 0; i < this.def.damageMultipliers.Count; i++)
					{
						if (this.def.damageMultipliers[i].damageDef == dinfo.Def)
						{
							int amount = Mathf.RoundToInt((float)dinfo.Amount * this.def.damageMultipliers[i].multiplier);
							dinfo.SetAmount(amount);
						}
					}
				}
				bool flag = default(bool);
				this.PreApplyDamage(dinfo, out flag);
				if (flag)
				{
					result = DamageWorker.DamageResult.MakeNew();
				}
				else
				{
					bool spawnedOrAnyParentSpawned = this.SpawnedOrAnyParentSpawned;
					Map mapHeld = this.MapHeld;
					DamageWorker.DamageResult damageResult = dinfo.Def.Worker.Apply(dinfo, this);
					if (dinfo.Def.harmsHealth && spawnedOrAnyParentSpawned)
					{
						mapHeld.damageWatcher.Notify_DamageTaken(this, damageResult.totalDamageDealt);
					}
					if (dinfo.Def.externalViolence)
					{
						GenLeaving.DropFilthDueToDamage(this, damageResult.totalDamageDealt);
						if (dinfo.Instigator != null)
						{
							Pawn pawn = dinfo.Instigator as Pawn;
							if (pawn != null)
							{
								pawn.records.AddTo(RecordDefOf.DamageDealt, damageResult.totalDamageDealt);
								pawn.records.AccumulateStoryEvent(StoryEventDefOf.DamageDealt);
							}
						}
					}
					this.PostApplyDamage(dinfo, damageResult.totalDamageDealt);
					result = damageResult;
				}
			}
			return result;
		}

		public virtual void PreApplyDamage(DamageInfo dinfo, out bool absorbed)
		{
			absorbed = false;
		}

		public virtual void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
		}

		public virtual bool CanStackWith(Thing other)
		{
			return !this.Destroyed && !other.Destroyed && this.def.category == ThingCategory.Item && this.def == other.def && this.Stuff == other.Stuff;
		}

		public virtual bool TryAbsorbStack(Thing other, bool respectStackLimit)
		{
			bool result;
			if (!this.CanStackWith(other))
			{
				result = false;
			}
			else
			{
				int num = ThingUtility.TryAbsorbStackNumToTake(this, other, respectStackLimit);
				if (this.def.useHitPoints)
				{
					this.HitPoints = Mathf.CeilToInt((float)(this.HitPoints * this.stackCount + other.HitPoints * num) / (float)(this.stackCount + num));
				}
				this.stackCount += num;
				other.stackCount -= num;
				StealAIDebugDrawer.Notify_ThingChanged(this);
				if (other.stackCount <= 0)
				{
					other.Destroy(DestroyMode.Vanish);
					result = true;
				}
				else
				{
					result = false;
				}
			}
			return result;
		}

		public virtual Thing SplitOff(int count)
		{
			if (count <= 0)
			{
				throw new ArgumentException("SplitOff with count <= 0", "count");
			}
			Thing result;
			if (count >= this.stackCount)
			{
				if (count > this.stackCount)
				{
					Log.Error("Tried to split off " + count + " of " + this + " but there are only " + this.stackCount);
				}
				if (this.Spawned)
				{
					this.DeSpawn();
				}
				if (this.holdingOwner != null)
				{
					this.holdingOwner.Remove(this);
				}
				result = this;
			}
			else
			{
				Thing thing = ThingMaker.MakeThing(this.def, this.Stuff);
				thing.stackCount = count;
				this.stackCount -= count;
				if (this.def.useHitPoints)
				{
					thing.HitPoints = this.HitPoints;
				}
				result = thing;
			}
			return result;
		}

		public virtual void Notify_ColorChanged()
		{
			this.graphicInt = null;
		}

		public virtual void Notify_SignalReceived(Signal signal)
		{
		}

		public virtual TipSignal GetTooltip()
		{
			string text = this.LabelCap;
			if (this.def.useHitPoints)
			{
				string text2 = text;
				text = text2 + "\n" + this.HitPoints + " / " + this.MaxHitPoints;
			}
			return new TipSignal(text, this.thingIDNumber * 251235);
		}

		public virtual bool BlocksPawn(Pawn p)
		{
			return this.def.passability == Traversability.Impassable;
		}

		public void SetFactionDirect(Faction newFaction)
		{
			if (!this.def.CanHaveFaction)
			{
				Log.Error("Tried to SetFactionDirect on " + this + " which cannot have a faction.");
			}
			else
			{
				this.factionInt = newFaction;
			}
		}

		public virtual void SetFaction(Faction newFaction, Pawn recruiter = null)
		{
			if (!this.def.CanHaveFaction)
			{
				Log.Error("Tried to SetFaction on " + this + " which cannot have a faction.");
			}
			else
			{
				this.factionInt = newFaction;
				if (this.Spawned)
				{
					IAttackTarget attackTarget = this as IAttackTarget;
					if (attackTarget != null)
					{
						this.Map.attackTargetsCache.UpdateTarget(attackTarget);
					}
				}
			}
		}

		public void SetPositionDirect(IntVec3 newPos)
		{
			this.positionInt = newPos;
		}

		public void SetStuffDirect(ThingDef newStuff)
		{
			this.stuffInt = newStuff;
		}

		public virtual string GetDescription()
		{
			return this.def.description;
		}

		public override string ToString()
		{
			return (this.def == null) ? base.GetType().ToString() : this.ThingID;
		}

		public override int GetHashCode()
		{
			return this.thingIDNumber;
		}

		public virtual void Discard(bool silentlyRemoveReferences = false)
		{
			if (this.mapIndexOrState != -2)
			{
				Log.Warning("Tried to discard " + this + " whose state is " + this.mapIndexOrState + ".");
			}
			else
			{
				this.mapIndexOrState = (sbyte)(-3);
			}
		}

		public virtual IEnumerable<Thing> ButcherProducts(Pawn butcher, float efficiency)
		{
			if (this.def.butcherProducts == null)
				yield break;
			int i = 0;
			ThingCountClass ta;
			int count;
			while (true)
			{
				if (i < this.def.butcherProducts.Count)
				{
					ta = this.def.butcherProducts[i];
					count = GenMath.RoundRandom((float)ta.count * efficiency);
					if (count <= 0)
					{
						i++;
						continue;
					}
					break;
				}
				yield break;
			}
			Thing t = ThingMaker.MakeThing(ta.thingDef, null);
			t.stackCount = count;
			yield return t;
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public virtual IEnumerable<Thing> SmeltProducts(float efficiency)
		{
			List<ThingCountClass> costListAdj = this.def.CostListAdjusted(this.Stuff, true);
			for (int j = 0; j < costListAdj.Count; j++)
			{
				if (!costListAdj[j].thingDef.intricate)
				{
					float countF = (float)((float)costListAdj[j].count * 0.25);
					int count = GenMath.RoundRandom(countF);
					if (count > 0)
					{
						Thing t = ThingMaker.MakeThing(costListAdj[j].thingDef, null);
						t.stackCount = count;
						yield return t;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			if (this.def.smeltProducts != null)
			{
				int i = 0;
				if (i < this.def.smeltProducts.Count)
				{
					ThingCountClass ta = this.def.smeltProducts[i];
					Thing t2 = ThingMaker.MakeThing(ta.thingDef, null);
					t2.stackCount = ta.count;
					yield return t2;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
		}

		public float Ingested(Pawn ingester, float nutritionWanted)
		{
			float result;
			if (this.Destroyed)
			{
				Log.Error(ingester + " ingested destroyed thing " + this);
				result = 0f;
			}
			else if (!this.IngestibleNow)
			{
				Log.Error(ingester + " ingested IngestibleNow=false thing " + this);
				result = 0f;
			}
			else
			{
				ingester.mindState.lastIngestTick = Find.TickManager.TicksGame;
				if (this.def.ingestible.outcomeDoers != null)
				{
					for (int i = 0; i < this.def.ingestible.outcomeDoers.Count; i++)
					{
						this.def.ingestible.outcomeDoers[i].DoIngestionOutcome(ingester, this);
					}
				}
				if (ingester.needs.mood != null)
				{
					List<ThoughtDef> list = FoodUtility.ThoughtsFromIngesting(ingester, this, this.def);
					for (int j = 0; j < list.Count; j++)
					{
						ingester.needs.mood.thoughts.memories.TryGainMemory(list[j], null);
					}
				}
				if (ingester.IsColonist && FoodUtility.IsHumanlikeMeatOrHumanlikeCorpse(this))
				{
					TaleRecorder.RecordTale(TaleDefOf.AteRawHumanlikeMeat, ingester);
				}
				int num = default(int);
				float num2 = default(float);
				this.IngestedCalculateAmounts(ingester, nutritionWanted, out num, out num2);
				if (!ingester.Dead && ingester.needs.joy != null && Mathf.Abs(this.def.ingestible.joy) > 9.9999997473787516E-05 && num > 0)
				{
					JoyKindDef joyKind = (this.def.ingestible.joyKind == null) ? JoyKindDefOf.Gluttonous : this.def.ingestible.joyKind;
					ingester.needs.joy.GainJoy((float)num * this.def.ingestible.joy, joyKind);
				}
				if (ingester.IsCaravanMember())
				{
					CaravanPawnsNeedsUtility.Notify_CaravanMemberIngestedFood(ingester, this.def);
				}
				if (num > 0)
				{
					if (num == this.stackCount)
					{
						this.Destroy(DestroyMode.Vanish);
					}
					else
					{
						this.SplitOff(num);
					}
				}
				this.PostIngested(ingester);
				result = num2;
			}
			return result;
		}

		protected virtual void PostIngested(Pawn ingester)
		{
		}

		protected virtual void IngestedCalculateAmounts(Pawn ingester, float nutritionWanted, out int numTaken, out float nutritionIngested)
		{
			numTaken = Mathf.CeilToInt(nutritionWanted / this.def.ingestible.nutrition);
			numTaken = Mathf.Min(numTaken, this.def.ingestible.maxNumToIngestAtOnce, this.stackCount);
			numTaken = Mathf.Max(numTaken, 1);
			nutritionIngested = (float)numTaken * this.def.ingestible.nutrition;
		}

		public virtual bool PreventPlayerSellingThingsNearby(out string reason)
		{
			reason = (string)null;
			return false;
		}
	}
}
