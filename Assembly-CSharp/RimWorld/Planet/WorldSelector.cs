using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x020008EF RID: 2287
	public class WorldSelector
	{
		// Token: 0x04001C8B RID: 7307
		public WorldDragBox dragBox = new WorldDragBox();

		// Token: 0x04001C8C RID: 7308
		private List<WorldObject> selected = new List<WorldObject>();

		// Token: 0x04001C8D RID: 7309
		public int selectedTile = -1;

		// Token: 0x04001C8E RID: 7310
		private const int MaxNumSelected = 80;

		// Token: 0x04001C8F RID: 7311
		private const float MaxDragBoxDiagonalToSelectTile = 30f;

		// Token: 0x17000886 RID: 2182
		// (get) Token: 0x060034CD RID: 13517 RVA: 0x001C3930 File Offset: 0x001C1D30
		private bool ShiftIsHeld
		{
			get
			{
				return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
			}
		}

		// Token: 0x17000887 RID: 2183
		// (get) Token: 0x060034CE RID: 13518 RVA: 0x001C3964 File Offset: 0x001C1D64
		public List<WorldObject> SelectedObjects
		{
			get
			{
				return this.selected;
			}
		}

		// Token: 0x17000888 RID: 2184
		// (get) Token: 0x060034CF RID: 13519 RVA: 0x001C3980 File Offset: 0x001C1D80
		public WorldObject SingleSelectedObject
		{
			get
			{
				WorldObject result;
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

		// Token: 0x17000889 RID: 2185
		// (get) Token: 0x060034D0 RID: 13520 RVA: 0x001C39BC File Offset: 0x001C1DBC
		public WorldObject FirstSelectedObject
		{
			get
			{
				WorldObject result;
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

		// Token: 0x1700088A RID: 2186
		// (get) Token: 0x060034D1 RID: 13521 RVA: 0x001C39F4 File Offset: 0x001C1DF4
		public int NumSelectedObjects
		{
			get
			{
				return this.selected.Count;
			}
		}

		// Token: 0x1700088B RID: 2187
		// (get) Token: 0x060034D2 RID: 13522 RVA: 0x001C3A14 File Offset: 0x001C1E14
		public bool AnyObjectOrTileSelected
		{
			get
			{
				return this.NumSelectedObjects != 0 || this.selectedTile >= 0;
			}
		}

		// Token: 0x060034D3 RID: 13523 RVA: 0x001C3A43 File Offset: 0x001C1E43
		public void WorldSelectorOnGUI()
		{
			this.HandleWorldClicks();
			if (KeyBindingDefOf.Cancel.KeyDownEvent)
			{
				if (this.selected.Count > 0)
				{
					this.ClearSelection();
					Event.current.Use();
				}
			}
		}

		// Token: 0x060034D4 RID: 13524 RVA: 0x001C3A80 File Offset: 0x001C1E80
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
				if (Event.current.button == 1)
				{
					if (this.selected.Count > 0)
					{
						if (this.selected.Count == 1 && this.selected[0] is Caravan)
						{
							Caravan caravan = (Caravan)this.selected[0];
							if (caravan.IsPlayerControlled)
							{
								if (!FloatMenuMakerWorld.TryMakeFloatMenu(caravan))
								{
									this.AutoOrderToTile(caravan, GenWorld.MouseTile(false));
								}
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
							this.SelectUnderMouse(true);
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

		// Token: 0x060034D5 RID: 13525 RVA: 0x001C3C50 File Offset: 0x001C2050
		public bool IsSelected(WorldObject obj)
		{
			return this.selected.Contains(obj);
		}

		// Token: 0x060034D6 RID: 13526 RVA: 0x001C3C71 File Offset: 0x001C2071
		public void ClearSelection()
		{
			WorldSelectionDrawer.Clear();
			this.selected.Clear();
			this.selectedTile = -1;
		}

		// Token: 0x060034D7 RID: 13527 RVA: 0x001C3C8B File Offset: 0x001C208B
		public void Deselect(WorldObject obj)
		{
			if (this.selected.Contains(obj))
			{
				this.selected.Remove(obj);
			}
		}

		// Token: 0x060034D8 RID: 13528 RVA: 0x001C3CAC File Offset: 0x001C20AC
		public void Select(WorldObject obj, bool playSound = true)
		{
			if (obj == null)
			{
				Log.Error("Cannot select null.", false);
			}
			else
			{
				this.selectedTile = -1;
				if (this.selected.Count < 80)
				{
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
			}
		}

		// Token: 0x060034D9 RID: 13529 RVA: 0x001C3D1C File Offset: 0x001C211C
		public void Notify_DialogOpened()
		{
			this.dragBox.active = false;
		}

		// Token: 0x060034DA RID: 13530 RVA: 0x001C3D2B File Offset: 0x001C212B
		private void PlaySelectionSoundFor(WorldObject obj)
		{
			SoundDefOf.ThingSelected.PlayOneShotOnCamera(null);
		}

		// Token: 0x060034DB RID: 13531 RVA: 0x001C3D3C File Offset: 0x001C213C
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

		// Token: 0x060034DC RID: 13532 RVA: 0x001C3F38 File Offset: 0x001C2338
		public IEnumerable<WorldObject> SelectableObjectsUnderMouse()
		{
			bool flag;
			return this.SelectableObjectsUnderMouse(out flag, out flag);
		}

		// Token: 0x060034DD RID: 13533 RVA: 0x001C3F58 File Offset: 0x001C2358
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
			if (list.Count > 0 && list[0] is Caravan)
			{
				if (list[0].DistanceToMouse(UI.MousePositionOnUI) < GenWorldUI.CaravanDirectClickRadius)
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
			}
			usedColonistBar = false;
			return list;
		}

		// Token: 0x060034DE RID: 13534 RVA: 0x001C404C File Offset: 0x001C244C
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

		// Token: 0x060034DF RID: 13535 RVA: 0x001C4078 File Offset: 0x001C2478
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

		// Token: 0x060034E0 RID: 13536 RVA: 0x001C4234 File Offset: 0x001C2634
		public void SelectFirstOrNextAt(int tileID)
		{
			this.SelectFirstOrNextFrom(WorldSelector.SelectableObjectsAt(tileID).ToList<WorldObject>(), tileID);
		}

		// Token: 0x060034E1 RID: 13537 RVA: 0x001C424C File Offset: 0x001C264C
		private void SelectAllMatchingObjectUnderMouseOnScreen()
		{
			List<WorldObject> list = this.SelectableObjectsUnderMouse().ToList<WorldObject>();
			if (list.Count != 0)
			{
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
		}

		// Token: 0x060034E2 RID: 13538 RVA: 0x001C4314 File Offset: 0x001C2714
		private void AutoOrderToTile(Caravan c, int tile)
		{
			if (tile >= 0)
			{
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
		}

		// Token: 0x060034E3 RID: 13539 RVA: 0x001C439C File Offset: 0x001C279C
		private void AutoOrderToTileNow(Caravan c, int tile)
		{
			if (tile >= 0 && (tile != c.Tile || c.pather.Moving))
			{
				int num = CaravanUtility.BestGotoDestNear(tile, c);
				if (num >= 0)
				{
					c.pather.StartPath(num, null, true, true);
					c.gotoMote.OrderedToTile(num);
					SoundDefOf.ColonistOrdered.PlayOneShotOnCamera(null);
				}
			}
		}

		// Token: 0x060034E4 RID: 13540 RVA: 0x001C440C File Offset: 0x001C280C
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
	}
}
