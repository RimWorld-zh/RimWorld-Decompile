using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020006E6 RID: 1766
	public class ActiveDropPodInfo : IThingHolder, IExposable
	{
		// Token: 0x06002662 RID: 9826 RVA: 0x0014968F File Offset: 0x00147A8F
		public ActiveDropPodInfo()
		{
			this.innerContainer = new ThingOwner<Thing>(this);
		}

		// Token: 0x06002663 RID: 9827 RVA: 0x001496CC File Offset: 0x00147ACC
		public ActiveDropPodInfo(IThingHolder parent)
		{
			this.innerContainer = new ThingOwner<Thing>(this);
			this.parent = parent;
		}

		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x06002664 RID: 9828 RVA: 0x00149718 File Offset: 0x00147B18
		// (set) Token: 0x06002665 RID: 9829 RVA: 0x0014976C File Offset: 0x00147B6C
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
		// (get) Token: 0x06002666 RID: 9830 RVA: 0x00149788 File Offset: 0x00147B88
		public IThingHolder ParentHolder
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x06002667 RID: 9831 RVA: 0x001497A4 File Offset: 0x00147BA4
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

		// Token: 0x06002668 RID: 9832 RVA: 0x00149928 File Offset: 0x00147D28
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x06002669 RID: 9833 RVA: 0x00149943 File Offset: 0x00147D43
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x04001562 RID: 5474
		public IThingHolder parent;

		// Token: 0x04001563 RID: 5475
		public ThingOwner innerContainer;

		// Token: 0x04001564 RID: 5476
		public int openDelay = 110;

		// Token: 0x04001565 RID: 5477
		public bool leaveSlag = false;

		// Token: 0x04001566 RID: 5478
		public bool savePawnsWithReferenceMode;

		// Token: 0x04001567 RID: 5479
		public const int DefaultOpenDelay = 110;

		// Token: 0x04001568 RID: 5480
		private List<Thing> tmpThings = new List<Thing>();

		// Token: 0x04001569 RID: 5481
		private List<Pawn> tmpSavedPawns = new List<Pawn>();
	}
}
