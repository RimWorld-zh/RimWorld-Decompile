using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000AA5 RID: 2725
	public class AttackTargetReservationManager : IExposable
	{
		// Token: 0x0400267A RID: 9850
		private Map map;

		// Token: 0x0400267B RID: 9851
		private List<AttackTargetReservationManager.AttackTargetReservation> reservations = new List<AttackTargetReservationManager.AttackTargetReservation>();

		// Token: 0x06003CC1 RID: 15553 RVA: 0x00202D47 File Offset: 0x00201147
		public AttackTargetReservationManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x06003CC2 RID: 15554 RVA: 0x00202D64 File Offset: 0x00201164
		public void Reserve(Pawn claimant, Job job, IAttackTarget target)
		{
			if (target == null)
			{
				Log.Warning(claimant + " tried to reserve null attack target.", false);
			}
			else if (!this.IsReservedBy(claimant, target))
			{
				AttackTargetReservationManager.AttackTargetReservation attackTargetReservation = new AttackTargetReservationManager.AttackTargetReservation();
				attackTargetReservation.target = target;
				attackTargetReservation.claimant = claimant;
				attackTargetReservation.job = job;
				this.reservations.Add(attackTargetReservation);
			}
		}

		// Token: 0x06003CC3 RID: 15555 RVA: 0x00202DC8 File Offset: 0x002011C8
		public void Release(Pawn claimant, Job job, IAttackTarget target)
		{
			if (target == null)
			{
				Log.Warning(claimant + " tried to release reservation on null attack target.", false);
			}
			else
			{
				for (int i = 0; i < this.reservations.Count; i++)
				{
					AttackTargetReservationManager.AttackTargetReservation attackTargetReservation = this.reservations[i];
					if (attackTargetReservation.target == target && attackTargetReservation.claimant == claimant && attackTargetReservation.job == job)
					{
						this.reservations.RemoveAt(i);
						return;
					}
				}
				Log.Warning(string.Concat(new object[]
				{
					claimant,
					" with job ",
					job,
					" tried to release reservation on target ",
					target,
					", but it's not reserved by him."
				}), false);
			}
		}

		// Token: 0x06003CC4 RID: 15556 RVA: 0x00202E8C File Offset: 0x0020128C
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

		// Token: 0x06003CC5 RID: 15557 RVA: 0x00202ED0 File Offset: 0x002012D0
		public bool IsReservedBy(Pawn claimant, IAttackTarget target)
		{
			for (int i = 0; i < this.reservations.Count; i++)
			{
				AttackTargetReservationManager.AttackTargetReservation attackTargetReservation = this.reservations[i];
				if (attackTargetReservation.target == target && attackTargetReservation.claimant == claimant)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003CC6 RID: 15558 RVA: 0x00202F30 File Offset: 0x00201330
		public void ReleaseAllForTarget(IAttackTarget target)
		{
			this.reservations.RemoveAll((AttackTargetReservationManager.AttackTargetReservation x) => x.target == target);
		}

		// Token: 0x06003CC7 RID: 15559 RVA: 0x00202F64 File Offset: 0x00201364
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

		// Token: 0x06003CC8 RID: 15560 RVA: 0x00202FCC File Offset: 0x002013CC
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

		// Token: 0x06003CC9 RID: 15561 RVA: 0x00203020 File Offset: 0x00201420
		public IAttackTarget FirstReservationFor(Pawn claimant)
		{
			for (int i = this.reservations.Count - 1; i >= 0; i--)
			{
				if (this.reservations[i].claimant == claimant)
				{
					return this.reservations[i].target;
				}
			}
			return null;
		}

		// Token: 0x06003CCA RID: 15562 RVA: 0x00203084 File Offset: 0x00201484
		public void ExposeData()
		{
			Scribe_Collections.Look<AttackTargetReservationManager.AttackTargetReservation>(ref this.reservations, "reservations", LookMode.Deep, new object[0]);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.reservations.RemoveAll((AttackTargetReservationManager.AttackTargetReservation x) => x.target == null);
				if (this.reservations.RemoveAll((AttackTargetReservationManager.AttackTargetReservation x) => x.claimant.DestroyedOrNull()) != 0)
				{
					Log.Warning("Some attack target reservations had null or destroyed claimant.", false);
				}
			}
		}

		// Token: 0x06003CCB RID: 15563 RVA: 0x00203118 File Offset: 0x00201518
		private int GetReservationsCount(IAttackTarget target, Faction faction)
		{
			int num = 0;
			for (int i = 0; i < this.reservations.Count; i++)
			{
				AttackTargetReservationManager.AttackTargetReservation attackTargetReservation = this.reservations[i];
				if (attackTargetReservation.target == target && attackTargetReservation.claimant.Faction == faction)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06003CCC RID: 15564 RVA: 0x0020317C File Offset: 0x0020157C
		private int GetMaxPreferredReservationsCount(IAttackTarget target)
		{
			int num = 0;
			Thing thing = target.Thing;
			CellRect cellRect = thing.OccupiedRect();
			foreach (IntVec3 c in cellRect.ExpandedBy(1))
			{
				if (!cellRect.Contains(c))
				{
					if (c.InBounds(this.map))
					{
						if (c.Standable(this.map))
						{
							num++;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x02000AA6 RID: 2726
		public class AttackTargetReservation : IExposable
		{
			// Token: 0x0400267E RID: 9854
			public IAttackTarget target;

			// Token: 0x0400267F RID: 9855
			public Pawn claimant;

			// Token: 0x04002680 RID: 9856
			public Job job;

			// Token: 0x06003CD0 RID: 15568 RVA: 0x0020327B File Offset: 0x0020167B
			public void ExposeData()
			{
				Scribe_References.Look<IAttackTarget>(ref this.target, "target", false);
				Scribe_References.Look<Pawn>(ref this.claimant, "claimant", false);
				Scribe_References.Look<Job>(ref this.job, "job", false);
			}
		}
	}
}
