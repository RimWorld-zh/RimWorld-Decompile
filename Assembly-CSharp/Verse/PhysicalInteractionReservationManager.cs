using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse.AI;

namespace Verse
{
	public class PhysicalInteractionReservationManager : IExposable
	{
		private List<PhysicalInteractionReservationManager.PhysicalInteractionReservation> reservations = new List<PhysicalInteractionReservationManager.PhysicalInteractionReservation>();

		[CompilerGenerated]
		private static Predicate<PhysicalInteractionReservationManager.PhysicalInteractionReservation> <>f__am$cache0;

		public PhysicalInteractionReservationManager()
		{
		}

		public void Reserve(Pawn claimant, Job job, LocalTargetInfo target)
		{
			if (!this.IsReservedBy(claimant, target))
			{
				PhysicalInteractionReservationManager.PhysicalInteractionReservation physicalInteractionReservation = new PhysicalInteractionReservationManager.PhysicalInteractionReservation();
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
				PhysicalInteractionReservationManager.PhysicalInteractionReservation physicalInteractionReservation = this.reservations[i];
				if (physicalInteractionReservation.target == target && physicalInteractionReservation.claimant == claimant && physicalInteractionReservation.job == job)
				{
					this.reservations.RemoveAt(i);
					return;
				}
			}
			Log.Warning(string.Concat(new object[]
			{
				claimant,
				" tried to release reservation on target ",
				target,
				", but it's not reserved by him."
			}), false);
		}

		public bool IsReservedBy(Pawn claimant, LocalTargetInfo target)
		{
			for (int i = 0; i < this.reservations.Count; i++)
			{
				PhysicalInteractionReservationManager.PhysicalInteractionReservation physicalInteractionReservation = this.reservations[i];
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
				PhysicalInteractionReservationManager.PhysicalInteractionReservation physicalInteractionReservation = this.reservations[i];
				if (physicalInteractionReservation.target == target)
				{
					return physicalInteractionReservation.claimant;
				}
			}
			return null;
		}

		public LocalTargetInfo FirstReservationFor(Pawn claimant)
		{
			for (int i = this.reservations.Count - 1; i >= 0; i--)
			{
				if (this.reservations[i].claimant == claimant)
				{
					return this.reservations[i].target;
				}
			}
			return LocalTargetInfo.Invalid;
		}

		public void ReleaseAllForTarget(LocalTargetInfo target)
		{
			this.reservations.RemoveAll((PhysicalInteractionReservationManager.PhysicalInteractionReservation x) => x.target == target);
		}

		public void ReleaseClaimedBy(Pawn claimant, Job job)
		{
			for (int i = this.reservations.Count - 1; i >= 0; i--)
			{
				if (this.reservations[i].claimant == claimant && this.reservations[i].job == job)
				{
					this.reservations.RemoveAt(i);
				}
			}
		}

		public void ReleaseAllClaimedBy(Pawn claimant)
		{
			for (int i = this.reservations.Count - 1; i >= 0; i--)
			{
				if (this.reservations[i].claimant == claimant)
				{
					this.reservations.RemoveAt(i);
				}
			}
		}

		public void ExposeData()
		{
			Scribe_Collections.Look<PhysicalInteractionReservationManager.PhysicalInteractionReservation>(ref this.reservations, "reservations", LookMode.Deep, new object[0]);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.reservations.RemoveAll((PhysicalInteractionReservationManager.PhysicalInteractionReservation x) => x.claimant.DestroyedOrNull()) != 0)
				{
					Log.Warning("Some physical interaction reservations had null or destroyed claimant.", false);
				}
			}
		}

		[CompilerGenerated]
		private static bool <ExposeData>m__0(PhysicalInteractionReservationManager.PhysicalInteractionReservation x)
		{
			return x.claimant.DestroyedOrNull();
		}

		public class PhysicalInteractionReservation : IExposable
		{
			public LocalTargetInfo target;

			public Pawn claimant;

			public Job job;

			public PhysicalInteractionReservation()
			{
			}

			public void ExposeData()
			{
				Scribe_TargetInfo.Look(ref this.target, "target");
				Scribe_References.Look<Pawn>(ref this.claimant, "claimant", false);
				Scribe_References.Look<Job>(ref this.job, "job", false);
			}
		}

		[CompilerGenerated]
		private sealed class <ReleaseAllForTarget>c__AnonStorey0
		{
			internal LocalTargetInfo target;

			public <ReleaseAllForTarget>c__AnonStorey0()
			{
			}

			internal bool <>m__0(PhysicalInteractionReservationManager.PhysicalInteractionReservation x)
			{
				return x.target == this.target;
			}
		}
	}
}
