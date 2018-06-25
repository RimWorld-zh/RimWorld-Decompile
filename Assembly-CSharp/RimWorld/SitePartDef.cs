using System;
using Verse;

namespace RimWorld
{
	public class SitePartDef : SiteDefBase
	{
		[MustTranslate]
		public string descriptionDialogue;

		public SitePartDef()
		{
			this.workerClass = typeof(SitePartWorker);
		}

		public new SitePartWorker Worker
		{
			get
			{
				return (SitePartWorker)base.Worker;
			}
		}

		public override bool FactionCanOwn(Faction faction)
		{
			return base.FactionCanOwn(faction) && this.Worker.FactionCanOwn(faction);
		}

		protected override SiteWorkerBase CreateWorker()
		{
			return (SitePartWorker)Activator.CreateInstance(this.workerClass);
		}
	}
}
