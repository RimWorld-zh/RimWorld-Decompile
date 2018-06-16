using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EDB RID: 3803
	public class CreditRecord_Text : CreditsEntry
	{
		// Token: 0x060059EB RID: 23019 RVA: 0x002E1D06 File Offset: 0x002E0106
		public CreditRecord_Text()
		{
		}

		// Token: 0x060059EC RID: 23020 RVA: 0x002E1D0F File Offset: 0x002E010F
		public CreditRecord_Text(string text, TextAnchor anchor = TextAnchor.UpperLeft)
		{
			this.text = text;
			this.anchor = anchor;
		}

		// Token: 0x060059ED RID: 23021 RVA: 0x002E1D28 File Offset: 0x002E0128
		public override float DrawHeight(float width)
		{
			return Text.CalcHeight(this.text, width);
		}

		// Token: 0x060059EE RID: 23022 RVA: 0x002E1D49 File Offset: 0x002E0149
		public override void Draw(Rect r)
		{
			Text.Anchor = this.anchor;
			Widgets.Label(r, this.text);
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x04003C5D RID: 15453
		public string text;

		// Token: 0x04003C5E RID: 15454
		public TextAnchor anchor;
	}
}
