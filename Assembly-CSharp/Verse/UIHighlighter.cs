using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E95 RID: 3733
	[StaticConstructorOnStartup]
	public static class UIHighlighter
	{
		// Token: 0x060057FA RID: 22522 RVA: 0x002D1488 File Offset: 0x002CF888
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

		// Token: 0x060057FB RID: 22523 RVA: 0x002D1530 File Offset: 0x002CF930
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

		// Token: 0x060057FC RID: 22524 RVA: 0x002D15EC File Offset: 0x002CF9EC
		public static void UIHighlighterUpdate()
		{
			UIHighlighter.liveTags.RemoveAll((Pair<string, int> pair) => Time.frameCount > pair.Second + 1);
		}

		// Token: 0x04003A35 RID: 14901
		private static List<Pair<string, int>> liveTags = new List<Pair<string, int>>();

		// Token: 0x04003A36 RID: 14902
		private const float PulseFrequency = 1.2f;

		// Token: 0x04003A37 RID: 14903
		private const float PulseAmplitude = 0.7f;

		// Token: 0x04003A38 RID: 14904
		private static readonly Texture2D TutorHighlightAtlas = ContentFinder<Texture2D>.Get("UI/Widgets/TutorHighlightAtlas", true);
	}
}
