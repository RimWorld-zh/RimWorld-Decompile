using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DF1 RID: 3569
	internal class MoteThrownAttached : MoteThrown
	{
		// Token: 0x06004FE6 RID: 20454 RVA: 0x00296810 File Offset: 0x00294C10
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (this.link1.Linked)
			{
				this.attacheeLastPosition = this.link1.LastDrawPos;
			}
			this.exactPosition += this.def.mote.attachedDrawOffset;
		}

		// Token: 0x06004FE7 RID: 20455 RVA: 0x00296868 File Offset: 0x00294C68
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

		// Token: 0x040034EF RID: 13551
		private Vector3 attacheeLastPosition = new Vector3(-1000f, -1000f, -1000f);
	}
}
