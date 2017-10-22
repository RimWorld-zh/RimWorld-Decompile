using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class CompUsable : ThingComp
	{
		public CompProperties_Usable Props
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
			_003CCompFloatMenuOptions_003Ec__Iterator0 _003CCompFloatMenuOptions_003Ec__Iterator = (_003CCompFloatMenuOptions_003Ec__Iterator0)/*Error near IL_0036: stateMachine*/;
			if (!myPawn.CanReserve((Thing)base.parent, 1, -1, null, false))
			{
				yield return new FloatMenuOption(this.FloatMenuOptionLabel + " (" + "Reserved".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
				/*Error: Unable to find new state assignment for yield return*/;
			}
			FloatMenuOption useopt = new FloatMenuOption(this.FloatMenuOptionLabel, (Action)delegate()
			{
				if (myPawn.CanReserveAndReach((Thing)_003CCompFloatMenuOptions_003Ec__Iterator._0024this.parent, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false))
				{
					foreach (CompUseEffect comp in _003CCompFloatMenuOptions_003Ec__Iterator._0024this.parent.GetComps<CompUseEffect>())
					{
						if (comp.SelectedUseOption(myPawn))
							return;
					}
					_003CCompFloatMenuOptions_003Ec__Iterator._0024this.TryStartUseJob(myPawn);
				}
			}, MenuOptionPriority.Default, null, null, 0f, null, null);
			yield return useopt;
			/*Error: Unable to find new state assignment for yield return*/;
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
