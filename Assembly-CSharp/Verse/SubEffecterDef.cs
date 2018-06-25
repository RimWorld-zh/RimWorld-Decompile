using System;

namespace Verse
{
	public class SubEffecterDef
	{
		public Type subEffecterClass = null;

		public IntRange burstCount = new IntRange(1, 1);

		public int ticksBetweenMotes = 40;

		public float chancePerTick = 0.1f;

		public MoteSpawnLocType spawnLocType = MoteSpawnLocType.BetweenPositions;

		public float positionLerpFactor = 0.5f;

		public float positionRadius = 0f;

		public ThingDef moteDef = null;

		public FloatRange angle = new FloatRange(0f, 360f);

		public bool absoluteAngle = false;

		public FloatRange speed = new FloatRange(0f, 0f);

		public FloatRange rotation = new FloatRange(0f, 360f);

		public FloatRange rotationRate = new FloatRange(0f, 0f);

		public FloatRange scale = new FloatRange(1f, 1f);

		public FloatRange airTime = new FloatRange(999999f, 999999f);

		public SoundDef soundDef = null;

		public IntRange intermittentSoundInterval = new IntRange(300, 600);

		public int ticksBeforeSustainerStart = 0;

		public SubEffecterDef()
		{
		}

		public SubEffecter Spawn(Effecter parent)
		{
			return (SubEffecter)Activator.CreateInstance(this.subEffecterClass, new object[]
			{
				this,
				parent
			});
		}
	}
}
