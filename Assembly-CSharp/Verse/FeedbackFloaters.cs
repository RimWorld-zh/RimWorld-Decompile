using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000E63 RID: 3683
	public class FeedbackFloaters
	{
		// Token: 0x04003978 RID: 14712
		protected List<FeedbackItem> feeders = new List<FeedbackItem>();

		// Token: 0x060056C7 RID: 22215 RVA: 0x002CBAFE File Offset: 0x002C9EFE
		public void AddFeedback(FeedbackItem newFeedback)
		{
			this.feeders.Add(newFeedback);
		}

		// Token: 0x060056C8 RID: 22216 RVA: 0x002CBB10 File Offset: 0x002C9F10
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

		// Token: 0x060056C9 RID: 22217 RVA: 0x002CBB84 File Offset: 0x002C9F84
		public void FeedbackOnGUI()
		{
			foreach (FeedbackItem feedbackItem in this.feeders)
			{
				feedbackItem.FeedbackOnGUI();
			}
		}
	}
}
