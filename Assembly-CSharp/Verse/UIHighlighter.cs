using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E96 RID: 3734
	[StaticConstructorOnStartup]
	public static class UIHighlighter
	{
		// Token: 0x04003A4B RID: 14923
		private static List<Pair<string, int>> liveTags = new List<Pair<string, int>>();

		// Token: 0x04003A4C RID: 14924
		private const float PulseFrequency = 1.2f;

		// Token: 0x04003A4D RID: 14925
		private const float PulseAmplitude = 0.7f;

		// Token: 0x04003A4E RID: 14926
		private static readonly Texture2D TutorHighlightAtlas = ContentFinder<Texture2D>.Get("UI/Widgets/TutorHighlightAtlas", true);

		// Token: 0x0600581C RID: 22556 RVA: 0x002D33B0 File Offset: 0x002D17B0
		public static void HighlightTag(string tag)
		{
			if (Event.current.type == EventType.Repaint)
			{
				if (!tag.NullOrEmpty())
				{
					for (int i = 0; i < UIHighlighter.liveTags.Count; i++)
					{
						if (UIHighlighter.liveTags[i].First == tag && UIHighlighter.liveTags[i].Second == Time.frameCount)
						{
							return;
						}
					}
					UIHighlighter.liveTags.Add(new Pair<string, int>(tag, Time.frameCount));
				}
			}
		}

		// Token: 0x0600581D RID: 22557 RVA: 0x002D3458 File Offset: 0x002D1858
		public static void HighlightOpportunity(Rect rect, string tag)
		{
			if (Event.current.type == EventType.Repaint)
			{
				for (int i = 0; i < UIHighlighter.liveTags.Count; i++)
				{
					Pair<string, int> pair = UIHighlighter.liveTags[i];
					if (tag == pair.First && Time.frameCount == pair.Second + 1)
					{
						Rect rect2 = rect.ContractedBy(-10f);
						GUI.color = new Color(1f, 1f, 1f, Pulser.PulseBrightness(1.2f, 0.7f));
						Widgets.DrawAtlas(rect2, UIHighlighter.TutorHighlightAtlas);
						GUI.color = Color.white;
					}
				}
			}
		}

		// Token: 0x0600581E RID: 22558 RVA: 0x002D3514 File Offset: 0x002D1914
		public static void UIHighlighterUpdate()
		{
			UIHighlighter.liveTags.RemoveAll((Pair<string, int> pair) => Time.frameCount > pair.Second + 1);
		}
	}
}
