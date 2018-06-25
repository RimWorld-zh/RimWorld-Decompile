using System;

namespace Verse
{
	// Token: 0x02000DEA RID: 3562
	public class MoteDualAttached : Mote
	{
		// Token: 0x040034E4 RID: 13540
		protected MoteAttachLink link2 = MoteAttachLink.Invalid;

		// Token: 0x06004FD8 RID: 20440 RVA: 0x001434CB File Offset: 0x001418CB
		public void Attach(TargetInfo a, TargetInfo b)
		{
			this.link1 = new MoteAttachLink(a);
			this.link2 = new MoteAttachLink(b);
		}

		// Token: 0x06004FD9 RID: 20441 RVA: 0x001434E6 File Offset: 0x001418E6
		public override void Draw()
		{
			this.UpdatePositionAndRotation();
			base.Draw();
		}

		// Token: 0x06004FDA RID: 20442 RVA: 0x001434F8 File Offset: 0x001418F8
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
