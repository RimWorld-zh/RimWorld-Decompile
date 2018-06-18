using System;

namespace Verse
{
	// Token: 0x02000B3E RID: 2878
	public class SubEffecterDef
	{
		// Token: 0x06003F2D RID: 16173 RVA: 0x00213E64 File Offset: 0x00212264
		public SubEffecter Spawn(Effecter parent)
		{
			return (SubEffecter)Activator.CreateInstance(this.subEffecterClass, new object[]
			{
				this,
				parent
			});
		}

		// Token: 0x04002955 RID: 10581
		public Type subEffecterClass = null;

		// Token: 0x04002956 RID: 10582
		public IntRange burstCount = new IntRange(1, 1);

		// Token: 0x04002957 RID: 10583
		public int ticksBetweenMotes = 40;

		// Token: 0x04002958 RID: 10584
		public float chancePerTick = 0.1f;

		// Token: 0x04002959 RID: 10585
		public MoteSpawnLocType spawnLocType = MoteSpawnLocType.BetweenPositions;

		// Token: 0x0400295A RID: 10586
		public float positionLerpFactor = 0.5f;

		// Token: 0x0400295B RID: 10587
		public float positionRadius = 0f;

		// Token: 0x0400295C RID: 10588
		public ThingDef moteDef = null;

		// Token: 0x0400295D RID: 10589
		public FloatRange angle = new FloatRange(0f, 360f);

		// Token: 0x0400295E RID: 10590
		public bool absoluteAngle = false;

		// Token: 0x0400295F RID: 10591
		public FloatRange speed = new FloatRange(0f, 0f);

		// Token: 0x04002960 RID: 10592
		public FloatRange rotation = new FloatRange(0f, 360f);

		// Token: 0x04002961 RID: 10593
		public FloatRange rotationRate = new FloatRange(0f, 0f);

		// Token: 0x04002962 RID: 10594
		public FloatRange scale = new FloatRange(1f, 1f);

		// Token: 0x04002963 RID: 10595
		public FloatRange airTime = new FloatRange(999999f, 999999f);

		// Token: 0x04002964 RID: 10596
		public SoundDef soundDef = null;

		// Token: 0x04002965 RID: 10597
		public IntRange intermittentSoundInterval = new IntRange(300, 600);

		// Token: 0x04002966 RID: 10598
		public int ticksBeforeSustainerStart = 0;
	}
}
