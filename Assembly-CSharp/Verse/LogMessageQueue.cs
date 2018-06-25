using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000F1A RID: 3866
	public class LogMessageQueue
	{
		// Token: 0x04003D8C RID: 15756
		public int maxMessages = 200;

		// Token: 0x04003D8D RID: 15757
		private Queue<LogMessage> messages = new Queue<LogMessage>();

		// Token: 0x04003D8E RID: 15758
		private LogMessage lastMessage = null;

		// Token: 0x17000EE3 RID: 3811
		// (get) Token: 0x06005CBD RID: 23741 RVA: 0x002F102C File Offset: 0x002EF42C
		public IEnumerable<LogMessage> Messages
		{
			get
			{
				return this.messages;
			}
		}

		// Token: 0x06005CBE RID: 23742 RVA: 0x002F1048 File Offset: 0x002EF448
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

		// Token: 0x06005CBF RID: 23743 RVA: 0x002F10C8 File Offset: 0x002EF4C8
		internal void Clear()
		{
			this.messages.Clear();
			this.lastMessage = null;
		}
	}
}
