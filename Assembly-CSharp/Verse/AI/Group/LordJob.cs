using System;

namespace Verse.AI.Group
{
	// Token: 0x020009E7 RID: 2535
	public abstract class LordJob : IExposable
	{
		// Token: 0x0400246D RID: 9325
		public Lord lord;

		// Token: 0x170008C5 RID: 2245
		// (get) Token: 0x060038F3 RID: 14579 RVA: 0x00049894 File Offset: 0x00047C94
		public virtual bool LostImportantReferenceDuringLoading
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008C6 RID: 2246
		// (get) Token: 0x060038F4 RID: 14580 RVA: 0x000498AC File Offset: 0x00047CAC
		public virtual bool AllowStartNewGatherings
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170008C7 RID: 2247
		// (get) Token: 0x060038F5 RID: 14581 RVA: 0x000498C4 File Offset: 0x00047CC4
		public virtual bool NeverInRestraints
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008C8 RID: 2248
		// (get) Token: 0x060038F6 RID: 14582 RVA: 0x000498DC File Offset: 0x00047CDC
		public virtual bool GuiltyOnDowned
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008C9 RID: 2249
		// (get) Token: 0x060038F7 RID: 14583 RVA: 0x000498F4 File Offset: 0x00047CF4
		protected Map Map
		{
			get
			{
				return this.lord.lordManager.map;
			}
		}

		// Token: 0x060038F8 RID: 14584
		public abstract StateGraph CreateGraph();

		// Token: 0x060038F9 RID: 14585 RVA: 0x00049919 File Offset: 0x00047D19
		public virtual void ExposeData()
		{
		}

		// Token: 0x060038FA RID: 14586 RVA: 0x0004991C File Offset: 0x00047D1C
		public virtual void Cleanup()
		{
		}

		// Token: 0x060038FB RID: 14587 RVA: 0x0004991F File Offset: 0x00047D1F
		public virtual void Notify_PawnAdded(Pawn p)
		{
		}

		// Token: 0x060038FC RID: 14588 RVA: 0x00049922 File Offset: 0x00047D22
		public virtual void Notify_PawnLost(Pawn p, PawnLostCondition condition)
		{
		}

		// Token: 0x060038FD RID: 14589 RVA: 0x00049928 File Offset: 0x00047D28
		public virtual string GetReport()
		{
			return null;
		}

		// Token: 0x060038FE RID: 14590 RVA: 0x00049940 File Offset: 0x00047D40
		public virtual bool CanOpenAnyDoor(Pawn p)
		{
			return false;
		}

		// Token: 0x060038FF RID: 14591 RVA: 0x00049958 File Offset: 0x00047D58
		public virtual bool ValidateAttackTarget(Pawn searcher, Thing target)
		{
			return true;
		}
	}
}
