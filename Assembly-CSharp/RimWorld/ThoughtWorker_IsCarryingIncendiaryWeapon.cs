using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000227 RID: 551
	public class ThoughtWorker_IsCarryingIncendiaryWeapon : ThoughtWorker
	{
		// Token: 0x06000A1A RID: 2586 RVA: 0x000597E0 File Offset: 0x00057BE0
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
