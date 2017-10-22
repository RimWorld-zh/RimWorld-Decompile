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
			return (this.Takee != null && !base.pawn.HostileTo(this.Takee)) ? JobDefOf.Rescue.reportString.Replace("TargetA", this.Takee.LabelShort) : base.GetReport();
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn((Func<bool>)(() => ((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_002a: stateMachine*/)._0024this.Takee == null || (!((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_002a: stateMachine*/)._0024this.Takee.Downed && ((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_002a: stateMachine*/)._0024this.Takee.Awake())));
			using (IEnumerator<Toil> enumerator = this._003CMakeNewToils_003E__BaseCallProxy0().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Toil t = enumerator.Current;
					yield return t;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			yield break;
			IL_00d5:
			/*Error near IL_00d6: Unexpected return in MoveNext()*/;
		}
	}
}
