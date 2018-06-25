using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020008BC RID: 2236
	public class Instruction_BuildRoomWalls : Lesson_Instruction
	{
		// Token: 0x04001B94 RID: 7060
		private List<IntVec3> cachedEdgeCells = new List<IntVec3>();

		// Token: 0x17000822 RID: 2082
		// (get) Token: 0x06003320 RID: 13088 RVA: 0x001B853C File Offset: 0x001B693C
		// (set) Token: 0x06003321 RID: 13089 RVA: 0x001B855B File Offset: 0x001B695B
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
		// (get) Token: 0x06003322 RID: 13090 RVA: 0x001B856C File Offset: 0x001B696C
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

		// Token: 0x06003323 RID: 13091 RVA: 0x001B85FC File Offset: 0x001B69FC
		public override void OnActivated()
		{
			base.OnActivated();
			this.RoomRect = TutorUtility.FindUsableRect(12, 8, base.Map, 0f, false);
		}

		// Token: 0x06003324 RID: 13092 RVA: 0x001B861F File Offset: 0x001B6A1F
		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.RoomRect, this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x06003325 RID: 13093 RVA: 0x001B8640 File Offset: 0x001B6A40
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

		// Token: 0x06003326 RID: 13094 RVA: 0x001B86D8 File Offset: 0x001B6AD8
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
