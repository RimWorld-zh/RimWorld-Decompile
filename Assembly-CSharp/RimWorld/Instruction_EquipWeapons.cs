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
		// (get) Token: 0x0600333F RID: 13119 RVA: 0x001B8888 File Offset: 0x001B6C88
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
		// (get) Token: 0x06003340 RID: 13120 RVA: 0x001B88E8 File Offset: 0x001B6CE8
		private IEnumerable<Thing> Weapons
		{
			get
			{
				return from it in Find.TutorialState.startingItems
				where Instruction_EquipWeapons.IsWeapon(it) && it.Spawned
				select it;
			}
		}

		// Token: 0x06003341 RID: 13121 RVA: 0x001B892C File Offset: 0x001B6D2C
		public static bool IsWeapon(Thing t)
		{
			return t.def.IsWeapon && t.def.BaseMarketValue > 30f;
		}

		// Token: 0x06003342 RID: 13122 RVA: 0x001B8968 File Offset: 0x001B6D68
		public override void LessonOnGUI()
		{
			foreach (Thing t in this.Weapons)
			{
				TutorUtility.DrawLabelOnThingOnGUI(t, this.def.onMapInstruction);
			}
			base.LessonOnGUI();
		}

		// Token: 0x06003343 RID: 13123 RVA: 0x001B89D8 File Offset: 0x001B6DD8
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
