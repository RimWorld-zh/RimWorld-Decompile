using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F5D RID: 3933
	public class KeyBindingData
	{
		// Token: 0x04003E71 RID: 15985
		public KeyCode keyBindingA;

		// Token: 0x04003E72 RID: 15986
		public KeyCode keyBindingB;

		// Token: 0x06005F39 RID: 24377 RVA: 0x00308C6F File Offset: 0x0030706F
		public KeyBindingData()
		{
		}

		// Token: 0x06005F3A RID: 24378 RVA: 0x00308C78 File Offset: 0x00307078
		public KeyBindingData(KeyCode keyBindingA, KeyCode keyBindingB)
		{
			this.keyBindingA = keyBindingA;
			this.keyBindingB = keyBindingB;
		}

		// Token: 0x06005F3B RID: 24379 RVA: 0x00308C90 File Offset: 0x00307090
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
	}
}
