using System;

namespace Verse
{
	// Token: 0x02000DE8 RID: 3560
	public class MoteDualAttached : Mote
	{
		// Token: 0x040034E4 RID: 13540
		protected MoteAttachLink link2 = MoteAttachLink.Invalid;

		// Token: 0x06004FD4 RID: 20436 RVA: 0x0014337B File Offset: 0x0014177B
		public void Attach(TargetInfo a, TargetInfo b)
		{
			this.link1 = new MoteAttachLink(a);
			this.link2 = new MoteAttachLink(b);
		}

		// Token: 0x06004FD5 RID: 20437 RVA: 0x00143396 File Offset: 0x00141796
		public override void Draw()
		{
			this.UpdatePositionAndRotation();
			base.Draw();
		}

		// Token: 0x06004FD6 RID: 20438 RVA: 0x001433A8 File Offset: 0x001417A8
		protected void UpdatePositionAndRotation()
		{
			if (this.link1.Linked)
			{
				if (this.link2.Linked)
				{
					if (!this.link1.Target.ThingDestroyed)
					{
						this.link1.UpdateDrawPos();
					}
					if (!this.link2.Target.ThingDestroyed)
					{
						this.link2.UpdateDrawPos();
					}
					this.exactPosition = (this.link1.LastDrawPos + this.link2.LastDrawPos) * 0.5f;
					if (this.def.mote.rotateTowardsTarget)
					{
						this.exactRotation = this.link1.LastDrawPos.AngleToFlat(this.link2.LastDrawPos) + 90f;
					}
				}
				else
				{
					if (!this.link1.Target.ThingDestroyed)
					{
						this.link1.UpdateDrawPos();
					}
					this.exactPosition = this.link1.LastDrawPos + this.def.mote.attachedDrawOffset;
				}
			}
			this.exactPosition.y = this.def.altitudeLayer.AltitudeFor();
		}
	}
}
