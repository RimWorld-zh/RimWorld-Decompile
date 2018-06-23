using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E93 RID: 3731
	[StaticConstructorOnStartup]
	public static class UIHighlighter
	{
		// Token: 0x04003A43 RID: 14915
		private static List<Pair<string, int>> liveTags = new List<Pair<string, int>>();

		// Token: 0x04003A44 RID: 14916
		private const float PulseFrequency = 1.2f;

		// Token: 0x04003A45 RID: 14917
		private const float PulseAmplitude = 0.7f;

		// Token: 0x04003A46 RID: 14918
		private static readonly Texture2D TutorHighlightAtlas = ContentFinder<Texture2D>.Get("UI/Widgets/TutorHighlightAtlas", true);

		// Token: 0x06005818 RID: 22552 RVA: 0x002D3098 File Offset: 0x002D1498
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

		// Token: 0x06005819 RID: 22553 RVA: 0x002D3140 File Offset: 0x002D1540
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

		// Token: 0x0600581A RID: 22554 RVA: 0x002D31FC File Offset: 0x002D15FC
		public static void UIHighlighterUpdate()
		{
			UIHighlighter.liveTags.RemoveAll((Pair<string, int> pair) => Time.frameCount > pair.Second + 1);
		}
	}
}
