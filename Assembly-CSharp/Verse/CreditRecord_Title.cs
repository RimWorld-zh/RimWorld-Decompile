using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EDB RID: 3803
	public class CreditRecord_Title : CreditsEntry
	{
		// Token: 0x06005A12 RID: 23058 RVA: 0x002E3D8B File Offset: 0x002E218B
		public CreditRecord_Title()
		{
		}

		// Token: 0x06005A13 RID: 23059 RVA: 0x002E3D94 File Offset: 0x002E2194
		public CreditRecord_Title(string title)
		{
			this.title = title;
		}

		// Token: 0x06005A14 RID: 23060 RVA: 0x002E3DA4 File Offset: 0x002E21A4
		public override float DrawHeight(float width)
		{
			return 100f;
		}

		// Token: 0x06005A15 RID: 23061 RVA: 0x002E3DC0 File Offset: 0x002E21C0
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

		// Token: 0x04003C71 RID: 15473
		public string title;
	}
}
