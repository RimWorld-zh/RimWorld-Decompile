using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000899 RID: 2201
	public class PawnColumnWorker_Timetable : PawnColumnWorker
	{
		// Token: 0x0600323A RID: 12858 RVA: 0x001B06A4 File Offset: 0x001AEAA4
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

		// Token: 0x0600323B RID: 12859 RVA: 0x001B071C File Offset: 0x001AEB1C
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

		// Token: 0x0600323C RID: 12860 RVA: 0x001B07A4 File Offset: 0x001AEBA4
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 360);
		}

		// Token: 0x0600323D RID: 12861 RVA: 0x001B07CC File Offset: 0x001AEBCC
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(504, this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		// Token: 0x0600323E RID: 12862 RVA: 0x001B07FC File Offset: 0x001AEBFC
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), 600);
		}

		// Token: 0x0600323F RID: 12863 RVA: 0x001B0824 File Offset: 0x001AEC24
		public override int GetMinHeaderHeight(PawnTable table)
		{
			return Mathf.Max(base.GetMinHeaderHeight(table), 15);
		}

		// Token: 0x06003240 RID: 12864 RVA: 0x001B0848 File Offset: 0x001AEC48
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x06003241 RID: 12865 RVA: 0x001B0874 File Offset: 0x001AEC74
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

		// Token: 0x06003242 RID: 12866 RVA: 0x001B08CC File Offset: 0x001AECCC
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
