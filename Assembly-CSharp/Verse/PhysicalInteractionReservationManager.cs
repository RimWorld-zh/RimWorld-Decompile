using System;
using System.Collections.Generic;

namespace Verse
{
	public class PhysicalInteractionReservationManager : IExposable
	{
		public class PhysicalInteractionReservation : IExposable
		{
			public LocalTargetInfo target;

			public Pawn claimant;

			public void ExposeData()
			{
				Scribe_TargetInfo.Look(ref this.target, "target");
				Scribe_References.Look<Pawn>(ref this.claimant, "claimant", false);
			}
		}

		private List<PhysicalInteractionReservation> reservations = new List<PhysicalInteractionReservation>();

		public void Reserve(Pawn claimant, LocalTargetInfo target)
		{
			if (!this.IsReservedBy(claimant, target))
			{
				PhysicalInteractionReservation physicalInteractionReservation = new PhysicalInteractionReservation();
				physicalInteractionReservation.target = target;
				physicalInteractionReservation.claimant = claimant;
				this.reservations.Add(physicalInteractionReservation);
			}
		}

		public void Release(Pawn claimant, LocalTargetInfo target)
		{
			for (int i = 0; i < this.reservations.Count; i++)
			{
				PhysicalInteractionReservation physicalInteractionReservation = this.reservations[i];
				if (physicalInteractionReservation.target == target && physicalInteractionReservation.claimant == claimant)
				{
					this.reservations.RemoveAt(i);
					return;
				}
			}
			Log.Warning(claimant + " tried to release reservation on target " + target + ", but it's not reserved by him.");
		}

		public bool IsReservedBy(Pawn claimant, LocalTargetInfo target)
		{
			for (int i = 0; i < this.reservations.Count; i++)
			{
				PhysicalInteractionReservation physicalInteractionReservation = this.reservations[i];
				if (physicalInteractionReservation.target == target && physicalInteractionReservation.claimant == claimant)
				{
					return true;
				}
			}
			return false;
		}

		public bool IsReserved(LocalTargetInfo target)
		{
			return this.FirstReserverOf(target) != null;
		}

		public Pawn FirstReserverOf(LocalTargetInfo target)
		{
			for (int i = 0; i < this.reservations.Count; i++)
			{
				PhysicalInteractionReservation physicalInteractionReservation = this.reservations[i];
				if (physicalInteractionReservation.target == target)
				{
					return physicalInteractionReservation.claimant;
				}
			}
			return null;
		}

		public void ReleaseAllForTarget(LocalTargetInfo target)
		{
			this.reservations.RemoveAll((Predicate<PhysicalInteractionReservation>)((PhysicalInteractionReservation x) => x.target == target));
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
		}
	}
}
