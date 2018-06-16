using System;

namespace Verse
{
	// Token: 0x02000DEC RID: 3564
	public class MoteDualAttached : Mote
	{
		// Token: 0x06004FC1 RID: 20417 RVA: 0x001431B7 File Offset: 0x001415B7
		public void Attach(TargetInfo a, TargetInfo b)
		{
			this.link1 = new MoteAttachLink(a);
			this.link2 = new MoteAttachLink(b);
		}

		// Token: 0x06004FC2 RID: 20418 RVA: 0x001431D2 File Offset: 0x001415D2
		public override void Draw()
		{
			this.UpdatePositionAndRotation();
			base.Draw();
		}

		// Token: 0x06004FC3 RID: 20419 RVA: 0x001431E4 File Offset: 0x001415E4
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

		// Token: 0x040034DB RID: 13531
		protected MoteAttachLink link2 = MoteAttachLink.Invalid;
	}
}
