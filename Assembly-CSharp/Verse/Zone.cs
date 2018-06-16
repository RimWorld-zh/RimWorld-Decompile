using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000CBB RID: 3259
	public abstract class Zone : IExposable, ISelectable, ILoadReferenceable
	{
		// Token: 0x060047B9 RID: 18361 RVA: 0x000A6EEC File Offset: 0x000A52EC
		public Zone()
		{
		}

		// Token: 0x060047BA RID: 18362 RVA: 0x000A6F40 File Offset: 0x000A5340
		public Zone(string baseName, ZoneManager zoneManager)
		{
			this.label = zoneManager.NewZoneName(baseName);
			this.zoneManager = zoneManager;
			this.color = this.NextZoneColor;
		}

		// Token: 0x17000B53 RID: 2899
		// (get) Token: 0x060047BB RID: 18363 RVA: 0x000A6FB4 File Offset: 0x000A53B4
		public Map Map
		{
			get
			{
				return this.zoneManager.map;
			}
		}

		// Token: 0x17000B54 RID: 2900
		// (get) Token: 0x060047BC RID: 18364 RVA: 0x000A6FD4 File Offset: 0x000A53D4
		public IntVec3 Position
		{
			get
			{
				return (this.cells.Count == 0) ? IntVec3.Invalid : this.cells[0];
			}
		}

		// Token: 0x17000B55 RID: 2901
		// (get) Token: 0x060047BD RID: 18365 RVA: 0x000A7010 File Offset: 0x000A5410
		public Material Material
		{
			get
			{
				if (this.materialInt == null)
				{
					this.materialInt = SolidColorMaterials.SimpleSolidColorMaterial(this.color, false);
					this.materialInt.renderQueue = 3600;
				}
				return this.materialInt;
			}
		}

		// Token: 0x060047BE RID: 18366 RVA: 0x000A7060 File Offset: 0x000A5460
		public IEnumerator<IntVec3> GetEnumerator()
		{
			for (int i = 0; i < this.cells.Count; i++)
			{
				yield return this.cells[i];
			}
			yield break;
		}

		// Token: 0x17000B56 RID: 2902
		// (get) Token: 0x060047BF RID: 18367 RVA: 0x000A7084 File Offset: 0x000A5484
		public List<IntVec3> Cells
		{
			get
			{
				if (!this.cellsShuffled)
				{
					this.cells.Shuffle<IntVec3>();
					this.cellsShuffled = true;
				}
				return this.cells;
			}
		}

		// Token: 0x17000B57 RID: 2903
		// (get) Token: 0x060047C0 RID: 18368 RVA: 0x000A70C0 File Offset: 0x000A54C0
		public IEnumerable<Thing> AllContainedThings
		{
			get
			{
				ThingGrid grids = this.Map.thingGrid;
				for (int i = 0; i < this.cells.Count; i++)
				{
					List<Thing> thingList = grids.ThingsListAt(this.cells[i]);
					for (int j = 0; j < thingList.Count; j++)
					{
						yield return thingList[j];
					}
				}
				yield break;
			}
		}

		// Token: 0x17000B58 RID: 2904
		// (get) Token: 0x060047C1 RID: 18369 RVA: 0x000A70EC File Offset: 0x000A54EC
		public bool ContainsStaticFire
		{
			get
			{
				if (Find.TickManager.TicksGame > this.lastStaticFireCheckTick + 1000)
				{
					this.lastStaticFireCheckResult = false;
					for (int i = 0; i < this.cells.Count; i++)
					{
						if (this.cells[i].ContainsStaticFire(this.Map))
						{
							this.lastStaticFireCheckResult = true;
							break;
						}
					}
				}
				return this.lastStaticFireCheckResult;
			}
		}

		// Token: 0x17000B59 RID: 2905
		// (get) Token: 0x060047C2 RID: 18370 RVA: 0x000A7174 File Offset: 0x000A5574
		public virtual bool IsMultiselectable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000B5A RID: 2906
		// (get) Token: 0x060047C3 RID: 18371
		protected abstract Color NextZoneColor { get; }

		// Token: 0x060047C4 RID: 18372 RVA: 0x000A718C File Offset: 0x000A558C
		public virtual void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.label, "label", null, false);
			Scribe_Values.Look<Color>(ref this.color, "color", default(Color), false);
			Scribe_Values.Look<bool>(ref this.hidden, "hidden", false, false);
			Scribe_Collections.Look<IntVec3>(ref this.cells, "cells", LookMode.Undefined, new object[0]);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.CheckAddHaulDestination();
			}
		}

		// Token: 0x060047C5 RID: 18373 RVA: 0x000A7200 File Offset: 0x000A5600
		public virtual void AddCell(IntVec3 c)
		{
			if (this.cells.Contains(c))
			{
				Log.Error(string.Concat(new object[]
				{
					"Adding cell to zone which already has it. c=",
					c,
					", zone=",
					this
				}), false);
			}
			else
			{
				List<Thing> list = this.Map.thingGrid.ThingsListAt(c);
				for (int i = 0; i < list.Count; i++)
				{
					Thing thing = list[i];
					if (!thing.def.CanOverlapZones)
					{
						Log.Error("Added zone over zone-incompatible thing " + thing, false);
						return;
					}
				}
				this.cells.Add(c);
				this.zoneManager.AddZoneGridCell(this, c);
				this.Map.mapDrawer.MapMeshDirty(c, MapMeshFlag.Zone);
				AutoHomeAreaMaker.Notify_ZoneCellAdded(c, this);
				this.cellsShuffled = false;
			}
		}

		// Token: 0x060047C6 RID: 18374 RVA: 0x000A72EC File Offset: 0x000A56EC
		public virtual void RemoveCell(IntVec3 c)
		{
			if (!this.cells.Contains(c))
			{
				Log.Error(string.Concat(new object[]
				{
					"Cannot remove cell from zone which doesn't have it. c=",
					c,
					", zone=",
					this
				}), false);
			}
			else
			{
				this.cells.Remove(c);
				this.zoneManager.ClearZoneGridCell(c);
				this.Map.mapDrawer.MapMeshDirty(c, MapMeshFlag.Zone);
				this.cellsShuffled = false;
				if (this.cells.Count == 0)
				{
					this.Deregister();
				}
			}
		}

		// Token: 0x060047C7 RID: 18375 RVA: 0x000A738C File Offset: 0x000A578C
		public virtual void Delete()
		{
			SoundDefOf.Designate_ZoneDelete.PlayOneShotOnCamera(this.Map);
			if (this.cells.Count == 0)
			{
				this.Deregister();
			}
			else
			{
				while (this.cells.Count > 0)
				{
					this.RemoveCell(this.cells[this.cells.Count - 1]);
				}
			}
			Find.Selector.Deselect(this);
		}

		// Token: 0x060047C8 RID: 18376 RVA: 0x000A740A File Offset: 0x000A580A
		public void Deregister()
		{
			this.zoneManager.DeregisterZone(this);
		}

		// Token: 0x060047C9 RID: 18377 RVA: 0x000A7419 File Offset: 0x000A5819
		public virtual void PostRegister()
		{
			this.CheckAddHaulDestination();
		}

		// Token: 0x060047CA RID: 18378 RVA: 0x000A7424 File Offset: 0x000A5824
		public virtual void PostDeregister()
		{
			IHaulDestination haulDestination = this as IHaulDestination;
			if (haulDestination != null)
			{
				this.Map.haulDestinationManager.RemoveHaulDestination(haulDestination);
			}
		}

		// Token: 0x060047CB RID: 18379 RVA: 0x000A7450 File Offset: 0x000A5850
		public bool ContainsCell(IntVec3 c)
		{
			for (int i = 0; i < this.cells.Count; i++)
			{
				if (this.cells[i] == c)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060047CC RID: 18380 RVA: 0x000A74A4 File Offset: 0x000A58A4
		public virtual string GetInspectString()
		{
			return "";
		}

		// Token: 0x060047CD RID: 18381 RVA: 0x000A74C0 File Offset: 0x000A58C0
		public virtual IEnumerable<InspectTabBase> GetInspectTabs()
		{
			yield break;
		}

		// Token: 0x060047CE RID: 18382 RVA: 0x000A74E4 File Offset: 0x000A58E4
		public virtual IEnumerable<Gizmo> GetGizmos()
		{
			yield return new Command_Action
			{
				icon = ContentFinder<Texture2D>.Get("UI/Commands/RenameZone", true),
				defaultLabel = "CommandRenameZoneLabel".Translate(),
				defaultDesc = "CommandRenameZoneDesc".Translate(),
				action = delegate()
				{
					Find.WindowStack.Add(new Dialog_RenameZone(this));
				},
				hotKey = KeyBindingDefOf.Misc1
			};
			yield return new Command_Toggle
			{
				icon = ContentFinder<Texture2D>.Get("UI/Commands/HideZone", true),
				defaultLabel = ((!this.hidden) ? "CommandHideZoneLabel".Translate() : "CommandUnhideZoneLabel".Translate()),
				defaultDesc = "CommandHideZoneDesc".Translate(),
				isActive = (() => this.hidden),
				toggleAction = delegate()
				{
					this.hidden = !this.hidden;
					foreach (IntVec3 loc in this.Cells)
					{
						this.Map.mapDrawer.MapMeshDirty(loc, MapMeshFlag.Zone);
					}
				},
				hotKey = KeyBindingDefOf.Misc2
			};
			foreach (Gizmo gizmo in this.GetZoneAddGizmos())
			{
				yield return gizmo;
			}
			Designator delete = DesignatorUtility.FindAllowedDesignator<Designator_ZoneDelete_Shrink>();
			if (delete != null)
			{
				yield return delete;
			}
			yield return new Command_Action
			{
				icon = ContentFinder<Texture2D>.Get("UI/Buttons/Delete", true),
				defaultLabel = "CommandDeleteZoneLabel".Translate(),
				defaultDesc = "CommandDeleteZoneDesc".Translate(),
				action = new Action(this.Delete),
				hotKey = KeyBindingDefOf.Misc3
			};
			yield break;
		}

		// Token: 0x060047CF RID: 18383 RVA: 0x000A7510 File Offset: 0x000A5910
		public virtual IEnumerable<Gizmo> GetZoneAddGizmos()
		{
			yield break;
		}

		// Token: 0x060047D0 RID: 18384 RVA: 0x000A7534 File Offset: 0x000A5934
		public void CheckContiguous()
		{
			if (this.cells.Count != 0)
			{
				if (Zone.extantGrid == null)
				{
					Zone.extantGrid = new BoolGrid(this.Map);
				}
				else
				{
					Zone.extantGrid.ClearAndResizeTo(this.Map);
				}
				if (Zone.foundGrid == null)
				{
					Zone.foundGrid = new BoolGrid(this.Map);
				}
				else
				{
					Zone.foundGrid.ClearAndResizeTo(this.Map);
				}
				for (int i = 0; i < this.cells.Count; i++)
				{
					Zone.extantGrid.Set(this.cells[i], true);
				}
				Predicate<IntVec3> passCheck = (IntVec3 c) => Zone.extantGrid[c] && !Zone.foundGrid[c];
				int numFound = 0;
				Action<IntVec3> processor = delegate(IntVec3 c)
				{
					Zone.foundGrid.Set(c, true);
					numFound++;
				};
				this.Map.floodFiller.FloodFill(this.cells[0], passCheck, processor, int.MaxValue, false, null);
				if (numFound < this.cells.Count)
				{
					foreach (IntVec3 c2 in this.Map.AllCells)
					{
						if (Zone.extantGrid[c2] && !Zone.foundGrid[c2])
						{
							this.RemoveCell(c2);
						}
					}
				}
			}
		}

		// Token: 0x060047D1 RID: 18385 RVA: 0x000A76E0 File Offset: 0x000A5AE0
		private void CheckAddHaulDestination()
		{
			IHaulDestination haulDestination = this as IHaulDestination;
			if (haulDestination != null)
			{
				this.Map.haulDestinationManager.AddHaulDestination(haulDestination);
			}
		}

		// Token: 0x060047D2 RID: 18386 RVA: 0x000A770C File Offset: 0x000A5B0C
		public override string ToString()
		{
			return this.label;
		}

		// Token: 0x060047D3 RID: 18387 RVA: 0x000A7728 File Offset: 0x000A5B28
		public string GetUniqueLoadID()
		{
			return "Zone_" + this.zoneManager.AllZones.IndexOf(this);
		}

		// Token: 0x040030A8 RID: 12456
		public ZoneManager zoneManager;

		// Token: 0x040030A9 RID: 12457
		public string label;

		// Token: 0x040030AA RID: 12458
		public List<IntVec3> cells = new List<IntVec3>();

		// Token: 0x040030AB RID: 12459
		private bool cellsShuffled = false;

		// Token: 0x040030AC RID: 12460
		public Color color = Color.white;

		// Token: 0x040030AD RID: 12461
		private Material materialInt = null;

		// Token: 0x040030AE RID: 12462
		public bool hidden = false;

		// Token: 0x040030AF RID: 12463
		private int lastStaticFireCheckTick = -9999;

		// Token: 0x040030B0 RID: 12464
		private bool lastStaticFireCheckResult = false;

		// Token: 0x040030B1 RID: 12465
		private const int StaticFireCheckInterval = 1000;

		// Token: 0x040030B2 RID: 12466
		private static BoolGrid extantGrid;

		// Token: 0x040030B3 RID: 12467
		private static BoolGrid foundGrid;
	}
}
