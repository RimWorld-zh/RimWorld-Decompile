using System;

namespace Verse
{
	// Token: 0x02000DEB RID: 3563
	public class MoteDualAttached : Mote
	{
		// Token: 0x06004FBF RID: 20415 RVA: 0x0014322F File Offset: 0x0014162F
		public void Attach(TargetInfo a, TargetInfo b)
		{
			this.link1 = new MoteAttachLink(a);
			this.link2 = new MoteAttachLink(b);
		}

		// Token: 0x06004FC0 RID: 20416 RVA: 0x0014324A File Offset: 0x0014164A
		public override void Draw()
		{
			this.UpdatePositionAndRotation();
			base.Draw();
		}

		// Token: 0x06004FC1 RID: 20417 RVA: 0x0014325C File Offset: 0x0014165C
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

		// Token: 0x040034D9 RID: 13529
		protected MoteAttachLink link2 = MoteAttachLink.Invalid;
	}
}
