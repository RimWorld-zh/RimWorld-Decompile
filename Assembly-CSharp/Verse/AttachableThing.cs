using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DC4 RID: 3524
	public abstract class AttachableThing : Thing
	{
		// Token: 0x17000CB8 RID: 3256
		// (get) Token: 0x06004EBA RID: 20154 RVA: 0x001404A8 File Offset: 0x0013E8A8
		public override Vector3 DrawPos
		{
			get
			{
				Vector3 result;
				if (this.parent != null)
				{
					result = this.parent.DrawPos + Vector3.up * 0.046875f * 0.9f;
				}
				else
				{
					result = base.DrawPos;
				}
				return result;
			}
		}

		// Token: 0x17000CB9 RID: 3257
		// (get) Token: 0x06004EBB RID: 20155
		public abstract string InspectStringAddon { get; }

		// Token: 0x06004EBC RID: 20156 RVA: 0x001404FD File Offset: 0x0013E8FD
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.parent, "parent", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.parent != null)
				{
					this.AttachTo(this.parent);
				}
			}
		}

		// Token: 0x06004EBD RID: 20157 RVA: 0x0014053C File Offset: 0x0013E93C
		public virtual void AttachTo(Thing parent)
		{
			this.parent = parent;
			CompAttachBase compAttachBase = parent.TryGetComp<CompAttachBase>();
			if (compAttachBase == null)
			{
				Log.Error(string.Concat(new object[]
				{
					"Cannot attach ",
					this,
					" to ",
					parent,
					": parent has no CompAttachBase."
				}), false);
			}
			else
			{
				compAttachBase.AddAttachment(this);
			}
		}

		// Token: 0x06004EBE RID: 20158 RVA: 0x0014059C File Offset: 0x0013E99C
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			base.Destroy(mode);
			if (this.parent != null)
			{
				this.parent.TryGetComp<CompAttachBase>().RemoveAttachment(this);
			}
		}

		// Token: 0x0400346B RID: 13419
		public Thing parent;
	}
}
