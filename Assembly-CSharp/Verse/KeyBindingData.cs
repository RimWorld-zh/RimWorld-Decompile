using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F58 RID: 3928
	public class KeyBindingData
	{
		// Token: 0x06005F2F RID: 24367 RVA: 0x003083AB File Offset: 0x003067AB
		public KeyBindingData()
		{
		}

		// Token: 0x06005F30 RID: 24368 RVA: 0x003083B4 File Offset: 0x003067B4
		public KeyBindingData(KeyCode keyBindingA, KeyCode keyBindingB)
		{
			this.keyBindingA = keyBindingA;
			this.keyBindingB = keyBindingB;
		}

		// Token: 0x06005F31 RID: 24369 RVA: 0x003083CC File Offset: 0x003067CC
		public override string ToString()
		{
			string str = "[";
			if (this.keyBindingA != KeyCode.None)
			{
				str += this.keyBindingA.ToString();
			}
			if (this.keyBindingB != KeyCode.None)
			{
				str = str + ", " + this.keyBindingB.ToString();
			}
			return str + "]";
		}

		// Token: 0x04003E66 RID: 15974
		public KeyCode keyBindingA;

		// Token: 0x04003E67 RID: 15975
		public KeyCode keyBindingB;
	}
}
