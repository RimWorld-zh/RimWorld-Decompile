using RimWorld.Planet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
				return (this.selected.Count == 1) ? ((!(this.selected[0] is Thing)) ? null : ((Thing)this.selected[0])) : null;
			}
		}

		public object FirstSelectedObject
		{
			get
			{
				return (this.selected.Count != 0) ? this.selected[0] : null;
			}
		}

		public object SingleSelectedObject
		{
			get
			{
				return (this.selected.Count == 1) ? this.selected[0] : null;
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
				return (this.selected.Count != 0) ? (this.selected[0] as Zone) : null;
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
			if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape && this.selected.Count > 0)
			{
				this.ClearSelection();
				Event.current.Use();
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
				if (Event.current.button == 1 && this.selected.Count > 0)
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
								Selector.AutoOrderToCell(pawn, UI.MouseCell());
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
						this.SelectUnderMouse();
					}
					else
					{
						this.SelectInsideDragBox();
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
				Log.Error("Cannot select null.");
			}
			else
			{
				Thing thing = obj as Thing;
				if (thing == null && !(obj is Zone))
				{
					Log.Error("Tried to select " + obj + " which is neither a Thing nor a Zone.");
				}
				else if (thing != null && thing.Destroyed)
				{
					Log.Error("Cannot select destroyed thing.");
				}
				else
				{
					Pawn pawn = obj as Pawn;
					if (pawn != null && pawn.IsWorldPawn())
					{
						Log.Error("Cannot select world pawns.");
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
						for (int num = this.selected.Count - 1; num >= 0; num--)
						{
							Thing thing2 = this.selected[num] as Thing;
							Map map2 = (thing2 == null) ? ((Zone)this.selected[num]).Map : thing2.Map;
							if (map2 != map)
							{
								this.Deselect(this.selected[num]);
							}
						}
						if (this.selected.Count < 80 && !this.IsSelected(obj))
						{
							if (map != Current.Game.VisibleMap)
							{
								Current.Game.VisibleMap = map;
								SoundDefOf.MapSelected.PlayOneShotOnCamera(null);
								IntVec3 cell = (thing == null) ? ((Zone)obj).Cells[0] : thing.Position;
								Find.CameraDriver.JumpToVisibleMapLoc(cell);
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
				Log.Warning("Can't determine selection sound for " + obj);
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
						CameraJumper.TryJumpAndSelect((WorldObject)list2[j]);
						selectedSomething = true;
					}
					else
					{
						Find.WorldSelector.Select(list2[j], true);
					}
				}
				if (!selectedSomething)
				{
					List<Thing> boxThings = ThingSelectionUtility.MultiSelectableThingsInScreenRectDistinct(this.dragBox.ScreenRect).ToList();
					Func<Predicate<Thing>, bool> func = (Func<Predicate<Thing>, bool>)delegate(Predicate<Thing> predicate)
					{
						foreach (Thing item in from t in boxThings
						where predicate(t)
						select t)
						{
							this.Select(item, true, true);
							selectedSomething = true;
						}
						return selectedSomething;
					};
					Predicate<Thing> arg = (Predicate<Thing>)((Thing t) => t.def.category == ThingCategory.Pawn && ((Pawn)t).RaceProps.Humanlike && t.Faction == Faction.OfPlayer);
					if (!func(arg))
					{
						Predicate<Thing> arg2 = (Predicate<Thing>)((Thing t) => t.def.category == ThingCategory.Pawn && ((Pawn)t).RaceProps.Humanlike);
						if (!func(arg2))
						{
							Predicate<Thing> arg3 = (Predicate<Thing>)((Thing t) => t.def.CountAsResource);
							if (!func(arg3))
							{
								Predicate<Thing> arg4 = (Predicate<Thing>)((Thing t) => t.def.category == ThingCategory.Pawn);
								if (!func(arg4) && !func((Predicate<Thing>)((Thing t) => t.def.selectable)))
								{
									List<Zone> list3 = ThingSelectionUtility.MultiSelectableZonesInScreenRectDistinct(this.dragBox.ScreenRect).ToList();
									foreach (Zone item2 in list3)
									{
										selectedSomething = true;
										this.Select(item2, true, true);
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

		private IEnumerable<object> SelectableObjectsUnderMouse()
		{
			Vector2 mousePos = UI.MousePositionOnUIInverted;
			Thing colonistOrCorpse = Find.ColonistBar.ColonistOrCorpseAt(mousePos);
			if (colonistOrCorpse != null && colonistOrCorpse.Spawned)
			{
				yield return (object)colonistOrCorpse;
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (UI.MouseCell().InBounds(Find.VisibleMap))
			{
				TargetingParameters selectParams = new TargetingParameters
				{
					mustBeSelectable = true,
					canTargetPawns = true,
					canTargetBuildings = true,
					canTargetItems = true,
					mapObjectTargetsMustBeAutoAttackable = false
				};
				List<Thing> selectableList = GenUI.ThingsUnderMouse(UI.MouseMapPosition(), 1f, selectParams);
				if (selectableList.Count > 0 && selectableList[0] is Pawn && (selectableList[0].DrawPos - UI.MouseMapPosition()).MagnitudeHorizontal() < 0.40000000596046448)
				{
					for (int num = selectableList.Count - 1; num >= 0; num--)
					{
						Thing thing = selectableList[num];
						if (thing.def.category == ThingCategory.Pawn && (thing.DrawPos - UI.MouseMapPosition()).MagnitudeHorizontal() > 0.40000000596046448)
						{
							selectableList.Remove(thing);
						}
					}
				}
				int i = 0;
				if (i < selectableList.Count)
				{
					yield return (object)selectableList[i];
					/*Error: Unable to find new state assignment for yield return*/;
				}
				Zone z = Find.VisibleMap.zoneManager.ZoneAt(UI.MouseCell());
				if (z == null)
					yield break;
				yield return (object)z;
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}

		public static IEnumerable<object> SelectableObjectsAt(IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Thing t = thingList[i];
				if (ThingSelectionUtility.SelectableByMapClick(t))
				{
					yield return (object)t;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			Zone z = map.zoneManager.ZoneAt(c);
			if (z == null)
				yield break;
			yield return (object)z;
			/*Error: Unable to find new state assignment for yield return*/;
		}

		private void SelectUnderMouse()
		{
			Caravan caravan = Find.ColonistBar.CaravanMemberCaravanAt(UI.MousePositionOnUIInverted);
			if (caravan != null)
			{
				CameraJumper.TryJumpAndSelect((WorldObject)caravan);
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
					List<object> list = this.SelectableObjectsUnderMouse().ToList();
					if (list.Count == 0)
					{
						if (!this.ShiftIsHeld)
						{
							this.ClearSelection();
						}
					}
					else if (list.Count == 1)
					{
						object obj2 = list[0];
						if (!this.ShiftIsHeld)
						{
							this.ClearSelection();
							this.Select(obj2, true, true);
						}
						else if (!this.selected.Contains(obj2))
						{
							this.Select(obj2, true, true);
						}
						else
						{
							this.Deselect(obj2);
						}
					}
					else if (list.Count > 1)
					{
						object obj3 = (from obj in list
						where this.selected.Contains(obj)
						select obj).FirstOrDefault();
						if (obj3 != null)
						{
							if (!this.ShiftIsHeld)
							{
								int num = list.IndexOf(obj3) + 1;
								if (num >= list.Count)
								{
									num -= list.Count;
								}
								this.ClearSelection();
								this.Select(list[num], true, true);
							}
							else
							{
								foreach (object item in list)
								{
									if (this.selected.Contains(item))
									{
										this.Deselect(item);
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
			if (this.SelectedObjects.Count() != 1)
			{
				Log.Error("Cannot select next at with < or > 1 selected.");
			}
			else
			{
				List<object> list = Selector.SelectableObjectsAt(c, map).ToList();
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
			List<object> list = this.SelectableObjectsUnderMouse().ToList();
			if (list.Count != 0)
			{
				Thing clickedThing = list.FirstOrDefault((Func<object, bool>)((object o) => o is Pawn && ((Pawn)o).Faction == Faction.OfPlayer && !((Pawn)o).IsPrisoner)) as Thing;
				clickedThing = (list.FirstOrDefault((Func<object, bool>)((object o) => o is Pawn)) as Thing);
				if (clickedThing == null)
				{
					clickedThing = ((from o in list
					where o is Thing && !((Thing)o).def.neverMultiSelect
					select o).FirstOrDefault() as Thing);
				}
				Rect rect = new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight);
				if (clickedThing == null)
				{
					object obj = list.FirstOrDefault((Func<object, bool>)((object o) => o is Zone && ((Zone)o).IsMultiselectable));
					if (obj != null)
					{
						IEnumerable<Zone> enumerable = ThingSelectionUtility.MultiSelectableZonesInScreenRectDistinct(rect);
						foreach (Zone item in enumerable)
						{
							if (!this.IsSelected(item))
							{
								this.Select(item, true, true);
							}
						}
					}
				}
				else
				{
					IEnumerable enumerable2 = ThingSelectionUtility.MultiSelectableThingsInScreenRectDistinct(rect);
					Predicate<Thing> predicate = (Predicate<Thing>)delegate(Thing t)
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
									result = false;
									goto IL_0097;
								}
								if (pawn2.HostFaction != pawn.HostFaction)
								{
									result = false;
									goto IL_0097;
								}
							}
							result = true;
						}
						goto IL_0097;
						IL_0097:
						return result;
					};
					IEnumerator enumerator2 = enumerable2.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							Thing obj2 = (Thing)enumerator2.Current;
							if (predicate(obj2))
							{
								this.Select(obj2, true, true);
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

		private static void AutoOrderToCell(Pawn pawn, IntVec3 dest)
		{
			using (List<FloatMenuOption>.Enumerator enumerator = FloatMenuMakerMap.ChoicesAtFor(dest.ToVector3Shifted(), pawn).GetEnumerator())
			{
				FloatMenuOption current;
				while (true)
				{
					if (enumerator.MoveNext())
					{
						current = enumerator.Current;
						if (current.autoTakeable)
							break;
						continue;
					}
					return;
				}
				current.Chosen(true);
			}
		}
	}
}
