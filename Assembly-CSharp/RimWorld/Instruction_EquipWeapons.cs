using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020008C2 RID: 2242
	public class Instruction_EquipWeapons : Lesson_Instruction
	{
		// Token: 0x17000825 RID: 2085
		// (get) Token: 0x06003341 RID: 13121 RVA: 0x001B8950 File Offset: 0x001B6D50
		protected override float ProgressPercent
		{
			get
			{
				return (float)(from c in base.Map.mapPawns.FreeColonists
				where c.equipment.Primary != null
				select c).Count<Pawn>() / (float)base.Map.mapPawns.FreeColonistsCount;
			}
		}

		// Token: 0x17000826 RID: 2086
		// (get) Token: 0x06003342 RID: 13122 RVA: 0x001B89B0 File Offset: 0x001B6DB0
		private IEnumerable<Thing> Weapons
		{
			get
			{
				return from it in Find.TutorialState.startingItems
				where Instruction_EquipWeapons.IsWeapon(it) && it.Spawned
				select it;
			}
		}

		// Token: 0x06003343 RID: 13123 RVA: 0x001B89F4 File Offset: 0x001B6DF4
		public static bool IsWeapon(Thing t)
		{
			return t.def.IsWeapon && t.def.BaseMarketValue > 30f;
		}

		// Token: 0x06003344 RID: 13124 RVA: 0x001B8A30 File Offset: 0x001B6E30
		public override void LessonOnGUI()
		{
			foreach (Thing t in this.Weapons)
			{
				TutorUtility.DrawLabelOnThingOnGUI(t, this.def.onMapInstruction);
			}
			base.LessonOnGUI();
		}

		// Token: 0x06003345 RID: 13125 RVA: 0x001B8AA0 File Offset: 0x001B6EA0
		public override void LessonUpdate()
		{
			foreach (Thing thing in this.Weapons)
			{
				GenDraw.DrawArrowPointingAt(thing.DrawPos, true);
			}
			if (this.ProgressPercent > 0.9999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}
