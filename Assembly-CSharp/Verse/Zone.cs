using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	public abstract class Zone : IExposable, ISelectable
	{
		public ZoneManager zoneManager;

		public string label;

		public List<IntVec3> cells = new List<IntVec3>();

		private bool cellsShuffled;

		public Color color = Color.white;

		private Material materialInt;

		public bool hidden;

		private int lastStaticFireCheckTick = -9999;

		private bool lastStaticFireCheckResult;

		private const int StaticFireCheckInterval = 1000;

		private static BoolGrid extantGrid;

		private static BoolGrid foundGrid;

		public Map Map
		{
			get
			{
				return this.zoneManager.map;
			}
		}

		public Material Material
		{
			get
			{
				if ((UnityEngine.Object)this.materialInt == (UnityEngine.Object)null)
				{
					this.materialInt = SolidColorMaterials.SimpleSolidColorMaterial(this.color, false);
					this.materialInt.renderQueue = 3600;
				}
				return this.materialInt;
			}
		}

		public List<IntVec3> Cells
		{
			get
			{
				if (!this.cellsShuffled)
				{
					this.cells.Shuffle();
					this.cellsShuffled = true;
				}
				return this.cells;
			}
		}

		public IEnumerable<Thing> AllContainedThings
		{
			get
			{
				ThingGrid grids = this.Map.thingGrid;
				int j = 0;
				List<Thing> thingList;
				int i;
				while (true)
				{
					if (j < this.cells.Count)
					{
						thingList = grids.ThingsListAt(this.cells[j]);
						i = 0;
						if (i < thingList.Count)
							break;
						j++;
						continue;
					}
					yield break;
				}
				yield return thingList[i];
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}

		public bool ContainsStaticFire
		{
			get
			{
				if (Find.TickManager.TicksGame > this.lastStaticFireCheckTick + 1000)
				{
					this.lastStaticFireCheckResult = false;
					int num = 0;
					while (num < this.cells.Count)
					{
						if (!this.cells[num].ContainsStaticFire(this.Map))
						{
							num++;
							continue;
						}
						this.lastStaticFireCheckResult = true;
						break;
					}
				}
				return this.lastStaticFireCheckResult;
			}
		}

		public virtual bool IsMultiselectable
		{
			get
			{
				return false;
			}
		}

		protected abstract Color NextZoneColor
		{
			get;
		}

		public Zone()
		{
		}

		public Zone(string baseName, ZoneManager zoneManager)
		{
			this.label = zoneManager.NewZoneName(baseName);
			this.zoneManager = zoneManager;
			this.color = this.NextZoneColor;
			zoneManager.RegisterZone(this);
		}

		public IEnumerator<IntVec3> GetEnumerator()
		{
			int i = 0;
			if (i < this.cells.Count)
			{
				yield return this.cells[i];
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}

		public virtual void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.label, "label", (string)null, false);
			Scribe_Values.Look<Color>(ref this.color, "color", default(Color), false);
			Scribe_Values.Look<bool>(ref this.hidden, "hidden", false, false);
			Scribe_Collections.Look<IntVec3>(ref this.cells, "cells", LookMode.Undefined, new object[0]);
		}

		public virtual void AddCell(IntVec3 c)
		{
			if (this.cells.Contains(c))
			{
				Log.Error("Adding cell to zone which already has it. c=" + c + ", zone=" + this);
			}
			else
			{
				List<Thing> list = this.Map.thingGrid.ThingsListAt(c);
				for (int i = 0; i < list.Count; i++)
				{
					Thing thing = list[i];
					if (!thing.def.CanOverlapZones)
					{
						Log.Error("Added zone over zone-incompatible thing " + thing);
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

		public virtual void RemoveCell(IntVec3 c)
		{
			if (!this.cells.Contains(c))
			{
				Log.Error("Cannot remove cell from zone which doesn't have it. c=" + c + ", zone=" + this);
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

		public virtual void Delete()
		{
			SoundDefOf.DesignateZoneDelete.PlayOneShotOnCamera(this.Map);
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

		public virtual void Deregister()
		{
			this.zoneManager.DeregisterZone(this);
		}

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

		public virtual string GetInspectString()
		{
			return string.Empty;
		}

		public virtual IEnumerable<InspectTabBase> GetInspectTabs()
		{
			yield break;
		}

		public virtual IEnumerable<Gizmo> GetGizmos()
		{
			yield return (Gizmo)new Command_Action
			{
				icon = ContentFinder<Texture2D>.Get("UI/Commands/RenameZone", true),
				defaultLabel = "CommandRenameZoneLabel".Translate(),
				defaultDesc = "CommandRenameZoneDesc".Translate(),
				action = delegate
				{
					Find.WindowStack.Add(new Dialog_RenameZone(((_003CGetGizmos_003Ec__Iterator3)/*Error near IL_007a: stateMachine*/)._0024this));
				},
				hotKey = KeyBindingDefOf.Misc1
			};
			/*Error: Unable to find new state assignment for yield return*/;
		}

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
				Predicate<IntVec3> passCheck = delegate(IntVec3 c)
				{
					if (!Zone.extantGrid[c])
					{
						return false;
					}
					if (Zone.foundGrid[c])
					{
						return false;
					}
					return true;
				};
				int numFound = 0;
				Action<IntVec3> processor = delegate(IntVec3 c)
				{
					Zone.foundGrid.Set(c, true);
					numFound++;
				};
				this.Map.floodFiller.FloodFill(this.cells[0], passCheck, processor, 2147483647, false, null);
				if (numFound < this.cells.Count)
				{
					foreach (IntVec3 allCell in this.Map.AllCells)
					{
						if (Zone.extantGrid[allCell] && !Zone.foundGrid[allCell])
						{
							this.RemoveCell(allCell);
						}
					}
				}
			}
		}

		public override string ToString()
		{
			return this.label;
		}
	}
}
