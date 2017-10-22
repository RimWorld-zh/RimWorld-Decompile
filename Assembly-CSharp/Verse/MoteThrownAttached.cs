using UnityEngine;

namespace Verse
{
	internal class MoteThrownAttached : MoteThrown
	{
		private Vector3 attacheeLastPosition = new Vector3(-1000f, -1000f, -1000f);

		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (base.link1.Linked)
			{
				this.attacheeLastPosition = base.link1.LastDrawPos;
			}
			base.exactPosition += base.def.mote.attachedDrawOffset;
		}

		protected override Vector3 NextExactPosition(float deltaTime)
		{
			Vector3 vector = base.NextExactPosition(deltaTime);
			if (base.link1.Linked)
			{
				if (!base.link1.Target.ThingDestroyed)
				{
					base.link1.UpdateDrawPos();
				}
				Vector3 b = base.link1.LastDrawPos - this.attacheeLastPosition;
				vector += b;
				vector.y = Altitudes.AltitudeFor(AltitudeLayer.MoteOverhead);
				this.attacheeLastPosition = base.link1.LastDrawPos;
			}
			return vector;
		}
	}
}
