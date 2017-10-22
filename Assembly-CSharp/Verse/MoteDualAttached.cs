namespace Verse
{
	public class MoteDualAttached : Mote
	{
		protected MoteAttachLink link2 = MoteAttachLink.Invalid;

		public void Attach(TargetInfo a, TargetInfo b)
		{
			base.link1 = new MoteAttachLink(a);
			this.link2 = new MoteAttachLink(b);
		}

		public override void Draw()
		{
			this.UpdatePositionAndRotation();
			base.Draw();
		}

		protected void UpdatePositionAndRotation()
		{
			if (base.link1.Linked)
			{
				if (this.link2.Linked)
				{
					if (!base.link1.Target.ThingDestroyed)
					{
						base.link1.UpdateDrawPos();
					}
					if (!this.link2.Target.ThingDestroyed)
					{
						this.link2.UpdateDrawPos();
					}
					base.exactPosition = (base.link1.LastDrawPos + this.link2.LastDrawPos) * 0.5f;
					if (base.def.mote.rotateTowardsTarget)
					{
						base.exactRotation = (float)(base.link1.LastDrawPos.AngleToFlat(this.link2.LastDrawPos) + 90.0);
					}
				}
				else
				{
					if (!base.link1.Target.ThingDestroyed)
					{
						base.link1.UpdateDrawPos();
					}
					base.exactPosition = base.link1.LastDrawPos + base.def.mote.attachedDrawOffset;
				}
			}
			base.exactPosition.y = Altitudes.AltitudeFor(base.def.altitudeLayer);
		}
	}
}
