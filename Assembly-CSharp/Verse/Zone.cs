using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000CB7 RID: 3255
	public abstract class Zone : IExposable, ISelectable, ILoadReferenceable
	{
		// Token: 0x060047C0 RID: 18368 RVA: 0x000A6F08 File Offset: 0x000A5308
		public Zone()
		{
		}

		// Token: 0x060047C1 RID: 18369 RVA: 0x000A6F5C File Offset: 0x000A535C
		public Zone(string baseName, ZoneManager zoneManager)
		{
			this.label = zoneManager.NewZoneName(baseName);
			this.zoneManager = zoneManager;
			this.color = this.NextZoneColor;
		}

		// Token: 0x17000B54 RID: 2900
		// (get) Token: 0x060047C2 RID: 18370 RVA: 0x000A6FD0 File Offset: 0x000A53D0
		public Map Map
		{
			get
			{
				return this.zoneManager.map;
			}
		}

		// Token: 0x17000B55 RID: 2901
		// (get) Token: 0x060047C3 RID: 18371 RVA: 0x000A6FF0 File Offset: 0x000A53F0
		public IntVec3 Position
		{
			get
			{
				return (this.cells.Count == 0) ? IntVec3.Invalid : this.cells[0];
			}
		}

		// Token: 0x17000B56 RID: 2902
		// (get) Token: 0x060047C4 RID: 18372 RVA: 0x000A702C File Offset: 0x000A542C
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

		// Token: 0x060047C5 RID: 18373 RVA: 0x000A707C File Offset: 0x000A547C
		public IEnumerator<IntVec3> GetEnumerator()
		{
			for (int i = 0; i < this.cells.Count; i++)
			{
				yield return this.cells[i];
			}
			yield break;
		}

		// Token: 0x17000B57 RID: 2903
		// (get) Token: 0x060047C6 RID: 18374 RVA: 0x000A70A0 File Offset: 0x000A54A0
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

		// Token: 0x17000B58 RID: 2904
		// (get) Token: 0x060047C7 RID: 18375 RVA: 0x000A70DC File Offset: 0x000A54DC
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

		// Token: 0x17000B59 RID: 2905
		// (get) Token: 0x060047C8 RID: 18376 RVA: 0x000A7108 File Offset: 0x000A5508
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

		// Token: 0x17000B5A RID: 2906
		// (get) Token: 0x060047C9 RID: 18377 RVA: 0x000A7190 File Offset: 0x000A5590
		public virtual bool IsMultiselectable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000B5B RID: 2907
		// (get) Token: 0x060047CA RID: 18378
		protected abstract Color NextZoneColor { get; }

		// Token: 0x060047CB RID: 18379 RVA: 0x000A71A8 File Offset: 0x000A55A8
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

		// Token: 0x060047CC RID: 18380 RVA: 0x000A721C File Offset: 0x000A561C
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

		// Token: 0x060047CD RID: 18381 RVA: 0x000A7308 File Offset: 0x000A5708
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

		// Token: 0x060047CE RID: 18382 RVA: 0x000A73A8 File Offset: 0x000A57A8
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

		// Token: 0x060047CF RID: 18383 RVA: 0x000A7426 File Offset: 0x000A5826
		public void Deregister()
		{
			this.zoneManager.DeregisterZone(this);
		}

		// Token: 0x060047D0 RID: 18384 RVA: 0x000A7435 File Offset: 0x000A5835
		public virtual void PostRegister()
		{
			this.CheckAddHaulDestination();
		}

		// Token: 0x060047D1 RID: 18385 RVA: 0x000A7440 File Offset: 0x000A5840
		public virtual void PostDeregister()
		{
			IHaulDestination haulDestination = this as IHaulDestination;
			if (haulDestination != null)
			{
				this.Map.haulDestinationManager.RemoveHaulDestination(haulDestination);
			}
		}

		// Token: 0x060047D2 RID: 18386 RVA: 0x000A746C File Offset: 0x000A586C
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

		// Token: 0x060047D3 RID: 18387 RVA: 0x000A74C0 File Offset: 0x000A58C0
		public virtual string GetInspectString()
		{
			return "";
		}

		// Token: 0x060047D4 RID: 18388 RVA: 0x000A74DC File Offset: 0x000A58DC
		public virtual IEnumerable<InspectTabBase> GetInspectTabs()
		{
			yield break;
		}

		// Token: 0x060047D5 RID: 18389 RVA: 0x000A7500 File Offset: 0x000A5900
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

		// Token: 0x060047D6 RID: 18390 RVA: 0x000A752C File Offset: 0x000A592C
		public virtual IEnumerable<Gizmo> GetZoneAddGizmos()
		{
			yield break;
		}

		// Token: 0x060047D7 RID: 18391 RVA: 0x000A7550 File Offset: 0x000A5950
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

		// Token: 0x060047D8 RID: 18392 RVA: 0x000A76FC File Offset: 0x000A5AFC
		private void CheckAddHaulDestination()
		{
			IHaulDestination haulDestination = this as IHaulDestination;
			if (haulDestination != null)
			{
				this.Map.haulDestinationManager.AddHaulDestination(haulDestination);
			}
		}

		// Token: 0x060047D9 RID: 18393 RVA: 0x000A7728 File Offset: 0x000A5B28
		public override string ToString()
		{
			return this.label;
		}

		// Token: 0x060047DA RID: 18394 RVA: 0x000A7744 File Offset: 0x000A5B44
		public string GetUniqueLoadID()
		{
			return "Zone_" + this.zoneManager.AllZones.IndexOf(this);
		}

		// Token: 0x040030B1 RID: 12465
		public ZoneManager zoneManager;

		// Token: 0x040030B2 RID: 12466
		public string label;

		// Token: 0x040030B3 RID: 12467
		public List<IntVec3> cells = new List<IntVec3>();

		// Token: 0x040030B4 RID: 12468
		private bool cellsShuffled = false;

		// Token: 0x040030B5 RID: 12469
		public Color color = Color.white;

		// Token: 0x040030B6 RID: 12470
		private Material materialInt = null;

		// Token: 0x040030B7 RID: 12471
		public bool hidden = false;

		// Token: 0x040030B8 RID: 12472
		private int lastStaticFireCheckTick = -9999;

		// Token: 0x040030B9 RID: 12473
		private bool lastStaticFireCheckResult = false;

		// Token: 0x040030BA RID: 12474
		private const int StaticFireCheckInterval = 1000;

		// Token: 0x040030BB RID: 12475
		private static BoolGrid extantGrid;

		// Token: 0x040030BC RID: 12476
		private static BoolGrid foundGrid;
	}
}
