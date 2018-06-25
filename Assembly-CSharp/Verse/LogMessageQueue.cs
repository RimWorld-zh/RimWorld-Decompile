using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000F1B RID: 3867
	public class LogMessageQueue
	{
		// Token: 0x04003D94 RID: 15764
		public int maxMessages = 200;

		// Token: 0x04003D95 RID: 15765
		private Queue<LogMessage> messages = new Queue<LogMessage>();

		// Token: 0x04003D96 RID: 15766
		private LogMessage lastMessage = null;

		// Token: 0x17000EE3 RID: 3811
		// (get) Token: 0x06005CBD RID: 23741 RVA: 0x002F124C File Offset: 0x002EF64C
		public IEnumerable<LogMessage> Messages
		{
			get
			{
				return this.messages;
			}
		}

		// Token: 0x06005CBE RID: 23742 RVA: 0x002F1268 File Offset: 0x002EF668
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

		// Token: 0x06005CBF RID: 23743 RVA: 0x002F12E8 File Offset: 0x002EF6E8
		internal void Clear()
		{
			this.messages.Clear();
			this.lastMessage = null;
		}
	}
}
