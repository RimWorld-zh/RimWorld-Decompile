using System;
using UnityEngine;

namespace Verse
{
	public abstract class AttachableThing : Thing
	{
		public Thing parent;

		protected AttachableThing()
		{
		}

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

		public abstract string InspectStringAddon { get; }

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
