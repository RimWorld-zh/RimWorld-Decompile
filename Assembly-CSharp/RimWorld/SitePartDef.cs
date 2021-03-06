﻿using System;

namespace RimWorld
{
	public class SitePartDef : SiteCoreOrPartDefBase
	{
		public bool alwaysHidden;

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

		protected override SiteCoreOrPartWorkerBase CreateWorker()
		{
			return (SitePartWorker)Activator.CreateInstance(this.workerClass);
		}
	}
}
