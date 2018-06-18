using System;

namespace Verse.AI.Group
{
	// Token: 0x020009E8 RID: 2536
	public abstract class LordJob : IExposable
	{
		// Token: 0x170008C4 RID: 2244
		// (get) Token: 0x060038F4 RID: 14580 RVA: 0x000498AC File Offset: 0x00047CAC
		public virtual bool LostImportantReferenceDuringLoading
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008C5 RID: 2245
		// (get) Token: 0x060038F5 RID: 14581 RVA: 0x000498C4 File Offset: 0x00047CC4
		public virtual bool AllowStartNewGatherings
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170008C6 RID: 2246
		// (get) Token: 0x060038F6 RID: 14582 RVA: 0x000498DC File Offset: 0x00047CDC
		public virtual bool NeverInRestraints
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008C7 RID: 2247
		// (get) Token: 0x060038F7 RID: 14583 RVA: 0x000498F4 File Offset: 0x00047CF4
		public virtual bool GuiltyOnDowned
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008C8 RID: 2248
		// (get) Token: 0x060038F8 RID: 14584 RVA: 0x0004990C File Offset: 0x00047D0C
		protected Map Map
		{
			get
			{
				return this.lord.lordManager.map;
			}
		}

		// Token: 0x060038F9 RID: 14585
		public abstract StateGraph CreateGraph();

		// Token: 0x060038FA RID: 14586 RVA: 0x00049931 File Offset: 0x00047D31
		public virtual void ExposeData()
		{
		}

		// Token: 0x060038FB RID: 14587 RVA: 0x00049934 File Offset: 0x00047D34
		public virtual void Cleanup()
		{
		}

		// Token: 0x060038FC RID: 14588 RVA: 0x00049937 File Offset: 0x00047D37
		public virtual void Notify_PawnAdded(Pawn p)
		{
		}

		// Token: 0x060038FD RID: 14589 RVA: 0x0004993A File Offset: 0x00047D3A
		public virtual void Notify_PawnLost(Pawn p, PawnLostCondition condition)
		{
		}

		// Token: 0x060038FE RID: 14590 RVA: 0x00049940 File Offset: 0x00047D40
		public virtual string GetReport()
		{
			return null;
		}

		// Token: 0x060038FF RID: 14591 RVA: 0x00049958 File Offset: 0x00047D58
		public virtual bool CanOpenAnyDoor(Pawn p)
		{
			return false;
		}

		// Token: 0x06003900 RID: 14592 RVA: 0x00049970 File Offset: 0x00047D70
		public virtual bool ValidateAttackTarget(Pawn searcher, Thing target)
		{
			return true;
		}

		// Token: 0x04002461 RID: 9313
		public Lord lord;
	}
}
