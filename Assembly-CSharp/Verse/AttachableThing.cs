using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DC8 RID: 3528
	public abstract class AttachableThing : Thing
	{
		// Token: 0x17000CB7 RID: 3255
		// (get) Token: 0x06004EA7 RID: 20135 RVA: 0x001402E4 File Offset: 0x0013E6E4
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
		// (get) Token: 0x06004EA8 RID: 20136
		public abstract string InspectStringAddon { get; }

		// Token: 0x06004EA9 RID: 20137 RVA: 0x00140339 File Offset: 0x0013E739
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

		// Token: 0x06004EAA RID: 20138 RVA: 0x00140378 File Offset: 0x0013E778
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

		// Token: 0x06004EAB RID: 20139 RVA: 0x001403D8 File Offset: 0x0013E7D8
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			base.Destroy(mode);
			if (this.parent != null)
			{
				this.parent.TryGetComp<CompAttachBase>().RemoveAttachment(this);
			}
		}

		// Token: 0x04003462 RID: 13410
		public Thing parent;
	}
}
