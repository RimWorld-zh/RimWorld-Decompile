using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000306 RID: 774
	public class SignalManager
	{
		// Token: 0x06000CE1 RID: 3297 RVA: 0x00070DDC File Offset: 0x0006F1DC
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

		// Token: 0x06000CE2 RID: 3298 RVA: 0x00070E3A File Offset: 0x0006F23A
		public void DeregisterReceiver(ISignalReceiver receiver)
		{
			this.receivers.Remove(receiver);
		}

		// Token: 0x06000CE3 RID: 3299 RVA: 0x00070E4C File Offset: 0x0006F24C
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

		// Token: 0x0400085A RID: 2138
		public List<ISignalReceiver> receivers = new List<ISignalReceiver>();
	}
}
