using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000E87 RID: 3719
	public class TreeNode
	{
		// Token: 0x04003A04 RID: 14852
		public TreeNode parentNode;

		// Token: 0x04003A05 RID: 14853
		public List<TreeNode> children;

		// Token: 0x04003A06 RID: 14854
		public int nestDepth;

		// Token: 0x04003A07 RID: 14855
		private int openBits;

		// Token: 0x060057C4 RID: 22468 RVA: 0x002CE840 File Offset: 0x002CCC40
		public bool IsOpen(int mask)
		{
			return (this.openBits & mask) != 0;
		}

		// Token: 0x060057C5 RID: 22469 RVA: 0x002CE863 File Offset: 0x002CCC63
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
		// (get) Token: 0x060057C6 RID: 22470 RVA: 0x002CE890 File Offset: 0x002CCC90
		public virtual bool Openable
		{
			get
			{
				return true;
			}
		}
	}
}
