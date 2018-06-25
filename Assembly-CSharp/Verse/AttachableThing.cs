using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DC7 RID: 3527
	public abstract class AttachableThing : Thing
	{
		// Token: 0x04003472 RID: 13426
		public Thing parent;

		// Token: 0x17000CB7 RID: 3255
		// (get) Token: 0x06004EBE RID: 20158 RVA: 0x00140858 File Offset: 0x0013EC58
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

		// Token: 0x17000CB8 RID: 3256
		// (get) Token: 0x06004EBF RID: 20159
		public abstract string InspectStringAddon { get; }

		// Token: 0x06004EC0 RID: 20160 RVA: 0x001408AD File Offset: 0x0013ECAD
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

		// Token: 0x06004EC1 RID: 20161 RVA: 0x001408EC File Offset: 0x0013ECEC
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

		// Token: 0x06004EC2 RID: 20162 RVA: 0x0014094C File Offset: 0x0013ED4C
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			base.Destroy(mode);
			if (this.parent != null)
			{
				this.parent.TryGetComp<CompAttachBase>().RemoveAttachment(this);
			}
		}
	}
}
