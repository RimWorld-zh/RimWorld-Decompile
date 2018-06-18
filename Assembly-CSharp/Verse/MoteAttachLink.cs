using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DEA RID: 3562
	public struct MoteAttachLink
	{
		// Token: 0x06004FB8 RID: 20408 RVA: 0x00295CE7 File Offset: 0x002940E7
		public MoteAttachLink(TargetInfo target)
		{
			this.targetInt = target;
			this.lastDrawPosInt = Vector3.zero;
			if (target.IsValid)
			{
				this.UpdateDrawPos();
			}
		}

		// Token: 0x17000CF0 RID: 3312
		// (get) Token: 0x06004FB9 RID: 20409 RVA: 0x00295D10 File Offset: 0x00294110
		public bool Linked
		{
			get
			{
				return this.targetInt.IsValid;
			}
		}

		// Token: 0x17000CF1 RID: 3313
		// (get) Token: 0x06004FBA RID: 20410 RVA: 0x00295D30 File Offset: 0x00294130
		public TargetInfo Target
		{
			get
			{
				return this.targetInt;
			}
		}

		// Token: 0x17000CF2 RID: 3314
		// (get) Token: 0x06004FBB RID: 20411 RVA: 0x00295D4C File Offset: 0x0029414C
		public Vector3 LastDrawPos
		{
			get
			{
				return this.lastDrawPosInt;
			}
		}

		// Token: 0x17000CF3 RID: 3315
		// (get) Token: 0x06004FBC RID: 20412 RVA: 0x00295D68 File Offset: 0x00294168
		public static MoteAttachLink Invalid
		{
			get
			{
				return new MoteAttachLink(TargetInfo.Invalid);
			}
		}

		// Token: 0x06004FBD RID: 20413 RVA: 0x00295D88 File Offset: 0x00294188
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

		// Token: 0x040034D7 RID: 13527
		private TargetInfo targetInt;

		// Token: 0x040034D8 RID: 13528
		private Vector3 lastDrawPosInt;
	}
}
