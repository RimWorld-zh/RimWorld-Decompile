using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006BA RID: 1722
	public abstract class SignalAction : Thing
	{
		// Token: 0x0600251B RID: 9499 RVA: 0x0013E688 File Offset: 0x0013CA88
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

		// Token: 0x0600251C RID: 9500
		protected abstract void DoAction(object[] args);

		// Token: 0x0600251D RID: 9501 RVA: 0x0013E6D5 File Offset: 0x0013CAD5
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.signalTag, "signalTag", null, false);
		}

		// Token: 0x04001483 RID: 5251
		public string signalTag;
	}
}
