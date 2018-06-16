using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000977 RID: 2423
	public abstract class SubEffecter_DrifterEmote : SubEffecter
	{
		// Token: 0x06003680 RID: 13952 RVA: 0x001D0DF9 File Offset: 0x001CF1F9
		public SubEffecter_DrifterEmote(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06003681 RID: 13953 RVA: 0x001D0E04 File Offset: 0x001CF204
		protected void MakeMote(TargetInfo A)
		{
			Vector3 vector = (!A.HasThing) ? A.Cell.ToVector3Shifted() : A.Thing.DrawPos;
			if (vector.ShouldSpawnMotesAt(A.Map))
			{
				int randomInRange = this.def.burstCount.RandomInRange;
				for (int i = 0; i < randomInRange; i++)
				{
					Mote mote = (Mote)ThingMaker.MakeThing(this.def.moteDef, null);
					mote.Scale = this.def.scale.RandomInRange;
					mote.exactPosition = vector + Gen.RandomHorizontalVector(this.def.positionRadius);
					mote.rotationRate = this.def.rotationRate.RandomInRange;
					mote.exactRotation = this.def.rotation.RandomInRange;
					MoteThrown moteThrown = mote as MoteThrown;
					if (moteThrown != null)
					{
						moteThrown.airTimeLeft = this.def.airTime.RandomInRange;
						moteThrown.SetVelocity(this.def.angle.RandomInRange, this.def.speed.RandomInRange);
					}
					if (A.HasThing)
					{
						mote.Attach(A);
					}
					GenSpawn.Spawn(mote, vector.ToIntVec3(), A.Map, WipeMode.Vanish);
				}
			}
		}
	}
}
