using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020008C9 RID: 2249
	public class Instruction_SetGrowingZonePlant : Lesson_Instruction
	{
		// Token: 0x1700082F RID: 2095
		// (get) Token: 0x0600336F RID: 13167 RVA: 0x001B98E8 File Offset: 0x001B7CE8
		private Zone_Growing GrowZone
		{
			get
			{
				return (Zone_Growing)base.Map.zoneManager.AllZones.FirstOrDefault((Zone z) => z is Zone_Growing);
			}
		}

		// Token: 0x06003370 RID: 13168 RVA: 0x001B9934 File Offset: 0x001B7D34
		public override void LessonOnGUI()
		{
			TutorUtility.DrawLabelOnGUI(Gen.AveragePosition(this.GrowZone.cells), this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x06003371 RID: 13169 RVA: 0x001B995D File Offset: 0x001B7D5D
		public override void LessonUpdate()
		{
			GenDraw.DrawArrowPointingAt(Gen.AveragePosition(this.GrowZone.cells), false);
		}
	}
}
