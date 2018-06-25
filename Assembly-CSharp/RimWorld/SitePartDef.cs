using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020002D4 RID: 724
	public class SitePartDef : SiteDefBase
	{
		// Token: 0x04000735 RID: 1845
		[MustTranslate]
		public string descriptionDialogue;

		// Token: 0x06000BF5 RID: 3061 RVA: 0x0006A649 File Offset: 0x00068A49
		public SitePartDef()
		{
			this.workerClass = typeof(SitePartWorker);
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000BF6 RID: 3062 RVA: 0x0006A664 File Offset: 0x00068A64
		public new SitePartWorker Worker
		{
			get
			{
				return (SitePartWorker)base.Worker;
			}
		}

		// Token: 0x06000BF7 RID: 3063 RVA: 0x0006A684 File Offset: 0x00068A84
		public override bool FactionCanOwn(Faction faction)
		{
			return base.FactionCanOwn(faction) && this.Worker.FactionCanOwn(faction);
		}

		// Token: 0x06000BF8 RID: 3064 RVA: 0x0006A6B4 File Offset: 0x00068AB4
		protected override SiteWorkerBase CreateWorker()
		{
			return (SitePartWorker)Activator.CreateInstance(this.workerClass);
		}
	}
}
