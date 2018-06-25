using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DE9 RID: 3561
	public struct MoteAttachLink
	{
		// Token: 0x040034E2 RID: 13538
		private TargetInfo targetInt;

		// Token: 0x040034E3 RID: 13539
		private Vector3 lastDrawPosInt;

		// Token: 0x06004FD1 RID: 20433 RVA: 0x002973EF File Offset: 0x002957EF
		public MoteAttachLink(TargetInfo target)
		{
			this.targetInt = target;
			this.lastDrawPosInt = Vector3.zero;
			if (target.IsValid)
			{
				this.UpdateDrawPos();
			}
		}

		// Token: 0x17000CF1 RID: 3313
		// (get) Token: 0x06004FD2 RID: 20434 RVA: 0x00297418 File Offset: 0x00295818
		public bool Linked
		{
			get
			{
				return this.targetInt.IsValid;
			}
		}

		// Token: 0x17000CF2 RID: 3314
		// (get) Token: 0x06004FD3 RID: 20435 RVA: 0x00297438 File Offset: 0x00295838
		public TargetInfo Target
		{
			get
			{
				return this.targetInt;
			}
		}

		// Token: 0x17000CF3 RID: 3315
		// (get) Token: 0x06004FD4 RID: 20436 RVA: 0x00297454 File Offset: 0x00295854
		public Vector3 LastDrawPos
		{
			get
			{
				return this.lastDrawPosInt;
			}
		}

		// Token: 0x17000CF4 RID: 3316
		// (get) Token: 0x06004FD5 RID: 20437 RVA: 0x00297470 File Offset: 0x00295870
		public static MoteAttachLink Invalid
		{
			get
			{
				return new MoteAttachLink(TargetInfo.Invalid);
			}
		}

		// Token: 0x06004FD6 RID: 20438 RVA: 0x00297490 File Offset: 0x00295890
		public void UpdateDrawPos()
		{
			if (this.targetInt.HasThing)
			{
				this.lastDrawPosInt = this.targetInt.Thing.DrawPos;
			}
			else
			{
				this.lastDrawPosInt = this.targetInt.Cell.ToVector3Shifted();
			}
		}
	}
}
