using System;
using System.Collections.Generic;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000AA6 RID: 2726
	public class PhysicalInteractionReservationManager : IExposable
	{
		// Token: 0x0400267A RID: 9850
		private List<PhysicalInteractionReservationManager.PhysicalInteractionReservation> reservations = new List<PhysicalInteractionReservationManager.PhysicalInteractionReservation>();

		// Token: 0x06003CD2 RID: 15570 RVA: 0x00203014 File Offset: 0x00201414
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

		// Token: 0x06003CD3 RID: 15571 RVA: 0x0020305C File Offset: 0x0020145C
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

		// Token: 0x06003CD4 RID: 15572 RVA: 0x00203100 File Offset: 0x00201500
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

		// Token: 0x06003CD5 RID: 15573 RVA: 0x00203168 File Offset: 0x00201568
		public bool IsReserved(LocalTargetInfo target)
		{
			return this.FirstReserverOf(target) != null;
		}

		// Token: 0x06003CD6 RID: 15574 RVA: 0x0020318C File Offset: 0x0020158C
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

		// Token: 0x06003CD7 RID: 15575 RVA: 0x002031EC File Offset: 0x002015EC
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

		// Token: 0x06003CD8 RID: 15576 RVA: 0x00203254 File Offset: 0x00201654
		public void ReleaseAllForTarget(LocalTargetInfo target)
		{
			this.reservations.RemoveAll((PhysicalInteractionReservationManager.PhysicalInteractionReservation x) => x.target == target);
		}

		// Token: 0x06003CD9 RID: 15577 RVA: 0x00203288 File Offset: 0x00201688
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

		// Token: 0x06003CDA RID: 15578 RVA: 0x002032F0 File Offset: 0x002016F0
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

		// Token: 0x06003CDB RID: 15579 RVA: 0x00203344 File Offset: 0x00201744
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

		// Token: 0x02000AA7 RID: 2727
		public class PhysicalInteractionReservation : IExposable
		{
			// Token: 0x0400267C RID: 9852
			public LocalTargetInfo target;

			// Token: 0x0400267D RID: 9853
			public Pawn claimant;

			// Token: 0x0400267E RID: 9854
			public Job job;

			// Token: 0x06003CDE RID: 15582 RVA: 0x002033D7 File Offset: 0x002017D7
			public void ExposeData()
			{
				Scribe_TargetInfo.Look(ref this.target, "target");
				Scribe_References.Look<Pawn>(ref this.claimant, "claimant", false);
				Scribe_References.Look<Job>(ref this.job, "job", false);
			}
		}
	}
}
