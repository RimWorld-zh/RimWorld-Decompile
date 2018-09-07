using System;
using Verse;

namespace RimWorld
{
	public class ThoughtWorker_IsCarryingRangedWeapon : ThoughtWorker
	{
		public ThoughtWorker_IsCarryingRangedWeapon()
		{
		}

		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.equipment.Primary != null && p.equipment.Primary.def.IsRangedWeapon;
		}
	}
}
