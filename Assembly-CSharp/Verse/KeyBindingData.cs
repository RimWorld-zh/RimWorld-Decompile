using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F5C RID: 3932
	public class KeyBindingData
	{
		// Token: 0x04003E69 RID: 15977
		public KeyCode keyBindingA;

		// Token: 0x04003E6A RID: 15978
		public KeyCode keyBindingB;

		// Token: 0x06005F39 RID: 24377 RVA: 0x00308A2B File Offset: 0x00306E2B
		public KeyBindingData()
		{
		}

		// Token: 0x06005F3A RID: 24378 RVA: 0x00308A34 File Offset: 0x00306E34
		public KeyBindingData(KeyCode keyBindingA, KeyCode keyBindingB)
		{
			this.keyBindingA = keyBindingA;
			this.keyBindingB = keyBindingB;
		}

		// Token: 0x06005F3B RID: 24379 RVA: 0x00308A4C File Offset: 0x00306E4C
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
