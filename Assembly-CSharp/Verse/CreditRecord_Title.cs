using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EDC RID: 3804
	public class CreditRecord_Title : CreditsEntry
	{
		// Token: 0x060059F1 RID: 23025 RVA: 0x002E1F77 File Offset: 0x002E0377
		public CreditRecord_Title()
		{
		}

		// Token: 0x060059F2 RID: 23026 RVA: 0x002E1F80 File Offset: 0x002E0380
		public CreditRecord_Title(string title)
		{
			this.title = title;
		}

		// Token: 0x060059F3 RID: 23027 RVA: 0x002E1F90 File Offset: 0x002E0390
		public override float DrawHeight(float width)
		{
			return 100f;
		}

		// Token: 0x060059F4 RID: 23028 RVA: 0x002E1FAC File Offset: 0x002E03AC
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

		// Token: 0x04003C61 RID: 15457
		public string title;
	}
}
