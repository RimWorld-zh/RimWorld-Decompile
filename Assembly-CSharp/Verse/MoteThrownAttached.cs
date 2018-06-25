using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DF0 RID: 3568
	internal class MoteThrownAttached : MoteThrown
	{
		// Token: 0x040034FF RID: 13567
		private Vector3 attacheeLastPosition = new Vector3(-1000f, -1000f, -1000f);

		// Token: 0x06004FFD RID: 20477 RVA: 0x002981D8 File Offset: 0x002965D8
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (this.link1.Linked)
			{
				this.attacheeLastPosition = this.link1.LastDrawPos;
			}
			this.exactPosition += this.def.mote.attachedDrawOffset;
		}

		// Token: 0x06004FFE RID: 20478 RVA: 0x00298230 File Offset: 0x00296630
		protected override Vector3 NextExactPosition(float deltaTime)
		{
			Vector3 vector = base.NextExactPosition(deltaTime);
			if (this.link1.Linked)
			{
				if (!this.link1.Target.ThingDestroyed)
				{
					this.link1.UpdateDrawPos();
				}
				Vector3 b = this.link1.LastDrawPos - this.attacheeLastPosition;
				vector += b;
				vector.y = AltitudeLayer.MoteOverhead.AltitudeFor();
				this.attacheeLastPosition = this.link1.LastDrawPos;
			}
			return vector;
		}
	}
}
