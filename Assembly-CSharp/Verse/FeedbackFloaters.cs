using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000E64 RID: 3684
	public class FeedbackFloaters
	{
		// Token: 0x04003980 RID: 14720
		protected List<FeedbackItem> feeders = new List<FeedbackItem>();

		// Token: 0x060056C7 RID: 22215 RVA: 0x002CBCEA File Offset: 0x002CA0EA
		public void AddFeedback(FeedbackItem newFeedback)
		{
			this.feeders.Add(newFeedback);
		}

		// Token: 0x060056C8 RID: 22216 RVA: 0x002CBCFC File Offset: 0x002CA0FC
		public void FeedbackUpdate()
		{
			for (int i = this.feeders.Count - 1; i >= 0; i--)
			{
				this.feeders[i].Update();
				if (this.feeders[i].TimeLeft <= 0f)
				{
					this.feeders.Remove(this.feeders[i]);
				}
			}
		}

		// Token: 0x060056C9 RID: 22217 RVA: 0x002CBD70 File Offset: 0x002CA170
		public void FeedbackOnGUI()
		{
			foreach (FeedbackItem feedbackItem in this.feeders)
			{
				feedbackItem.FeedbackOnGUI();
			}
		}
	}
}
