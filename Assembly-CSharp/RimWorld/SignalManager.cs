using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000308 RID: 776
	public class SignalManager
	{
		// Token: 0x0400085F RID: 2143
		public List<ISignalReceiver> receivers = new List<ISignalReceiver>();

		// Token: 0x06000CE4 RID: 3300 RVA: 0x00070FE8 File Offset: 0x0006F3E8
		public void RegisterReceiver(ISignalReceiver receiver)
		{
			if (receiver == null)
			{
				Log.Error("Tried to register a null reciever.", false);
			}
			else if (this.receivers.Contains(receiver))
			{
				Log.Error("Tried to register the same receiver twice: " + receiver.ToStringSafe<ISignalReceiver>(), false);
			}
			else
			{
				this.receivers.Add(receiver);
			}
		}

		// Token: 0x06000CE5 RID: 3301 RVA: 0x00071046 File Offset: 0x0006F446
		public void DeregisterReceiver(ISignalReceiver receiver)
		{
			this.receivers.Remove(receiver);
		}

		// Token: 0x06000CE6 RID: 3302 RVA: 0x00071058 File Offset: 0x0006F458
		public void SendSignal(Signal signal)
		{
			if (DebugViewSettings.logSignals)
			{
				Log.Message("Signal: tag=" + signal.tag.ToStringSafe<string>() + " args=" + signal.args.ToStringSafeEnumerable(), false);
			}
			for (int i = 0; i < this.receivers.Count; i++)
			{
				try
				{
					this.receivers[i].Notify_SignalReceived(signal);
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Error while sending signal to ",
						this.receivers[i].ToStringSafe<ISignalReceiver>(),
						": ",
						ex
					}), false);
				}
			}
		}
	}
}
