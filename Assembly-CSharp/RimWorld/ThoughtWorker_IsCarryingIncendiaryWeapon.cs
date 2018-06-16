using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000227 RID: 551
	public class ThoughtWorker_IsCarryingIncendiaryWeapon : ThoughtWorker
	{
		// Token: 0x06000A1C RID: 2588 RVA: 0x0005979C File Offset: 0x00057B9C
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
