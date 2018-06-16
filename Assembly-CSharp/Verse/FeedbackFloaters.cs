using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000E63 RID: 3683
	public class FeedbackFloaters
	{
		// Token: 0x060056A5 RID: 22181 RVA: 0x002C9DC2 File Offset: 0x002C81C2
		public void AddFeedback(FeedbackItem newFeedback)
		{
			this.feeders.Add(newFeedback);
		}

		// Token: 0x060056A6 RID: 22182 RVA: 0x002C9DD4 File Offset: 0x002C81D4
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

		// Token: 0x060056A7 RID: 22183 RVA: 0x002C9E48 File Offset: 0x002C8248
		public void FeedbackOnGUI()
		{
			foreach (FeedbackItem feedbackItem in this.feeders)
			{
				feedbackItem.FeedbackOnGUI();
			}
		}

		// Token: 0x0400396B RID: 14699
		protected List<FeedbackItem> feeders = new List<FeedbackItem>();
	}
}
