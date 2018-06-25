using System;

namespace Verse
{
	// Token: 0x02000B3D RID: 2877
	public class SubEffecterDef
	{
		// Token: 0x04002959 RID: 10585
		public Type subEffecterClass = null;

		// Token: 0x0400295A RID: 10586
		public IntRange burstCount = new IntRange(1, 1);

		// Token: 0x0400295B RID: 10587
		public int ticksBetweenMotes = 40;

		// Token: 0x0400295C RID: 10588
		public float chancePerTick = 0.1f;

		// Token: 0x0400295D RID: 10589
		public MoteSpawnLocType spawnLocType = MoteSpawnLocType.BetweenPositions;

		// Token: 0x0400295E RID: 10590
		public float positionLerpFactor = 0.5f;

		// Token: 0x0400295F RID: 10591
		public float positionRadius = 0f;

		// Token: 0x04002960 RID: 10592
		public ThingDef moteDef = null;

		// Token: 0x04002961 RID: 10593
		public FloatRange angle = new FloatRange(0f, 360f);

		// Token: 0x04002962 RID: 10594
		public bool absoluteAngle = false;

		// Token: 0x04002963 RID: 10595
		public FloatRange speed = new FloatRange(0f, 0f);

		// Token: 0x04002964 RID: 10596
		public FloatRange rotation = new FloatRange(0f, 360f);

		// Token: 0x04002965 RID: 10597
		public FloatRange rotationRate = new FloatRange(0f, 0f);

		// Token: 0x04002966 RID: 10598
		public FloatRange scale = new FloatRange(1f, 1f);

		// Token: 0x04002967 RID: 10599
		public FloatRange airTime = new FloatRange(999999f, 999999f);

		// Token: 0x04002968 RID: 10600
		public SoundDef soundDef = null;

		// Token: 0x04002969 RID: 10601
		public IntRange intermittentSoundInterval = new IntRange(300, 600);

		// Token: 0x0400296A RID: 10602
		public int ticksBeforeSustainerStart = 0;

		// Token: 0x06003F2F RID: 16175 RVA: 0x00214838 File Offset: 0x00212C38
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
