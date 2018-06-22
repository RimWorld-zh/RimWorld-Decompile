using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000E61 RID: 3681
	public class FeedbackFloaters
	{
		// Token: 0x060056C3 RID: 22211 RVA: 0x002CB9D2 File Offset: 0x002C9DD2
		public void AddFeedback(FeedbackItem newFeedback)
		{
			this.feeders.Add(newFeedback);
		}

		// Token: 0x060056C4 RID: 22212 RVA: 0x002CB9E4 File Offset: 0x002C9DE4
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

		// Token: 0x060056C5 RID: 22213 RVA: 0x002CBA58 File Offset: 0x002C9E58
		public void FeedbackOnGUI()
		{
			foreach (FeedbackItem feedbackItem in this.feeders)
			{
				feedbackItem.FeedbackOnGUI();
			}
		}

		// Token: 0x04003978 RID: 14712
		protected List<FeedbackItem> feeders = new List<FeedbackItem>();
	}
}
