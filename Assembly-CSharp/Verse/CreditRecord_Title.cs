using UnityEngine;

namespace Verse
{
	public class CreditRecord_Title : CreditsEntry
	{
		public string title;

		public CreditRecord_Title()
		{
		}

		public CreditRecord_Title(string title)
		{
			this.title = title;
		}

		public override float DrawHeight(float width)
		{
			return 100f;
		}

		public override void Draw(Rect rect)
		{
			rect.yMin += 31f;
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, this.title);
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			Widgets.DrawLineHorizontal((float)(rect.x + 10.0), (float)(Mathf.Round(rect.yMax) - 14.0), (float)(rect.width - 20.0));
			GUI.color = Color.white;
		}
	}
}
