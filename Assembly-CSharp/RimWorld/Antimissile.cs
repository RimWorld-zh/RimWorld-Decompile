using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Antimissile : Projectile
	{
		public override Vector3 ExactPosition
		{
			get
			{
				Vector3 b = (((Projectile)base.assignedTarget).ExactPosition - base.origin) * (float)(1.0 - (float)base.ticksToImpact / (float)base.StartingTicksToImpact);
				return base.origin + b + Vector3.up * base.def.Altitude;
			}
		}

		public override Quaternion ExactRotation
		{
			get
			{
				return Quaternion.LookRotation(((Projectile)base.assignedTarget).ExactPosition - this.ExactPosition);
			}
		}

		protected override void Impact(Thing hitThing)
		{
			base.Impact(hitThing);
			hitThing.Destroy(DestroyMode.Vanish);
		}
	}
}
