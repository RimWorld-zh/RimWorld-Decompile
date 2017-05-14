using System;
using System.Collections.Generic;
using System.Diagnostics;
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
				return (CompProperties_Usable)this.props;
			}
		}

		protected virtual string FloatMenuOptionLabel
		{
			get
			{
				return this.Props.useLabel;
			}
		}

		[DebuggerHidden]
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn myPawn)
		{
			CompUsable.<CompFloatMenuOptions>c__Iterator169 <CompFloatMenuOptions>c__Iterator = new CompUsable.<CompFloatMenuOptions>c__Iterator169();
			<CompFloatMenuOptions>c__Iterator.myPawn = myPawn;
			<CompFloatMenuOptions>c__Iterator.<$>myPawn = myPawn;
			<CompFloatMenuOptions>c__Iterator.<>f__this = this;
			CompUsable.<CompFloatMenuOptions>c__Iterator169 expr_1C = <CompFloatMenuOptions>c__Iterator;
			expr_1C.$PC = -2;
			return expr_1C;
		}

		public void TryStartUseJob(Pawn user)
		{
			if (!user.CanReserveAndReach(this.parent, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false))
			{
				return;
			}
			Job job = new Job(this.Props.useJob, this.parent);
			user.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		}

		public void UsedBy(Pawn p)
		{
			foreach (CompUseEffect current in from x in this.parent.GetComps<CompUseEffect>()
			orderby x.OrderPriority descending
			select x)
			{
				try
				{
					current.DoEffect(p);
				}
				catch (Exception arg)
				{
					Log.Error("Error in CompUseEffect: " + arg);
				}
			}
		}
	}
}
