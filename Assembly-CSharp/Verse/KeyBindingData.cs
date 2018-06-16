using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F59 RID: 3929
	public class KeyBindingData
	{
		// Token: 0x06005F08 RID: 24328 RVA: 0x0030622B File Offset: 0x0030462B
		public KeyBindingData()
		{
		}

		// Token: 0x06005F09 RID: 24329 RVA: 0x00306234 File Offset: 0x00304634
		public KeyBindingData(KeyCode keyBindingA, KeyCode keyBindingB)
		{
			this.keyBindingA = keyBindingA;
			this.keyBindingB = keyBindingB;
		}

		// Token: 0x06005F0A RID: 24330 RVA: 0x0030624C File Offset: 0x0030464C
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

		// Token: 0x04003E55 RID: 15957
		public KeyCode keyBindingA;

		// Token: 0x04003E56 RID: 15958
		public KeyCode keyBindingB;
	}
}
