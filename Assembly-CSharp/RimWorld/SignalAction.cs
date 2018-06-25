using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006BC RID: 1724
	public abstract class SignalAction : Thing
	{
		// Token: 0x04001483 RID: 5251
		public string signalTag;

		// Token: 0x0600251F RID: 9503 RVA: 0x0013E7D8 File Offset: 0x0013CBD8
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

		// Token: 0x06002520 RID: 9504
		protected abstract void DoAction(object[] args);

		// Token: 0x06002521 RID: 9505 RVA: 0x0013E825 File Offset: 0x0013CC25
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.signalTag, "signalTag", null, false);
		}
	}
}
