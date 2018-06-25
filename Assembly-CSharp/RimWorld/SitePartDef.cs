using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020002D4 RID: 724
	public class SitePartDef : SiteDefBase
	{
		// Token: 0x04000733 RID: 1843
		[MustTranslate]
		public string descriptionDialogue;

		// Token: 0x06000BF6 RID: 3062 RVA: 0x0006A64D File Offset: 0x00068A4D
		public SitePartDef()
		{
			this.workerClass = typeof(SitePartWorker);
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000BF7 RID: 3063 RVA: 0x0006A668 File Offset: 0x00068A68
		public new SitePartWorker Worker
		{
			get
			{
				return (SitePartWorker)base.Worker;
			}
		}

		// Token: 0x06000BF8 RID: 3064 RVA: 0x0006A688 File Offset: 0x00068A88
		public override bool FactionCanOwn(Faction faction)
		{
			return base.FactionCanOwn(faction) && this.Worker.FactionCanOwn(faction);
		}

		// Token: 0x06000BF9 RID: 3065 RVA: 0x0006A6B8 File Offset: 0x00068AB8
		protected override SiteWorkerBase CreateWorker()
		{
			return (SitePartWorker)Activator.CreateInstance(this.workerClass);
		}
	}
}
