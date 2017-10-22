using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class ThingSelectionUtility
	{
		private static HashSet<Thing> yieldedThings = new HashSet<Thing>();

		private static HashSet<Zone> yieldedZones = new HashSet<Zone>();

		private static List<Pawn> tmpColonists = new List<Pawn>();

		public static bool SelectableByMapClick(Thing t)
		{
			if (!t.def.selectable)
			{
				return false;
			}
			if (!t.Spawned)
			{
				return false;
			}
			if (t.def.size.x == 1 && t.def.size.z == 1)
			{
				return !t.Position.Fogged(t.Map);
			}
			CellRect.CellRectIterator iterator = t.OccupiedRect().GetIterator();
			while (!iterator.Done())
			{
				if (!iterator.Current.Fogged(t.Map))
				{
					return true;
				}
				iterator.MoveNext();
			}
			return false;
		}

		public static bool SelectableByHotkey(Thing t)
		{
			return t.def.selectable && t.Spawned;
		}

		public static IEnumerable<Thing> MultiSelectableThingsInScreenRectDistinct(Rect rect)
		{
			CellRect mapRect = ThingSelectionUtility.GetMapRect(rect);
			ThingSelectionUtility.yieldedThings.Clear();
			foreach (IntVec3 item in mapRect)
			{
				if (item.InBounds(Find.VisibleMap))
				{
					List<Thing> cellThings = Find.VisibleMap.thingGrid.ThingsListAt(item);
					if (cellThings != null)
					{
						for (int i = 0; i < cellThings.Count; i++)
						{
							Thing t = cellThings[i];
							if (ThingSelectionUtility.SelectableByMapClick(t) && !t.def.neverMultiSelect && !ThingSelectionUtility.yieldedThings.Contains(t))
							{
								yield return t;
								ThingSelectionUtility.yieldedThings.Add(t);
							}
						}
					}
				}
			}
		}

		public static IEnumerable<Zone> MultiSelectableZonesInScreenRectDistinct(Rect rect)
		{
			CellRect mapRect = ThingSelectionUtility.GetMapRect(rect);
			ThingSelectionUtility.yieldedZones.Clear();
			foreach (IntVec3 item in mapRect)
			{
				if (item.InBounds(Find.VisibleMap))
				{
					Zone zone = item.GetZone(Find.VisibleMap);
					if (zone != null && zone.IsMultiselectable && !ThingSelectionUtility.yieldedZones.Contains(zone))
					{
						yield return zone;
						ThingSelectionUtility.yieldedZones.Add(zone);
					}
				}
			}
		}

		private static CellRect GetMapRect(Rect rect)
		{
			Vector2 screenLoc = new Vector2(rect.x, (float)UI.screenHeight - rect.y);
			Vector2 screenLoc2 = new Vector2(rect.x + rect.width, (float)UI.screenHeight - (rect.y + rect.height));
			Vector3 vector = UI.UIToMapPosition(screenLoc);
			Vector3 vector2 = UI.UIToMapPosition(screenLoc2);
			return new CellRect
			{
				minX = Mathf.FloorToInt(vector.x),
				minZ = Mathf.FloorToInt(vector2.z),
				maxX = Mathf.FloorToInt(vector2.x),
				maxZ = Mathf.FloorToInt(vector.z)
			};
		}

		public static void SelectNextColonist()
		{
			ThingSelectionUtility.tmpColonists.Clear();
			ThingSelectionUtility.tmpColonists.AddRange(Find.ColonistBar.GetColonistsInOrder().Where(new Func<Pawn, bool>(ThingSelectionUtility.SelectableByHotkey)));
			if (ThingSelectionUtility.tmpColonists.Count != 0)
			{
				bool worldRenderedNow = WorldRendererUtility.WorldRenderedNow;
				int num = -1;
				int num2 = ThingSelectionUtility.tmpColonists.Count - 1;
				while (num2 >= 0)
				{
					if (!worldRenderedNow && Find.Selector.IsSelected(ThingSelectionUtility.tmpColonists[num2]))
					{
						goto IL_00b3;
					}
					if (worldRenderedNow && ThingSelectionUtility.tmpColonists[num2].IsCaravanMember() && Find.WorldSelector.IsSelected(ThingSelectionUtility.tmpColonists[num2].GetCaravan()))
						goto IL_00b3;
					num2--;
					continue;
					IL_00b3:
					num = num2;
					break;
				}
				if (num == -1)
				{
					CameraJumper.TryJumpAndSelect((Thing)ThingSelectionUtility.tmpColonists[0]);
				}
				else
				{
					CameraJumper.TryJumpAndSelect((Thing)ThingSelectionUtility.tmpColonists[(num + 1) % ThingSelectionUtility.tmpColonists.Count]);
				}
				ThingSelectionUtility.tmpColonists.Clear();
			}
		}

		public static void SelectPreviousColonist()
		{
			ThingSelectionUtility.tmpColonists.Clear();
			ThingSelectionUtility.tmpColonists.AddRange(Find.ColonistBar.GetColonistsInOrder().Where(new Func<Pawn, bool>(ThingSelectionUtility.SelectableByHotkey)));
			if (ThingSelectionUtility.tmpColonists.Count != 0)
			{
				bool worldRenderedNow = WorldRendererUtility.WorldRenderedNow;
				int num = -1;
				for (int i = 0; i < ThingSelectionUtility.tmpColonists.Count; i++)
				{
					if (!worldRenderedNow && Find.Selector.IsSelected(ThingSelectionUtility.tmpColonists[i]))
					{
						goto IL_00a8;
					}
					if (worldRenderedNow && ThingSelectionUtility.tmpColonists[i].IsCaravanMember() && Find.WorldSelector.IsSelected(ThingSelectionUtility.tmpColonists[i].GetCaravan()))
						goto IL_00a8;
					continue;
					IL_00a8:
					num = i;
					break;
				}
				if (num == -1)
				{
					CameraJumper.TryJumpAndSelect((Thing)ThingSelectionUtility.tmpColonists[ThingSelectionUtility.tmpColonists.Count - 1]);
				}
				else
				{
					CameraJumper.TryJumpAndSelect((Thing)ThingSelectionUtility.tmpColonists[GenMath.PositiveMod(num - 1, ThingSelectionUtility.tmpColonists.Count)]);
				}
				ThingSelectionUtility.tmpColonists.Clear();
			}
		}
	}
}
