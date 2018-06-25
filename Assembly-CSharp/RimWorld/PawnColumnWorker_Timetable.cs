using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000897 RID: 2199
	public class PawnColumnWorker_Timetable : PawnColumnWorker
	{
		// Token: 0x06003236 RID: 12854 RVA: 0x001B0C34 File Offset: 0x001AF034
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.timetable != null)
			{
				float num = rect.x;
				float num2 = rect.width / 24f;
				for (int i = 0; i < 24; i++)
				{
					Rect rect2 = new Rect(num, rect.y, num2, rect.height);
					this.DoTimeAssignment(rect2, pawn, i);
					num += num2;
				}
				GUI.color = Color.white;
			}
		}

		// Token: 0x06003237 RID: 12855 RVA: 0x001B0CAC File Offset: 0x001AF0AC
		public override void DoHeader(Rect rect, PawnTable table)
		{
			float num = rect.x;
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.LowerCenter;
			float num2 = rect.width / 24f;
			for (int i = 0; i < 24; i++)
			{
				Rect rect2 = new Rect(num, rect.y, num2, rect.height + 3f);
				Widgets.Label(rect2, i.ToString());
				num += num2;
			}
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06003238 RID: 12856 RVA: 0x001B0D34 File Offset: 0x001AF134
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 360);
		}

		// Token: 0x06003239 RID: 12857 RVA: 0x001B0D5C File Offset: 0x001AF15C
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(504, this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		// Token: 0x0600323A RID: 12858 RVA: 0x001B0D8C File Offset: 0x001AF18C
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), 600);
		}

		// Token: 0x0600323B RID: 12859 RVA: 0x001B0DB4 File Offset: 0x001AF1B4
		public override int GetMinHeaderHeight(PawnTable table)
		{
			return Mathf.Max(base.GetMinHeaderHeight(table), 15);
		}

		// Token: 0x0600323C RID: 12860 RVA: 0x001B0DD8 File Offset: 0x001AF1D8
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x0600323D RID: 12861 RVA: 0x001B0E04 File Offset: 0x001AF204
		private int GetValueToCompare(Pawn pawn)
		{
			int result;
			if (pawn.timetable == null)
			{
				result = int.MinValue;
			}
			else
			{
				result = pawn.timetable.times.FirstIndexOf((TimeAssignmentDef x) => x == TimeAssignmentDefOf.Work);
			}
			return result;
		}

		// Token: 0x0600323E RID: 12862 RVA: 0x001B0E5C File Offset: 0x001AF25C
		private void DoTimeAssignment(Rect rect, Pawn p, int hour)
		{
			rect = rect.ContractedBy(1f);
			TimeAssignmentDef assignment = p.timetable.GetAssignment(hour);
			GUI.DrawTexture(rect, assignment.ColorTexture);
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawBox(rect, 2);
				if (assignment != TimeAssignmentSelector.selectedAssignment && TimeAssignmentSelector.selectedAssignment != null && Input.GetMouseButton(0))
				{
					SoundDefOf.Designate_DragStandard_Changed.PlayOneShotOnCamera(null);
					p.timetable.SetAssignment(hour, TimeAssignmentSelector.selectedAssignment);
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.TimeAssignments, KnowledgeAmount.SmallInteraction);
				}
			}
		}
	}
}
