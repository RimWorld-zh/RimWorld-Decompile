using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;

namespace RimWorld
{
	public class Building_BlastingCharge : Building
	{
		[DebuggerHidden]
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Building_BlastingCharge.<GetGizmos>c__Iterator144 <GetGizmos>c__Iterator = new Building_BlastingCharge.<GetGizmos>c__Iterator144();
			<GetGizmos>c__Iterator.<>f__this = this;
			Building_BlastingCharge.<GetGizmos>c__Iterator144 expr_0E = <GetGizmos>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		private void Command_Detonate()
		{
			base.GetComp<CompExplosive>().StartWick(null);
		}
	}
}
