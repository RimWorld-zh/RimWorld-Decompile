using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public class Instruction_SetGrowingZonePlant : Lesson_Instruction
	{
		[CompilerGenerated]
		private static Func<Zone, bool> <>f__am$cache0;

		public Instruction_SetGrowingZonePlant()
		{
		}

		private Zone_Growing GrowZone
		{
			get
			{
				return (Zone_Growing)base.Map.zoneManager.AllZones.FirstOrDefault((Zone z) => z is Zone_Growing);
			}
		}

		public override void LessonOnGUI()
		{
			TutorUtility.DrawLabelOnGUI(Gen.AveragePosition(this.GrowZone.cells), this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		public override void LessonUpdate()
		{
			GenDraw.DrawArrowPointingAt(Gen.AveragePosition(this.GrowZone.cells), false);
		}

		[CompilerGenerated]
		private static bool <get_GrowZone>m__0(Zone z)
		{
			return z is Zone_Growing;
		}
	}
}
