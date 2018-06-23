using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020008BA RID: 2234
	public class Instruction_BuildRoomWalls : Lesson_Instruction
	{
		// Token: 0x04001B8E RID: 7054
		private List<IntVec3> cachedEdgeCells = new List<IntVec3>();

		// Token: 0x17000822 RID: 2082
		// (get) Token: 0x0600331C RID: 13084 RVA: 0x001B8128 File Offset: 0x001B6528
		// (set) Token: 0x0600331D RID: 13085 RVA: 0x001B8147 File Offset: 0x001B6547
		private CellRect RoomRect
		{
			get
			{
				return Find.TutorialState.roomRect;
			}
			set
			{
				Find.TutorialState.roomRect = value;
			}
		}

		// Token: 0x17000823 RID: 2083
		// (get) Token: 0x0600331E RID: 13086 RVA: 0x001B8158 File Offset: 0x001B6558
		protected override float ProgressPercent
		{
			get
			{
				int num = 0;
				int num2 = 0;
				foreach (IntVec3 c in this.RoomRect.EdgeCells)
				{
					if (TutorUtility.BuildingOrBlueprintOrFrameCenterExists(c, base.Map, ThingDefOf.Wall))
					{
						num2++;
					}
					num++;
				}
				return (float)num2 / (float)num;
			}
		}

		// Token: 0x0600331F RID: 13087 RVA: 0x001B81E8 File Offset: 0x001B65E8
		public override void OnActivated()
		{
			base.OnActivated();
			this.RoomRect = TutorUtility.FindUsableRect(12, 8, base.Map, 0f, false);
		}

		// Token: 0x06003320 RID: 13088 RVA: 0x001B820B File Offset: 0x001B660B
		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.RoomRect, this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x06003321 RID: 13089 RVA: 0x001B822C File Offset: 0x001B662C
		public override void LessonUpdate()
		{
			this.cachedEdgeCells.Clear();
			this.cachedEdgeCells.AddRange((from c in this.RoomRect.EdgeCells
			where !TutorUtility.BuildingOrBlueprintOrFrameCenterExists(c, base.Map, ThingDefOf.Wall)
			select c).ToList<IntVec3>());
			GenDraw.DrawFieldEdges((from c in this.cachedEdgeCells
			where c.GetEdifice(base.Map) == null
			select c).ToList<IntVec3>());
			GenDraw.DrawArrowPointingAt(this.RoomRect.CenterVector3, false);
			if (this.ProgressPercent > 0.9999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}

		// Token: 0x06003322 RID: 13090 RVA: 0x001B82C4 File Offset: 0x001B66C4
		public override AcceptanceReport AllowAction(EventPack ep)
		{
			AcceptanceReport result;
			if (ep.Tag == "Designate-Wall")
			{
				result = TutorUtility.EventCellsAreWithin(ep, this.cachedEdgeCells);
			}
			else
			{
				result = base.AllowAction(ep);
			}
			return result;
		}
	}
}
