using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EDA RID: 3802
	public class CreditRecord_Role : CreditsEntry
	{
		// Token: 0x06005A0E RID: 23054 RVA: 0x002E3C55 File Offset: 0x002E2055
		public CreditRecord_Role()
		{
		}

		// Token: 0x06005A0F RID: 23055 RVA: 0x002E3C5E File Offset: 0x002E205E
		public CreditRecord_Role(string roleKey, string creditee, string extra = null)
		{
			this.roleKey = roleKey;
			this.creditee = creditee;
			this.extra = extra;
		}

		// Token: 0x06005A10 RID: 23056 RVA: 0x002E3C7C File Offset: 0x002E207C
		public override float DrawHeight(float width)
		{
			return 50f;
		}

		// Token: 0x06005A11 RID: 23057 RVA: 0x002E3C98 File Offset: 0x002E2098
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

		// Token: 0x04003C6E RID: 15470
		public string roleKey;

		// Token: 0x04003C6F RID: 15471
		public string creditee;

		// Token: 0x04003C70 RID: 15472
		public string extra;
	}
}
