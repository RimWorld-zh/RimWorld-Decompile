using System;

namespace RimWorld
{
	// Token: 0x020002CE RID: 718
	public class SiteCoreDef : SiteDefBase
	{
		// Token: 0x06000BDD RID: 3037 RVA: 0x00069D66 File Offset: 0x00068166
		public SiteCoreDef()
		{
			this.workerClass = typeof(SiteCoreWorker);
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000BDE RID: 3038 RVA: 0x00069D94 File Offset: 0x00068194
		public new SiteCoreWorker Worker
		{
			get
			{
				return (SiteCoreWorker)base.Worker;
			}
		}

		// Token: 0x06000BDF RID: 3039 RVA: 0x00069DB4 File Offset: 0x000681B4
		public override bool FactionCanOwn(Faction faction)
		{
			return base.FactionCanOwn(faction) && this.Worker.FactionCanOwn(faction);
		}

		// Token: 0x06000BE0 RID: 3040 RVA: 0x00069DE4 File Offset: 0x000681E4
		protected override SiteWorkerBase CreateWorker()
		{
			return (SiteCoreWorker)Activator.CreateInstance(this.workerClass);
		}

		// Token: 0x04000720 RID: 1824
		public bool transportPodsCanLandAndGenerateMap = true;

		// Token: 0x04000721 RID: 1825
		public float forceExitAndRemoveMapCountdownDurationDays = 3f;
	}
}
