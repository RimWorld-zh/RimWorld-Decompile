using RimWorld;
using System;
using System.Collections.Generic;

namespace Verse.AI
{
	public class AttackTargetReservationManager : IExposable
	{
		public class AttackTargetReservation : IExposable
		{
			public IAttackTarget target;

			public Pawn claimant;

			public Job job;

			public void ExposeData()
			{
				Scribe_References.Look<IAttackTarget>(ref this.target, "target", false);
				Scribe_References.Look<Pawn>(ref this.claimant, "claimant", false);
				Scribe_References.Look<Job>(ref this.job, "job", false);
			}
		}

		private Map map;

		private List<AttackTargetReservation> reservations = new List<AttackTargetReservation>();

		public AttackTargetReservationManager(Map map)
		{
			this.map = map;
		}

		public void Reserve(Pawn claimant, Job job, IAttackTarget target)
		{
			if (target == null)
			{
				Log.Warning(claimant + " tried to reserve null attack target.");
			}
			else if (!this.IsReservedBy(claimant, target))
			{
				AttackTargetReservation attackTargetReservation = new AttackTargetReservation();
				attackTargetReservation.target = target;
				attackTargetReservation.claimant = claimant;
				attackTargetReservation.job = job;
				this.reservations.Add(attackTargetReservation);
			}
		}

		public void Release(Pawn claimant, Job job, IAttackTarget target)
		{
			if (target == null)
			{
				Log.Warning(claimant + " tried to release reservation on null attack target.");
			}
			else
			{
				for (int i = 0; i < this.reservations.Count; i++)
				{
					AttackTargetReservation attackTargetReservation = this.reservations[i];
					if (attackTargetReservation.target == target && attackTargetReservation.claimant == claimant && attackTargetReservation.job == job)
					{
						this.reservations.RemoveAt(i);
						return;
					}
				}
				Log.Warning(claimant + " with job " + job + " tried to release reservation on target " + target + ", but it's not reserved by him.");
			}
		}

		public bool CanReserve(Pawn claimant, IAttackTarget target)
		{
			bool result;
			if (this.IsReservedBy(claimant, target))
			{
				result = true;
			}
			else
			{
				int reservationsCount = this.GetReservationsCount(target, claimant.Faction);
				int maxPreferredReservationsCount = this.GetMaxPreferredReservationsCount(target);
				result = (reservationsCount < maxPreferredReservationsCount);
			}
			return result;
		}

		public bool IsReservedBy(Pawn claimant, IAttackTarget target)
		{
			int num = 0;
			bool result;
			while (true)
			{
				if (num < this.reservations.Count)
				{
					AttackTargetReservation attackTargetReservation = this.reservations[num];
					if (attackTargetReservation.target == target && attackTargetReservation.claimant == claimant)
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}

		public void ReleaseAllForTarget(IAttackTarget target)
		{
			this.reservations.RemoveAll((Predicate<AttackTargetReservation>)((AttackTargetReservation x) => x.target == target));
		}

		public void ReleaseClaimedBy(Pawn claimant, Job job)
		{
			for (int num = this.reservations.Count - 1; num >= 0; num--)
			{
				if (this.reservations[num].claimant == claimant && this.reservations[num].job == job)
				{
					this.reservations.RemoveAt(num);
				}
			}
		}

		public void ReleaseAllClaimedBy(Pawn claimant)
		{
			for (int num = this.reservations.Count - 1; num >= 0; num--)
			{
				if (this.reservations[num].claimant == claimant)
				{
					this.reservations.RemoveAt(num);
				}
			}
		}

		public IAttackTarget FirstReservationFor(Pawn claimant)
		{
			int num = this.reservations.Count - 1;
			IAttackTarget result;
			while (true)
			{
				if (num >= 0)
				{
					if (this.reservations[num].claimant == claimant)
					{
						result = this.reservations[num].target;
						break;
					}
					num--;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}

		public void ExposeData()
		{
			Scribe_Collections.Look<AttackTargetReservation>(ref this.reservations, "reservations", LookMode.Deep, new object[0]);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.reservations.RemoveAll((Predicate<AttackTargetReservation>)((AttackTargetReservation x) => x.target == null));
				if (this.reservations.RemoveAll((Predicate<AttackTargetReservation>)((AttackTargetReservation x) => x.claimant.DestroyedOrNull())) != 0)
				{
					Log.Warning("Some attack target reservations had null or destroyed claimant.");
				}
			}
		}

		private int GetReservationsCount(IAttackTarget target, Faction faction)
		{
			int num = 0;
			for (int i = 0; i < this.reservations.Count; i++)
			{
				AttackTargetReservation attackTargetReservation = this.reservations[i];
				if (attackTargetReservation.target == target && attackTargetReservation.claimant.Faction == faction)
				{
					num++;
				}
			}
			return num;
		}

		private int GetMaxPreferredReservationsCount(IAttackTarget target)
		{
			int num = 0;
			Thing thing = target.Thing;
			CellRect cellRect = thing.OccupiedRect();
			foreach (IntVec3 item in cellRect.ExpandedBy(1))
			{
				if (!cellRect.Contains(item) && item.InBounds(this.map) && item.Standable(this.map))
				{
					num++;
				}
			}
			return num;
		}
	}
}
