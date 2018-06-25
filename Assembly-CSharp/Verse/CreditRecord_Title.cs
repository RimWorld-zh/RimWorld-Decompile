using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EDD RID: 3805
	public class CreditRecord_Title : CreditsEntry
	{
		// Token: 0x04003C71 RID: 15473
		public string title;

		// Token: 0x06005A15 RID: 23061 RVA: 0x002E3EAB File Offset: 0x002E22AB
		public CreditRecord_Title()
		{
		}

		// Token: 0x06005A16 RID: 23062 RVA: 0x002E3EB4 File Offset: 0x002E22B4
		public CreditRecord_Title(string title)
		{
			this.title = title;
		}

		// Token: 0x06005A17 RID: 23063 RVA: 0x002E3EC4 File Offset: 0x002E22C4
		public override float DrawHeight(float width)
		{
			return 100f;
		}

		// Token: 0x06005A18 RID: 23064 RVA: 0x002E3EE0 File Offset: 0x002E22E0
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
	}
}
