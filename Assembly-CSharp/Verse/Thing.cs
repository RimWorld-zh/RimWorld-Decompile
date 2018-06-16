using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000DF6 RID: 3574
	public class Thing : Entity, IExposable, ISelectable, ILoadReferenceable, ISignalReceiver
	{
		// Token: 0x17000D07 RID: 3335
		// (get) Token: 0x06005003 RID: 20483 RVA: 0x0012556C File Offset: 0x0012396C
		// (set) Token: 0x06005004 RID: 20484 RVA: 0x00125587 File Offset: 0x00123987
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

		// Token: 0x17000D08 RID: 3336
		// (get) Token: 0x06005005 RID: 20485 RVA: 0x00125594 File Offset: 0x00123994
		public int MaxHitPoints
		{
			get
			{
				return Mathf.RoundToInt(this.GetStatValue(StatDefOf.MaxHitPoints, true));
			}
		}

		// Token: 0x17000D09 RID: 3337
		// (get) Token: 0x06005006 RID: 20486 RVA: 0x001255BC File Offset: 0x001239BC
		public float MarketValue
		{
			get
			{
				return this.GetStatValue(StatDefOf.MarketValue, true);
			}
		}

		// Token: 0x17000D0A RID: 3338
		// (get) Token: 0x06005007 RID: 20487 RVA: 0x001255E0 File Offset: 0x001239E0
		public bool FlammableNow
		{
			get
			{
				bool result;
				if (this.GetStatValue(StatDefOf.Flammability, true) < 0.01f)
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
								{
									return false;
								}
							}
						}
					}
					result = true;
				}
				return result;
			}
		}

		// Token: 0x17000D0B RID: 3339
		// (get) Token: 0x06005008 RID: 20488 RVA: 0x00125678 File Offset: 0x00123A78
		public virtual bool FireBulwark
		{
			get
			{
				return this.def.Fillage == FillCategory.Full;
			}
		}

		// Token: 0x17000D0C RID: 3340
		// (get) Token: 0x06005009 RID: 20489 RVA: 0x0012569C File Offset: 0x00123A9C
		public bool Destroyed
		{
			get
			{
				return (int)this.mapIndexOrState == -2 || (int)this.mapIndexOrState == -3;
			}
		}

		// Token: 0x17000D0D RID: 3341
		// (get) Token: 0x0600500A RID: 20490 RVA: 0x001256D0 File Offset: 0x00123AD0
		public bool Discarded
		{
			get
			{
				return (int)this.mapIndexOrState == -3;
			}
		}

		// Token: 0x17000D0E RID: 3342
		// (get) Token: 0x0600500B RID: 20491 RVA: 0x001256F0 File Offset: 0x00123AF0
		public bool Spawned
		{
			get
			{
				bool result;
				if ((int)this.mapIndexOrState < 0)
				{
					result = false;
				}
				else if ((int)this.mapIndexOrState < Find.Maps.Count)
				{
					result = true;
				}
				else
				{
					Log.ErrorOnce("Thing is associated with invalid map index", 64664487, false);
					result = false;
				}
				return result;
			}
		}

		// Token: 0x17000D0F RID: 3343
		// (get) Token: 0x0600500C RID: 20492 RVA: 0x00125748 File Offset: 0x00123B48
		public bool SpawnedOrAnyParentSpawned
		{
			get
			{
				return this.SpawnedParentOrMe != null;
			}
		}

		// Token: 0x17000D10 RID: 3344
		// (get) Token: 0x0600500D RID: 20493 RVA: 0x0012576C File Offset: 0x00123B6C
		public Thing SpawnedParentOrMe
		{
			get
			{
				Thing result;
				if (this.Spawned)
				{
					result = this;
				}
				else if (this.ParentHolder != null)
				{
					result = ThingOwnerUtility.SpawnedParentOrMe(this.ParentHolder);
				}
				else
				{
					result = null;
				}
				return result;
			}
		}

		// Token: 0x17000D11 RID: 3345
		// (get) Token: 0x0600500E RID: 20494 RVA: 0x001257B0 File Offset: 0x00123BB0
		public Map Map
		{
			get
			{
				Map result;
				if ((int)this.mapIndexOrState >= 0)
				{
					result = Find.Maps[(int)this.mapIndexOrState];
				}
				else
				{
					result = null;
				}
				return result;
			}
		}

		// Token: 0x17000D12 RID: 3346
		// (get) Token: 0x0600500F RID: 20495 RVA: 0x001257EC File Offset: 0x00123BEC
		public Map MapHeld
		{
			get
			{
				Map result;
				if (this.Spawned)
				{
					result = this.Map;
				}
				else if (this.ParentHolder != null)
				{
					result = ThingOwnerUtility.GetRootMap(this.ParentHolder);
				}
				else
				{
					result = null;
				}
				return result;
			}
		}

		// Token: 0x17000D13 RID: 3347
		// (get) Token: 0x06005010 RID: 20496 RVA: 0x00125838 File Offset: 0x00123C38
		// (set) Token: 0x06005011 RID: 20497 RVA: 0x00125854 File Offset: 0x00123C54
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
							Log.Warning("Changed position of a spawned thing which affects regions. This is not supported.", false);
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

		// Token: 0x17000D14 RID: 3348
		// (get) Token: 0x06005012 RID: 20498 RVA: 0x00125928 File Offset: 0x00123D28
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
					if (rootPosition.IsValid)
					{
						result = rootPosition;
					}
					else
					{
						result = this.Position;
					}
				}
				return result;
			}
		}

		// Token: 0x17000D15 RID: 3349
		// (get) Token: 0x06005013 RID: 20499 RVA: 0x0012597C File Offset: 0x00123D7C
		// (set) Token: 0x06005014 RID: 20500 RVA: 0x00125998 File Offset: 0x00123D98
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
							Log.Warning("Changed rotation of a spawned non-single-cell thing which affects regions. This is not supported.", false);
						}
						RegionListersUpdater.DeregisterInRegions(this, this.Map);
						this.Map.thingGrid.Deregister(this, false);
					}
					this.rotationInt = value;
					if (this.Spawned && (this.def.size.x != 1 || this.def.size.z != 1))
					{
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

		// Token: 0x17000D16 RID: 3350
		// (get) Token: 0x06005015 RID: 20501 RVA: 0x00125AAC File Offset: 0x00123EAC
		public bool Smeltable
		{
			get
			{
				return this.def.smeltable && (!this.def.MadeFromStuff || this.Stuff.stuffProps.smeltable);
			}
		}

		// Token: 0x17000D17 RID: 3351
		// (get) Token: 0x06005016 RID: 20502 RVA: 0x00125AF8 File Offset: 0x00123EF8
		public IThingHolder ParentHolder
		{
			get
			{
				return (this.holdingOwner == null) ? null : this.holdingOwner.Owner;
			}
		}

		// Token: 0x17000D18 RID: 3352
		// (get) Token: 0x06005017 RID: 20503 RVA: 0x00125B2C File Offset: 0x00123F2C
		public Faction Faction
		{
			get
			{
				return this.factionInt;
			}
		}

		// Token: 0x17000D19 RID: 3353
		// (get) Token: 0x06005018 RID: 20504 RVA: 0x00125B48 File Offset: 0x00123F48
		// (set) Token: 0x06005019 RID: 20505 RVA: 0x00125B9F File Offset: 0x00123F9F
		public string ThingID
		{
			get
			{
				string result;
				if (this.def.HasThingIDNumber)
				{
					result = this.def.defName + this.thingIDNumber.ToString();
				}
				else
				{
					result = this.def.defName;
				}
				return result;
			}
			set
			{
				this.thingIDNumber = Thing.IDNumberFromThingID(value);
			}
		}

		// Token: 0x0600501A RID: 20506 RVA: 0x00125BB0 File Offset: 0x00123FB0
		public string UniqueVerbOwnerID()
		{
			return this.ThingID;
		}

		// Token: 0x0600501B RID: 20507 RVA: 0x00125BCC File Offset: 0x00123FCC
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
				Log.Error(string.Concat(new string[]
				{
					"Could not convert id number from thingID=",
					thingID,
					", numString=",
					value,
					" Exception=",
					ex.ToString()
				}), false);
			}
			return result;
		}

		// Token: 0x17000D1A RID: 3354
		// (get) Token: 0x0600501C RID: 20508 RVA: 0x00125C58 File Offset: 0x00124058
		public IntVec2 RotatedSize
		{
			get
			{
				IntVec2 result;
				if (!this.rotationInt.IsHorizontal)
				{
					result = this.def.size;
				}
				else
				{
					result = new IntVec2(this.def.size.z, this.def.size.x);
				}
				return result;
			}
		}

		// Token: 0x17000D1B RID: 3355
		// (get) Token: 0x0600501D RID: 20509 RVA: 0x00125CB4 File Offset: 0x001240B4
		public override string Label
		{
			get
			{
				string result;
				if (this.stackCount > 1)
				{
					result = this.LabelNoCount + " x" + this.stackCount.ToStringCached();
				}
				else
				{
					result = this.LabelNoCount;
				}
				return result;
			}
		}

		// Token: 0x17000D1C RID: 3356
		// (get) Token: 0x0600501E RID: 20510 RVA: 0x00125CFC File Offset: 0x001240FC
		public virtual string LabelNoCount
		{
			get
			{
				return GenLabel.ThingLabel(this, 1, true);
			}
		}

		// Token: 0x17000D1D RID: 3357
		// (get) Token: 0x0600501F RID: 20511 RVA: 0x00125D1C File Offset: 0x0012411C
		public override string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		// Token: 0x17000D1E RID: 3358
		// (get) Token: 0x06005020 RID: 20512 RVA: 0x00125D3C File Offset: 0x0012413C
		public virtual string LabelCapNoCount
		{
			get
			{
				return this.LabelNoCount.CapitalizeFirst();
			}
		}

		// Token: 0x17000D1F RID: 3359
		// (get) Token: 0x06005021 RID: 20513 RVA: 0x00125D5C File Offset: 0x0012415C
		public override string LabelShort
		{
			get
			{
				return this.LabelNoCount;
			}
		}

		// Token: 0x17000D20 RID: 3360
		// (get) Token: 0x06005022 RID: 20514 RVA: 0x00125D78 File Offset: 0x00124178
		public virtual bool IngestibleNow
		{
			get
			{
				return !this.IsBurning() && this.def.IsIngestible;
			}
		}

		// Token: 0x17000D21 RID: 3361
		// (get) Token: 0x06005023 RID: 20515 RVA: 0x00125DAC File Offset: 0x001241AC
		public ThingDef Stuff
		{
			get
			{
				return this.stuffInt;
			}
		}

		// Token: 0x17000D22 RID: 3362
		// (get) Token: 0x06005024 RID: 20516 RVA: 0x00125DC8 File Offset: 0x001241C8
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x17000D23 RID: 3363
		// (get) Token: 0x06005025 RID: 20517 RVA: 0x00125DEC File Offset: 0x001241EC
		public Graphic DefaultGraphic
		{
			get
			{
				if (this.graphicInt == null)
				{
					if (this.def.graphicData == null)
					{
						Log.ErrorOnce(this.def + " has no graphicData but we are trying to access it.", 764532, false);
						return BaseContent.BadGraphic;
					}
					this.graphicInt = this.def.graphicData.GraphicColoredFor(this);
				}
				return this.graphicInt;
			}
		}

		// Token: 0x17000D24 RID: 3364
		// (get) Token: 0x06005026 RID: 20518 RVA: 0x00125E64 File Offset: 0x00124264
		public virtual Graphic Graphic
		{
			get
			{
				return this.DefaultGraphic;
			}
		}

		// Token: 0x17000D25 RID: 3365
		// (get) Token: 0x06005027 RID: 20519 RVA: 0x00125E80 File Offset: 0x00124280
		public virtual IntVec3 InteractionCell
		{
			get
			{
				return ThingUtility.InteractionCellWhenAt(this.def, this.Position, this.Rotation, this.Map);
			}
		}

		// Token: 0x17000D26 RID: 3366
		// (get) Token: 0x06005028 RID: 20520 RVA: 0x00125EB4 File Offset: 0x001242B4
		public float AmbientTemperature
		{
			get
			{
				float result;
				if (this.Spawned)
				{
					result = GenTemperature.GetTemperatureForCell(this.Position, this.Map);
				}
				else
				{
					if (this.ParentHolder != null)
					{
						for (IThingHolder parentHolder = this.ParentHolder; parentHolder != null; parentHolder = parentHolder.ParentHolder)
						{
							float result2;
							if (ThingOwnerUtility.TryGetFixedTemperature(parentHolder, this, out result2))
							{
								return result2;
							}
						}
					}
					if (this.SpawnedOrAnyParentSpawned)
					{
						result = GenTemperature.GetTemperatureForCell(this.PositionHeld, this.MapHeld);
					}
					else if (this.Tile >= 0)
					{
						result = GenTemperature.GetTemperatureAtTile(this.Tile);
					}
					else
					{
						result = 21f;
					}
				}
				return result;
			}
		}

		// Token: 0x17000D27 RID: 3367
		// (get) Token: 0x06005029 RID: 20521 RVA: 0x00125F6C File Offset: 0x0012436C
		public int Tile
		{
			get
			{
				int result;
				if (this.Spawned)
				{
					result = this.Map.Tile;
				}
				else if (this.ParentHolder != null)
				{
					result = ThingOwnerUtility.GetRootTile(this.ParentHolder);
				}
				else
				{
					result = -1;
				}
				return result;
			}
		}

		// Token: 0x17000D28 RID: 3368
		// (get) Token: 0x0600502A RID: 20522 RVA: 0x00125FBC File Offset: 0x001243BC
		public bool Suspended
		{
			get
			{
				return !this.Spawned && this.ParentHolder != null && ThingOwnerUtility.ContentsSuspended(this.ParentHolder);
			}
		}

		// Token: 0x17000D29 RID: 3369
		// (get) Token: 0x0600502B RID: 20523 RVA: 0x00126000 File Offset: 0x00124400
		public virtual string DescriptionDetailed
		{
			get
			{
				return this.def.DescriptionDetailed;
			}
		}

		// Token: 0x17000D2A RID: 3370
		// (get) Token: 0x0600502C RID: 20524 RVA: 0x00126020 File Offset: 0x00124420
		public virtual string DescriptionFlavor
		{
			get
			{
				return this.def.description;
			}
		}

		// Token: 0x0600502D RID: 20525 RVA: 0x00126040 File Offset: 0x00124440
		public virtual void PostMake()
		{
			ThingIDMaker.GiveIDTo(this);
			if (this.def.useHitPoints)
			{
				this.HitPoints = Mathf.RoundToInt((float)this.MaxHitPoints * Mathf.Clamp01(this.def.startingHpRange.RandomInRange));
			}
		}

		// Token: 0x0600502E RID: 20526 RVA: 0x0012608C File Offset: 0x0012448C
		public string GetUniqueLoadID()
		{
			return "Thing_" + this.ThingID;
		}

		// Token: 0x0600502F RID: 20527 RVA: 0x001260B4 File Offset: 0x001244B4
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (this.Destroyed)
			{
				Log.Error(string.Concat(new object[]
				{
					"Spawning destroyed thing ",
					this,
					" at ",
					this.Position,
					". Correcting."
				}), false);
				this.mapIndexOrState = -1;
				if (this.HitPoints <= 0 && this.def.useHitPoints)
				{
					this.HitPoints = 1;
				}
			}
			if (this.Spawned)
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to spawn already-spawned thing ",
					this,
					" at ",
					this.Position
				}), false);
			}
			else
			{
				int num = Find.Maps.IndexOf(map);
				if (num < 0)
				{
					Log.Error("Tried to spawn thing " + this + ", but the map provided does not exist.", false);
				}
				else
				{
					if (this.stackCount > this.def.stackLimit)
					{
						Log.Error(string.Concat(new object[]
						{
							"Spawned ",
							this,
							" with stackCount ",
							this.stackCount,
							" but stackLimit is ",
							this.def.stackLimit,
							". Truncating."
						}), false);
						this.stackCount = this.def.stackLimit;
					}
					this.mapIndexOrState = (sbyte)num;
					RegionListersUpdater.RegisterInRegions(this, map);
					if (!map.spawnedThings.TryAdd(this, false))
					{
						Log.Error("Couldn't add thing " + this + " to spawned things.", false);
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
						map.listerMergeables.Notify_Spawned(this);
					}
					map.attackTargetsCache.Notify_ThingSpawned(this);
					Region validRegionAt_NoRebuild = map.regionGrid.GetValidRegionAt_NoRebuild(this.Position);
					Room room = (validRegionAt_NoRebuild != null) ? validRegionAt_NoRebuild.Room : null;
					if (room != null)
					{
						room.Notify_ContainedThingSpawnedOrDespawned(this);
					}
					StealAIDebugDrawer.Notify_ThingChanged(this);
					IHaulDestination haulDestination = this as IHaulDestination;
					if (haulDestination != null)
					{
						map.haulDestinationManager.AddHaulDestination(haulDestination);
					}
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

		// Token: 0x06005030 RID: 20528 RVA: 0x00126490 File Offset: 0x00124890
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			if (this.Destroyed)
			{
				Log.Error("Tried to despawn " + this.ToStringSafe<Thing>() + " which is already destroyed.", false);
			}
			else if (!this.Spawned)
			{
				Log.Error("Tried to despawn " + this.ToStringSafe<Thing>() + " which is not spawned.", false);
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
				this.mapIndexOrState = -1;
				if (this.def.category == ThingCategory.Item)
				{
					map.listerHaulables.Notify_DeSpawned(this);
					map.listerMergeables.Notify_DeSpawned(this);
				}
				map.attackTargetsCache.Notify_ThingDespawned(this);
				map.physicalInteractionReservationManager.ReleaseAllForTarget(this);
				StealAIDebugDrawer.Notify_ThingChanged(this);
				IHaulDestination haulDestination = this as IHaulDestination;
				if (haulDestination != null)
				{
					map.haulDestinationManager.RemoveHaulDestination(haulDestination);
				}
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

		// Token: 0x06005031 RID: 20529 RVA: 0x00126742 File Offset: 0x00124B42
		public virtual void Kill(DamageInfo? dinfo = null, Hediff exactCulprit = null)
		{
			this.Destroy(DestroyMode.KillFinalize);
		}

		// Token: 0x06005032 RID: 20530 RVA: 0x0012674C File Offset: 0x00124B4C
		public virtual void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (!Thing.allowDestroyNonDestroyable)
			{
				if (!this.def.destroyable)
				{
					Log.Error("Tried to destroy non-destroyable thing " + this, false);
					return;
				}
			}
			if (this.Destroyed)
			{
				Log.Error("Tried to destroy already-destroyed thing " + this, false);
			}
			else
			{
				bool spawned = this.Spawned;
				Map map = this.Map;
				if (this.Spawned)
				{
					this.DeSpawn(mode);
				}
				this.mapIndexOrState = -2;
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

		// Token: 0x06005033 RID: 20531 RVA: 0x00126848 File Offset: 0x00124C48
		public virtual void PreTraded(TradeAction action, Pawn playerNegotiator, ITrader trader)
		{
		}

		// Token: 0x06005034 RID: 20532 RVA: 0x0012684B File Offset: 0x00124C4B
		public virtual void PostGeneratedForTrader(TraderKindDef trader, int forTile, Faction forFaction)
		{
			if (this.def.colorGeneratorInTraderStock != null)
			{
				this.SetColor(this.def.colorGeneratorInTraderStock.NewRandomizedColor(), true);
			}
		}

		// Token: 0x06005035 RID: 20533 RVA: 0x00126875 File Offset: 0x00124C75
		public virtual void Notify_MyMapRemoved()
		{
			if (this.def.receivesSignals)
			{
				Find.SignalManager.DeregisterReceiver(this);
			}
			if (!ThingOwnerUtility.AnyParentIs<Pawn>(this))
			{
				this.mapIndexOrState = -3;
			}
			this.RemoveAllReservationsAndDesignationsOnThis();
		}

		// Token: 0x06005036 RID: 20534 RVA: 0x001268AC File Offset: 0x00124CAC
		public void ForceSetStateToUnspawned()
		{
			this.mapIndexOrState = -1;
		}

		// Token: 0x06005037 RID: 20535 RVA: 0x001268B8 File Offset: 0x00124CB8
		public void DecrementMapIndex()
		{
			if ((int)this.mapIndexOrState <= 0)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to decrement map index for ",
					this,
					", but mapIndexOrState=",
					this.mapIndexOrState
				}), false);
			}
			else
			{
				this.mapIndexOrState = (sbyte)((int)this.mapIndexOrState - 1);
			}
		}

		// Token: 0x06005038 RID: 20536 RVA: 0x0012691C File Offset: 0x00124D1C
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

		// Token: 0x06005039 RID: 20537 RVA: 0x001269BC File Offset: 0x00124DBC
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.def, "def");
			if (this.def.HasThingIDNumber)
			{
				string thingID = this.ThingID;
				Scribe_Values.Look<string>(ref thingID, "id", null, false);
				this.ThingID = thingID;
			}
			Scribe_Values.Look<sbyte>(ref this.mapIndexOrState, "map", -1, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars && (int)this.mapIndexOrState >= 0)
			{
				this.mapIndexOrState = -1;
			}
			Scribe_Values.Look<IntVec3>(ref this.positionInt, "pos", IntVec3.Invalid, false);
			Scribe_Values.Look<Rot4>(ref this.rotationInt, "rot", Rot4.North, false);
			if (this.def.useHitPoints)
			{
				Scribe_Values.Look<int>(ref this.hitPointsInt, "health", -1, false);
			}
			bool flag = this.def.tradeability != Tradeability.None && this.def.category == ThingCategory.Item;
			if (this.def.stackLimit > 1 || flag)
			{
				Scribe_Values.Look<int>(ref this.stackCount, "stackCount", 0, true);
			}
			Scribe_Defs.Look<ThingDef>(ref this.stuffInt, "stuff");
			string facID = (this.factionInt == null) ? "null" : this.factionInt.GetUniqueLoadID();
			Scribe_Values.Look<string>(ref facID, "faction", "null", false);
			if (Scribe.mode == LoadSaveMode.LoadingVars || Scribe.mode == LoadSaveMode.ResolvingCrossRefs || Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (facID == "null")
				{
					this.factionInt = null;
				}
				else if (Find.World != null && Find.FactionManager != null)
				{
					this.factionInt = Find.FactionManager.AllFactions.FirstOrDefault((Faction fa) => fa.GetUniqueLoadID() == facID);
				}
			}
		}

		// Token: 0x0600503A RID: 20538 RVA: 0x00126B9D File Offset: 0x00124F9D
		public virtual void PostMapInit()
		{
		}

		// Token: 0x17000D2B RID: 3371
		// (get) Token: 0x0600503B RID: 20539 RVA: 0x00126BA0 File Offset: 0x00124FA0
		public virtual Vector3 DrawPos
		{
			get
			{
				return this.TrueCenter();
			}
		}

		// Token: 0x17000D2C RID: 3372
		// (get) Token: 0x0600503C RID: 20540 RVA: 0x00126BBC File Offset: 0x00124FBC
		// (set) Token: 0x0600503D RID: 20541 RVA: 0x00126C20 File Offset: 0x00125020
		public virtual Color DrawColor
		{
			get
			{
				Color result;
				if (this.Stuff != null)
				{
					result = this.Stuff.stuffProps.color;
				}
				else if (this.def.graphicData != null)
				{
					result = this.def.graphicData.color;
				}
				else
				{
					result = Color.white;
				}
				return result;
			}
			set
			{
				Log.Error(string.Concat(new object[]
				{
					"Cannot set instance color on non-ThingWithComps ",
					this.LabelCap,
					" at ",
					this.Position,
					"."
				}), false);
			}
		}

		// Token: 0x17000D2D RID: 3373
		// (get) Token: 0x0600503E RID: 20542 RVA: 0x00126C70 File Offset: 0x00125070
		public virtual Color DrawColorTwo
		{
			get
			{
				Color result;
				if (this.def.graphicData != null)
				{
					result = this.def.graphicData.colorTwo;
				}
				else
				{
					result = Color.white;
				}
				return result;
			}
		}

		// Token: 0x0600503F RID: 20543 RVA: 0x00126CB0 File Offset: 0x001250B0
		public virtual void Draw()
		{
			this.DrawAt(this.DrawPos, false);
		}

		// Token: 0x06005040 RID: 20544 RVA: 0x00126CC0 File Offset: 0x001250C0
		public virtual void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			this.Graphic.Draw(drawLoc, (!flip) ? this.Rotation : this.Rotation.Opposite, this, 0f);
		}

		// Token: 0x06005041 RID: 20545 RVA: 0x00126CFF File Offset: 0x001250FF
		public virtual void Print(SectionLayer layer)
		{
			this.Graphic.Print(layer, this);
		}

		// Token: 0x06005042 RID: 20546 RVA: 0x00126D10 File Offset: 0x00125110
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

		// Token: 0x06005043 RID: 20547 RVA: 0x00126D70 File Offset: 0x00125170
		public virtual void DrawGUIOverlay()
		{
			if (Find.CameraDriver.CurrentZoom == CameraZoomRange.Closest)
			{
				QualityCategory cat;
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

		// Token: 0x06005044 RID: 20548 RVA: 0x00126DD4 File Offset: 0x001251D4
		public virtual void DrawExtraSelectionOverlays()
		{
			if (this.def.specialDisplayRadius > 0.1f)
			{
				GenDraw.DrawRadiusRing(this.Position, this.def.specialDisplayRadius);
			}
			if (this.def.drawPlaceWorkersWhileSelected && this.def.PlaceWorkers != null)
			{
				for (int i = 0; i < this.def.PlaceWorkers.Count; i++)
				{
					this.def.PlaceWorkers[i].DrawGhost(this.def, this.Position, this.Rotation, Color.white);
				}
			}
			if (this.def.hasInteractionCell)
			{
				GenDraw.DrawInteractionCell(this.def, this.Position, this.rotationInt);
			}
		}

		// Token: 0x06005045 RID: 20549 RVA: 0x00126EA8 File Offset: 0x001252A8
		public virtual string GetInspectString()
		{
			return "";
		}

		// Token: 0x06005046 RID: 20550 RVA: 0x00126EC4 File Offset: 0x001252C4
		public virtual string GetInspectStringLowPriority()
		{
			string result = null;
			Thing.tmpDeteriorationReasons.Clear();
			SteadyEnvironmentEffects.FinalDeteriorationRate(this, Thing.tmpDeteriorationReasons);
			if (Thing.tmpDeteriorationReasons.Count != 0)
			{
				result = string.Format("{0}: {1}", "DeterioratingBecauseOf".Translate(), Thing.tmpDeteriorationReasons.ToCommaList(false).CapitalizeFirst());
			}
			return result;
		}

		// Token: 0x06005047 RID: 20551 RVA: 0x00126F28 File Offset: 0x00125328
		public virtual IEnumerable<Gizmo> GetGizmos()
		{
			yield break;
		}

		// Token: 0x06005048 RID: 20552 RVA: 0x00126F4C File Offset: 0x0012534C
		public virtual IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
		{
			yield break;
		}

		// Token: 0x06005049 RID: 20553 RVA: 0x00126F70 File Offset: 0x00125370
		public virtual IEnumerable<InspectTabBase> GetInspectTabs()
		{
			return this.def.inspectorTabsResolved;
		}

		// Token: 0x0600504A RID: 20554 RVA: 0x00126F90 File Offset: 0x00125390
		public virtual string GetCustomLabelNoCount(bool includeHp = true)
		{
			return GenLabel.ThingLabel(this, 1, includeHp);
		}

		// Token: 0x0600504B RID: 20555 RVA: 0x00126FB0 File Offset: 0x001253B0
		public DamageWorker.DamageResult TakeDamage(DamageInfo dinfo)
		{
			DamageWorker.DamageResult result;
			if (this.Destroyed)
			{
				result = new DamageWorker.DamageResult();
			}
			else if (dinfo.Amount == 0f)
			{
				result = new DamageWorker.DamageResult();
			}
			else
			{
				if (this.def.damageMultipliers != null)
				{
					for (int i = 0; i < this.def.damageMultipliers.Count; i++)
					{
						if (this.def.damageMultipliers[i].damageDef == dinfo.Def)
						{
							int num = Mathf.RoundToInt(dinfo.Amount * this.def.damageMultipliers[i].multiplier);
							dinfo.SetAmount((float)num);
						}
					}
				}
				bool flag;
				this.PreApplyDamage(ref dinfo, out flag);
				if (flag)
				{
					result = new DamageWorker.DamageResult();
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

		// Token: 0x0600504C RID: 20556 RVA: 0x00127167 File Offset: 0x00125567
		public virtual void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			absorbed = false;
		}

		// Token: 0x0600504D RID: 20557 RVA: 0x0012716D File Offset: 0x0012556D
		public virtual void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
		}

		// Token: 0x0600504E RID: 20558 RVA: 0x00127170 File Offset: 0x00125570
		public virtual bool CanStackWith(Thing other)
		{
			return !this.Destroyed && !other.Destroyed && this.def.category == ThingCategory.Item && this.def == other.def && this.Stuff == other.Stuff;
		}

		// Token: 0x0600504F RID: 20559 RVA: 0x001271DC File Offset: 0x001255DC
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
				if (this.Spawned)
				{
					this.Map.listerMergeables.Notify_ThingStackChanged(this);
				}
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

		// Token: 0x06005050 RID: 20560 RVA: 0x001272A4 File Offset: 0x001256A4
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
					Log.Error(string.Concat(new object[]
					{
						"Tried to split off ",
						count,
						" of ",
						this,
						" but there are only ",
						this.stackCount
					}), false);
				}
				if (this.Spawned)
				{
					this.DeSpawn(DestroyMode.Vanish);
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
				if (this.Spawned)
				{
					this.Map.listerMergeables.Notify_ThingStackChanged(this);
				}
				if (this.def.useHitPoints)
				{
					thing.HitPoints = this.HitPoints;
				}
				result = thing;
			}
			return result;
		}

		// Token: 0x06005051 RID: 20561 RVA: 0x001273C0 File Offset: 0x001257C0
		public virtual void Notify_ColorChanged()
		{
			this.graphicInt = null;
			if (this.Spawned && (this.def.drawerType == DrawerType.MapMeshOnly || this.def.drawerType == DrawerType.MapMeshAndRealTime))
			{
				this.Map.mapDrawer.MapMeshDirty(this.Position, MapMeshFlag.Things);
			}
		}

		// Token: 0x06005052 RID: 20562 RVA: 0x0012741B File Offset: 0x0012581B
		public virtual void Notify_SignalReceived(Signal signal)
		{
		}

		// Token: 0x06005053 RID: 20563 RVA: 0x00127420 File Offset: 0x00125820
		public virtual TipSignal GetTooltip()
		{
			string text = this.LabelCap;
			if (this.def.useHitPoints)
			{
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					"\n",
					this.HitPoints,
					" / ",
					this.MaxHitPoints
				});
			}
			return new TipSignal(text, this.thingIDNumber * 251235);
		}

		// Token: 0x06005054 RID: 20564 RVA: 0x0012749C File Offset: 0x0012589C
		public virtual bool BlocksPawn(Pawn p)
		{
			return this.def.passability == Traversability.Impassable;
		}

		// Token: 0x06005055 RID: 20565 RVA: 0x001274BF File Offset: 0x001258BF
		public void SetFactionDirect(Faction newFaction)
		{
			if (!this.def.CanHaveFaction)
			{
				Log.Error("Tried to SetFactionDirect on " + this + " which cannot have a faction.", false);
			}
			else
			{
				this.factionInt = newFaction;
			}
		}

		// Token: 0x06005056 RID: 20566 RVA: 0x001274F8 File Offset: 0x001258F8
		public virtual void SetFaction(Faction newFaction, Pawn recruiter = null)
		{
			if (!this.def.CanHaveFaction)
			{
				Log.Error("Tried to SetFaction on " + this + " which cannot have a faction.", false);
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

		// Token: 0x06005057 RID: 20567 RVA: 0x00127564 File Offset: 0x00125964
		public void SetPositionDirect(IntVec3 newPos)
		{
			this.positionInt = newPos;
		}

		// Token: 0x06005058 RID: 20568 RVA: 0x0012756E File Offset: 0x0012596E
		public void SetStuffDirect(ThingDef newStuff)
		{
			this.stuffInt = newStuff;
		}

		// Token: 0x06005059 RID: 20569 RVA: 0x00127578 File Offset: 0x00125978
		public override string ToString()
		{
			string result;
			if (this.def != null)
			{
				result = this.ThingID;
			}
			else
			{
				result = base.GetType().ToString();
			}
			return result;
		}

		// Token: 0x0600505A RID: 20570 RVA: 0x001275B0 File Offset: 0x001259B0
		public override int GetHashCode()
		{
			return this.thingIDNumber;
		}

		// Token: 0x0600505B RID: 20571 RVA: 0x001275CC File Offset: 0x001259CC
		public virtual void Discard(bool silentlyRemoveReferences = false)
		{
			if ((int)this.mapIndexOrState != -2)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to discard ",
					this,
					" whose state is ",
					this.mapIndexOrState,
					"."
				}), false);
			}
			else
			{
				this.mapIndexOrState = -3;
			}
		}

		// Token: 0x0600505C RID: 20572 RVA: 0x00127634 File Offset: 0x00125A34
		public virtual IEnumerable<Thing> ButcherProducts(Pawn butcher, float efficiency)
		{
			if (this.def.butcherProducts != null)
			{
				for (int i = 0; i < this.def.butcherProducts.Count; i++)
				{
					ThingDefCountClass ta = this.def.butcherProducts[i];
					int count = GenMath.RoundRandom((float)ta.count * efficiency);
					if (count > 0)
					{
						Thing t = ThingMaker.MakeThing(ta.thingDef, null);
						t.stackCount = count;
						yield return t;
					}
				}
			}
			yield break;
		}

		// Token: 0x0600505D RID: 20573 RVA: 0x00127668 File Offset: 0x00125A68
		public virtual IEnumerable<Thing> SmeltProducts(float efficiency)
		{
			List<ThingDefCountClass> costListAdj = this.def.CostListAdjusted(this.Stuff, true);
			for (int i = 0; i < costListAdj.Count; i++)
			{
				if (!costListAdj[i].thingDef.intricate)
				{
					float countF = (float)costListAdj[i].count * 0.25f;
					int count = GenMath.RoundRandom(countF);
					if (count > 0)
					{
						Thing t = ThingMaker.MakeThing(costListAdj[i].thingDef, null);
						t.stackCount = count;
						yield return t;
					}
				}
			}
			if (this.def.smeltProducts != null)
			{
				for (int j = 0; j < this.def.smeltProducts.Count; j++)
				{
					ThingDefCountClass ta = this.def.smeltProducts[j];
					Thing t2 = ThingMaker.MakeThing(ta.thingDef, null);
					t2.stackCount = ta.count;
					yield return t2;
				}
			}
			yield break;
		}

		// Token: 0x0600505E RID: 20574 RVA: 0x00127694 File Offset: 0x00125A94
		public float Ingested(Pawn ingester, float nutritionWanted)
		{
			float result;
			if (this.Destroyed)
			{
				Log.Error(ingester + " ingested destroyed thing " + this, false);
				result = 0f;
			}
			else if (!this.IngestibleNow)
			{
				Log.Error(ingester + " ingested IngestibleNow=false thing " + this, false);
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
					TaleRecorder.RecordTale(TaleDefOf.AteRawHumanlikeMeat, new object[]
					{
						ingester
					});
				}
				int num;
				float num2;
				this.IngestedCalculateAmounts(ingester, nutritionWanted, out num, out num2);
				if (!ingester.Dead && ingester.needs.joy != null && Mathf.Abs(this.def.ingestible.joy) > 0.0001f && num > 0)
				{
					JoyKindDef joyKind = (this.def.ingestible.joyKind == null) ? JoyKindDefOf.Gluttonous : this.def.ingestible.joyKind;
					ingester.needs.joy.GainJoy((float)num * this.def.ingestible.joy, joyKind);
				}
				if (ingester.IsCaravanMember())
				{
					CaravanPawnsNeedsUtility.Notify_CaravanMemberIngestedFood(ingester, num2);
				}
				if (ingester.RaceProps.Humanlike && Rand.Chance(this.GetStatValue(StatDefOf.FoodPoisonChanceFixedHuman, true)))
				{
					FoodUtility.AddFoodPoisoningHediff(ingester, this);
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

		// Token: 0x0600505F RID: 20575 RVA: 0x0012790B File Offset: 0x00125D0B
		protected virtual void PostIngested(Pawn ingester)
		{
		}

		// Token: 0x06005060 RID: 20576 RVA: 0x00127910 File Offset: 0x00125D10
		protected virtual void IngestedCalculateAmounts(Pawn ingester, float nutritionWanted, out int numTaken, out float nutritionIngested)
		{
			numTaken = Mathf.CeilToInt(nutritionWanted / this.GetStatValue(StatDefOf.Nutrition, true));
			numTaken = Mathf.Min(new int[]
			{
				numTaken,
				this.def.ingestible.maxNumToIngestAtOnce,
				this.stackCount
			});
			numTaken = Mathf.Max(numTaken, 1);
			nutritionIngested = (float)numTaken * this.GetStatValue(StatDefOf.Nutrition, true);
		}

		// Token: 0x06005061 RID: 20577 RVA: 0x00127980 File Offset: 0x00125D80
		public virtual bool PreventPlayerSellingThingsNearby(out string reason)
		{
			reason = null;
			return false;
		}

		// Token: 0x06005062 RID: 20578 RVA: 0x0012799C File Offset: 0x00125D9C
		public virtual ushort PathFindCostFor(Pawn p)
		{
			return 0;
		}

		// Token: 0x04003510 RID: 13584
		public ThingDef def = null;

		// Token: 0x04003511 RID: 13585
		public int thingIDNumber = -1;

		// Token: 0x04003512 RID: 13586
		private sbyte mapIndexOrState = -1;

		// Token: 0x04003513 RID: 13587
		private IntVec3 positionInt = IntVec3.Invalid;

		// Token: 0x04003514 RID: 13588
		private Rot4 rotationInt = Rot4.North;

		// Token: 0x04003515 RID: 13589
		public int stackCount = 1;

		// Token: 0x04003516 RID: 13590
		protected Faction factionInt = null;

		// Token: 0x04003517 RID: 13591
		private ThingDef stuffInt = null;

		// Token: 0x04003518 RID: 13592
		private Graphic graphicInt = null;

		// Token: 0x04003519 RID: 13593
		private int hitPointsInt = -1;

		// Token: 0x0400351A RID: 13594
		public ThingOwner holdingOwner = null;

		// Token: 0x0400351B RID: 13595
		protected const sbyte UnspawnedState = -1;

		// Token: 0x0400351C RID: 13596
		private const sbyte MemoryState = -2;

		// Token: 0x0400351D RID: 13597
		private const sbyte DiscardedState = -3;

		// Token: 0x0400351E RID: 13598
		public static bool allowDestroyNonDestroyable = false;

		// Token: 0x0400351F RID: 13599
		private static List<string> tmpDeteriorationReasons = new List<string>();

		// Token: 0x04003520 RID: 13600
		public const float SmeltCostRecoverFraction = 0.25f;
	}
}
