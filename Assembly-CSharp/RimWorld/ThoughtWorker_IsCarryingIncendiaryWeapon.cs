using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class ThoughtWorker_IsCarryingIncendiaryWeapon : ThoughtWorker
	{
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
						goto IL_004c;
				}
				result = false;
			}
			goto IL_0075;
			IL_004c:
			result = true;
			goto IL_0075;
			IL_0075:
			return result;
		}
	}
}
