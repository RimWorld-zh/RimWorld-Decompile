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
	// Token: 0x02000DF5 RID: 3573
	public class Thing : Entity, IExposable, ISelectable, ILoadReferenceable, ISignalReceiver
	{
		// Token: 0x0400351C RID: 13596
		public ThingDef def = null;

		// Token: 0x0400351D RID: 13597
		public int thingIDNumber = -1;

		// Token: 0x0400351E RID: 13598
		private sbyte mapIndexOrState = -1;

		// Token: 0x0400351F RID: 13599
		private IntVec3 positionInt = IntVec3.Invalid;

		// Token: 0x04003520 RID: 13600
		private Rot4 rotationInt = Rot4.North;

		// Token: 0x04003521 RID: 13601
		public int stackCount = 1;

		// Token: 0x04003522 RID: 13602
		protected Faction factionInt = null;

		// Token: 0x04003523 RID: 13603
		private ThingDef stuffInt = null;

		// Token: 0x04003524 RID: 13604
		private Graphic graphicInt = null;

		// Token: 0x04003525 RID: 13605
		private int hitPointsInt = -1;

		// Token: 0x04003526 RID: 13606
		public ThingOwner holdingOwner = null;

		// Token: 0x04003527 RID: 13607
		protected const sbyte UnspawnedState = -1;

		// Token: 0x04003528 RID: 13608
		private const sbyte MemoryState = -2;

		// Token: 0x04003529 RID: 13609
		private const sbyte DiscardedState = -3;

		// Token: 0x0400352A RID: 13610
		public static bool allowDestroyNonDestroyable = false;

		// Token: 0x0400352B RID: 13611
		private static List<string> tmpDeteriorationReasons = new List<string>();

		// Token: 0x0400352C RID: 13612
		public const float SmeltCostRecoverFraction = 0.25f;

		// Token: 0x17000D07 RID: 3335
		// (get) Token: 0x06005019 RID: 20505 RVA: 0x00125AD4 File Offset: 0x00123ED4
		// (set) Token: 0x0600501A RID: 20506 RVA: 0x00125AEF File Offset: 0x00123EEF
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
		// (get) Token: 0x0600501B RID: 20507 RVA: 0x00125AFC File Offset: 0x00123EFC
		public int MaxHitPoints
		{
			get
			{
				return Mathf.RoundToInt(this.GetStatValue(StatDefOf.MaxHitPoints, true));
			}
		}

		// Token: 0x17000D09 RID: 3337
		// (get) Token: 0x0600501C RID: 20508 RVA: 0x00125B24 File Offset: 0x00123F24
		public float MarketValue
		{
			get
			{
				return this.GetStatValue(StatDefOf.MarketValue, true);
			}
		}

		// Token: 0x17000D0A RID: 3338
		// (get) Token: 0x0600501D RID: 20509 RVA: 0x00125B48 File Offset: 0x00123F48
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
		// (get) Token: 0x0600501E RID: 20510 RVA: 0x00125BE0 File Offset: 0x00123FE0
		public virtual bool FireBulwark
		{
			get
			{
				return this.def.Fillage == FillCategory.Full;
			}
		}

		// Token: 0x17000D0C RID: 3340
		// (get) Token: 0x0600501F RID: 20511 RVA: 0x00125C04 File Offset: 0x00124004
		public bool Destroyed
		{
			get
			{
				return (int)this.mapIndexOrState == -2 || (int)this.mapIndexOrState == -3;
			}
		}

		// Token: 0x17000D0D RID: 3341
		// (get) Token: 0x06005020 RID: 20512 RVA: 0x00125C38 File Offset: 0x00124038
		public bool Discarded
		{
			get
			{
				return (int)this.mapIndexOrState == -3;
			}
		}

		// Token: 0x17000D0E RID: 3342
		// (get) Token: 0x06005021 RID: 20513 RVA: 0x00125C58 File Offset: 0x00124058
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
		// (get) Token: 0x06005022 RID: 20514 RVA: 0x00125CB0 File Offset: 0x001240B0
		public bool SpawnedOrAnyParentSpawned
		{
			get
			{
				return this.SpawnedParentOrMe != null;
			}
		}

		// Token: 0x17000D10 RID: 3344
		// (get) Token: 0x06005023 RID: 20515 RVA: 0x00125CD4 File Offset: 0x001240D4
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
		// (get) Token: 0x06005024 RID: 20516 RVA: 0x00125D18 File Offset: 0x00124118
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
		// (get) Token: 0x06005025 RID: 20517 RVA: 0x00125D54 File Offset: 0x00124154
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
		// (get) Token: 0x06005026 RID: 20518 RVA: 0x00125DA0 File Offset: 0x001241A0
		// (set) Token: 0x06005027 RID: 20519 RVA: 0x00125DBC File Offset: 0x001241BC
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
		// (get) Token: 0x06005028 RID: 20520 RVA: 0x00125E90 File Offset: 0x00124290
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
		// (get) Token: 0x06005029 RID: 20521 RVA: 0x00125EE4 File Offset: 0x001242E4
		// (set) Token: 0x0600502A RID: 20522 RVA: 0x00125F00 File Offset: 0x00124300
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
		// (get) Token: 0x0600502B RID: 20523 RVA: 0x00126014 File Offset: 0x00124414
		public bool Smeltable
		{
			get
			{
				return this.def.smeltable && (!this.def.MadeFromStuff || this.Stuff.stuffProps.smeltable);
			}
		}

		// Token: 0x17000D17 RID: 3351
		// (get) Token: 0x0600502C RID: 20524 RVA: 0x00126060 File Offset: 0x00124460
		public IThingHolder ParentHolder
		{
			get
			{
				return (this.holdingOwner == null) ? null : this.holdingOwner.Owner;
			}
		}

		// Token: 0x17000D18 RID: 3352
		// (get) Token: 0x0600502D RID: 20525 RVA: 0x00126094 File Offset: 0x00124494
		public Faction Faction
		{
			get
			{
				return this.factionInt;
			}
		}

		// Token: 0x17000D19 RID: 3353
		// (get) Token: 0x0600502E RID: 20526 RVA: 0x001260B0 File Offset: 0x001244B0
		// (set) Token: 0x0600502F RID: 20527 RVA: 0x00126107 File Offset: 0x00124507
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

		// Token: 0x06005030 RID: 20528 RVA: 0x00126118 File Offset: 0x00124518
		public string UniqueVerbOwnerID()
		{
			return this.ThingID;
		}

		// Token: 0x06005031 RID: 20529 RVA: 0x00126134 File Offset: 0x00124534
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
		// (get) Token: 0x06005032 RID: 20530 RVA: 0x001261C0 File Offset: 0x001245C0
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
		// (get) Token: 0x06005033 RID: 20531 RVA: 0x0012621C File Offset: 0x0012461C
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
		// (get) Token: 0x06005034 RID: 20532 RVA: 0x00126264 File Offset: 0x00124664
		public virtual string LabelNoCount
		{
			get
			{
				return GenLabel.ThingLabel(this, 1, true);
			}
		}

		// Token: 0x17000D1D RID: 3357
		// (get) Token: 0x06005035 RID: 20533 RVA: 0x00126284 File Offset: 0x00124684
		public override string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		// Token: 0x17000D1E RID: 3358
		// (get) Token: 0x06005036 RID: 20534 RVA: 0x001262A4 File Offset: 0x001246A4
		public virtual string LabelCapNoCount
		{
			get
			{
				return this.LabelNoCount.CapitalizeFirst();
			}
		}

		// Token: 0x17000D1F RID: 3359
		// (get) Token: 0x06005037 RID: 20535 RVA: 0x001262C4 File Offset: 0x001246C4
		public override string LabelShort
		{
			get
			{
				return this.LabelNoCount;
			}
		}

		// Token: 0x17000D20 RID: 3360
		// (get) Token: 0x06005038 RID: 20536 RVA: 0x001262E0 File Offset: 0x001246E0
		public virtual bool IngestibleNow
		{
			get
			{
				return !this.IsBurning() && this.def.IsIngestible;
			}
		}

		// Token: 0x17000D21 RID: 3361
		// (get) Token: 0x06005039 RID: 20537 RVA: 0x00126314 File Offset: 0x00124714
		public ThingDef Stuff
		{
			get
			{
				return this.stuffInt;
			}
		}

		// Token: 0x17000D22 RID: 3362
		// (get) Token: 0x0600503A RID: 20538 RVA: 0x00126330 File Offset: 0x00124730
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x17000D23 RID: 3363
		// (get) Token: 0x0600503B RID: 20539 RVA: 0x00126354 File Offset: 0x00124754
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
		// (get) Token: 0x0600503C RID: 20540 RVA: 0x001263CC File Offset: 0x001247CC
		public virtual Graphic Graphic
		{
			get
			{
				return this.DefaultGraphic;
			}
		}

		// Token: 0x17000D25 RID: 3365
		// (get) Token: 0x0600503D RID: 20541 RVA: 0x001263E8 File Offset: 0x001247E8
		public virtual IntVec3 InteractionCell
		{
			get
			{
				return ThingUtility.InteractionCellWhenAt(this.def, this.Position, this.Rotation, this.Map);
			}
		}

		// Token: 0x17000D26 RID: 3366
		// (get) Token: 0x0600503E RID: 20542 RVA: 0x0012641C File Offset: 0x0012481C
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
		// (get) Token: 0x0600503F RID: 20543 RVA: 0x001264D4 File Offset: 0x001248D4
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
		// (get) Token: 0x06005040 RID: 20544 RVA: 0x00126524 File Offset: 0x00124924
		public bool Suspended
		{
			get
			{
				return !this.Spawned && this.ParentHolder != null && ThingOwnerUtility.ContentsSuspended(this.ParentHolder);
			}
		}

		// Token: 0x17000D29 RID: 3369
		// (get) Token: 0x06005041 RID: 20545 RVA: 0x00126568 File Offset: 0x00124968
		public virtual string DescriptionDetailed
		{
			get
			{
				return this.def.DescriptionDetailed;
			}
		}

		// Token: 0x17000D2A RID: 3370
		// (get) Token: 0x06005042 RID: 20546 RVA: 0x00126588 File Offset: 0x00124988
		public virtual string DescriptionFlavor
		{
			get
			{
				return this.def.description;
			}
		}

		// Token: 0x06005043 RID: 20547 RVA: 0x001265A8 File Offset: 0x001249A8
		public virtual void PostMake()
		{
			ThingIDMaker.GiveIDTo(this);
			if (this.def.useHitPoints)
			{
				this.HitPoints = Mathf.RoundToInt((float)this.MaxHitPoints * Mathf.Clamp01(this.def.startingHpRange.RandomInRange));
			}
		}

		// Token: 0x06005044 RID: 20548 RVA: 0x001265F4 File Offset: 0x001249F4
		public string GetUniqueLoadID()
		{
			return "Thing_" + this.ThingID;
		}

		// Token: 0x06005045 RID: 20549 RVA: 0x0012661C File Offset: 0x00124A1C
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

		// Token: 0x06005046 RID: 20550 RVA: 0x001269F8 File Offset: 0x00124DF8
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

		// Token: 0x06005047 RID: 20551 RVA: 0x00126CAA File Offset: 0x001250AA
		public virtual void Kill(DamageInfo? dinfo = null, Hediff exactCulprit = null)
		{
			this.Destroy(DestroyMode.KillFinalize);
		}

		// Token: 0x06005048 RID: 20552 RVA: 0x00126CB4 File Offset: 0x001250B4
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

		// Token: 0x06005049 RID: 20553 RVA: 0x00126DB0 File Offset: 0x001251B0
		public virtual void PreTraded(TradeAction action, Pawn playerNegotiator, ITrader trader)
		{
		}

		// Token: 0x0600504A RID: 20554 RVA: 0x00126DB3 File Offset: 0x001251B3
		public virtual void PostGeneratedForTrader(TraderKindDef trader, int forTile, Faction forFaction)
		{
			if (this.def.colorGeneratorInTraderStock != null)
			{
				this.SetColor(this.def.colorGeneratorInTraderStock.NewRandomizedColor(), true);
			}
		}

		// Token: 0x0600504B RID: 20555 RVA: 0x00126DDD File Offset: 0x001251DD
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

		// Token: 0x0600504C RID: 20556 RVA: 0x00126E14 File Offset: 0x00125214
		public void ForceSetStateToUnspawned()
		{
			this.mapIndexOrState = -1;
		}

		// Token: 0x0600504D RID: 20557 RVA: 0x00126E20 File Offset: 0x00125220
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

		// Token: 0x0600504E RID: 20558 RVA: 0x00126E84 File Offset: 0x00125284
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

		// Token: 0x0600504F RID: 20559 RVA: 0x00126F24 File Offset: 0x00125324
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

		// Token: 0x06005050 RID: 20560 RVA: 0x00127105 File Offset: 0x00125505
		public virtual void PostMapInit()
		{
		}

		// Token: 0x17000D2B RID: 3371
		// (get) Token: 0x06005051 RID: 20561 RVA: 0x00127108 File Offset: 0x00125508
		public virtual Vector3 DrawPos
		{
			get
			{
				return this.TrueCenter();
			}
		}

		// Token: 0x17000D2C RID: 3372
		// (get) Token: 0x06005052 RID: 20562 RVA: 0x00127124 File Offset: 0x00125524
		// (set) Token: 0x06005053 RID: 20563 RVA: 0x00127188 File Offset: 0x00125588
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
		// (get) Token: 0x06005054 RID: 20564 RVA: 0x001271D8 File Offset: 0x001255D8
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

		// Token: 0x06005055 RID: 20565 RVA: 0x00127218 File Offset: 0x00125618
		public virtual void Draw()
		{
			this.DrawAt(this.DrawPos, false);
		}

		// Token: 0x06005056 RID: 20566 RVA: 0x00127228 File Offset: 0x00125628
		public virtual void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			this.Graphic.Draw(drawLoc, (!flip) ? this.Rotation : this.Rotation.Opposite, this, 0f);
		}

		// Token: 0x06005057 RID: 20567 RVA: 0x00127267 File Offset: 0x00125667
		public virtual void Print(SectionLayer layer)
		{
			this.Graphic.Print(layer, this);
		}

		// Token: 0x06005058 RID: 20568 RVA: 0x00127278 File Offset: 0x00125678
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

		// Token: 0x06005059 RID: 20569 RVA: 0x001272D8 File Offset: 0x001256D8
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

		// Token: 0x0600505A RID: 20570 RVA: 0x0012733C File Offset: 0x0012573C
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

		// Token: 0x0600505B RID: 20571 RVA: 0x00127410 File Offset: 0x00125810
		public virtual string GetInspectString()
		{
			return "";
		}

		// Token: 0x0600505C RID: 20572 RVA: 0x0012742C File Offset: 0x0012582C
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

		// Token: 0x0600505D RID: 20573 RVA: 0x00127490 File Offset: 0x00125890
		public virtual IEnumerable<Gizmo> GetGizmos()
		{
			yield break;
		}

		// Token: 0x0600505E RID: 20574 RVA: 0x001274B4 File Offset: 0x001258B4
		public virtual IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
		{
			yield break;
		}

		// Token: 0x0600505F RID: 20575 RVA: 0x001274D8 File Offset: 0x001258D8
		public virtual IEnumerable<InspectTabBase> GetInspectTabs()
		{
			return this.def.inspectorTabsResolved;
		}

		// Token: 0x06005060 RID: 20576 RVA: 0x001274F8 File Offset: 0x001258F8
		public virtual string GetCustomLabelNoCount(bool includeHp = true)
		{
			return GenLabel.ThingLabel(this, 1, includeHp);
		}

		// Token: 0x06005061 RID: 20577 RVA: 0x00127518 File Offset: 0x00125918
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

		// Token: 0x06005062 RID: 20578 RVA: 0x001276CF File Offset: 0x00125ACF
		public virtual void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			absorbed = false;
		}

		// Token: 0x06005063 RID: 20579 RVA: 0x001276D5 File Offset: 0x00125AD5
		public virtual void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
		}

		// Token: 0x06005064 RID: 20580 RVA: 0x001276D8 File Offset: 0x00125AD8
		public virtual bool CanStackWith(Thing other)
		{
			return !this.Destroyed && !other.Destroyed && this.def.category == ThingCategory.Item && this.def == other.def && this.Stuff == other.Stuff;
		}

		// Token: 0x06005065 RID: 20581 RVA: 0x00127744 File Offset: 0x00125B44
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

		// Token: 0x06005066 RID: 20582 RVA: 0x0012780C File Offset: 0x00125C0C
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

		// Token: 0x06005067 RID: 20583 RVA: 0x00127928 File Offset: 0x00125D28
		public virtual void Notify_ColorChanged()
		{
			this.graphicInt = null;
			if (this.Spawned && (this.def.drawerType == DrawerType.MapMeshOnly || this.def.drawerType == DrawerType.MapMeshAndRealTime))
			{
				this.Map.mapDrawer.MapMeshDirty(this.Position, MapMeshFlag.Things);
			}
		}

		// Token: 0x06005068 RID: 20584 RVA: 0x00127983 File Offset: 0x00125D83
		public virtual void Notify_SignalReceived(Signal signal)
		{
		}

		// Token: 0x06005069 RID: 20585 RVA: 0x00127988 File Offset: 0x00125D88
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

		// Token: 0x0600506A RID: 20586 RVA: 0x00127A04 File Offset: 0x00125E04
		public virtual bool BlocksPawn(Pawn p)
		{
			return this.def.passability == Traversability.Impassable;
		}

		// Token: 0x0600506B RID: 20587 RVA: 0x00127A27 File Offset: 0x00125E27
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

		// Token: 0x0600506C RID: 20588 RVA: 0x00127A60 File Offset: 0x00125E60
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

		// Token: 0x0600506D RID: 20589 RVA: 0x00127ACC File Offset: 0x00125ECC
		public void SetPositionDirect(IntVec3 newPos)
		{
			this.positionInt = newPos;
		}

		// Token: 0x0600506E RID: 20590 RVA: 0x00127AD6 File Offset: 0x00125ED6
		public void SetStuffDirect(ThingDef newStuff)
		{
			this.stuffInt = newStuff;
		}

		// Token: 0x0600506F RID: 20591 RVA: 0x00127AE0 File Offset: 0x00125EE0
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

		// Token: 0x06005070 RID: 20592 RVA: 0x00127B18 File Offset: 0x00125F18
		public override int GetHashCode()
		{
			return this.thingIDNumber;
		}

		// Token: 0x06005071 RID: 20593 RVA: 0x00127B34 File Offset: 0x00125F34
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

		// Token: 0x06005072 RID: 20594 RVA: 0x00127B9C File Offset: 0x00125F9C
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

		// Token: 0x06005073 RID: 20595 RVA: 0x00127BD0 File Offset: 0x00125FD0
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

		// Token: 0x06005074 RID: 20596 RVA: 0x00127BFC File Offset: 0x00125FFC
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
				if (ingester.RaceProps.Humanlike && Rand.Chance(this.GetStatValue(StatDefOf.FoodPoisonChanceFixedHuman, true) * Find.Storyteller.difficulty.foodPoisonChanceFactor))
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

		// Token: 0x06005075 RID: 20597 RVA: 0x00127E83 File Offset: 0x00126283
		protected virtual void PostIngested(Pawn ingester)
		{
		}

		// Token: 0x06005076 RID: 20598 RVA: 0x00127E88 File Offset: 0x00126288
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

		// Token: 0x06005077 RID: 20599 RVA: 0x00127EF8 File Offset: 0x001262F8
		public virtual bool PreventPlayerSellingThingsNearby(out string reason)
		{
			reason = null;
			return false;
		}

		// Token: 0x06005078 RID: 20600 RVA: 0x00127F14 File Offset: 0x00126314
		public virtual ushort PathFindCostFor(Pawn p)
		{
			return 0;
		}
	}
}
