using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EDD RID: 3805
	public class CreditRecord_Title : CreditsEntry
	{
		// Token: 0x060059F3 RID: 23027 RVA: 0x002E1E9F File Offset: 0x002E029F
		public CreditRecord_Title()
		{
		}

		// Token: 0x060059F4 RID: 23028 RVA: 0x002E1EA8 File Offset: 0x002E02A8
		public CreditRecord_Title(string title)
		{
			this.title = title;
		}

		// Token: 0x060059F5 RID: 23029 RVA: 0x002E1EB8 File Offset: 0x002E02B8
		public override float DrawHeight(float width)
		{
			return 100f;
		}

		// Token: 0x060059F6 RID: 23030 RVA: 0x002E1ED4 File Offset: 0x002E02D4
		public override void Draw(Rect rect)
		{
			rect.yMin += 31f;
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, this.title);
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			Widgets.DrawLineHorizontal(rect.x + 10f, Mathf.Round(rect.yMax) - 14f, rect.width - 20f);
			GUI.color = Color.white;
		}

		// Token: 0x04003C62 RID: 15458
		public string title;
	}
}
