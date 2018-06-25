using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006BC RID: 1724
	public abstract class SignalAction : Thing
	{
		// Token: 0x04001487 RID: 5255
		public string signalTag;

		// Token: 0x0600251E RID: 9502 RVA: 0x0013EA40 File Offset: 0x0013CE40
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

		// Token: 0x0600251F RID: 9503
		protected abstract void DoAction(object[] args);

		// Token: 0x06002520 RID: 9504 RVA: 0x0013EA8D File Offset: 0x0013CE8D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.signalTag, "signalTag", null, false);
		}
	}
}
