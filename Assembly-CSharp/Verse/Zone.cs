using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	public abstract class Zone : IExposable, ISelectable, ILoadReferenceable
	{
		public ZoneManager zoneManager;

		public string label;

		public List<IntVec3> cells = new List<IntVec3>();

		private bool cellsShuffled = false;

		public Color color = Color.white;

		private Material materialInt = null;

		public bool hidden = false;

		private int lastStaticFireCheckTick = -9999;

		private bool lastStaticFireCheckResult = false;

		private const int StaticFireCheckInterval = 1000;

		private static BoolGrid extantGrid;

		private static BoolGrid foundGrid;

		[CompilerGenerated]
		private static Predicate<IntVec3> <>f__am$cache0;

		public Zone()
		{
		}

		public Zone(string baseName, ZoneManager zoneManager)
		{
			this.label = zoneManager.NewZoneName(baseName);
			this.zoneManager = zoneManager;
			this.color = this.NextZoneColor;
		}

		public Map Map
		{
			get
			{
				return this.zoneManager.map;
			}
		}

		public IntVec3 Position
		{
			get
			{
				return (this.cells.Count == 0) ? IntVec3.Invalid : this.cells[0];
			}
		}

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

		public IEnumerator<IntVec3> GetEnumerator()
		{
			for (int i = 0; i < this.cells.Count; i++)
			{
				yield return this.cells[i];
			}
			yield break;
		}

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

		public virtual bool IsMultiselectable
		{
			get
			{
				return false;
			}
		}

		protected abstract Color NextZoneColor { get; }

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

		public void Deregister()
		{
			this.zoneManager.DeregisterZone(this);
		}

		public virtual void PostRegister()
		{
			this.CheckAddHaulDestination();
		}

		public virtual void PostDeregister()
		{
			IHaulDestination haulDestination = this as IHaulDestination;
			if (haulDestination != null)
			{
				this.Map.haulDestinationManager.RemoveHaulDestination(haulDestination);
			}
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
			return "";
		}

		public virtual IEnumerable<InspectTabBase> GetInspectTabs()
		{
			yield break;
		}

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

		public virtual IEnumerable<Gizmo> GetZoneAddGizmos()
		{
			yield break;
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

		private void CheckAddHaulDestination()
		{
			IHaulDestination haulDestination = this as IHaulDestination;
			if (haulDestination != null)
			{
				this.Map.haulDestinationManager.AddHaulDestination(haulDestination);
			}
		}

		public override string ToString()
		{
			return this.label;
		}

		public string GetUniqueLoadID()
		{
			return "Zone_" + this.zoneManager.AllZones.IndexOf(this);
		}

		[CompilerGenerated]
		private static bool <CheckContiguous>m__0(IntVec3 c)
		{
			return Zone.extantGrid[c] && !Zone.foundGrid[c];
		}

		[CompilerGenerated]
		private sealed class <GetEnumerator>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<IntVec3>
		{
			internal int <i>__1;

			internal Zone $this;

			internal IntVec3 $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetEnumerator>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					i = 0;
					break;
				case 1u:
					i++;
					break;
				default:
					return false;
				}
				if (i < this.cells.Count)
				{
					this.$current = this.cells[i];
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				this.$PC = -1;
				return false;
			}

			IntVec3 IEnumerator<IntVec3>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator1 : IEnumerable, IEnumerable<Thing>, IEnumerator, IDisposable, IEnumerator<Thing>
		{
			internal ThingGrid <grids>__0;

			internal int <i>__1;

			internal List<Thing> <thingList>__2;

			internal int <j>__3;

			internal Zone $this;

			internal Thing $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator1()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					grids = base.Map.thingGrid;
					i = 0;
					goto IL_D8;
				case 1u:
					j++;
					break;
				default:
					return false;
				}
				IL_B3:
				if (j < thingList.Count)
				{
					this.$current = thingList[j];
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				i++;
				IL_D8:
				if (i < this.cells.Count)
				{
					thingList = grids.ThingsListAt(this.cells[i]);
					j = 0;
					goto IL_B3;
				}
				this.$PC = -1;
				return false;
			}

			Thing IEnumerator<Thing>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Thing>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Zone.<>c__Iterator1 <>c__Iterator = new Zone.<>c__Iterator1();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <GetInspectTabs>c__Iterator2 : IEnumerable, IEnumerable<InspectTabBase>, IEnumerator, IDisposable, IEnumerator<InspectTabBase>
		{
			internal InspectTabBase $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetInspectTabs>c__Iterator2()
			{
			}

			public bool MoveNext()
			{
				bool flag = this.$PC != 0;
				this.$PC = -1;
				if (!flag)
				{
				}
				return false;
			}

			InspectTabBase IEnumerator<InspectTabBase>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.InspectTabBase>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<InspectTabBase> IEnumerable<InspectTabBase>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				return new Zone.<GetInspectTabs>c__Iterator2();
			}
		}

		[CompilerGenerated]
		private sealed class <GetGizmos>c__Iterator3 : IEnumerable, IEnumerable<Gizmo>, IEnumerator, IDisposable, IEnumerator<Gizmo>
		{
			internal Command_Action <rename>__1;

			internal Command_Toggle <hide>__2;

			internal IEnumerator<Gizmo> $locvar0;

			internal Gizmo <gizmo>__3;

			internal Designator <delete>__4;

			internal Command_Action <delete>__5;

			internal Zone $this;

			internal Gizmo $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetGizmos>c__Iterator3()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
				{
					Command_Action rename = new Command_Action();
					rename.icon = ContentFinder<Texture2D>.Get("UI/Commands/RenameZone", true);
					rename.defaultLabel = "CommandRenameZoneLabel".Translate();
					rename.defaultDesc = "CommandRenameZoneDesc".Translate();
					rename.action = delegate()
					{
						Find.WindowStack.Add(new Dialog_RenameZone(this));
					};
					rename.hotKey = KeyBindingDefOf.Misc1;
					this.$current = rename;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				case 1u:
				{
					Command_Toggle hide = new Command_Toggle();
					hide.icon = ContentFinder<Texture2D>.Get("UI/Commands/HideZone", true);
					hide.defaultLabel = ((!this.hidden) ? "CommandHideZoneLabel".Translate() : "CommandUnhideZoneLabel".Translate());
					hide.defaultDesc = "CommandHideZoneDesc".Translate();
					hide.isActive = (() => this.hidden);
					hide.toggleAction = delegate()
					{
						this.hidden = !this.hidden;
						foreach (IntVec3 loc in base.Cells)
						{
							base.Map.mapDrawer.MapMeshDirty(loc, MapMeshFlag.Zone);
						}
					};
					hide.hotKey = KeyBindingDefOf.Misc2;
					this.$current = hide;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				case 2u:
					enumerator = this.GetZoneAddGizmos().GetEnumerator();
					num = 4294967293u;
					break;
				case 3u:
					break;
				case 4u:
					goto IL_259;
				case 5u:
					this.$PC = -1;
					return false;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						gizmo = enumerator.Current;
						this.$current = gizmo;
						if (!this.$disposing)
						{
							this.$PC = 3;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				delete = DesignatorUtility.FindAllowedDesignator<Designator_ZoneDelete_Shrink>();
				if (delete != null)
				{
					this.$current = delete;
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				}
				IL_259:
				Command_Action delete2 = new Command_Action();
				delete2.icon = ContentFinder<Texture2D>.Get("UI/Buttons/Delete", true);
				delete2.defaultLabel = "CommandDeleteZoneLabel".Translate();
				delete2.defaultDesc = "CommandDeleteZoneDesc".Translate();
				delete2.action = new Action(this.Delete);
				delete2.hotKey = KeyBindingDefOf.Misc3;
				this.$current = delete2;
				if (!this.$disposing)
				{
					this.$PC = 5;
				}
				return true;
			}

			Gizmo IEnumerator<Gizmo>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 3u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Gizmo>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Zone.<GetGizmos>c__Iterator3 <GetGizmos>c__Iterator = new Zone.<GetGizmos>c__Iterator3();
				<GetGizmos>c__Iterator.$this = this;
				return <GetGizmos>c__Iterator;
			}

			internal void <>m__0()
			{
				Find.WindowStack.Add(new Dialog_RenameZone(this));
			}

			internal bool <>m__1()
			{
				return this.hidden;
			}

			internal void <>m__2()
			{
				this.hidden = !this.hidden;
				foreach (IntVec3 loc in base.Cells)
				{
					base.Map.mapDrawer.MapMeshDirty(loc, MapMeshFlag.Zone);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <GetZoneAddGizmos>c__Iterator4 : IEnumerable, IEnumerable<Gizmo>, IEnumerator, IDisposable, IEnumerator<Gizmo>
		{
			internal Gizmo $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetZoneAddGizmos>c__Iterator4()
			{
			}

			public bool MoveNext()
			{
				bool flag = this.$PC != 0;
				this.$PC = -1;
				if (!flag)
				{
				}
				return false;
			}

			Gizmo IEnumerator<Gizmo>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Gizmo>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				return new Zone.<GetZoneAddGizmos>c__Iterator4();
			}
		}

		[CompilerGenerated]
		private sealed class <CheckContiguous>c__AnonStorey5
		{
			internal int numFound;

			public <CheckContiguous>c__AnonStorey5()
			{
			}

			internal void <>m__0(IntVec3 c)
			{
				Zone.foundGrid.Set(c, true);
				this.numFound++;
			}
		}
	}
}
