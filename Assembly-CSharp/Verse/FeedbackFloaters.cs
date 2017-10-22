using System;
using System.Collections.Generic;

namespace Verse
{
	public class FeedbackFloaters
	{
		protected List<FeedbackItem> feeders = new List<FeedbackItem>();

		public void AddFeedback(FeedbackItem newFeedback)
		{
			this.feeders.Add(newFeedback);
		}

		public void FeedbackUpdate()
		{
			for (int num = this.feeders.Count - 1; num >= 0; num--)
			{
				this.feeders[num].Update();
				if (this.feeders[num].TimeLeft <= 0.0)
				{
					this.feeders.Remove(this.feeders[num]);
				}
			}
		}

		public void FeedbackOnGUI()
		{
			List<FeedbackItem>.Enumerator enumerator = this.feeders.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					FeedbackItem current = enumerator.Current;
					current.FeedbackOnGUI();
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
		}
	}
}
