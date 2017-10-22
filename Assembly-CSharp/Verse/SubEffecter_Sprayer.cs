using UnityEngine;

namespace Verse
{
	public abstract class SubEffecter_Sprayer : SubEffecter
	{
		public SubEffecter_Sprayer(SubEffecterDef def) : base(def)
		{
		}

		protected void MakeMote(TargetInfo A, TargetInfo B)
		{
			Vector3 vector = Vector3.zero;
			switch (base.def.spawnLocType)
			{
			case MoteSpawnLocType.OnSource:
			{
				vector = A.Cell.ToVector3Shifted();
				break;
			}
			case MoteSpawnLocType.BetweenPositions:
			{
				Vector3 vector2 = (!A.HasThing) ? A.Cell.ToVector3Shifted() : A.Thing.DrawPos;
				Vector3 vector3 = (!B.HasThing) ? B.Cell.ToVector3Shifted() : B.Thing.DrawPos;
				vector = ((!A.HasThing || A.Thing.Spawned) ? ((!B.HasThing || B.Thing.Spawned) ? (vector2 * base.def.positionLerpFactor + vector3 * (float)(1.0 - base.def.positionLerpFactor)) : vector2) : vector3);
				break;
			}
			case MoteSpawnLocType.RandomCellOnTarget:
			{
				vector = ((!B.HasThing) ? CellRect.CenteredOn(B.Cell, 0) : B.Thing.OccupiedRect()).RandomCell.ToVector3Shifted();
				break;
			}
			case MoteSpawnLocType.BetweenTouchingCells:
			{
				vector = A.Cell.ToVector3Shifted() + (B.Cell - A.Cell).ToVector3().normalized * 0.5f;
				break;
			}
			}
			Map map = A.Map ?? B.Map;
			if (map != null && vector.ShouldSpawnMotesAt(map))
			{
				int randomInRange = base.def.burstCount.RandomInRange;
				for (int num = 0; num < randomInRange; num++)
				{
					Mote mote = (Mote)ThingMaker.MakeThing(base.def.moteDef, null);
					GenSpawn.Spawn(mote, vector.ToIntVec3(), map);
					mote.Scale = base.def.scale.RandomInRange;
					mote.exactPosition = vector + Gen.RandomHorizontalVector(base.def.positionRadius);
					mote.rotationRate = base.def.rotationRate.RandomInRange;
					float num2 = (float)((!base.def.absoluteAngle) ? (B.Cell - A.Cell).AngleFlat : 0.0);
					mote.exactRotation = base.def.rotation.RandomInRange + num2;
					MoteThrown moteThrown = mote as MoteThrown;
					if (moteThrown != null)
					{
						moteThrown.airTimeLeft = base.def.airTime.RandomInRange;
						moteThrown.SetVelocity(base.def.angle.RandomInRange + num2, base.def.speed.RandomInRange);
					}
				}
			}
		}
	}
}
