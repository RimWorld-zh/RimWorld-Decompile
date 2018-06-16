using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020008D7 RID: 2263
	public static class TutorUtility
	{
		// Token: 0x060033C1 RID: 13249 RVA: 0x001BA23C File Offset: 0x001B863C
		public static bool BuildingOrBlueprintOrFrameCenterExists(IntVec3 c, Map map, ThingDef buildingDef)
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Thing thing = thingList[i];
				if (!(thing.Position != c))
				{
					bool result;
					if (thing.def == buildingDef)
					{
						result = true;
					}
					else
					{
						if (thing.def.entityDefToBuild != buildingDef)
						{
							goto IL_5B;
						}
						result = true;
					}
					return result;
				}
				IL_5B:;
			}
			return false;
		}

		// Token: 0x060033C2 RID: 13250 RVA: 0x001BA2BC File Offset: 0x001B86BC
		public static CellRect FindUsableRect(int width, int height, Map map, float minFertility = 0f, bool noItems = false)
		{
			IntVec3 center = map.Center;
			float num = 1f;
			CellRect cellRect;
			for (;;)
			{
				IntVec3 center2 = center + new IntVec3((int)Rand.Range(-num, num), 0, (int)Rand.Range(-num, num));
				cellRect = CellRect.CenteredOn(center2, width / 2);
				cellRect.Width = width;
				cellRect.Height = height;
				cellRect = cellRect.ExpandedBy(1);
				bool flag = true;
				CellRect.CellRectIterator iterator = cellRect.GetIterator();
				while (!iterator.Done())
				{
					IntVec3 intVec = iterator.Current;
					if (intVec.Fogged(map) || !intVec.Walkable(map) || !intVec.GetTerrain(map).affordances.Contains(TerrainAffordanceDefOf.Heavy) || intVec.GetTerrain(map).fertility < minFertility || intVec.GetZone(map) != null || TutorUtility.ContainsBlockingThing(intVec, map, noItems) || intVec.InNoBuildEdgeArea(map) || intVec.InNoZoneEdgeArea(map))
					{
						flag = false;
						break;
					}
					iterator.MoveNext();
				}
				if (flag)
				{
					break;
				}
				num += 0.25f;
			}
			return cellRect.ContractedBy(1);
		}

		// Token: 0x060033C3 RID: 13251 RVA: 0x001BA3F8 File Offset: 0x001B87F8
		private static bool ContainsBlockingThing(IntVec3 cell, Map map, bool noItems)
		{
			List<Thing> thingList = cell.GetThingList(map);
			int i = 0;
			while (i < thingList.Count)
			{
				bool result;
				if (thingList[i].def.category == ThingCategory.Building)
				{
					result = true;
				}
				else if (thingList[i] is Blueprint)
				{
					result = true;
				}
				else
				{
					if (!noItems || thingList[i].def.category != ThingCategory.Item)
					{
						i++;
						continue;
					}
					result = true;
				}
				return result;
			}
			return false;
		}

		// Token: 0x060033C4 RID: 13252 RVA: 0x001BA48C File Offset: 0x001B888C
		public static void DrawLabelOnThingOnGUI(Thing t, string label)
		{
			Vector2 vector = (t.DrawPos + new Vector3(0f, 0f, 0.5f)).MapToUIPosition();
			Vector2 vector2 = Text.CalcSize(label);
			Rect rect = new Rect(vector.x - vector2.x / 2f, vector.y - vector2.y / 2f, vector2.x, vector2.y);
			GUI.DrawTexture(rect, TexUI.GrayTextBG);
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, label);
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x060033C5 RID: 13253 RVA: 0x001BA52C File Offset: 0x001B892C
		public static void DrawLabelOnGUI(Vector3 mapPos, string label)
		{
			Vector2 vector = mapPos.MapToUIPosition();
			Vector2 vector2 = Text.CalcSize(label);
			Rect rect = new Rect(vector.x - vector2.x / 2f, vector.y - vector2.y / 2f, vector2.x, vector2.y);
			GUI.DrawTexture(rect, TexUI.GrayTextBG);
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, label);
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x060033C6 RID: 13254 RVA: 0x001BA5AC File Offset: 0x001B89AC
		public static void DrawCellRectOnGUI(CellRect cellRect, string label = null)
		{
			if (label != null)
			{
				Vector3 centerVector = cellRect.CenterVector3;
				TutorUtility.DrawLabelOnGUI(centerVector, label);
			}
		}

		// Token: 0x060033C7 RID: 13255 RVA: 0x001BA5D4 File Offset: 0x001B89D4
		public static void DrawCellRectUpdate(CellRect cellRect)
		{
			CellRect.CellRectIterator iterator = cellRect.GetIterator();
			while (!iterator.Done())
			{
				CellRenderer.RenderCell(iterator.Current, 0.5f);
				iterator.MoveNext();
			}
		}

		// Token: 0x060033C8 RID: 13256 RVA: 0x001BA618 File Offset: 0x001B8A18
		public static void DoModalDialogIfNotKnown(ConceptDef conc)
		{
			if (!PlayerKnowledgeDatabase.IsComplete(conc))
			{
				string helpTextAdjusted = conc.HelpTextAdjusted;
				Find.WindowStack.Add(new Dialog_MessageBox(helpTextAdjusted, null, null, null, null, null, false, null, null));
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(conc, KnowledgeAmount.Total);
			}
		}

		// Token: 0x060033C9 RID: 13257 RVA: 0x001BA65C File Offset: 0x001B8A5C
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
				foreach (IntVec3 item in ep.Cells)
				{
					if (!targetCells.Contains(item))
					{
						return false;
					}
					num++;
				}
				result = (num == targetCells.Count);
			}
			return result;
		}

		// Token: 0x060033CA RID: 13258 RVA: 0x001BA730 File Offset: 0x001B8B30
		public static bool EventCellsAreWithin(EventPack ep, List<IntVec3> targetCells)
		{
			bool result;
			if (ep.Cell.IsValid)
			{
				result = targetCells.Contains(ep.Cell);
			}
			else
			{
				result = (ep.Cells != null && !ep.Cells.Any((IntVec3 c) => !targetCells.Contains(c)));
			}
			return result;
		}
	}
}
