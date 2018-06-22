using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020008C7 RID: 2247
	public class Instruction_SetGrowingZonePlant : Lesson_Instruction
	{
		// Token: 0x1700082F RID: 2095
		// (get) Token: 0x0600336B RID: 13163 RVA: 0x001B94D4 File Offset: 0x001B78D4
		private Zone_Growing GrowZone
		{
			get
			{
				return (Zone_Growing)base.Map.zoneManager.AllZones.FirstOrDefault((Zone z) => z is Zone_Growing);
			}
		}

		// Token: 0x0600336C RID: 13164 RVA: 0x001B9520 File Offset: 0x001B7920
		public override void LessonOnGUI()
		{
			TutorUtility.DrawLabelOnGUI(Gen.AveragePosition(this.GrowZone.cells), this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x0600336D RID: 13165 RVA: 0x001B9549 File Offset: 0x001B7949
		public override void LessonUpdate()
		{
			GenDraw.DrawArrowPointingAt(Gen.AveragePosition(this.GrowZone.cells), false);
		}
	}
}
