using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EDD RID: 3805
	public class CreditRecord_Role : CreditsEntry
	{
		// Token: 0x04003C76 RID: 15478
		public string roleKey;

		// Token: 0x04003C77 RID: 15479
		public string creditee;

		// Token: 0x04003C78 RID: 15480
		public string extra;

		// Token: 0x06005A11 RID: 23057 RVA: 0x002E3F95 File Offset: 0x002E2395
		public CreditRecord_Role()
		{
		}

		// Token: 0x06005A12 RID: 23058 RVA: 0x002E3F9E File Offset: 0x002E239E
		public CreditRecord_Role(string roleKey, string creditee, string extra = null)
		{
			this.roleKey = roleKey;
			this.creditee = creditee;
			this.extra = extra;
		}

		// Token: 0x06005A13 RID: 23059 RVA: 0x002E3FBC File Offset: 0x002E23BC
		public override float DrawHeight(float width)
		{
			return 50f;
		}

		// Token: 0x06005A14 RID: 23060 RVA: 0x002E3FD8 File Offset: 0x002E23D8
		public override void Draw(Rect rect)
		{
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleLeft;
			Rect rect2 = rect;
			rect2.width = 0f;
			if (!this.roleKey.NullOrEmpty())
			{
				rect2.width = rect.width / 2f;
				Widgets.Label(rect2, this.roleKey.Translate());
			}
			Rect rect3 = rect;
			rect3.xMin = rect2.xMax;
			if (this.roleKey.NullOrEmpty())
			{
				Text.Anchor = TextAnchor.MiddleCenter;
			}
			Widgets.Label(rect3, this.creditee);
			Text.Anchor = TextAnchor.MiddleLeft;
			if (!this.extra.NullOrEmpty())
			{
				Rect rect4 = rect3;
				rect4.yMin += 28f;
				Text.Font = GameFont.Tiny;
				GUI.color = new Color(0.7f, 0.7f, 0.7f);
				Widgets.Label(rect4, this.extra);
				GUI.color = Color.white;
			}
		}
	}
}
