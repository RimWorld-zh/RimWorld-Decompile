using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DE7 RID: 3559
	public struct MoteAttachLink
	{
		// Token: 0x040034E2 RID: 13538
		private TargetInfo targetInt;

		// Token: 0x040034E3 RID: 13539
		private Vector3 lastDrawPosInt;

		// Token: 0x06004FCD RID: 20429 RVA: 0x002972C3 File Offset: 0x002956C3
		public MoteAttachLink(TargetInfo target)
		{
			this.targetInt = target;
			this.lastDrawPosInt = Vector3.zero;
			if (target.IsValid)
			{
				this.UpdateDrawPos();
			}
		}

		// Token: 0x17000CF2 RID: 3314
		// (get) Token: 0x06004FCE RID: 20430 RVA: 0x002972EC File Offset: 0x002956EC
		public bool Linked
		{
			get
			{
				return this.targetInt.IsValid;
			}
		}

		// Token: 0x17000CF3 RID: 3315
		// (get) Token: 0x06004FCF RID: 20431 RVA: 0x0029730C File Offset: 0x0029570C
		public TargetInfo Target
		{
			get
			{
				return this.targetInt;
			}
		}

		// Token: 0x17000CF4 RID: 3316
		// (get) Token: 0x06004FD0 RID: 20432 RVA: 0x00297328 File Offset: 0x00295728
		public Vector3 LastDrawPos
		{
			get
			{
				return this.lastDrawPosInt;
			}
		}

		// Token: 0x17000CF5 RID: 3317
		// (get) Token: 0x06004FD1 RID: 20433 RVA: 0x00297344 File Offset: 0x00295744
		public static MoteAttachLink Invalid
		{
			get
			{
				return new MoteAttachLink(TargetInfo.Invalid);
			}
		}

		// Token: 0x06004FD2 RID: 20434 RVA: 0x00297364 File Offset: 0x00295764
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
