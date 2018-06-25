using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000CB9 RID: 3257
	public abstract class Zone : IExposable, ISelectable, ILoadReferenceable
	{
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

		// Token: 0x060047C3 RID: 18371 RVA: 0x000A7058 File Offset: 0x000A5458
		public Zone()
		{
		}

		// Token: 0x060047C4 RID: 18372 RVA: 0x000A70AC File Offset: 0x000A54AC
		public Zone(string baseName, ZoneManager zoneManager)
		{
			this.label = zoneManager.NewZoneName(baseName);
			this.zoneManager = zoneManager;
			this.color = this.NextZoneColor;
		}

		// Token: 0x17000B53 RID: 2899
		// (get) Token: 0x060047C5 RID: 18373 RVA: 0x000A7120 File Offset: 0x000A5520
		public Map Map
		{
			get
			{
				return this.zoneManager.map;
			}
		}

		// Token: 0x17000B54 RID: 2900
		// (get) Token: 0x060047C6 RID: 18374 RVA: 0x000A7140 File Offset: 0x000A5540
		public IntVec3 Position
		{
			get
			{
				return (this.cells.Count == 0) ? IntVec3.Invalid : this.cells[0];
			}
		}

		// Token: 0x17000B55 RID: 2901
		// (get) Token: 0x060047C7 RID: 18375 RVA: 0x000A717C File Offset: 0x000A557C
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

		// Token: 0x060047C8 RID: 18376 RVA: 0x000A71CC File Offset: 0x000A55CC
		public IEnumerator<IntVec3> GetEnumerator()
		{
			for (int i = 0; i < this.cells.Count; i++)
			{
				yield return this.cells[i];
			}
			yield break;
		}

		// Token: 0x17000B56 RID: 2902
		// (get) Token: 0x060047C9 RID: 18377 RVA: 0x000A71F0 File Offset: 0x000A55F0
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
		// (get) Token: 0x060047CA RID: 18378 RVA: 0x000A722C File Offset: 0x000A562C
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
		// (get) Token: 0x060047CB RID: 18379 RVA: 0x000A7258 File Offset: 0x000A5658
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
		// (get) Token: 0x060047CC RID: 18380 RVA: 0x000A72E0 File Offset: 0x000A56E0
		public virtual bool IsMultiselectable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000B5A RID: 2906
		// (get) Token: 0x060047CD RID: 18381
		protected abstract Color NextZoneColor { get; }

		// Token: 0x060047CE RID: 18382 RVA: 0x000A72F8 File Offset: 0x000A56F8
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

		// Token: 0x060047CF RID: 18383 RVA: 0x000A736C File Offset: 0x000A576C
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

		// Token: 0x060047D0 RID: 18384 RVA: 0x000A7458 File Offset: 0x000A5858
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

		// Token: 0x060047D1 RID: 18385 RVA: 0x000A74F8 File Offset: 0x000A58F8
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

		// Token: 0x060047D2 RID: 18386 RVA: 0x000A7576 File Offset: 0x000A5976
		public void Deregister()
		{
			this.zoneManager.DeregisterZone(this);
		}

		// Token: 0x060047D3 RID: 18387 RVA: 0x000A7585 File Offset: 0x000A5985
		public virtual void PostRegister()
		{
			this.CheckAddHaulDestination();
		}

		// Token: 0x060047D4 RID: 18388 RVA: 0x000A7590 File Offset: 0x000A5990
		public virtual void PostDeregister()
		{
			IHaulDestination haulDestination = this as IHaulDestination;
			if (haulDestination != null)
			{
				this.Map.haulDestinationManager.RemoveHaulDestination(haulDestination);
			}
		}

		// Token: 0x060047D5 RID: 18389 RVA: 0x000A75BC File Offset: 0x000A59BC
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

		// Token: 0x060047D6 RID: 18390 RVA: 0x000A7610 File Offset: 0x000A5A10
		public virtual string GetInspectString()
		{
			return "";
		}

		// Token: 0x060047D7 RID: 18391 RVA: 0x000A762C File Offset: 0x000A5A2C
		public virtual IEnumerable<InspectTabBase> GetInspectTabs()
		{
			yield break;
		}

		// Token: 0x060047D8 RID: 18392 RVA: 0x000A7650 File Offset: 0x000A5A50
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

		// Token: 0x060047D9 RID: 18393 RVA: 0x000A767C File Offset: 0x000A5A7C
		public virtual IEnumerable<Gizmo> GetZoneAddGizmos()
		{
			yield break;
		}

		// Token: 0x060047DA RID: 18394 RVA: 0x000A76A0 File Offset: 0x000A5AA0
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

		// Token: 0x060047DB RID: 18395 RVA: 0x000A784C File Offset: 0x000A5C4C
		private void CheckAddHaulDestination()
		{
			IHaulDestination haulDestination = this as IHaulDestination;
			if (haulDestination != null)
			{
				this.Map.haulDestinationManager.AddHaulDestination(haulDestination);
			}
		}

		// Token: 0x060047DC RID: 18396 RVA: 0x000A7878 File Offset: 0x000A5C78
		public override string ToString()
		{
			return this.label;
		}

		// Token: 0x060047DD RID: 18397 RVA: 0x000A7894 File Offset: 0x000A5C94
		public string GetUniqueLoadID()
		{
			return "Zone_" + this.zoneManager.AllZones.IndexOf(this);
		}
	}
}
