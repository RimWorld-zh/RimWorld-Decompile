using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000F16 RID: 3862
	public class LogMessageQueue
	{
		// Token: 0x17000EE0 RID: 3808
		// (get) Token: 0x06005C8B RID: 23691 RVA: 0x002EE980 File Offset: 0x002ECD80
		public IEnumerable<LogMessage> Messages
		{
			get
			{
				return this.messages;
			}
		}

		// Token: 0x06005C8C RID: 23692 RVA: 0x002EE99C File Offset: 0x002ECD9C
		public void Enqueue(LogMessage msg)
		{
			if (this.lastMessage != null && msg.CanCombineWith(this.lastMessage))
			{
				this.lastMessage.repeats++;
			}
			else
			{
				this.lastMessage = msg;
				this.messages.Enqueue(msg);
				if (this.messages.Count > this.maxMessages)
				{
					LogMessage oldMessage = this.messages.Dequeue();
					EditWindow_Log.Notify_MessageDequeued(oldMessage);
				}
			}
		}

		// Token: 0x06005C8D RID: 23693 RVA: 0x002EEA1C File Offset: 0x002ECE1C
		internal void Clear()
		{
			this.messages.Clear();
			this.lastMessage = null;
		}

		// Token: 0x04003D77 RID: 15735
		public int maxMessages = 200;

		// Token: 0x04003D78 RID: 15736
		private Queue<LogMessage> messages = new Queue<LogMessage>();

		// Token: 0x04003D79 RID: 15737
		private LogMessage lastMessage = null;
	}
}
