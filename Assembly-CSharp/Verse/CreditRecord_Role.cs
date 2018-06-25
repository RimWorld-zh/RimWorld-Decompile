using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EDC RID: 3804
	public class CreditRecord_Role : CreditsEntry
	{
		// Token: 0x04003C6E RID: 15470
		public string roleKey;

		// Token: 0x04003C6F RID: 15471
		public string creditee;

		// Token: 0x04003C70 RID: 15472
		public string extra;

		// Token: 0x06005A11 RID: 23057 RVA: 0x002E3D75 File Offset: 0x002E2175
		public CreditRecord_Role()
		{
		}

		// Token: 0x06005A12 RID: 23058 RVA: 0x002E3D7E File Offset: 0x002E217E
		public CreditRecord_Role(string roleKey, string creditee, string extra = null)
		{
			this.roleKey = roleKey;
			this.creditee = creditee;
			this.extra = extra;
		}

		// Token: 0x06005A13 RID: 23059 RVA: 0x002E3D9C File Offset: 0x002E219C
		public override float DrawHeight(float width)
		{
			return 50f;
		}

		// Token: 0x06005A14 RID: 23060 RVA: 0x002E3DB8 File Offset: 0x002E21B8
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
