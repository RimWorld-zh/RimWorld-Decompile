using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class CompUsable : ThingComp
	{
		protected CompProperties_Usable Props
		{
			get
			{
				return (CompProperties_Usable)base.props;
			}
		}

		protected virtual string FloatMenuOptionLabel
		{
			get
			{
				return this.Props.useLabel;
			}
		}

		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn myPawn)
		{
			if (!myPawn.CanReserve((Thing)base.parent, 1, -1, null, false))
			{
				yield return new FloatMenuOption(this.FloatMenuOptionLabel + " (" + "Reserved".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			else
			{
				FloatMenuOption useopt = new FloatMenuOption(this.FloatMenuOptionLabel, (Action)delegate
				{
					if (((_003CCompFloatMenuOptions_003Ec__Iterator169)/*Error near IL_00a0: stateMachine*/).myPawn.CanReserveAndReach((Thing)((_003CCompFloatMenuOptions_003Ec__Iterator169)/*Error near IL_00a0: stateMachine*/)._003C_003Ef__this.parent, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false))
					{
						foreach (CompUseEffect comp in ((_003CCompFloatMenuOptions_003Ec__Iterator169)/*Error near IL_00a0: stateMachine*/)._003C_003Ef__this.parent.GetComps<CompUseEffect>())
						{
							if (comp.SelectedUseOption(((_003CCompFloatMenuOptions_003Ec__Iterator169)/*Error near IL_00a0: stateMachine*/).myPawn))
								return;
						}
						((_003CCompFloatMenuOptions_003Ec__Iterator169)/*Error near IL_00a0: stateMachine*/)._003C_003Ef__this.TryStartUseJob(((_003CCompFloatMenuOptions_003Ec__Iterator169)/*Error near IL_00a0: stateMachine*/).myPawn);
					}
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
				yield return useopt;
			}
		}

		public void TryStartUseJob(Pawn user)
		{
			if (user.CanReserveAndReach((Thing)base.parent, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false))
			{
				Job job = new Job(this.Props.useJob, (Thing)base.parent);
				user.jobs.TryTakeOrderedJob(job, JobTag.Misc);
			}
		}

		public void UsedBy(Pawn p)
		{
			foreach (CompUseEffect item in from x in base.parent.GetComps<CompUseEffect>()
			orderby x.OrderPriority descending
			select x)
			{
				try
				{
					item.DoEffect(p);
				}
				catch (Exception arg)
				{
					Log.Error("Error in CompUseEffect: " + arg);
				}
			}
		}
	}
}
