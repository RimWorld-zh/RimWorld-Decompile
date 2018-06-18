using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EDB RID: 3803
	public class CreditRecord_Role : CreditsEntry
	{
		// Token: 0x060059ED RID: 23021 RVA: 0x002E1E41 File Offset: 0x002E0241
		public CreditRecord_Role()
		{
		}

		// Token: 0x060059EE RID: 23022 RVA: 0x002E1E4A File Offset: 0x002E024A
		public CreditRecord_Role(string roleKey, string creditee, string extra = null)
		{
			this.roleKey = roleKey;
			this.creditee = creditee;
			this.extra = extra;
		}

		// Token: 0x060059EF RID: 23023 RVA: 0x002E1E68 File Offset: 0x002E0268
		public override float DrawHeight(float width)
		{
			return 50f;
		}

		// Token: 0x060059F0 RID: 23024 RVA: 0x002E1E84 File Offset: 0x002E0284
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

		// Token: 0x04003C5E RID: 15454
		public string roleKey;

		// Token: 0x04003C5F RID: 15455
		public string creditee;

		// Token: 0x04003C60 RID: 15456
		public string extra;
	}
}
