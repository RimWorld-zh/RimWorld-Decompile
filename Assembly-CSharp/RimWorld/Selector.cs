using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000864 RID: 2148
	public class Selector
	{
		// Token: 0x04001A63 RID: 6755
		public DragBox dragBox = new DragBox();

		// Token: 0x04001A64 RID: 6756
		private List<object> selected = new List<object>();

		// Token: 0x04001A65 RID: 6757
		private const float PawnSelectRadius = 1f;

		// Token: 0x04001A66 RID: 6758
		private const int MaxNumSelected = 80;

		// Token: 0x170007CB RID: 1995
		// (get) Token: 0x060030C0 RID: 12480 RVA: 0x001A7B38 File Offset: 0x001A5F38
		private bool ShiftIsHeld
		{
			get
			{
				return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
			}
		}

		// Token: 0x170007CC RID: 1996
		// (get) Token: 0x060030C1 RID: 12481 RVA: 0x001A7B6C File Offset: 0x001A5F6C
		public List<object> SelectedObjects
		{
			get
			{
				return this.selected;
			}
		}

		// Token: 0x170007CD RID: 1997
		// (get) Token: 0x060030C2 RID: 12482 RVA: 0x001A7B88 File Offset: 0x001A5F88
		public List<object> SelectedObjectsListForReading
		{
			get
			{
				return this.selected;
			}
		}

		// Token: 0x170007CE RID: 1998
		// (get) Token: 0x060030C3 RID: 12483 RVA: 0x001A7BA4 File Offset: 0x001A5FA4
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

		// Token: 0x170007CF RID: 1999
		// (get) Token: 0x060030C4 RID: 12484 RVA: 0x001A7C00 File Offset: 0x001A6000
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

		// Token: 0x170007D0 RID: 2000
		// (get) Token: 0x060030C5 RID: 12485 RVA: 0x001A7C38 File Offset: 0x001A6038
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

		// Token: 0x170007D1 RID: 2001
		// (get) Token: 0x060030C6 RID: 12486 RVA: 0x001A7C74 File Offset: 0x001A6074
		public int NumSelected
		{
			get
			{
				return this.selected.Count;
			}
		}

		// Token: 0x170007D2 RID: 2002
		// (get) Token: 0x060030C7 RID: 12487 RVA: 0x001A7C94 File Offset: 0x001A6094
		// (set) Token: 0x060030C8 RID: 12488 RVA: 0x001A7CD1 File Offset: 0x001A60D1
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

		// Token: 0x060030C9 RID: 12489 RVA: 0x001A7CEC File Offset: 0x001A60EC
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

		// Token: 0x060030CA RID: 12490 RVA: 0x001A7D6C File Offset: 0x001A616C
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

		// Token: 0x060030CB RID: 12491 RVA: 0x001A7F18 File Offset: 0x001A6318
		public bool IsSelected(object obj)
		{
			return this.selected.Contains(obj);
		}

		// Token: 0x060030CC RID: 12492 RVA: 0x001A7F39 File Offset: 0x001A6339
		public void ClearSelection()
		{
			SelectionDrawer.Clear();
			this.selected.Clear();
		}

		// Token: 0x060030CD RID: 12493 RVA: 0x001A7F4C File Offset: 0x001A634C
		public void Deselect(object obj)
		{
			if (this.selected.Contains(obj))
			{
				this.selected.Remove(obj);
			}
		}

		// Token: 0x060030CE RID: 12494 RVA: 0x001A7F70 File Offset: 0x001A6370
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

		// Token: 0x060030CF RID: 12495 RVA: 0x001A8188 File Offset: 0x001A6588
		public void Notify_DialogOpened()
		{
			this.dragBox.active = false;
		}

		// Token: 0x060030D0 RID: 12496 RVA: 0x001A8198 File Offset: 0x001A6598
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

		// Token: 0x060030D1 RID: 12497 RVA: 0x001A8234 File Offset: 0x001A6634
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

		// Token: 0x060030D2 RID: 12498 RVA: 0x001A84E4 File Offset: 0x001A68E4
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

		// Token: 0x060030D3 RID: 12499 RVA: 0x001A8508 File Offset: 0x001A6908
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

		// Token: 0x060030D4 RID: 12500 RVA: 0x001A853C File Offset: 0x001A693C
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

		// Token: 0x060030D5 RID: 12501 RVA: 0x001A8740 File Offset: 0x001A6B40
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

		// Token: 0x060030D6 RID: 12502 RVA: 0x001A87B8 File Offset: 0x001A6BB8
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

		// Token: 0x060030D7 RID: 12503 RVA: 0x001A89D8 File Offset: 0x001A6DD8
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
	}
}
