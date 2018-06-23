using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020006E2 RID: 1762
	public class ActiveDropPodInfo : IThingHolder, IExposable
	{
		// Token: 0x04001560 RID: 5472
		public IThingHolder parent;

		// Token: 0x04001561 RID: 5473
		public ThingOwner innerContainer;

		// Token: 0x04001562 RID: 5474
		public int openDelay = 110;

		// Token: 0x04001563 RID: 5475
		public bool leaveSlag = false;

		// Token: 0x04001564 RID: 5476
		public bool savePawnsWithReferenceMode;

		// Token: 0x04001565 RID: 5477
		public const int DefaultOpenDelay = 110;

		// Token: 0x04001566 RID: 5478
		private List<Thing> tmpThings = new List<Thing>();

		// Token: 0x04001567 RID: 5479
		private List<Pawn> tmpSavedPawns = new List<Pawn>();

		// Token: 0x0600265C RID: 9820 RVA: 0x001498AB File Offset: 0x00147CAB
		public ActiveDropPodInfo()
		{
			this.innerContainer = new ThingOwner<Thing>(this);
		}

		// Token: 0x0600265D RID: 9821 RVA: 0x001498E8 File Offset: 0x00147CE8
		public ActiveDropPodInfo(IThingHolder parent)
		{
			this.innerContainer = new ThingOwner<Thing>(this);
			this.parent = parent;
		}

		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x0600265E RID: 9822 RVA: 0x00149934 File Offset: 0x00147D34
		// (set) Token: 0x0600265F RID: 9823 RVA: 0x00149988 File Offset: 0x00147D88
		public Thing SingleContainedThing
		{
			get
			{
				Thing result;
				if (this.innerContainer.Count == 0)
				{
					result = null;
				}
				else
				{
					if (this.innerContainer.Count > 1)
					{
						Log.Error("ContainedThing used on a DropPodInfo holding > 1 thing.", false);
					}
					result = this.innerContainer[0];
				}
				return result;
			}
			set
			{
				this.innerContainer.Clear();
				this.innerContainer.TryAdd(value, true);
			}
		}

		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x06002660 RID: 9824 RVA: 0x001499A4 File Offset: 0x00147DA4
		public IThingHolder ParentHolder
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x06002661 RID: 9825 RVA: 0x001499C0 File Offset: 0x00147DC0
		public void ExposeData()
		{
			if (this.savePawnsWithReferenceMode && Scribe.mode == LoadSaveMode.Saving)
			{
				this.tmpThings.Clear();
				this.tmpThings.AddRange(this.innerContainer);
				this.tmpSavedPawns.Clear();
				for (int i = 0; i < this.tmpThings.Count; i++)
				{
					Pawn pawn = this.tmpThings[i] as Pawn;
					if (pawn != null)
					{
						this.innerContainer.Remove(pawn);
						this.tmpSavedPawns.Add(pawn);
					}
				}
				this.tmpThings.Clear();
			}
			Scribe_Values.Look<bool>(ref this.savePawnsWithReferenceMode, "savePawnsWithReferenceMode", false, false);
			if (this.savePawnsWithReferenceMode)
			{
				Scribe_Collections.Look<Pawn>(ref this.tmpSavedPawns, "tmpSavedPawns", LookMode.Reference, new object[0]);
			}
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Values.Look<int>(ref this.openDelay, "openDelay", 110, false);
			Scribe_Values.Look<bool>(ref this.leaveSlag, "leaveSlag", false, false);
			if (this.savePawnsWithReferenceMode && (Scribe.mode == LoadSaveMode.PostLoadInit || Scribe.mode == LoadSaveMode.Saving))
			{
				for (int j = 0; j < this.tmpSavedPawns.Count; j++)
				{
					this.innerContainer.TryAdd(this.tmpSavedPawns[j], true);
				}
				this.tmpSavedPawns.Clear();
			}
		}

		// Token: 0x06002662 RID: 9826 RVA: 0x00149B44 File Offset: 0x00147F44
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x06002663 RID: 9827 RVA: 0x00149B5F File Offset: 0x00147F5F
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}
	}
}
