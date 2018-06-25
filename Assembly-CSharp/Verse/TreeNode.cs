using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000E88 RID: 3720
	public class TreeNode
	{
		// Token: 0x04003A0C RID: 14860
		public TreeNode parentNode;

		// Token: 0x04003A0D RID: 14861
		public List<TreeNode> children;

		// Token: 0x04003A0E RID: 14862
		public int nestDepth;

		// Token: 0x04003A0F RID: 14863
		private int openBits;

		// Token: 0x060057C4 RID: 22468 RVA: 0x002CEA2C File Offset: 0x002CCE2C
		public bool IsOpen(int mask)
		{
			return (this.openBits & mask) != 0;
		}

		// Token: 0x060057C5 RID: 22469 RVA: 0x002CEA4F File Offset: 0x002CCE4F
		public void SetOpen(int mask, bool val)
		{
			if (val)
			{
				this.openBits |= mask;
			}
			else
			{
				this.openBits &= ~mask;
			}
		}

		// Token: 0x17000DD9 RID: 3545
		// (get) Token: 0x060057C6 RID: 22470 RVA: 0x002CEA7C File Offset: 0x002CCE7C
		public virtual bool Openable
		{
			get
			{
				return true;
			}
		}
	}
}
