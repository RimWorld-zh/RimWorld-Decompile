using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DEC RID: 3564
	public class MoteLeaf : Mote
	{
		// Token: 0x040034EC RID: 13548
		private Vector3 startSpatialPosition;

		// Token: 0x040034ED RID: 13549
		private Vector3 currentSpatialPosition;

		// Token: 0x040034EE RID: 13550
		private float spawnDelay;

		// Token: 0x040034EF RID: 13551
		private bool front;

		// Token: 0x040034F0 RID: 13552
		private float treeHeight;

		// Token: 0x040034F1 RID: 13553
		[TweakValue("Graphics", 0f, 5f)]
		private static float FallSpeed = 0.5f;

		// Token: 0x17000CF5 RID: 3317
		// (get) Token: 0x06004FDC RID: 20444 RVA: 0x002977CC File Offset: 0x00295BCC
		protected override bool EndOfLife
		{
			get
			{
				return base.AgeSecs >= this.spawnDelay + this.FallTime + this.def.mote.solidTime + this.def.mote.fadeOutTime;
			}
		}

		// Token: 0x17000CF6 RID: 3318
		// (get) Token: 0x06004FDD RID: 20445 RVA: 0x0029781C File Offset: 0x00295C1C
		private float FallTime
		{
			get
			{
				return this.startSpatialPosition.y / MoteLeaf.FallSpeed;
			}
		}

		// Token: 0x17000CF7 RID: 3319
		// (get) Token: 0x06004FDE RID: 20446 RVA: 0x00297844 File Offset: 0x00295C44
		public override float Alpha
		{
			get
			{
				float num = base.AgeSecs;
				float result;
				if (num <= this.spawnDelay)
				{
					result = 0f;
				}
				else
				{
					num -= this.spawnDelay;
					if (num <= this.def.mote.fadeInTime)
					{
						if (this.def.mote.fadeInTime > 0f)
						{
							result = num / this.def.mote.fadeInTime;
						}
						else
						{
							result = 1f;
						}
					}
					else if (num <= this.FallTime + this.def.mote.solidTime)
					{
						result = 1f;
					}
					else
					{
						num -= this.FallTime + this.def.mote.solidTime;
						if (num <= this.def.mote.fadeOutTime)
						{
							result = 1f - Mathf.InverseLerp(0f, this.def.mote.fadeOutTime, num);
						}
						else
						{
							num -= this.def.mote.fadeOutTime;
							result = 0f;
						}
					}
				}
				return result;
			}
		}

		// Token: 0x06004FDF RID: 20447 RVA: 0x0029796E File Offset: 0x00295D6E
		public void Initialize(Vector3 position, float spawnDelay, bool front, float treeHeight)
		{
			this.startSpatialPosition = position;
			this.spawnDelay = spawnDelay;
			this.front = front;
			this.treeHeight = treeHeight;
			this.TimeInterval(0f);
		}

		// Token: 0x06004FE0 RID: 20448 RVA: 0x0029799C File Offset: 0x00295D9C
		protected override void TimeInterval(float deltaTime)
		{
			base.TimeInterval(deltaTime);
			if (!base.Destroyed)
			{
				float ageSecs = base.AgeSecs;
				this.exactPosition = this.startSpatialPosition;
				if (ageSecs > this.spawnDelay)
				{
					this.exactPosition.y = this.exactPosition.y - MoteLeaf.FallSpeed * (ageSecs - this.spawnDelay);
				}
				this.exactPosition.y = Mathf.Max(this.exactPosition.y, 0f);
				this.currentSpatialPosition = this.exactPosition;
				this.exactPosition.z = this.exactPosition.z + this.exactPosition.y;
				this.exactPosition.y = 0f;
			}
		}

		// Token: 0x06004FE1 RID: 20449 RVA: 0x00297A5C File Offset: 0x00295E5C
		public override void Draw()
		{
			base.Draw((!this.front) ? this.def.altitudeLayer.AltitudeFor() : (this.def.altitudeLayer.AltitudeFor() + 0.1f * GenMath.InverseLerp(0f, this.treeHeight, this.currentSpatialPosition.y) * 2f));
		}
	}
}
