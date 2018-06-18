using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EDA RID: 3802
	public class CreditRecord_Text : CreditsEntry
	{
		// Token: 0x060059E9 RID: 23017 RVA: 0x002E1DDE File Offset: 0x002E01DE
		public CreditRecord_Text()
		{
		}

		// Token: 0x060059EA RID: 23018 RVA: 0x002E1DE7 File Offset: 0x002E01E7
		public CreditRecord_Text(string text, TextAnchor anchor = TextAnchor.UpperLeft)
		{
			this.text = text;
			this.anchor = anchor;
		}

		// Token: 0x060059EB RID: 23019 RVA: 0x002E1E00 File Offset: 0x002E0200
		public override float DrawHeight(float width)
		{
			return Text.CalcHeight(this.text, width);
		}

		// Token: 0x060059EC RID: 23020 RVA: 0x002E1E21 File Offset: 0x002E0221
		public override void Draw(Rect r)
		{
			Text.Anchor = this.anchor;
			Widgets.Label(r, this.text);
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x04003C5C RID: 15452
		public string text;

		// Token: 0x04003C5D RID: 15453
		public TextAnchor anchor;
	}
}
