using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200089B RID: 2203
	public class PawnColumnWorker_WorkPriority : PawnColumnWorker
	{
		// Token: 0x0600324C RID: 12876 RVA: 0x001B0C60 File Offset: 0x001AF060
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (!pawn.Dead && pawn.workSettings != null && pawn.workSettings.EverWork)
			{
				Text.Font = GameFont.Medium;
				float x = rect.x + (rect.width - 25f) / 2f;
				float y = rect.y + 2.5f;
				bool incapable = this.IsIncapableOfWholeWorkType(pawn, this.def.workType);
				WidgetsWork.DrawWorkBoxFor(x, y, pawn, this.def.workType, incapable);
				Rect rect2 = new Rect(x, y, 25f, 25f);
				TooltipHandler.TipRegion(rect2, () => WidgetsWork.TipForPawnWorker(pawn, this.def.workType, incapable), pawn.thingIDNumber ^ this.def.workType.GetHashCode());
				Text.Font = GameFont.Small;
			}
		}

		// Token: 0x0600324D RID: 12877 RVA: 0x001B0D70 File Offset: 0x001AF170
		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			Text.Font = GameFont.Small;
			if (this.cachedWorkLabelSize == default(Vector2))
			{
				this.cachedWorkLabelSize = Text.CalcSize(this.def.workType.labelShort);
			}
			Rect labelRect = this.GetLabelRect(rect);
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(labelRect, this.def.workType.labelShort);
			GUI.color = new Color(1f, 1f, 1f, 0.3f);
			Widgets.DrawLineVertical(labelRect.center.x, labelRect.yMax - 3f, rect.y + 50f - labelRect.yMax + 3f);
			Widgets.DrawLineVertical(labelRect.center.x + 1f, labelRect.yMax - 3f, rect.y + 50f - labelRect.yMax + 3f);
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x0600324E RID: 12878 RVA: 0x001B0E94 File Offset: 0x001AF294
		public override int GetMinHeaderHeight(PawnTable table)
		{
			return 50;
		}

		// Token: 0x0600324F RID: 12879 RVA: 0x001B0EAC File Offset: 0x001AF2AC
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 32);
		}

		// Token: 0x06003250 RID: 12880 RVA: 0x001B0ED0 File Offset: 0x001AF2D0
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(39, this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		// Token: 0x06003251 RID: 12881 RVA: 0x001B0EFC File Offset: 0x001AF2FC
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), 80);
		}

		// Token: 0x06003252 RID: 12882 RVA: 0x001B0F20 File Offset: 0x001AF320
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

		// Token: 0x06003253 RID: 12883 RVA: 0x001B0FC8 File Offset: 0x001AF3C8
		protected override Rect GetInteractableHeaderRect(Rect headerRect, PawnTable table)
		{
			return this.GetLabelRect(headerRect);
		}

		// Token: 0x06003254 RID: 12884 RVA: 0x001B0FE4 File Offset: 0x001AF3E4
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x06003255 RID: 12885 RVA: 0x001B1010 File Offset: 0x001AF410
		private float GetValueToCompare(Pawn pawn)
		{
			float result;
			if (pawn.workSettings == null || !pawn.workSettings.EverWork)
			{
				result = -2f;
			}
			else if (pawn.story != null && pawn.story.WorkTypeIsDisabled(this.def.workType))
			{
				result = -1f;
			}
			else
			{
				result = pawn.skills.AverageOfRelevantSkillsFor(this.def.workType);
			}
			return result;
		}

		// Token: 0x06003256 RID: 12886 RVA: 0x001B1094 File Offset: 0x001AF494
		private Rect GetLabelRect(Rect headerRect)
		{
			float x = headerRect.center.x;
			Rect result = new Rect(x - this.cachedWorkLabelSize.x / 2f, headerRect.y, this.cachedWorkLabelSize.x, this.cachedWorkLabelSize.y);
			if (this.def.moveWorkTypeLabelDown)
			{
				result.y += 20f;
			}
			return result;
		}

		// Token: 0x06003257 RID: 12887 RVA: 0x001B1114 File Offset: 0x001AF514
		protected override string GetHeaderTip(PawnTable table)
		{
			string text = string.Concat(new string[]
			{
				this.def.workType.gerundLabel.CapitalizeFirst(),
				"\n\n",
				this.def.workType.description,
				"\n\n",
				PawnColumnWorker_WorkPriority.SpecificWorkListString(this.def.workType)
			});
			text += "\n";
			if (this.def.sortable)
			{
				text = text + "\n" + "ClickToSortByThisColumn".Translate();
			}
			if (Find.PlaySettings.useWorkPriorities)
			{
				text = text + "\n" + "WorkPriorityShiftClickTip".Translate();
			}
			else
			{
				text = text + "\n" + "WorkPriorityShiftClickEnableDisableTip".Translate();
			}
			return text;
		}

		// Token: 0x06003258 RID: 12888 RVA: 0x001B11F8 File Offset: 0x001AF5F8
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

		// Token: 0x06003259 RID: 12889 RVA: 0x001B12A0 File Offset: 0x001AF6A0
		protected override void HeaderClicked(Rect headerRect, PawnTable table)
		{
			base.HeaderClicked(headerRect, table);
			if (Event.current.shift)
			{
				List<Pawn> pawnsListForReading = table.PawnsListForReading;
				for (int i = 0; i < pawnsListForReading.Count; i++)
				{
					Pawn pawn = pawnsListForReading[i];
					if (pawn.workSettings != null && pawn.workSettings.EverWork)
					{
						if (pawn.story == null || !pawn.story.WorkTypeIsDisabled(this.def.workType))
						{
							if (Find.PlaySettings.useWorkPriorities)
							{
								int priority = pawn.workSettings.GetPriority(this.def.workType);
								if (Event.current.button == 0 && priority != 1)
								{
									int num = priority - 1;
									if (num < 0)
									{
										num = 4;
									}
									pawn.workSettings.SetPriority(this.def.workType, num);
								}
								if (Event.current.button == 1 && priority != 0)
								{
									int num2 = priority + 1;
									if (num2 > 4)
									{
										num2 = 0;
									}
									pawn.workSettings.SetPriority(this.def.workType, num2);
								}
							}
							else if (pawn.workSettings.GetPriority(this.def.workType) > 0)
							{
								if (Event.current.button == 1)
								{
									pawn.workSettings.SetPriority(this.def.workType, 0);
								}
							}
							else if (Event.current.button == 0)
							{
								pawn.workSettings.SetPriority(this.def.workType, 3);
							}
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
					SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
				}
				else if (Event.current.button == 1)
				{
					SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
				}
			}
		}

		// Token: 0x04001AE5 RID: 6885
		private const int LabelRowHeight = 50;

		// Token: 0x04001AE6 RID: 6886
		private Vector2 cachedWorkLabelSize;
	}
}
