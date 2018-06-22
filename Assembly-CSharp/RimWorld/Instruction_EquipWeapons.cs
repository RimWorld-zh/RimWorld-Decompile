using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020008BE RID: 2238
	public class Instruction_EquipWeapons : Lesson_Instruction
	{
		// Token: 0x17000826 RID: 2086
		// (get) Token: 0x0600333A RID: 13114 RVA: 0x001B8B38 File Offset: 0x001B6F38
		protected override float ProgressPercent
		{
			get
			{
				return (float)(from c in base.Map.mapPawns.FreeColonists
				where c.equipment.Primary != null
				select c).Count<Pawn>() / (float)base.Map.mapPawns.FreeColonistsCount;
			}
		}

		// Token: 0x17000827 RID: 2087
		// (get) Token: 0x0600333B RID: 13115 RVA: 0x001B8B98 File Offset: 0x001B6F98
		private IEnumerable<Thing> Weapons
		{
			get
			{
				return from it in Find.TutorialState.startingItems
				where Instruction_EquipWeapons.IsWeapon(it) && it.Spawned
				select it;
			}
		}

		// Token: 0x0600333C RID: 13116 RVA: 0x001B8BDC File Offset: 0x001B6FDC
		public static bool IsWeapon(Thing t)
		{
			return t.def.IsWeapon && t.def.BaseMarketValue > 30f;
		}

		// Token: 0x0600333D RID: 13117 RVA: 0x001B8C18 File Offset: 0x001B7018
		public override void LessonOnGUI()
		{
			foreach (Thing t in this.Weapons)
			{
				TutorUtility.DrawLabelOnThingOnGUI(t, this.def.onMapInstruction);
			}
			base.LessonOnGUI();
		}

		// Token: 0x0600333E RID: 13118 RVA: 0x001B8C88 File Offset: 0x001B7088
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
