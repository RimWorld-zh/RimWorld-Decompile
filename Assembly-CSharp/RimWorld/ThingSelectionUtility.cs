using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000863 RID: 2147
	public static class ThingSelectionUtility
	{
		// Token: 0x060030B7 RID: 12471 RVA: 0x001A7114 File Offset: 0x001A5514
		public static bool SelectableByMapClick(Thing t)
		{
			bool result;
			if (!t.def.selectable)
			{
				result = false;
			}
			else if (!t.Spawned)
			{
				result = false;
			}
			else if (t.def.size.x == 1 && t.def.size.z == 1)
			{
				result = !t.Position.Fogged(t.Map);
			}
			else
			{
				CellRect.CellRectIterator iterator = t.OccupiedRect().GetIterator();
				while (!iterator.Done())
				{
					if (!iterator.Current.Fogged(t.Map))
					{
						return true;
					}
					iterator.MoveNext();
				}
				result = false;
			}
			return result;
		}

		// Token: 0x060030B8 RID: 12472 RVA: 0x001A71E4 File Offset: 0x001A55E4
		public static bool SelectableByHotkey(Thing t)
		{
			return t.def.selectable && t.Spawned;
		}

		// Token: 0x060030B9 RID: 12473 RVA: 0x001A7214 File Offset: 0x001A5614
		public static IEnumerable<Thing> MultiSelectableThingsInScreenRectDistinct(Rect rect)
		{
			CellRect mapRect = ThingSelectionUtility.GetMapRect(rect);
			ThingSelectionUtility.yieldedThings.Clear();
			foreach (IntVec3 c in mapRect)
			{
				if (c.InBounds(Find.CurrentMap))
				{
					List<Thing> cellThings = Find.CurrentMap.thingGrid.ThingsListAt(c);
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
			yield break;
		}

		// Token: 0x060030BA RID: 12474 RVA: 0x001A7240 File Offset: 0x001A5640
		public static IEnumerable<Zone> MultiSelectableZonesInScreenRectDistinct(Rect rect)
		{
			CellRect mapRect = ThingSelectionUtility.GetMapRect(rect);
			ThingSelectionUtility.yieldedZones.Clear();
			foreach (IntVec3 c in mapRect)
			{
				if (c.InBounds(Find.CurrentMap))
				{
					Zone zone = c.GetZone(Find.CurrentMap);
					if (zone != null)
					{
						if (zone.IsMultiselectable)
						{
							if (!ThingSelectionUtility.yieldedZones.Contains(zone))
							{
								yield return zone;
								ThingSelectionUtility.yieldedZones.Add(zone);
							}
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x060030BB RID: 12475 RVA: 0x001A726C File Offset: 0x001A566C
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

		// Token: 0x060030BC RID: 12476 RVA: 0x001A7330 File Offset: 0x001A5730
		public static void SelectNextColonist()
		{
			ThingSelectionUtility.tmpColonists.Clear();
			List<Pawn> list = ThingSelectionUtility.tmpColonists;
			IEnumerable<Pawn> colonistsInOrder = Find.ColonistBar.GetColonistsInOrder();
			if (ThingSelectionUtility.<>f__mg$cache0 == null)
			{
				ThingSelectionUtility.<>f__mg$cache0 = new Func<Pawn, bool>(ThingSelectionUtility.SelectableByHotkey);
			}
			list.AddRange(colonistsInOrder.Where(ThingSelectionUtility.<>f__mg$cache0));
			if (ThingSelectionUtility.tmpColonists.Count != 0)
			{
				bool worldRenderedNow = WorldRendererUtility.WorldRenderedNow;
				int num = -1;
				for (int i = ThingSelectionUtility.tmpColonists.Count - 1; i >= 0; i--)
				{
					if ((!worldRenderedNow && Find.Selector.IsSelected(ThingSelectionUtility.tmpColonists[i])) || (worldRenderedNow && ThingSelectionUtility.tmpColonists[i].IsCaravanMember() && Find.WorldSelector.IsSelected(ThingSelectionUtility.tmpColonists[i].GetCaravan())))
					{
						num = i;
						break;
					}
				}
				if (num == -1)
				{
					CameraJumper.TryJumpAndSelect(ThingSelectionUtility.tmpColonists[0]);
				}
				else
				{
					CameraJumper.TryJumpAndSelect(ThingSelectionUtility.tmpColonists[(num + 1) % ThingSelectionUtility.tmpColonists.Count]);
				}
				ThingSelectionUtility.tmpColonists.Clear();
			}
		}

		// Token: 0x060030BD RID: 12477 RVA: 0x001A746C File Offset: 0x001A586C
		public static void SelectPreviousColonist()
		{
			ThingSelectionUtility.tmpColonists.Clear();
			List<Pawn> list = ThingSelectionUtility.tmpColonists;
			IEnumerable<Pawn> colonistsInOrder = Find.ColonistBar.GetColonistsInOrder();
			if (ThingSelectionUtility.<>f__mg$cache1 == null)
			{
				ThingSelectionUtility.<>f__mg$cache1 = new Func<Pawn, bool>(ThingSelectionUtility.SelectableByHotkey);
			}
			list.AddRange(colonistsInOrder.Where(ThingSelectionUtility.<>f__mg$cache1));
			if (ThingSelectionUtility.tmpColonists.Count != 0)
			{
				bool worldRenderedNow = WorldRendererUtility.WorldRenderedNow;
				int num = -1;
				for (int i = 0; i < ThingSelectionUtility.tmpColonists.Count; i++)
				{
					if ((!worldRenderedNow && Find.Selector.IsSelected(ThingSelectionUtility.tmpColonists[i])) || (worldRenderedNow && ThingSelectionUtility.tmpColonists[i].IsCaravanMember() && Find.WorldSelector.IsSelected(ThingSelectionUtility.tmpColonists[i].GetCaravan())))
					{
						num = i;
						break;
					}
				}
				if (num == -1)
				{
					CameraJumper.TryJumpAndSelect(ThingSelectionUtility.tmpColonists[ThingSelectionUtility.tmpColonists.Count - 1]);
				}
				else
				{
					CameraJumper.TryJumpAndSelect(ThingSelectionUtility.tmpColonists[GenMath.PositiveMod(num - 1, ThingSelectionUtility.tmpColonists.Count)]);
				}
				ThingSelectionUtility.tmpColonists.Clear();
			}
		}

		// Token: 0x04001A5E RID: 6750
		private static HashSet<Thing> yieldedThings = new HashSet<Thing>();

		// Token: 0x04001A5F RID: 6751
		private static HashSet<Zone> yieldedZones = new HashSet<Zone>();

		// Token: 0x04001A60 RID: 6752
		private static List<Pawn> tmpColonists = new List<Pawn>();

		// Token: 0x04001A61 RID: 6753
		[CompilerGenerated]
		private static Func<Pawn, bool> <>f__mg$cache0;

		// Token: 0x04001A62 RID: 6754
		[CompilerGenerated]
		private static Func<Pawn, bool> <>f__mg$cache1;
	}
}
