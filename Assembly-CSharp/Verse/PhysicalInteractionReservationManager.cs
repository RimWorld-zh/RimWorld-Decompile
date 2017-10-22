using System;
using System.Collections.Generic;
using Verse.AI;

namespace Verse
{
	public class PhysicalInteractionReservationManager : IExposable
	{
		public class PhysicalInteractionReservation : IExposable
		{
			public LocalTargetInfo target;

			public Pawn claimant;

			public Job job;

			public void ExposeData()
			{
				Scribe_TargetInfo.Look(ref this.target, "target");
				Scribe_References.Look<Pawn>(ref this.claimant, "claimant", false);
				Scribe_References.Look<Job>(ref this.job, "job", false);
			}
		}

		private List<PhysicalInteractionReservation> reservations = new List<PhysicalInteractionReservation>();

		public void Reserve(Pawn claimant, Job job, LocalTargetInfo target)
		{
			if (!this.IsReservedBy(claimant, target))
			{
				PhysicalInteractionReservation physicalInteractionReservation = new PhysicalInteractionReservation();
				physicalInteractionReservation.target = target;
				physicalInteractionReservation.claimant = claimant;
				physicalInteractionReservation.job = job;
				this.reservations.Add(physicalInteractionReservation);
			}
		}

		public void Release(Pawn claimant, Job job, LocalTargetInfo target)
		{
			for (int i = 0; i < this.reservations.Count; i++)
			{
				PhysicalInteractionReservation physicalInteractionReservation = this.reservations[i];
				if (physicalInteractionReservation.target == target && physicalInteractionReservation.claimant == claimant && physicalInteractionReservation.job == job)
				{
					this.reservations.RemoveAt(i);
					return;
				}
			}
			Log.Warning(claimant + " tried to release reservation on target " + target + ", but it's not reserved by him.");
		}

		public bool IsReservedBy(Pawn claimant, LocalTargetInfo target)
		{
			int num = 0;
			bool result;
			while (true)
			{
				if (num < this.reservations.Count)
				{
					PhysicalInteractionReservation physicalInteractionReservation = this.reservations[num];
					if (physicalInteractionReservation.target == target && physicalInteractionReservation.claimant == claimant)
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

		public bool IsReserved(LocalTargetInfo target)
		{
			return this.FirstReserverOf(target) != null;
		}

		public Pawn FirstReserverOf(LocalTargetInfo target)
		{
			int num = 0;
			Pawn result;
			while (true)
			{
				if (num < this.reservations.Count)
				{
					PhysicalInteractionReservation physicalInteractionReservation = this.reservations[num];
					if (physicalInteractionReservation.target == target)
					{
						result = physicalInteractionReservation.claimant;
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}

		public LocalTargetInfo FirstReservationFor(Pawn claimant)
		{
			int num = this.reservations.Count - 1;
			LocalTargetInfo result;
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
				result = LocalTargetInfo.Invalid;
				break;
			}
			return result;
		}

		public void ReleaseAllForTarget(LocalTargetInfo target)
		{
			this.reservations.RemoveAll((Predicate<PhysicalInteractionReservation>)((PhysicalInteractionReservation x) => x.target == target));
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

		public void ExposeData()
		{
			Scribe_Collections.Look<PhysicalInteractionReservation>(ref this.reservations, "reservations", LookMode.Deep, new object[0]);
			if (((Scribe.mode == LoadSaveMode.PostLoadInit) ? this.reservations.RemoveAll((Predicate<PhysicalInteractionReservation>)((PhysicalInteractionReservation x) => x.claimant.DestroyedOrNull())) : 0) != 0)
			{
				Log.Warning("Some physical interaction reservations had null or destroyed claimant.");
			}
		}
	}
}
