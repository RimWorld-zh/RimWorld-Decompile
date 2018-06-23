using System;

namespace Verse.AI.Group
{
	// Token: 0x020009E4 RID: 2532
	public abstract class LordJob : IExposable
	{
		// Token: 0x0400245C RID: 9308
		public Lord lord;

		// Token: 0x170008C5 RID: 2245
		// (get) Token: 0x060038EE RID: 14574 RVA: 0x00049898 File Offset: 0x00047C98
		public virtual bool LostImportantReferenceDuringLoading
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008C6 RID: 2246
		// (get) Token: 0x060038EF RID: 14575 RVA: 0x000498B0 File Offset: 0x00047CB0
		public virtual bool AllowStartNewGatherings
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170008C7 RID: 2247
		// (get) Token: 0x060038F0 RID: 14576 RVA: 0x000498C8 File Offset: 0x00047CC8
		public virtual bool NeverInRestraints
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008C8 RID: 2248
		// (get) Token: 0x060038F1 RID: 14577 RVA: 0x000498E0 File Offset: 0x00047CE0
		public virtual bool GuiltyOnDowned
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008C9 RID: 2249
		// (get) Token: 0x060038F2 RID: 14578 RVA: 0x000498F8 File Offset: 0x00047CF8
		protected Map Map
		{
			get
			{
				return this.lord.lordManager.map;
			}
		}

		// Token: 0x060038F3 RID: 14579
		public abstract StateGraph CreateGraph();

		// Token: 0x060038F4 RID: 14580 RVA: 0x0004991D File Offset: 0x00047D1D
		public virtual void ExposeData()
		{
		}

		// Token: 0x060038F5 RID: 14581 RVA: 0x00049920 File Offset: 0x00047D20
		public virtual void Cleanup()
		{
		}

		// Token: 0x060038F6 RID: 14582 RVA: 0x00049923 File Offset: 0x00047D23
		public virtual void Notify_PawnAdded(Pawn p)
		{
		}

		// Token: 0x060038F7 RID: 14583 RVA: 0x00049926 File Offset: 0x00047D26
		public virtual void Notify_PawnLost(Pawn p, PawnLostCondition condition)
		{
		}

		// Token: 0x060038F8 RID: 14584 RVA: 0x0004992C File Offset: 0x00047D2C
		public virtual string GetReport()
		{
			return null;
		}

		// Token: 0x060038F9 RID: 14585 RVA: 0x00049944 File Offset: 0x00047D44
		public virtual bool CanOpenAnyDoor(Pawn p)
		{
			return false;
		}

		// Token: 0x060038FA RID: 14586 RVA: 0x0004995C File Offset: 0x00047D5C
		public virtual bool ValidateAttackTarget(Pawn searcher, Thing target)
		{
			return true;
		}
	}
}
