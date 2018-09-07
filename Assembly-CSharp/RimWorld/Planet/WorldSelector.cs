using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	public class WorldSelector
	{
		public WorldDragBox dragBox = new WorldDragBox();

		private List<WorldObject> selected = new List<WorldObject>();

		public int selectedTile = -1;

		private const int MaxNumSelected = 80;

		private const float MaxDragBoxDiagonalToSelectTile = 30f;

		[CompilerGenerated]
		private static Predicate<WorldObject> <>f__am$cache0;

		[CompilerGenerated]
		private static Predicate<WorldObject> <>f__am$cache1;

		[CompilerGenerated]
		private static Predicate<WorldObject> <>f__am$cache2;

		[CompilerGenerated]
		private static Predicate<WorldObject> <>f__am$cache3;

		public WorldSelector()
		{
		}

		private bool ShiftIsHeld
		{
			get
			{
				return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
			}
		}

		public List<WorldObject> SelectedObjects
		{
			get
			{
				return this.selected;
			}
		}

		public WorldObject SingleSelectedObject
		{
			get
			{
				if (this.selected.Count != 1)
				{
					return null;
				}
				return this.selected[0];
			}
		}

		public WorldObject FirstSelectedObject
		{
			get
			{
				if (this.selected.Count == 0)
				{
					return null;
				}
				return this.selected[0];
			}
		}

		public int NumSelectedObjects
		{
			get
			{
				return this.selected.Count;
			}
		}

		public bool AnyObjectOrTileSelected
		{
			get
			{
				return this.NumSelectedObjects != 0 || this.selectedTile >= 0;
			}
		}

		public void WorldSelectorOnGUI()
		{
			this.HandleWorldClicks();
			if (KeyBindingDefOf.Cancel.KeyDownEvent && this.selected.Count > 0)
			{
				this.ClearSelection();
				Event.current.Use();
			}
		}

		private void HandleWorldClicks()
		{
			if (Event.current.type == EventType.MouseDown)
			{
				if (Event.current.button == 0)
				{
					if (Event.current.clickCount == 1)
					{
						this.dragBox.active = true;
						this.dragBox.start = UI.MousePositionOnUIInverted;
					}
					if (Event.current.clickCount == 2)
					{
						this.SelectAllMatchingObjectUnderMouseOnScreen();
					}
					Event.current.Use();
				}
				if (Event.current.button == 1 && this.selected.Count > 0)
				{
					if (this.selected.Count == 1 && this.selected[0] is Caravan)
					{
						Caravan caravan = (Caravan)this.selected[0];
						if (caravan.IsPlayerControlled && !FloatMenuMakerWorld.TryMakeFloatMenu(caravan))
						{
							this.AutoOrderToTile(caravan, GenWorld.MouseTile(false));
						}
					}
					else
					{
						for (int i = 0; i < this.selected.Count; i++)
						{
							Caravan caravan2 = this.selected[i] as Caravan;
							if (caravan2 != null && caravan2.IsPlayerControlled)
							{
								this.AutoOrderToTile(caravan2, GenWorld.MouseTile(false));
							}
						}
					}
					Event.current.Use();
				}
			}
			if (Event.current.rawType == EventType.MouseUp)
			{
				if (Event.current.button == 0 && this.dragBox.active)
				{
					this.dragBox.active = false;
					if (!this.dragBox.IsValid)
					{
						this.SelectUnderMouse(true);
					}
					else
					{
						this.SelectInsideDragBox();
					}
				}
				Event.current.Use();
			}
		}

		public bool IsSelected(WorldObject obj)
		{
			return this.selected.Contains(obj);
		}

		public void ClearSelection()
		{
			WorldSelectionDrawer.Clear();
			this.selected.Clear();
			this.selectedTile = -1;
		}

		public void Deselect(WorldObject obj)
		{
			if (this.selected.Contains(obj))
			{
				this.selected.Remove(obj);
			}
		}

		public void Select(WorldObject obj, bool playSound = true)
		{
			if (obj == null)
			{
				Log.Error("Cannot select null.", false);
				return;
			}
			this.selectedTile = -1;
			if (this.selected.Count >= 80)
			{
				return;
			}
			if (!this.IsSelected(obj))
			{
				if (playSound)
				{
					this.PlaySelectionSoundFor(obj);
				}
				this.selected.Add(obj);
				WorldSelectionDrawer.Notify_Selected(obj);
			}
		}

		public void Notify_DialogOpened()
		{
			this.dragBox.active = false;
		}

		private void PlaySelectionSoundFor(WorldObject obj)
		{
			SoundDefOf.ThingSelected.PlayOneShotOnCamera(null);
		}

		private void SelectInsideDragBox()
		{
			if (!this.ShiftIsHeld)
			{
				this.ClearSelection();
			}
			bool flag = false;
			if (Current.ProgramState == ProgramState.Playing)
			{
				List<Caravan> list = Find.ColonistBar.CaravanMembersCaravansInScreenRect(this.dragBox.ScreenRect);
				for (int i = 0; i < list.Count; i++)
				{
					flag = true;
					this.Select(list[i], true);
				}
			}
			if (!flag && Current.ProgramState == ProgramState.Playing)
			{
				List<Thing> list2 = Find.ColonistBar.MapColonistsOrCorpsesInScreenRect(this.dragBox.ScreenRect);
				for (int j = 0; j < list2.Count; j++)
				{
					if (!flag)
					{
						CameraJumper.TryJumpAndSelect(list2[j]);
						flag = true;
					}
					else
					{
						Find.Selector.Select(list2[j], true, true);
					}
				}
			}
			if (!flag)
			{
				List<WorldObject> list3 = WorldObjectSelectionUtility.MultiSelectableWorldObjectsInScreenRectDistinct(this.dragBox.ScreenRect).ToList<WorldObject>();
				if (list3.Any((WorldObject x) => x is Caravan))
				{
					list3.RemoveAll((WorldObject x) => !(x is Caravan));
					if (list3.Any((WorldObject x) => x.Faction == Faction.OfPlayer))
					{
						list3.RemoveAll((WorldObject x) => x.Faction != Faction.OfPlayer);
					}
				}
				for (int k = 0; k < list3.Count; k++)
				{
					flag = true;
					this.Select(list3[k], true);
				}
			}
			if (!flag)
			{
				bool canSelectTile = this.dragBox.Diagonal < 30f;
				this.SelectUnderMouse(canSelectTile);
			}
		}

		public IEnumerable<WorldObject> SelectableObjectsUnderMouse()
		{
			bool flag;
			return this.SelectableObjectsUnderMouse(out flag, out flag);
		}

		public IEnumerable<WorldObject> SelectableObjectsUnderMouse(out bool clickedDirectlyOnCaravan, out bool usedColonistBar)
		{
			Vector2 mousePositionOnUIInverted = UI.MousePositionOnUIInverted;
			if (Current.ProgramState == ProgramState.Playing)
			{
				Caravan caravan = Find.ColonistBar.CaravanMemberCaravanAt(mousePositionOnUIInverted);
				if (caravan != null)
				{
					clickedDirectlyOnCaravan = true;
					usedColonistBar = true;
					return Gen.YieldSingle<WorldObject>(caravan);
				}
			}
			List<WorldObject> list = GenWorldUI.WorldObjectsUnderMouse(UI.MousePositionOnUI);
			clickedDirectlyOnCaravan = false;
			if (list.Count > 0 && list[0] is Caravan && list[0].DistanceToMouse(UI.MousePositionOnUI) < GenWorldUI.CaravanDirectClickRadius)
			{
				clickedDirectlyOnCaravan = true;
				for (int i = list.Count - 1; i >= 0; i--)
				{
					WorldObject worldObject = list[i];
					if (worldObject is Caravan && worldObject.DistanceToMouse(UI.MousePositionOnUI) > GenWorldUI.CaravanDirectClickRadius)
					{
						list.Remove(worldObject);
					}
				}
			}
			usedColonistBar = false;
			return list;
		}

		public static IEnumerable<WorldObject> SelectableObjectsAt(int tileID)
		{
			foreach (WorldObject o in Find.WorldObjects.ObjectsAt(tileID))
			{
				if (o.SelectableNow)
				{
					yield return o;
				}
			}
			yield break;
		}

		private void SelectUnderMouse(bool canSelectTile = true)
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				Thing thing = Find.ColonistBar.ColonistOrCorpseAt(UI.MousePositionOnUIInverted);
				Pawn pawn = thing as Pawn;
				if (thing != null && (pawn == null || !pawn.IsCaravanMember()))
				{
					if (thing.Spawned)
					{
						CameraJumper.TryJumpAndSelect(thing);
					}
					else
					{
						CameraJumper.TryJump(thing);
					}
					return;
				}
			}
			bool flag;
			bool flag2;
			List<WorldObject> list = this.SelectableObjectsUnderMouse(out flag, out flag2).ToList<WorldObject>();
			if (flag2 || (flag && list.Count >= 2))
			{
				canSelectTile = false;
			}
			if (list.Count == 0)
			{
				if (!this.ShiftIsHeld)
				{
					this.ClearSelection();
					if (canSelectTile)
					{
						this.selectedTile = GenWorld.MouseTile(false);
					}
				}
			}
			else
			{
				WorldObject worldObject = (from obj in list
				where this.selected.Contains(obj)
				select obj).FirstOrDefault<WorldObject>();
				if (worldObject != null)
				{
					if (!this.ShiftIsHeld)
					{
						int tile = (!canSelectTile) ? -1 : GenWorld.MouseTile(false);
						this.SelectFirstOrNextFrom(list, tile);
					}
					else
					{
						foreach (WorldObject worldObject2 in list)
						{
							if (this.selected.Contains(worldObject2))
							{
								this.Deselect(worldObject2);
							}
						}
					}
				}
				else
				{
					if (!this.ShiftIsHeld)
					{
						this.ClearSelection();
					}
					this.Select(list[0], true);
				}
			}
		}

		public void SelectFirstOrNextAt(int tileID)
		{
			this.SelectFirstOrNextFrom(WorldSelector.SelectableObjectsAt(tileID).ToList<WorldObject>(), tileID);
		}

		private void SelectAllMatchingObjectUnderMouseOnScreen()
		{
			List<WorldObject> list = this.SelectableObjectsUnderMouse().ToList<WorldObject>();
			if (list.Count == 0)
			{
				return;
			}
			Type type = list[0].GetType();
			List<WorldObject> allWorldObjects = Find.WorldObjects.AllWorldObjects;
			for (int i = 0; i < allWorldObjects.Count; i++)
			{
				if (type == allWorldObjects[i].GetType())
				{
					if (allWorldObjects[i] == list[0] || allWorldObjects[i].AllMatchingObjectsOnScreenMatchesWith(list[0]))
					{
						if (allWorldObjects[i].VisibleToCameraNow())
						{
							this.Select(allWorldObjects[i], true);
						}
					}
				}
			}
		}

		private void AutoOrderToTile(Caravan c, int tile)
		{
			if (tile < 0)
			{
				return;
			}
			if (c.autoJoinable && CaravanExitMapUtility.AnyoneTryingToJoinCaravan(c))
			{
				CaravanExitMapUtility.OpenSomeoneTryingToJoinCaravanDialog(c, delegate
				{
					this.AutoOrderToTileNow(c, tile);
				});
			}
			else
			{
				this.AutoOrderToTileNow(c, tile);
			}
		}

		private void AutoOrderToTileNow(Caravan c, int tile)
		{
			if (tile < 0 || (tile == c.Tile && !c.pather.Moving))
			{
				return;
			}
			int num = CaravanUtility.BestGotoDestNear(tile, c);
			if (num >= 0)
			{
				c.pather.StartPath(num, null, true, true);
				c.gotoMote.OrderedToTile(num);
				SoundDefOf.ColonistOrdered.PlayOneShotOnCamera(null);
			}
		}

		private void SelectFirstOrNextFrom(List<WorldObject> objects, int tile)
		{
			int num = objects.FindIndex((WorldObject x) => this.selected.Contains(x));
			int num2 = -1;
			int num3 = -1;
			if (num != -1)
			{
				if (num == objects.Count - 1 || this.selected.Count >= 2)
				{
					if (this.selected.Count >= 2)
					{
						num3 = 0;
					}
					else if (tile >= 0)
					{
						num2 = tile;
					}
					else
					{
						num3 = 0;
					}
				}
				else
				{
					num3 = num + 1;
				}
			}
			else if (objects.Count == 0)
			{
				num2 = tile;
			}
			else
			{
				num3 = 0;
			}
			this.ClearSelection();
			if (num3 >= 0)
			{
				this.Select(objects[num3], true);
			}
			this.selectedTile = num2;
		}

		[CompilerGenerated]
		private static bool <SelectInsideDragBox>m__0(WorldObject x)
		{
			return x is Caravan;
		}

		[CompilerGenerated]
		private static bool <SelectInsideDragBox>m__1(WorldObject x)
		{
			return !(x is Caravan);
		}

		[CompilerGenerated]
		private static bool <SelectInsideDragBox>m__2(WorldObject x)
		{
			return x.Faction == Faction.OfPlayer;
		}

		[CompilerGenerated]
		private static bool <SelectInsideDragBox>m__3(WorldObject x)
		{
			return x.Faction != Faction.OfPlayer;
		}

		[CompilerGenerated]
		private bool <SelectUnderMouse>m__4(WorldObject obj)
		{
			return this.selected.Contains(obj);
		}

		[CompilerGenerated]
		private bool <SelectFirstOrNextFrom>m__5(WorldObject x)
		{
			return this.selected.Contains(x);
		}

		[CompilerGenerated]
		private sealed class <SelectableObjectsAt>c__Iterator0 : IEnumerable, IEnumerable<WorldObject>, IEnumerator, IDisposable, IEnumerator<WorldObject>
		{
			internal int tileID;

			internal IEnumerator<WorldObject> $locvar0;

			internal WorldObject <o>__1;

			internal WorldObject $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <SelectableObjectsAt>c__Iterator0()
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
					enumerator = Find.WorldObjects.ObjectsAt(tileID).GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					while (enumerator.MoveNext())
					{
						o = enumerator.Current;
						if (o.SelectableNow)
						{
							this.$current = o;
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							flag = true;
							return true;
						}
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
				this.$PC = -1;
				return false;
			}

			WorldObject IEnumerator<WorldObject>.Current
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
				case 1u:
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
				return this.System.Collections.Generic.IEnumerable<RimWorld.Planet.WorldObject>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<WorldObject> IEnumerable<WorldObject>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				WorldSelector.<SelectableObjectsAt>c__Iterator0 <SelectableObjectsAt>c__Iterator = new WorldSelector.<SelectableObjectsAt>c__Iterator0();
				<SelectableObjectsAt>c__Iterator.tileID = tileID;
				return <SelectableObjectsAt>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <AutoOrderToTile>c__AnonStorey1
		{
			internal Caravan c;

			internal int tile;

			internal WorldSelector $this;

			public <AutoOrderToTile>c__AnonStorey1()
			{
			}

			internal void <>m__0()
			{
				this.$this.AutoOrderToTileNow(this.c, this.tile);
			}
		}
	}
}
