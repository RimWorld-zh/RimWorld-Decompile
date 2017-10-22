using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class TutorUtility
	{
		public static bool BuildingOrBlueprintOrFrameCenterExists(IntVec3 c, Map map, ThingDef buildingDef)
		{
			List<Thing> thingList = c.GetThingList(map);
			int num = 0;
			bool result;
			while (true)
			{
				if (num < thingList.Count)
				{
					Thing thing = thingList[num];
					if (!(thing.Position != c))
					{
						if (thing.def == buildingDef)
						{
							result = true;
							break;
						}
						if (thing.def.entityDefToBuild == buildingDef)
						{
							result = true;
							break;
						}
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}

		public static CellRect FindUsableRect(int width, int height, Map map, float minFertility = 0f, bool noItems = false)
		{
			IntVec3 center = map.Center;
			float num = 1f;
			CellRect cellRect;
			while (true)
			{
				IntVec3 center2 = center + new IntVec3((int)Rand.Range((float)(0.0 - num), num), 0, (int)Rand.Range((float)(0.0 - num), num));
				cellRect = CellRect.CenteredOn(center2, width / 2);
				cellRect.Width = width;
				cellRect.Height = height;
				cellRect = cellRect.ExpandedBy(1);
				bool flag = true;
				CellRect.CellRectIterator iterator = cellRect.GetIterator();
				while (!iterator.Done())
				{
					IntVec3 current = iterator.Current;
					if (!current.Fogged(map) && current.Walkable(map) && current.GetTerrain(map).affordances.Contains(TerrainAffordance.Heavy) && !(current.GetTerrain(map).fertility < minFertility) && current.GetZone(map) == null && !TutorUtility.ContainsBlockingThing(current, map, noItems) && !current.InNoBuildEdgeArea(map) && !current.InNoZoneEdgeArea(map))
					{
						iterator.MoveNext();
						continue;
					}
					flag = false;
					break;
				}
				if (!flag)
				{
					num = (float)(num + 0.25);
					continue;
				}
				break;
			}
			return cellRect.ContractedBy(1);
		}

		private static bool ContainsBlockingThing(IntVec3 cell, Map map, bool noItems)
		{
			List<Thing> thingList = cell.GetThingList(map);
			int num = 0;
			bool result;
			while (true)
			{
				if (num < thingList.Count)
				{
					if (thingList[num].def.category == ThingCategory.Building)
					{
						result = true;
						break;
					}
					if (thingList[num] is Blueprint)
					{
						result = true;
						break;
					}
					if (noItems && thingList[num].def.category == ThingCategory.Item)
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}

		public static void DrawLabelOnThingOnGUI(Thing t, string label)
		{
			Vector2 vector = (t.DrawPos + new Vector3(0f, 0f, 0.5f)).MapToUIPosition();
			Vector2 vector2 = Text.CalcSize(label);
			Rect rect = new Rect((float)(vector.x - vector2.x / 2.0), (float)(vector.y - vector2.y / 2.0), vector2.x, vector2.y);
			GUI.DrawTexture(rect, TexUI.GrayTextBG);
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, label);
			Text.Anchor = TextAnchor.UpperLeft;
		}

		public static void DrawLabelOnGUI(Vector3 mapPos, string label)
		{
			Vector2 vector = mapPos.MapToUIPosition();
			Vector2 vector2 = Text.CalcSize(label);
			Rect rect = new Rect((float)(vector.x - vector2.x / 2.0), (float)(vector.y - vector2.y / 2.0), vector2.x, vector2.y);
			GUI.DrawTexture(rect, TexUI.GrayTextBG);
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, label);
			Text.Anchor = TextAnchor.UpperLeft;
		}

		public static void DrawCellRectOnGUI(CellRect cellRect, string label = null)
		{
			if (label != null)
			{
				Vector3 centerVector = cellRect.CenterVector3;
				TutorUtility.DrawLabelOnGUI(centerVector, label);
			}
		}

		public static void DrawCellRectUpdate(CellRect cellRect)
		{
			CellRect.CellRectIterator iterator = cellRect.GetIterator();
			while (!iterator.Done())
			{
				CellRenderer.RenderCell(iterator.Current, 0.5f);
				iterator.MoveNext();
			}
		}

		public static void DoModalDialogIfNotKnown(ConceptDef conc)
		{
			if (!PlayerKnowledgeDatabase.IsComplete(conc))
			{
				string helpTextAdjusted = conc.HelpTextAdjusted;
				Find.WindowStack.Add(new Dialog_MessageBox(helpTextAdjusted, (string)null, null, (string)null, null, (string)null, false));
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(conc, KnowledgeAmount.Total);
			}
		}

		public static bool EventCellsMatchExactly(EventPack ep, List<IntVec3> targetCells)
		{
			bool result;
			if (ep.Cell.IsValid)
			{
				result = (targetCells.Count == 1 && ep.Cell == targetCells[0]);
			}
			else if (ep.Cells == null)
			{
				result = false;
			}
			else
			{
				int num = 0;
				foreach (IntVec3 cell in ep.Cells)
				{
					if (!targetCells.Contains(cell))
					{
						return false;
					}
					num++;
				}
				result = (num == targetCells.Count);
			}
			return result;
		}

		public static bool EventCellsAreWithin(EventPack ep, List<IntVec3> targetCells)
		{
			return (!ep.Cell.IsValid) ? (ep.Cells != null && !ep.Cells.Any((Func<IntVec3, bool>)((IntVec3 c) => !targetCells.Contains(c)))) : targetCells.Contains(ep.Cell);
		}
	}
}
