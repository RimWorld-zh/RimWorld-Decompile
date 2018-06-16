using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000E87 RID: 3719
	public class TreeNode
	{
		// Token: 0x060057A2 RID: 22434 RVA: 0x002CCB04 File Offset: 0x002CAF04
		public bool IsOpen(int mask)
		{
			return (this.openBits & mask) != 0;
		}

		// Token: 0x060057A3 RID: 22435 RVA: 0x002CCB27 File Offset: 0x002CAF27
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

		// Token: 0x17000DD8 RID: 3544
		// (get) Token: 0x060057A4 RID: 22436 RVA: 0x002CCB54 File Offset: 0x002CAF54
		public virtual bool Openable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x040039F6 RID: 14838
		public TreeNode parentNode;

		// Token: 0x040039F7 RID: 14839
		public List<TreeNode> children;

		// Token: 0x040039F8 RID: 14840
		public int nestDepth;

		// Token: 0x040039F9 RID: 14841
		private int openBits;
	}
}
