using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	public class Selector
	{
		public DragBox dragBox = new DragBox();

		private List<object> selected = new List<object>();

		private const float PawnSelectRadius = 1f;

		private const int MaxNumSelected = 80;

		[CompilerGenerated]
		private static Predicate<Thing> <>f__am$cache0;

		[CompilerGenerated]
		private static Predicate<Thing> <>f__am$cache1;

		[CompilerGenerated]
		private static Predicate<Thing> <>f__am$cache2;

		[CompilerGenerated]
		private static Predicate<Thing> <>f__am$cache3;

		[CompilerGenerated]
		private static Predicate<Thing> <>f__am$cache4;

		[CompilerGenerated]
		private static Func<object, bool> <>f__am$cache5;

		[CompilerGenerated]
		private static Func<object, bool> <>f__am$cache6;

		[CompilerGenerated]
		private static Func<object, bool> <>f__am$cache7;

		[CompilerGenerated]
		private static Func<object, bool> <>f__am$cache8;

		public Selector()
		{
		}

		private bool ShiftIsHeld
		{
			get
			{
				return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
			}
		}

		public List<object> SelectedObjects
		{
			get
			{
				return this.selected;
			}
		}

		public List<object> SelectedObjectsListForReading
		{
			get
			{
				return this.selected;
			}
		}

		public Thing SingleSelectedThing
		{
			get
			{
				Thing result;
				if (this.selected.Count != 1)
				{
					result = null;
				}
				else if (this.selected[0] is Thing)
				{
					result = (Thing)this.selected[0];
				}
				else
				{
					result = null;
				}
				return result;
			}
		}

		public object FirstSelectedObject
		{
			get
			{
				object result;
				if (this.selected.Count == 0)
				{
					result = null;
				}
				else
				{
					result = this.selected[0];
				}
				return result;
			}
		}

		public object SingleSelectedObject
		{
			get
			{
				object result;
				if (this.selected.Count != 1)
				{
					result = null;
				}
				else
				{
					result = this.selected[0];
				}
				return result;
			}
		}

		public int NumSelected
		{
			get
			{
				return this.selected.Count;
			}
		}

		public Zone SelectedZone
		{
			get
			{
				Zone result;
				if (this.selected.Count == 0)
				{
					result = null;
				}
				else
				{
					result = (this.selected[0] as Zone);
				}
				return result;
			}
			set
			{
				this.ClearSelection();
				if (value != null)
				{
					this.Select(value, true, true);
				}
			}
		}

		public void SelectorOnGUI()
		{
			this.HandleMapClicks();
			if (KeyBindingDefOf.Cancel.KeyDownEvent)
			{
				if (this.selected.Count > 0)
				{
					this.ClearSelection();
					Event.current.Use();
				}
			}
			if (this.NumSelected > 0 && Find.MainTabsRoot.OpenTab == null && !WorldRendererUtility.WorldRenderedNow)
			{
				Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Inspect, false);
			}
		}

		private void HandleMapClicks()
		{
			if (Event.current.type == EventType.MouseDown)
			{
				if (Event.current.button == 0)
				{
					if (Event.current.clickCount == 1)
					{
						this.dragBox.active = true;
						this.dragBox.start = UI.MouseMapPosition();
					}
					if (Event.current.clickCount == 2)
					{
						this.SelectAllMatchingObjectUnderMouseOnScreen();
					}
					Event.current.Use();
				}
				if (Event.current.button == 1)
				{
					if (this.selected.Count > 0)
					{
						if (this.selected.Count == 1 && this.selected[0] is Pawn)
						{
							FloatMenuMakerMap.TryMakeFloatMenu((Pawn)this.selected[0]);
						}
						else
						{
							for (int i = 0; i < this.selected.Count; i++)
							{
								Pawn pawn = this.selected[i] as Pawn;
								if (pawn != null)
								{
									Selector.MassTakeFirstAutoTakeableOption(pawn, UI.MouseCell());
								}
							}
						}
						Event.current.Use();
					}
				}
			}
			if (Event.current.rawType == EventType.MouseUp)
			{
				if (Event.current.button == 0)
				{
					if (this.dragBox.active)
					{
						this.dragBox.active = false;
						if (!this.dragBox.IsValid)
						{
							this.SelectUnderMouse();
						}
						else
						{
							this.SelectInsideDragBox();
						}
					}
				}
				Event.current.Use();
			}
		}

		public bool IsSelected(object obj)
		{
			return this.selected.Contains(obj);
		}

		public void ClearSelection()
		{
			SelectionDrawer.Clear();
			this.selected.Clear();
		}

		public void Deselect(object obj)
		{
			if (this.selected.Contains(obj))
			{
				this.selected.Remove(obj);
			}
		}

		public void Select(object obj, bool playSound = true, bool forceDesignatorDeselect = true)
		{
			if (obj == null)
			{
				Log.Error("Cannot select null.", false);
			}
			else
			{
				Thing thing = obj as Thing;
				if (thing == null && !(obj is Zone))
				{
					Log.Error("Tried to select " + obj + " which is neither a Thing nor a Zone.", false);
				}
				else if (thing != null && thing.Destroyed)
				{
					Log.Error("Cannot select destroyed thing.", false);
				}
				else
				{
					Pawn pawn = obj as Pawn;
					if (pawn != null && pawn.IsWorldPawn())
					{
						Log.Error("Cannot select world pawns.", false);
					}
					else
					{
						if (forceDesignatorDeselect)
						{
							Find.DesignatorManager.Deselect();
						}
						if (this.SelectedZone != null && !(obj is Zone))
						{
							this.ClearSelection();
						}
						if (obj is Zone && this.SelectedZone == null)
						{
							this.ClearSelection();
						}
						Map map = (thing == null) ? ((Zone)obj).Map : thing.Map;
						for (int i = this.selected.Count - 1; i >= 0; i--)
						{
							Thing thing2 = this.selected[i] as Thing;
							Map map2 = (thing2 == null) ? ((Zone)this.selected[i]).Map : thing2.Map;
							if (map2 != map)
							{
								this.Deselect(this.selected[i]);
							}
						}
						if (this.selected.Count < 80)
						{
							if (!this.IsSelected(obj))
							{
								if (map != Find.CurrentMap)
								{
									Current.Game.CurrentMap = map;
									SoundDefOf.MapSelected.PlayOneShotOnCamera(null);
									IntVec3 cell = (thing == null) ? ((Zone)obj).Cells[0] : thing.Position;
									Find.CameraDriver.JumpToCurrentMapLoc(cell);
								}
								if (playSound)
								{
									this.PlaySelectionSoundFor(obj);
								}
								this.selected.Add(obj);
								SelectionDrawer.Notify_Selected(obj);
							}
						}
					}
				}
			}
		}

		public void Notify_DialogOpened()
		{
			this.dragBox.active = false;
		}

		private void PlaySelectionSoundFor(object obj)
		{
			if (obj is Pawn && ((Pawn)obj).Faction == Faction.OfPlayer && ((Pawn)obj).RaceProps.Humanlike)
			{
				SoundDefOf.ColonistSelected.PlayOneShotOnCamera(null);
			}
			else if (obj is Thing)
			{
				SoundDefOf.ThingSelected.PlayOneShotOnCamera(null);
			}
			else if (obj is Zone)
			{
				SoundDefOf.ZoneSelected.PlayOneShotOnCamera(null);
			}
			else
			{
				Log.Warning("Can't determine selection sound for " + obj, false);
			}
		}

		private void SelectInsideDragBox()
		{
			if (!this.ShiftIsHeld)
			{
				this.ClearSelection();
			}
			bool selectedSomething = false;
			List<Thing> list = Find.ColonistBar.MapColonistsOrCorpsesInScreenRect(this.dragBox.ScreenRect);
			for (int i = 0; i < list.Count; i++)
			{
				selectedSomething = true;
				this.Select(list[i], true, true);
			}
			if (!selectedSomething)
			{
				List<Caravan> list2 = Find.ColonistBar.CaravanMembersCaravansInScreenRect(this.dragBox.ScreenRect);
				for (int j = 0; j < list2.Count; j++)
				{
					if (!selectedSomething)
					{
						CameraJumper.TryJumpAndSelect(list2[j]);
						selectedSomething = true;
					}
					else
					{
						Find.WorldSelector.Select(list2[j], true);
					}
				}
				if (!selectedSomething)
				{
					List<Thing> boxThings = ThingSelectionUtility.MultiSelectableThingsInScreenRectDistinct(this.dragBox.ScreenRect).ToList<Thing>();
					Func<Predicate<Thing>, bool> func = delegate(Predicate<Thing> predicate)
					{
						foreach (Thing obj2 in from t in boxThings
						where predicate(t)
						select t)
						{
							this.Select(obj2, true, true);
							selectedSomething = true;
						}
						return selectedSomething;
					};
					Predicate<Thing> arg = (Thing t) => t.def.category == ThingCategory.Pawn && ((Pawn)t).RaceProps.Humanlike && t.Faction == Faction.OfPlayer;
					if (!func(arg))
					{
						Predicate<Thing> arg2 = (Thing t) => t.def.category == ThingCategory.Pawn && ((Pawn)t).RaceProps.Humanlike;
						if (!func(arg2))
						{
							Predicate<Thing> arg3 = (Thing t) => t.def.CountAsResource;
							if (!func(arg3))
							{
								Predicate<Thing> arg4 = (Thing t) => t.def.category == ThingCategory.Pawn;
								if (!func(arg4))
								{
									if (!func((Thing t) => t.def.selectable))
									{
										List<Zone> list3 = ThingSelectionUtility.MultiSelectableZonesInScreenRectDistinct(this.dragBox.ScreenRect).ToList<Zone>();
										foreach (Zone obj in list3)
										{
											selectedSomething = true;
											this.Select(obj, true, true);
										}
										if (!selectedSomething)
										{
											this.SelectUnderMouse();
										}
									}
								}
							}
						}
					}
				}
			}
		}

		private IEnumerable<object> SelectableObjectsUnderMouse()
		{
			Vector2 mousePos = UI.MousePositionOnUIInverted;
			Thing colonistOrCorpse = Find.ColonistBar.ColonistOrCorpseAt(mousePos);
			if (colonistOrCorpse != null && colonistOrCorpse.Spawned)
			{
				yield return colonistOrCorpse;
				yield break;
			}
			if (!UI.MouseCell().InBounds(Find.CurrentMap))
			{
				yield break;
			}
			TargetingParameters selectParams = new TargetingParameters();
			selectParams.mustBeSelectable = true;
			selectParams.canTargetPawns = true;
			selectParams.canTargetBuildings = true;
			selectParams.canTargetItems = true;
			selectParams.mapObjectTargetsMustBeAutoAttackable = false;
			List<Thing> selectableList = GenUI.ThingsUnderMouse(UI.MouseMapPosition(), 1f, selectParams);
			if (selectableList.Count > 0 && selectableList[0] is Pawn)
			{
				if ((selectableList[0].DrawPos - UI.MouseMapPosition()).MagnitudeHorizontal() < 0.4f)
				{
					for (int j = selectableList.Count - 1; j >= 0; j--)
					{
						Thing thing = selectableList[j];
						if (thing.def.category == ThingCategory.Pawn && (thing.DrawPos - UI.MouseMapPosition()).MagnitudeHorizontal() > 0.4f)
						{
							selectableList.Remove(thing);
						}
					}
				}
			}
			for (int i = 0; i < selectableList.Count; i++)
			{
				yield return selectableList[i];
			}
			Zone z = Find.CurrentMap.zoneManager.ZoneAt(UI.MouseCell());
			if (z != null)
			{
				yield return z;
			}
			yield break;
		}

		public static IEnumerable<object> SelectableObjectsAt(IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Thing t = thingList[i];
				if (ThingSelectionUtility.SelectableByMapClick(t))
				{
					yield return t;
				}
			}
			Zone z = map.zoneManager.ZoneAt(c);
			if (z != null)
			{
				yield return z;
			}
			yield break;
		}

		private void SelectUnderMouse()
		{
			Caravan caravan = Find.ColonistBar.CaravanMemberCaravanAt(UI.MousePositionOnUIInverted);
			if (caravan != null)
			{
				CameraJumper.TryJumpAndSelect(caravan);
			}
			else
			{
				Thing thing = Find.ColonistBar.ColonistOrCorpseAt(UI.MousePositionOnUIInverted);
				if (thing != null && !thing.Spawned)
				{
					CameraJumper.TryJump(thing);
				}
				else
				{
					List<object> list = this.SelectableObjectsUnderMouse().ToList<object>();
					if (list.Count == 0)
					{
						if (!this.ShiftIsHeld)
						{
							this.ClearSelection();
						}
					}
					else if (list.Count == 1)
					{
						object obj4 = list[0];
						if (!this.ShiftIsHeld)
						{
							this.ClearSelection();
							this.Select(obj4, true, true);
						}
						else if (!this.selected.Contains(obj4))
						{
							this.Select(obj4, true, true);
						}
						else
						{
							this.Deselect(obj4);
						}
					}
					else if (list.Count > 1)
					{
						object obj2 = (from obj in list
						where this.selected.Contains(obj)
						select obj).FirstOrDefault<object>();
						if (obj2 != null)
						{
							if (!this.ShiftIsHeld)
							{
								int num = list.IndexOf(obj2) + 1;
								if (num >= list.Count)
								{
									num -= list.Count;
								}
								this.ClearSelection();
								this.Select(list[num], true, true);
							}
							else
							{
								foreach (object obj3 in list)
								{
									if (this.selected.Contains(obj3))
									{
										this.Deselect(obj3);
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
							this.Select(list[0], true, true);
						}
					}
				}
			}
		}

		public void SelectNextAt(IntVec3 c, Map map)
		{
			if (this.SelectedObjects.Count<object>() != 1)
			{
				Log.Error("Cannot select next at with < or > 1 selected.", false);
			}
			else
			{
				List<object> list = Selector.SelectableObjectsAt(c, map).ToList<object>();
				int num = list.IndexOf(this.SingleSelectedThing) + 1;
				if (num >= list.Count)
				{
					num -= list.Count;
				}
				this.ClearSelection();
				this.Select(list[num], true, true);
			}
		}

		private void SelectAllMatchingObjectUnderMouseOnScreen()
		{
			List<object> list = this.SelectableObjectsUnderMouse().ToList<object>();
			if (list.Count != 0)
			{
				Thing clickedThing = list.FirstOrDefault((object o) => o is Pawn && ((Pawn)o).Faction == Faction.OfPlayer && !((Pawn)o).IsPrisoner) as Thing;
				clickedThing = (list.FirstOrDefault((object o) => o is Pawn) as Thing);
				if (clickedThing == null)
				{
					clickedThing = ((from o in list
					where o is Thing && !((Thing)o).def.neverMultiSelect
					select o).FirstOrDefault<object>() as Thing);
				}
				Rect rect = new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight);
				if (clickedThing == null)
				{
					if (list.FirstOrDefault((object o) => o is Zone && ((Zone)o).IsMultiselectable) != null)
					{
						IEnumerable<Zone> enumerable = ThingSelectionUtility.MultiSelectableZonesInScreenRectDistinct(rect);
						foreach (Zone obj in enumerable)
						{
							if (!this.IsSelected(obj))
							{
								this.Select(obj, true, true);
							}
						}
					}
				}
				else
				{
					IEnumerable enumerable2 = ThingSelectionUtility.MultiSelectableThingsInScreenRectDistinct(rect);
					Predicate<Thing> predicate = delegate(Thing t)
					{
						bool result;
						if (t.def != clickedThing.def || t.Faction != clickedThing.Faction || this.IsSelected(t))
						{
							result = false;
						}
						else
						{
							Pawn pawn = clickedThing as Pawn;
							if (pawn != null)
							{
								Pawn pawn2 = t as Pawn;
								if (pawn2.RaceProps != pawn.RaceProps)
								{
									return false;
								}
								if (pawn2.HostFaction != pawn.HostFaction)
								{
									return false;
								}
							}
							result = true;
						}
						return result;
					};
					IEnumerator enumerator2 = enumerable2.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							object obj2 = enumerator2.Current;
							Thing obj3 = (Thing)obj2;
							if (predicate(obj3))
							{
								this.Select(obj3, true, true);
							}
						}
					}
					finally
					{
						IDisposable disposable;
						if ((disposable = (enumerator2 as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
				}
			}
		}

		private static void MassTakeFirstAutoTakeableOption(Pawn pawn, IntVec3 dest)
		{
			FloatMenuOption floatMenuOption = null;
			foreach (FloatMenuOption floatMenuOption2 in FloatMenuMakerMap.ChoicesAtFor(dest.ToVector3Shifted(), pawn))
			{
				if (!floatMenuOption2.Disabled && floatMenuOption2.autoTakeable)
				{
					if (floatMenuOption == null || floatMenuOption2.autoTakeablePriority > floatMenuOption.autoTakeablePriority)
					{
						floatMenuOption = floatMenuOption2;
					}
				}
			}
			if (floatMenuOption != null)
			{
				floatMenuOption.Chosen(true);
			}
		}

		[CompilerGenerated]
		private static bool <SelectInsideDragBox>m__0(Thing t)
		{
			return t.def.category == ThingCategory.Pawn && ((Pawn)t).RaceProps.Humanlike && t.Faction == Faction.OfPlayer;
		}

		[CompilerGenerated]
		private static bool <SelectInsideDragBox>m__1(Thing t)
		{
			return t.def.category == ThingCategory.Pawn && ((Pawn)t).RaceProps.Humanlike;
		}

		[CompilerGenerated]
		private static bool <SelectInsideDragBox>m__2(Thing t)
		{
			return t.def.CountAsResource;
		}

		[CompilerGenerated]
		private static bool <SelectInsideDragBox>m__3(Thing t)
		{
			return t.def.category == ThingCategory.Pawn;
		}

		[CompilerGenerated]
		private static bool <SelectInsideDragBox>m__4(Thing t)
		{
			return t.def.selectable;
		}

		[CompilerGenerated]
		private bool <SelectUnderMouse>m__5(object obj)
		{
			return this.selected.Contains(obj);
		}

		[CompilerGenerated]
		private static bool <SelectAllMatchingObjectUnderMouseOnScreen>m__6(object o)
		{
			return o is Pawn && ((Pawn)o).Faction == Faction.OfPlayer && !((Pawn)o).IsPrisoner;
		}

		[CompilerGenerated]
		private static bool <SelectAllMatchingObjectUnderMouseOnScreen>m__7(object o)
		{
			return o is Pawn;
		}

		[CompilerGenerated]
		private static bool <SelectAllMatchingObjectUnderMouseOnScreen>m__8(object o)
		{
			return o is Thing && !((Thing)o).def.neverMultiSelect;
		}

		[CompilerGenerated]
		private static bool <SelectAllMatchingObjectUnderMouseOnScreen>m__9(object o)
		{
			return o is Zone && ((Zone)o).IsMultiselectable;
		}

		[CompilerGenerated]
		private sealed class <SelectInsideDragBox>c__AnonStorey2
		{
			internal List<Thing> boxThings;

			internal bool selectedSomething;

			internal Selector $this;

			public <SelectInsideDragBox>c__AnonStorey2()
			{
			}

			internal bool <>m__0(Predicate<Thing> predicate)
			{
				foreach (Thing obj in from t in this.boxThings
				where predicate(t)
				select t)
				{
					this.$this.Select(obj, true, true);
					this.selectedSomething = true;
				}
				return this.selectedSomething;
			}

			private sealed class <SelectInsideDragBox>c__AnonStorey3
			{
				internal Predicate<Thing> predicate;

				internal Selector.<SelectInsideDragBox>c__AnonStorey2 <>f__ref$2;

				public <SelectInsideDragBox>c__AnonStorey3()
				{
				}

				internal bool <>m__0(Thing t)
				{
					return this.predicate(t);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <SelectableObjectsUnderMouse>c__Iterator0 : IEnumerable, IEnumerable<object>, IEnumerator, IDisposable, IEnumerator<object>
		{
			internal Vector2 <mousePos>__0;

			internal Thing <colonistOrCorpse>__0;

			internal TargetingParameters <selectParams>__0;

			internal List<Thing> <selectableList>__0;

			internal int <i>__1;

			internal Zone <z>__0;

			internal object $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <SelectableObjectsUnderMouse>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					mousePos = UI.MousePositionOnUIInverted;
					colonistOrCorpse = Find.ColonistBar.ColonistOrCorpseAt(mousePos);
					if (colonistOrCorpse != null && colonistOrCorpse.Spawned)
					{
						this.$current = colonistOrCorpse;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					if (!UI.MouseCell().InBounds(Find.CurrentMap))
					{
						return false;
					}
					selectParams = new TargetingParameters();
					selectParams.mustBeSelectable = true;
					selectParams.canTargetPawns = true;
					selectParams.canTargetBuildings = true;
					selectParams.canTargetItems = true;
					selectParams.mapObjectTargetsMustBeAutoAttackable = false;
					selectableList = GenUI.ThingsUnderMouse(UI.MouseMapPosition(), 1f, selectParams);
					if (selectableList.Count > 0 && selectableList[0] is Pawn)
					{
						if ((selectableList[0].DrawPos - UI.MouseMapPosition()).MagnitudeHorizontal() < 0.4f)
						{
							for (int j = selectableList.Count - 1; j >= 0; j--)
							{
								Thing thing = selectableList[j];
								if (thing.def.category == ThingCategory.Pawn && (thing.DrawPos - UI.MouseMapPosition()).MagnitudeHorizontal() > 0.4f)
								{
									selectableList.Remove(thing);
								}
							}
						}
					}
					i = 0;
					break;
				case 1u:
					return false;
				case 2u:
					i++;
					break;
				case 3u:
					IL_268:
					this.$PC = -1;
					return false;
				default:
					return false;
				}
				if (i < selectableList.Count)
				{
					this.$current = selectableList[i];
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				z = Find.CurrentMap.zoneManager.ZoneAt(UI.MouseCell());
				if (z != null)
				{
					this.$current = z;
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				goto IL_268;
			}

			object IEnumerator<object>.Current
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
				return this.System.Collections.Generic.IEnumerable<object>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<object> IEnumerable<object>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				return new Selector.<SelectableObjectsUnderMouse>c__Iterator0();
			}
		}

		[CompilerGenerated]
		private sealed class <SelectableObjectsAt>c__Iterator1 : IEnumerable, IEnumerable<object>, IEnumerator, IDisposable, IEnumerator<object>
		{
			internal IntVec3 c;

			internal Map map;

			internal List<Thing> <thingList>__0;

			internal int <i>__1;

			internal Thing <t>__2;

			internal Zone <z>__0;

			internal object $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <SelectableObjectsAt>c__Iterator1()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					thingList = c.GetThingList(map);
					i = 0;
					break;
				case 1u:
					IL_91:
					i++;
					break;
				case 2u:
					IL_FD:
					this.$PC = -1;
					return false;
				default:
					return false;
				}
				if (i >= thingList.Count)
				{
					z = map.zoneManager.ZoneAt(c);
					if (z == null)
					{
						goto IL_FD;
					}
					this.$current = z;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
				}
				else
				{
					t = thingList[i];
					if (!ThingSelectionUtility.SelectableByMapClick(t))
					{
						goto IL_91;
					}
					this.$current = t;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
				}
				return true;
			}

			object IEnumerator<object>.Current
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
				return this.System.Collections.Generic.IEnumerable<object>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<object> IEnumerable<object>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Selector.<SelectableObjectsAt>c__Iterator1 <SelectableObjectsAt>c__Iterator = new Selector.<SelectableObjectsAt>c__Iterator1();
				<SelectableObjectsAt>c__Iterator.c = c;
				<SelectableObjectsAt>c__Iterator.map = map;
				return <SelectableObjectsAt>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <SelectAllMatchingObjectUnderMouseOnScreen>c__AnonStorey4
		{
			internal Thing clickedThing;

			internal Selector $this;

			public <SelectAllMatchingObjectUnderMouseOnScreen>c__AnonStorey4()
			{
			}

			internal bool <>m__0(Thing t)
			{
				bool result;
				if (t.def != this.clickedThing.def || t.Faction != this.clickedThing.Faction || this.$this.IsSelected(t))
				{
					result = false;
				}
				else
				{
					Pawn pawn = this.clickedThing as Pawn;
					if (pawn != null)
					{
						Pawn pawn2 = t as Pawn;
						if (pawn2.RaceProps != pawn.RaceProps)
						{
							return false;
						}
						if (pawn2.HostFaction != pawn.HostFaction)
						{
							return false;
						}
					}
					result = true;
				}
				return result;
			}
		}
	}
}
