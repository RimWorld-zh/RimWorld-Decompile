using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000745 RID: 1861
	public class CompUsable : ThingComp
	{
		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x0600293C RID: 10556 RVA: 0x00157C70 File Offset: 0x00156070
		public CompProperties_Usable Props
		{
			get
			{
				return (CompProperties_Usable)this.props;
			}
		}

		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x0600293D RID: 10557 RVA: 0x00157C90 File Offset: 0x00156090
		protected virtual string FloatMenuOptionLabel
		{
			get
			{
				return this.Props.useLabel;
			}
		}

		// Token: 0x0600293E RID: 10558 RVA: 0x00157CB0 File Offset: 0x001560B0
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn myPawn)
		{
			string failReason;
			if (!this.CanBeUsedBy(myPawn, out failReason))
			{
				yield return new FloatMenuOption(this.FloatMenuOptionLabel + ((failReason == null) ? "" : (" (" + failReason + ")")), null, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			else if (!myPawn.CanReach(this.parent, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
			{
				yield return new FloatMenuOption(this.FloatMenuOptionLabel + " (" + "NoPath".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			else if (!myPawn.CanReserve(this.parent, 1, -1, null, false))
			{
				yield return new FloatMenuOption(this.FloatMenuOptionLabel + " (" + "Reserved".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			else if (!myPawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				yield return new FloatMenuOption(this.FloatMenuOptionLabel + " (" + "Incapable".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			else
			{
				FloatMenuOption useopt = new FloatMenuOption(this.FloatMenuOptionLabel, delegate()
				{
					if (myPawn.CanReserveAndReach(this.parent, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false))
					{
						foreach (CompUseEffect compUseEffect in this.parent.GetComps<CompUseEffect>())
						{
							if (compUseEffect.SelectedUseOption(myPawn))
							{
								return;
							}
						}
						this.TryStartUseJob(myPawn);
					}
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
				yield return useopt;
			}
			yield break;
		}

		// Token: 0x0600293F RID: 10559 RVA: 0x00157CE4 File Offset: 0x001560E4
		public void TryStartUseJob(Pawn user)
		{
			if (user.CanReserveAndReach(this.parent, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false))
			{
				string text;
				if (this.CanBeUsedBy(user, out text))
				{
					Job job = new Job(this.Props.useJob, this.parent);
					user.jobs.TryTakeOrderedJob(job, JobTag.Misc);
				}
			}
		}

		// Token: 0x06002940 RID: 10560 RVA: 0x00157D50 File Offset: 0x00156150
		public void UsedBy(Pawn p)
		{
			string text;
			if (this.CanBeUsedBy(p, out text))
			{
				foreach (CompUseEffect compUseEffect in from x in this.parent.GetComps<CompUseEffect>()
				orderby x.OrderPriority descending
				select x)
				{
					try
					{
						compUseEffect.DoEffect(p);
					}
					catch (Exception arg)
					{
						Log.Error("Error in CompUseEffect: " + arg, false);
					}
				}
			}
		}

		// Token: 0x06002941 RID: 10561 RVA: 0x00157E14 File Offset: 0x00156214
		private bool CanBeUsedBy(Pawn p, out string failReason)
		{
			List<ThingComp> allComps = this.parent.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				CompUseEffect compUseEffect = allComps[i] as CompUseEffect;
				if (compUseEffect != null && !compUseEffect.CanBeUsedBy(p, out failReason))
				{
					return false;
				}
			}
			failReason = null;
			return true;
		}
	}
}
