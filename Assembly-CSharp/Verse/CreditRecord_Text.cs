using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EDC RID: 3804
	public class CreditRecord_Text : CreditsEntry
	{
		// Token: 0x04003C74 RID: 15476
		public string text;

		// Token: 0x04003C75 RID: 15477
		public TextAnchor anchor;

		// Token: 0x06005A0D RID: 23053 RVA: 0x002E3F32 File Offset: 0x002E2332
		public CreditRecord_Text()
		{
		}

		// Token: 0x06005A0E RID: 23054 RVA: 0x002E3F3B File Offset: 0x002E233B
		public CreditRecord_Text(string text, TextAnchor anchor = TextAnchor.UpperLeft)
		{
			this.text = text;
			this.anchor = anchor;
		}

		// Token: 0x06005A0F RID: 23055 RVA: 0x002E3F54 File Offset: 0x002E2354
		public override float DrawHeight(float width)
		{
			return Text.CalcHeight(this.text, width);
		}

		// Token: 0x06005A10 RID: 23056 RVA: 0x002E3F75 File Offset: 0x002E2375
		public override void Draw(Rect r)
		{
			Text.Anchor = this.anchor;
			Widgets.Label(r, this.text);
			Text.Anchor = TextAnchor.UpperLeft;
		}
	}
}
