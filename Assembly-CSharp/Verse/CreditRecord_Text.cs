using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EDB RID: 3803
	public class CreditRecord_Text : CreditsEntry
	{
		// Token: 0x04003C6C RID: 15468
		public string text;

		// Token: 0x04003C6D RID: 15469
		public TextAnchor anchor;

		// Token: 0x06005A0D RID: 23053 RVA: 0x002E3D12 File Offset: 0x002E2112
		public CreditRecord_Text()
		{
		}

		// Token: 0x06005A0E RID: 23054 RVA: 0x002E3D1B File Offset: 0x002E211B
		public CreditRecord_Text(string text, TextAnchor anchor = TextAnchor.UpperLeft)
		{
			this.text = text;
			this.anchor = anchor;
		}

		// Token: 0x06005A0F RID: 23055 RVA: 0x002E3D34 File Offset: 0x002E2134
		public override float DrawHeight(float width)
		{
			return Text.CalcHeight(this.text, width);
		}

		// Token: 0x06005A10 RID: 23056 RVA: 0x002E3D55 File Offset: 0x002E2155
		public override void Draw(Rect r)
		{
			Text.Anchor = this.anchor;
			Widgets.Label(r, this.text);
			Text.Anchor = TextAnchor.UpperLeft;
		}
	}
}
