using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006BE RID: 1726
	public abstract class SignalAction : Thing
	{
		// Token: 0x06002521 RID: 9505 RVA: 0x0013E4C8 File Offset: 0x0013C8C8
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

		// Token: 0x06002522 RID: 9506
		protected abstract void DoAction(object[] args);

		// Token: 0x06002523 RID: 9507 RVA: 0x0013E515 File Offset: 0x0013C915
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.signalTag, "signalTag", null, false);
		}

		// Token: 0x04001485 RID: 5253
		public string signalTag;
	}
}
