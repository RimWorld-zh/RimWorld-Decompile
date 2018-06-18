using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B2E RID: 2862
	public class SkyfallerProperties
	{
		// Token: 0x17000986 RID: 2438
		// (get) Token: 0x06003F04 RID: 16132 RVA: 0x00212E90 File Offset: 0x00211290
		public bool MakesShrapnel
		{
			get
			{
				return this.metalShrapnelCountRange.max > 0 || this.rubbleShrapnelCountRange.max > 0;
			}
		}

		// Token: 0x17000987 RID: 2439
		// (get) Token: 0x06003F05 RID: 16133 RVA: 0x00212EC8 File Offset: 0x002112C8
		public bool CausesExplosion
		{
			get
			{
				return this.explosionDamage != null && this.explosionRadius > 0f;
			}
		}

		// Token: 0x040028D2 RID: 10450
		public bool hitRoof = true;

		// Token: 0x040028D3 RID: 10451
		public IntRange ticksToImpactRange = new IntRange(120, 200);

		// Token: 0x040028D4 RID: 10452
		public bool reversed;

		// Token: 0x040028D5 RID: 10453
		public float explosionRadius = 3f;

		// Token: 0x040028D6 RID: 10454
		public DamageDef explosionDamage;

		// Token: 0x040028D7 RID: 10455
		public float explosionDamageFactor = 1f;

		// Token: 0x040028D8 RID: 10456
		public IntRange metalShrapnelCountRange = IntRange.zero;

		// Token: 0x040028D9 RID: 10457
		public IntRange rubbleShrapnelCountRange = IntRange.zero;

		// Token: 0x040028DA RID: 10458
		public float shrapnelDistanceFactor = 1f;

		// Token: 0x040028DB RID: 10459
		public SkyfallerMovementType movementType = SkyfallerMovementType.Accelerate;

		// Token: 0x040028DC RID: 10460
		public float speed = 1f;

		// Token: 0x040028DD RID: 10461
		public string shadow = "Things/Skyfaller/SkyfallerShadowCircle";

		// Token: 0x040028DE RID: 10462
		public Vector2 shadowSize = Vector2.one;

		// Token: 0x040028DF RID: 10463
		public float cameraShake;

		// Token: 0x040028E0 RID: 10464
		public SoundDef impactSound;

		// Token: 0x040028E1 RID: 10465
		public bool rotateGraphicTowardsDirection;

		// Token: 0x040028E2 RID: 10466
		public SoundDef anticipationSound;

		// Token: 0x040028E3 RID: 10467
		public int anticipationSoundTicks = 100;

		// Token: 0x040028E4 RID: 10468
		public int motesPerCell = 3;
	}
}
