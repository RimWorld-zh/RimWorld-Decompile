using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020008CB RID: 2251
	public class Instruction_SetGrowingZonePlant : Lesson_Instruction
	{
		// Token: 0x1700082E RID: 2094
		// (get) Token: 0x06003372 RID: 13170 RVA: 0x001B92EC File Offset: 0x001B76EC
		private Zone_Growing GrowZone
		{
			get
			{
				return (Zone_Growing)base.Map.zoneManager.AllZones.FirstOrDefault((Zone z) => z is Zone_Growing);
			}
		}

		// Token: 0x06003373 RID: 13171 RVA: 0x001B9338 File Offset: 0x001B7738
		public override void LessonOnGUI()
		{
			TutorUtility.DrawLabelOnGUI(Gen.AveragePosition(this.GrowZone.cells), this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x06003374 RID: 13172 RVA: 0x001B9361 File Offset: 0x001B7761
		public override void LessonUpdate()
		{
			GenDraw.DrawArrowPointingAt(Gen.AveragePosition(this.GrowZone.cells), false);
		}
	}
}
