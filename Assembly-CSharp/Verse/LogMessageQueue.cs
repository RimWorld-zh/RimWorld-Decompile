using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000F16 RID: 3862
	public class LogMessageQueue
	{
		// Token: 0x17000EE4 RID: 3812
		// (get) Token: 0x06005CB3 RID: 23731 RVA: 0x002F09AC File Offset: 0x002EEDAC
		public IEnumerable<LogMessage> Messages
		{
			get
			{
				return this.messages;
			}
		}

		// Token: 0x06005CB4 RID: 23732 RVA: 0x002F09C8 File Offset: 0x002EEDC8
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

		// Token: 0x06005CB5 RID: 23733 RVA: 0x002F0A48 File Offset: 0x002EEE48
		internal void Clear()
		{
			this.messages.Clear();
			this.lastMessage = null;
		}

		// Token: 0x04003D89 RID: 15753
		public int maxMessages = 200;

		// Token: 0x04003D8A RID: 15754
		private Queue<LogMessage> messages = new Queue<LogMessage>();

		// Token: 0x04003D8B RID: 15755
		private LogMessage lastMessage = null;
	}
}
