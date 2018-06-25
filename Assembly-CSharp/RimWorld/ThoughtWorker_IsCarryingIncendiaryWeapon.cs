using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class ThoughtWorker_IsCarryingIncendiaryWeapon : ThoughtWorker
	{
		public ThoughtWorker_IsCarryingIncendiaryWeapon()
		{
		}

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
