using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020008BE RID: 2238
	public class Instruction_BuildRoomWalls : Lesson_Instruction
	{
		// Token: 0x17000821 RID: 2081
		// (get) Token: 0x06003321 RID: 13089 RVA: 0x001B7E78 File Offset: 0x001B6278
		// (set) Token: 0x06003322 RID: 13090 RVA: 0x001B7E97 File Offset: 0x001B6297
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

		// Token: 0x17000822 RID: 2082
		// (get) Token: 0x06003323 RID: 13091 RVA: 0x001B7EA8 File Offset: 0x001B62A8
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

		// Token: 0x06003324 RID: 13092 RVA: 0x001B7F38 File Offset: 0x001B6338
		public override void OnActivated()
		{
			base.OnActivated();
			this.RoomRect = TutorUtility.FindUsableRect(12, 8, base.Map, 0f, false);
		}

		// Token: 0x06003325 RID: 13093 RVA: 0x001B7F5B File Offset: 0x001B635B
		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.RoomRect, this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x06003326 RID: 13094 RVA: 0x001B7F7C File Offset: 0x001B637C
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

		// Token: 0x06003327 RID: 13095 RVA: 0x001B8014 File Offset: 0x001B6414
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

		// Token: 0x04001B90 RID: 7056
		private List<IntVec3> cachedEdgeCells = new List<IntVec3>();
	}
}
