using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Kidnap : JobDriver_TakeAndExitMap
	{
		protected Pawn Takee
		{
			get
			{
				return (Pawn)base.Item;
			}
		}

		public override string GetReport()
		{
			if (base.pawn.HostileTo(this.Takee))
			{
				return base.GetReport();
			}
			return JobDefOf.Rescue.reportString.Replace("TargetA", this.Takee.LabelShort);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn((Func<bool>)(() => !((_003CMakeNewToils_003Ec__Iterator31)/*Error near IL_0029: stateMachine*/)._003C_003Ef__this.Takee.Downed && ((_003CMakeNewToils_003Ec__Iterator31)/*Error near IL_0029: stateMachine*/)._003C_003Ef__this.Takee.Awake()));
			foreach (Toil item in base.MakeNewToils())
			{
				yield return item;
			}
		}
	}
}
