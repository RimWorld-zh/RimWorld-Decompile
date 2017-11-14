using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class Instruction_EquipWeapons : Lesson_Instruction
	{
		protected override float ProgressPercent
		{
			get
			{
				return (float)(from c in base.Map.mapPawns.FreeColonists
				where c.equipment.Primary != null
				select c).Count() / (float)base.Map.mapPawns.FreeColonistsCount;
			}
		}

		private IEnumerable<Thing> Weapons
		{
			get
			{
				return from it in Find.TutorialState.startingItems
				where Instruction_EquipWeapons.IsWeapon(it) && it.Spawned
				select it;
			}
		}

		public static bool IsWeapon(Thing t)
		{
			return t.def.IsWeapon && t.def.BaseMarketValue > 30.0;
		}

		public override void LessonOnGUI()
		{
			foreach (Thing weapon in this.Weapons)
			{
				TutorUtility.DrawLabelOnThingOnGUI(weapon, base.def.onMapInstruction);
			}
			base.LessonOnGUI();
		}

		public override void LessonUpdate()
		{
			foreach (Thing weapon in this.Weapons)
			{
				GenDraw.DrawArrowPointingAt(weapon.DrawPos, true);
			}
			if (this.ProgressPercent > 0.99989998340606689)
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}
