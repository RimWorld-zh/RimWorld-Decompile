using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000E86 RID: 3718
	public class TreeNode
	{
		// Token: 0x060057A0 RID: 22432 RVA: 0x002CCB04 File Offset: 0x002CAF04
		public bool IsOpen(int mask)
		{
			return (this.openBits & mask) != 0;
		}

		// Token: 0x060057A1 RID: 22433 RVA: 0x002CCB27 File Offset: 0x002CAF27
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

		// Token: 0x17000DD7 RID: 3543
		// (get) Token: 0x060057A2 RID: 22434 RVA: 0x002CCB54 File Offset: 0x002CAF54
		public virtual bool Openable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x040039F4 RID: 14836
		public TreeNode parentNode;

		// Token: 0x040039F5 RID: 14837
		public List<TreeNode> children;

		// Token: 0x040039F6 RID: 14838
		public int nestDepth;

		// Token: 0x040039F7 RID: 14839
		private int openBits;
	}
}
