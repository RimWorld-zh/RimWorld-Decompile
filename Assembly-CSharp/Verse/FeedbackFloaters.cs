using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000E62 RID: 3682
	public class FeedbackFloaters
	{
		// Token: 0x060056A3 RID: 22179 RVA: 0x002C9DC2 File Offset: 0x002C81C2
		public void AddFeedback(FeedbackItem newFeedback)
		{
			this.feeders.Add(newFeedback);
		}

		// Token: 0x060056A4 RID: 22180 RVA: 0x002C9DD4 File Offset: 0x002C81D4
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

		// Token: 0x060056A5 RID: 22181 RVA: 0x002C9E48 File Offset: 0x002C8248
		public void FeedbackOnGUI()
		{
			foreach (FeedbackItem feedbackItem in this.feeders)
			{
				feedbackItem.FeedbackOnGUI();
			}
		}

		// Token: 0x04003969 RID: 14697
		protected List<FeedbackItem> feeders = new List<FeedbackItem>();
	}
}
