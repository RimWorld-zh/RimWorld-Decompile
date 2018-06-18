using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006BE RID: 1726
	public abstract class SignalAction : Thing
	{
		// Token: 0x06002523 RID: 9507 RVA: 0x0013E540 File Offset: 0x0013C940
		public override void Notify_SignalReceived(Signal signal)
		{
			base.Notify_SignalReceived(signal);
			if (signal.tag == this.signalTag)
			{
				this.DoAction(signal.args);
				if (!base.Destroyed)
				{
					this.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x06002524 RID: 9508
		protected abstract void DoAction(object[] args);

		// Token: 0x06002525 RID: 9509 RVA: 0x0013E58D File Offset: 0x0013C98D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.signalTag, "signalTag", null, false);
		}

		// Token: 0x04001485 RID: 5253
		public string signalTag;
	}
}
