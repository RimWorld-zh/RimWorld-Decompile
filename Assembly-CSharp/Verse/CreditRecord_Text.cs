using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000ED9 RID: 3801
	public class CreditRecord_Text : CreditsEntry
	{
		// Token: 0x06005A0A RID: 23050 RVA: 0x002E3BF2 File Offset: 0x002E1FF2
		public CreditRecord_Text()
		{
		}

		// Token: 0x06005A0B RID: 23051 RVA: 0x002E3BFB File Offset: 0x002E1FFB
		public CreditRecord_Text(string text, TextAnchor anchor = TextAnchor.UpperLeft)
		{
			this.text = text;
			this.anchor = anchor;
		}

		// Token: 0x06005A0C RID: 23052 RVA: 0x002E3C14 File Offset: 0x002E2014
		public override float DrawHeight(float width)
		{
			return Text.CalcHeight(this.text, width);
		}

		// Token: 0x06005A0D RID: 23053 RVA: 0x002E3C35 File Offset: 0x002E2035
		public override void Draw(Rect r)
		{
			Text.Anchor = this.anchor;
			Widgets.Label(r, this.text);
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x04003C6C RID: 15468
		public string text;

		// Token: 0x04003C6D RID: 15469
		public TextAnchor anchor;
	}
}
