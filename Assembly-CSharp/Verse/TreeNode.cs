using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000E85 RID: 3717
	public class TreeNode
	{
		// Token: 0x060057C0 RID: 22464 RVA: 0x002CE714 File Offset: 0x002CCB14
		public bool IsOpen(int mask)
		{
			return (this.openBits & mask) != 0;
		}

		// Token: 0x060057C1 RID: 22465 RVA: 0x002CE737 File Offset: 0x002CCB37
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

		// Token: 0x17000DDA RID: 3546
		// (get) Token: 0x060057C2 RID: 22466 RVA: 0x002CE764 File Offset: 0x002CCB64
		public virtual bool Openable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x04003A04 RID: 14852
		public TreeNode parentNode;

		// Token: 0x04003A05 RID: 14853
		public List<TreeNode> children;

		// Token: 0x04003A06 RID: 14854
		public int nestDepth;

		// Token: 0x04003A07 RID: 14855
		private int openBits;
	}
}
