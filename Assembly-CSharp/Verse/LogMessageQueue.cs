using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000F17 RID: 3863
	public class LogMessageQueue
	{
		// Token: 0x17000EE1 RID: 3809
		// (get) Token: 0x06005C8D RID: 23693 RVA: 0x002EE8A4 File Offset: 0x002ECCA4
		public IEnumerable<LogMessage> Messages
		{
			get
			{
				return this.messages;
			}
		}

		// Token: 0x06005C8E RID: 23694 RVA: 0x002EE8C0 File Offset: 0x002ECCC0
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

		// Token: 0x06005C8F RID: 23695 RVA: 0x002EE940 File Offset: 0x002ECD40
		internal void Clear()
		{
			this.messages.Clear();
			this.lastMessage = null;
		}

		// Token: 0x04003D78 RID: 15736
		public int maxMessages = 200;

		// Token: 0x04003D79 RID: 15737
		private Queue<LogMessage> messages = new Queue<LogMessage>();

		// Token: 0x04003D7A RID: 15738
		private LogMessage lastMessage = null;
	}
}
