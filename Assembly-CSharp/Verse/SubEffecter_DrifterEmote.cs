using UnityEngine;

namespace Verse
{
	public abstract class SubEffecter_DrifterEmote : SubEffecter
	{
		public SubEffecter_DrifterEmote(SubEffecterDef def)
			: base(def)
		{
		}

		protected void MakeMote(TargetInfo A)
		{
			Vector3 vector = (!A.HasThing) ? A.Cell.ToVector3Shifted() : A.Thing.DrawPos;
			if (vector.ShouldSpawnMotesAt(A.Map))
			{
				int randomInRange = base.def.burstCount.RandomInRange;
				for (int i = 0; i < randomInRange; i++)
				{
					Mote mote = (Mote)ThingMaker.MakeThing(base.def.moteDef, null);
					mote.Scale = base.def.scale.RandomInRange;
					mote.exactPosition = vector + Gen.RandomHorizontalVector(base.def.positionRadius);
					mote.rotationRate = base.def.rotationRate.RandomInRange;
					mote.exactRotation = base.def.rotation.RandomInRange;
					MoteThrown moteThrown = mote as MoteThrown;
					if (moteThrown != null)
					{
						moteThrown.airTimeLeft = base.def.airTime.RandomInRange;
						moteThrown.SetVelocity(base.def.angle.RandomInRange, base.def.speed.RandomInRange);
					}
					if (A.HasThing)
					{
						mote.Attach(A);
					}
					GenSpawn.Spawn(mote, vector.ToIntVec3(), A.Map);
				}
			}
		}
	}
}
