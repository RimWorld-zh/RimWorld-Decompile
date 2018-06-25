using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020008D6 RID: 2262
	public class TutorialState : IExposable
	{
		// Token: 0x04001BCA RID: 7114
		public List<Thing> startingItems = new List<Thing>();

		// Token: 0x04001BCB RID: 7115
		public CellRect roomRect;

		// Token: 0x04001BCC RID: 7116
		public CellRect sandbagsRect;

		// Token: 0x04001BCD RID: 7117
		public int endTick = -1;

		// Token: 0x04001BCE RID: 7118
		public bool introDone = false;

		// Token: 0x060033CB RID: 13259 RVA: 0x001BAEBC File Offset: 0x001B92BC
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving && this.startingItems != null)
			{
				this.startingItems.RemoveAll((Thing it) => it == null || it.Destroyed || (it.Map == null && it.MapHeld == null));
			}
			Scribe_Collections.Look<Thing>(ref this.startingItems, "startingItems", LookMode.Reference, new object[0]);
			Scribe_Values.Look<CellRect>(ref this.roomRect, "roomRect", default(CellRect), false);
			Scribe_Values.Look<CellRect>(ref this.sandbagsRect, "sandbagsRect", default(CellRect), false);
			Scribe_Values.Look<int>(ref this.endTick, "endTick", -1, false);
			Scribe_Values.Look<bool>(ref this.introDone, "introDone", false, false);
			if (this.startingItems != null)
			{
				this.startingItems.RemoveAll((Thing it) => it == null);
			}
		}

		// Token: 0x060033CC RID: 13260 RVA: 0x001BAFAC File Offset: 0x001B93AC
		public void Notify_TutorialEnding()
		{
			this.startingItems.Clear();
			this.roomRect = default(CellRect);
			this.sandbagsRect = default(CellRect);
			this.endTick = Find.TickManager.TicksGame;
		}

		// Token: 0x060033CD RID: 13261 RVA: 0x001BAFF3 File Offset: 0x001B93F3
		public void AddStartingItem(Thing t)
		{
			if (!this.startingItems.Contains(t))
			{
				this.startingItems.Add(t);
			}
		}
	}
}
