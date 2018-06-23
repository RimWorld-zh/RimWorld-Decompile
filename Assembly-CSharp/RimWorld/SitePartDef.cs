using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020002D2 RID: 722
	public class SitePartDef : SiteDefBase
	{
		// Token: 0x04000733 RID: 1843
		[MustTranslate]
		public string descriptionDialogue;

		// Token: 0x06000BF2 RID: 3058 RVA: 0x0006A4FD File Offset: 0x000688FD
		public SitePartDef()
		{
			this.workerClass = typeof(SitePartWorker);
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000BF3 RID: 3059 RVA: 0x0006A518 File Offset: 0x00068918
		public new SitePartWorker Worker
		{
			get
			{
				return (SitePartWorker)base.Worker;
			}
		}

		// Token: 0x06000BF4 RID: 3060 RVA: 0x0006A538 File Offset: 0x00068938
		public override bool FactionCanOwn(Faction faction)
		{
			return base.FactionCanOwn(faction) && this.Worker.FactionCanOwn(faction);
		}

		// Token: 0x06000BF5 RID: 3061 RVA: 0x0006A568 File Offset: 0x00068968
		protected override SiteWorkerBase CreateWorker()
		{
			return (SitePartWorker)Activator.CreateInstance(this.workerClass);
		}
	}
}
