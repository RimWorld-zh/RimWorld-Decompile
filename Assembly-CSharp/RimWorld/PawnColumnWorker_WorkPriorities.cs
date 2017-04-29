using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class PawnColumnWorker_WorkPriorities : PawnColumnWorker
	{
		private const int SpaceBetweenPriorityArrowsAndWorkLabels = 40;

		private const int LabelRowHeight = 50;

		private static List<WorkTypeDef> visibleWorkTypesInPriorityOrder;

		private static DefMap<WorkTypeDef, Vector2> cachedLabelSizes = new DefMap<WorkTypeDef, Vector2>();

		private static bool labelSizesDirty;

		public static void Reset()
		{
			PawnColumnWorker_WorkPriorities.visibleWorkTypesInPriorityOrder = (from def in WorkTypeDefsUtility.WorkTypeDefsInPriorityOrder
			where def.visible
			select def).ToList<WorkTypeDef>();
			PawnColumnWorker_WorkPriorities.labelSizesDirty = true;
		}

		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.Dead || pawn.workSettings == null || !pawn.workSettings.EverWork)
			{
				return;
			}
			Text.Font = GameFont.Medium;
			float num = rect.x;
			float y = rect.y + 2.5f;
			float num2 = rect.width / (float)PawnColumnWorker_WorkPriorities.visibleWorkTypesInPriorityOrder.Count;
			for (int i = 0; i < PawnColumnWorker_WorkPriorities.visibleWorkTypesInPriorityOrder.Count; i++)
			{
				WorkTypeDef def = PawnColumnWorker_WorkPriorities.visibleWorkTypesInPriorityOrder[i];
				bool incapable = this.IsIncapableOfWholeWorkType(pawn, PawnColumnWorker_WorkPriorities.visibleWorkTypesInPriorityOrder[i]);
				WidgetsWork.DrawWorkBoxFor(num, y, pawn, def, incapable);
				Rect rect2 = new Rect(num, y, 25f, 25f);
				TooltipHandler.TipRegion(rect2, () => WidgetsWork.TipForPawnWorker(pawn, def, incapable), pawn.thingIDNumber ^ def.GetHashCode());
				num += num2;
			}
			Text.Font = GameFont.Small;
		}

		public override void DoHeader(Rect rect, PawnTable table)
		{
			if (PawnColumnWorker_WorkPriorities.labelSizesDirty)
			{
				PawnColumnWorker_WorkPriorities.labelSizesDirty = false;
				Text.Font = GameFont.Small;
				List<WorkTypeDef> allDefsListForReading = DefDatabase<WorkTypeDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					PawnColumnWorker_WorkPriorities.cachedLabelSizes[allDefsListForReading[i]] = Text.CalcSize(allDefsListForReading[i].labelShort);
				}
			}
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			Text.Anchor = TextAnchor.UpperCenter;
			Text.Font = GameFont.Tiny;
			Rect rect2 = new Rect(rect.x + rect.width / 3f - 90f, rect.y + 5f, 160f, 30f);
			Widgets.Label(rect2, "<= " + "HigherPriority".Translate());
			Rect rect3 = new Rect(rect.x + rect.width * 2f / 3f - 90f, rect.y + 5f, 160f, 30f);
			Widgets.Label(rect3, "LowerPriority".Translate() + " =>");
			GUI.color = Color.white;
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
			rect.yMin += 40f;
			float num = rect.width / (float)PawnColumnWorker_WorkPriorities.visibleWorkTypesInPriorityOrder.Count;
			float num2 = rect.x;
			for (int j = 0; j < PawnColumnWorker_WorkPriorities.visibleWorkTypesInPriorityOrder.Count; j++)
			{
				WorkTypeDef def = PawnColumnWorker_WorkPriorities.visibleWorkTypesInPriorityOrder[j];
				Vector2 vector = PawnColumnWorker_WorkPriorities.cachedLabelSizes[def];
				float num3 = num2 + 12.5f;
				Rect rect4 = new Rect(num3 - vector.x / 2f, rect.y, vector.x, vector.y);
				if (j % 2 == 1)
				{
					rect4.y += 20f;
				}
				if (Mouse.IsOver(rect4))
				{
					Widgets.DrawHighlight(rect4);
				}
				Text.Anchor = TextAnchor.MiddleCenter;
				Widgets.Label(rect4, def.labelShort);
				TooltipHandler.TipRegion(rect4, new TipSignal(() => string.Concat(new string[]
				{
					def.gerundLabel,
					"\n\n",
					def.description,
					"\n\n",
					PawnColumnWorker_WorkPriorities.SpecificWorkListString(def)
				}), def.GetHashCode()));
				GUI.color = new Color(1f, 1f, 1f, 0.3f);
				Widgets.DrawLineVertical(num3, rect4.yMax - 3f, rect.y + 50f - rect4.yMax + 3f);
				Widgets.DrawLineVertical(num3 + 1f, rect4.yMax - 3f, rect.y + 50f - rect4.yMax + 3f);
				GUI.color = Color.white;
				num2 += num;
			}
			Text.Anchor = TextAnchor.UpperLeft;
		}

		public override int GetMinHeaderHeight(PawnTable table)
		{
			return 90;
		}

		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), Mathf.CeilToInt((float)PawnColumnWorker_WorkPriorities.visibleWorkTypesInPriorityOrder.Count * 32f));
		}

		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(Mathf.CeilToInt((float)PawnColumnWorker_WorkPriorities.visibleWorkTypesInPriorityOrder.Count * 39f), this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), Mathf.CeilToInt((float)PawnColumnWorker_WorkPriorities.visibleWorkTypesInPriorityOrder.Count * 47f));
		}

		private bool IsIncapableOfWholeWorkType(Pawn p, WorkTypeDef work)
		{
			for (int i = 0; i < work.workGiversByPriority.Count; i++)
			{
				bool flag = true;
				for (int j = 0; j < work.workGiversByPriority[i].requiredCapacities.Count; j++)
				{
					PawnCapacityDef capacity = work.workGiversByPriority[i].requiredCapacities[j];
					if (!p.health.capacities.CapableOf(capacity))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					return false;
				}
			}
			return true;
		}

		private static string SpecificWorkListString(WorkTypeDef def)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < def.workGiversByPriority.Count; i++)
			{
				stringBuilder.Append(def.workGiversByPriority[i].LabelCap);
				if (def.workGiversByPriority[i].emergency)
				{
					stringBuilder.Append(" (" + "EmergencyWorkMarker".Translate() + ")");
				}
				if (i < def.workGiversByPriority.Count - 1)
				{
					stringBuilder.AppendLine();
				}
			}
			return stringBuilder.ToString();
		}
	}
}
