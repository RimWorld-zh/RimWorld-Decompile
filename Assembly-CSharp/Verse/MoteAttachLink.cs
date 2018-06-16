using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DEB RID: 3563
	public struct MoteAttachLink
	{
		// Token: 0x06004FBA RID: 20410 RVA: 0x00295D07 File Offset: 0x00294107
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
		// (get) Token: 0x06004FBB RID: 20411 RVA: 0x00295D30 File Offset: 0x00294130
		public bool Linked
		{
			get
			{
				return this.targetInt.IsValid;
			}
		}

		// Token: 0x17000CF2 RID: 3314
		// (get) Token: 0x06004FBC RID: 20412 RVA: 0x00295D50 File Offset: 0x00294150
		public TargetInfo Target
		{
			get
			{
				return this.targetInt;
			}
		}

		// Token: 0x17000CF3 RID: 3315
		// (get) Token: 0x06004FBD RID: 20413 RVA: 0x00295D6C File Offset: 0x0029416C
		public Vector3 LastDrawPos
		{
			get
			{
				return this.lastDrawPosInt;
			}
		}

		// Token: 0x17000CF4 RID: 3316
		// (get) Token: 0x06004FBE RID: 20414 RVA: 0x00295D88 File Offset: 0x00294188
		public static MoteAttachLink Invalid
		{
			get
			{
				return new MoteAttachLink(TargetInfo.Invalid);
			}
		}

		// Token: 0x06004FBF RID: 20415 RVA: 0x00295DA8 File Offset: 0x002941A8
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

		// Token: 0x040034D9 RID: 13529
		private TargetInfo targetInt;

		// Token: 0x040034DA RID: 13530
		private Vector3 lastDrawPosInt;
	}
}
