using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F58 RID: 3928
	public class KeyBindingData
	{
		// Token: 0x06005F06 RID: 24326 RVA: 0x00306307 File Offset: 0x00304707
		public KeyBindingData()
		{
		}

		// Token: 0x06005F07 RID: 24327 RVA: 0x00306310 File Offset: 0x00304710
		public KeyBindingData(KeyCode keyBindingA, KeyCode keyBindingB)
		{
			this.keyBindingA = keyBindingA;
			this.keyBindingB = keyBindingB;
		}

		// Token: 0x06005F08 RID: 24328 RVA: 0x00306328 File Offset: 0x00304728
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

		// Token: 0x04003E54 RID: 15956
		public KeyCode keyBindingA;

		// Token: 0x04003E55 RID: 15957
		public KeyCode keyBindingB;
	}
}
