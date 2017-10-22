using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	public class PawnColumnWorker_WorkPriority : PawnColumnWorker
	{
		private const int LabelRowHeight = 50;

		private Vector2 cachedWorkLabelSize;

		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (!pawn.Dead && pawn.workSettings != null && pawn.workSettings.EverWork)
			{
				Text.Font = GameFont.Medium;
				float x = (float)(rect.x + (rect.width - 25.0) / 2.0);
				float y = (float)(rect.y + 2.5);
				bool incapable = this.IsIncapableOfWholeWorkType(pawn, base.def.workType);
				WidgetsWork.DrawWorkBoxFor(x, y, pawn, base.def.workType, incapable);
				Rect rect2 = new Rect(x, y, 25f, 25f);
				TooltipHandler.TipRegion(rect2, (Func<string>)(() => WidgetsWork.TipForPawnWorker(pawn, base.def.workType, incapable)), pawn.thingIDNumber ^ base.def.workType.GetHashCode());
				Text.Font = GameFont.Small;
			}
		}

		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			Text.Font = GameFont.Small;
			if (this.cachedWorkLabelSize == default(Vector2))
			{
				this.cachedWorkLabelSize = Text.CalcSize(base.def.workType.labelShort);
			}
			Rect labelRect = this.GetLabelRect(rect);
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(labelRect, base.def.workType.labelShort);
			GUI.color = new Color(1f, 1f, 1f, 0.3f);
			Vector2 center = labelRect.center;
			Widgets.DrawLineVertical(center.x, (float)(labelRect.yMax - 3.0), (float)(rect.y + 50.0 - labelRect.yMax + 3.0));
			Vector2 center2 = labelRect.center;
			Widgets.DrawLineVertical((float)(center2.x + 1.0), (float)(labelRect.yMax - 3.0), (float)(rect.y + 50.0 - labelRect.yMax + 3.0));
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		public override int GetMinHeaderHeight(PawnTable table)
		{
			return 50;
		}

		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 32);
		}

		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(39, this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), 80);
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

		protected override Rect GetInteractableHeaderRect(Rect headerRect, PawnTable table)
		{
			return this.GetLabelRect(headerRect);
		}

		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		private float GetValueToCompare(Pawn pawn)
		{
			if (pawn.workSettings != null && pawn.workSettings.EverWork)
			{
				if (pawn.story != null && pawn.story.WorkTypeIsDisabled(base.def.workType))
				{
					return -1f;
				}
				return pawn.skills.AverageOfRelevantSkillsFor(base.def.workType);
			}
			return -2f;
		}

		private Rect GetLabelRect(Rect headerRect)
		{
			Vector2 center = headerRect.center;
			float x = center.x;
			Rect result = new Rect((float)(x - this.cachedWorkLabelSize.x / 2.0), headerRect.y, this.cachedWorkLabelSize.x, this.cachedWorkLabelSize.y);
			if (base.def.moveWorkTypeLabelDown)
			{
				result.y += 20f;
			}
			return result;
		}

		protected override string GetHeaderTip(PawnTable table)
		{
			string str = base.def.workType.gerundLabel + "\n\n" + base.def.workType.description + "\n\n" + PawnColumnWorker_WorkPriority.SpecificWorkListString(base.def.workType);
			str += "\n";
			if (base.def.sortable)
			{
				str = str + "\n" + "ClickToSortByThisColumn".Translate();
			}
			return (!Find.PlaySettings.useWorkPriorities) ? (str + "\n" + "WorkPriorityShiftClickEnableDisableTip".Translate()) : (str + "\n" + "WorkPriorityShiftClickTip".Translate());
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

		protected override void HeaderClicked(Rect headerRect, PawnTable table)
		{
			base.HeaderClicked(headerRect, table);
			if (Event.current.shift)
			{
				List<Pawn> pawnsListForReading = table.PawnsListForReading;
				for (int i = 0; i < pawnsListForReading.Count; i++)
				{
					Pawn pawn = pawnsListForReading[i];
					if (pawn.workSettings != null && pawn.workSettings.EverWork && (pawn.story == null || !pawn.story.WorkTypeIsDisabled(base.def.workType)))
					{
						if (Find.PlaySettings.useWorkPriorities)
						{
							int priority = pawn.workSettings.GetPriority(base.def.workType);
							if (Event.current.button == 0 && priority != 1)
							{
								int num = priority - 1;
								if (num < 0)
								{
									num = 4;
								}
								pawn.workSettings.SetPriority(base.def.workType, num);
							}
							if (((Event.current.button == 1) ? priority : 0) != 0)
							{
								int num2 = priority + 1;
								if (num2 > 4)
								{
									num2 = 0;
								}
								pawn.workSettings.SetPriority(base.def.workType, num2);
							}
						}
						else if (pawn.workSettings.GetPriority(base.def.workType) > 0)
						{
							if (Event.current.button == 1)
							{
								pawn.workSettings.SetPriority(base.def.workType, 0);
							}
						}
						else if (Event.current.button == 0)
						{
							pawn.workSettings.SetPriority(base.def.workType, 3);
						}
					}
				}
				if (Find.PlaySettings.useWorkPriorities)
				{
					if (Event.current.button == 0)
					{
						SoundDefOf.AmountIncrement.PlayOneShotOnCamera(null);
					}
					else if (Event.current.button == 1)
					{
						SoundDefOf.AmountDecrement.PlayOneShotOnCamera(null);
					}
				}
				else if (Event.current.button == 0)
				{
					SoundDefOf.CheckboxTurnedOn.PlayOneShotOnCamera(null);
				}
				else if (Event.current.button == 1)
				{
					SoundDefOf.CheckboxTurnedOff.PlayOneShotOnCamera(null);
				}
			}
		}
	}
}
