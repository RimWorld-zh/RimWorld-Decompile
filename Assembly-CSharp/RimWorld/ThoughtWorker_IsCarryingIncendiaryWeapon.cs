using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000229 RID: 553
	public class ThoughtWorker_IsCarryingIncendiaryWeapon : ThoughtWorker
	{
		// Token: 0x06000A1E RID: 2590 RVA: 0x00059930 File Offset: 0x00057D30
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			ThoughtState result;
			if (p.equipment.Primary == null)
			{
				result = false;
			}
			else
			{
				List<Verb> allVerbs = p.equipment.Primary.GetComp<CompEquippable>().AllVerbs;
				for (int i = 0; i < allVerbs.Count; i++)
				{
					if (allVerbs[i].IsIncendiary())
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}
	}
}
