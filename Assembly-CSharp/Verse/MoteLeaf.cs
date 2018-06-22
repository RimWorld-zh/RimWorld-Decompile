using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DE9 RID: 3561
	public class MoteLeaf : Mote
	{
		// Token: 0x17000CF6 RID: 3318
		// (get) Token: 0x06004FD8 RID: 20440 RVA: 0x002973C0 File Offset: 0x002957C0
		protected override bool EndOfLife
		{
			get
			{
				return base.AgeSecs >= this.spawnDelay + this.FallTime + this.def.mote.solidTime + this.def.mote.fadeOutTime;
			}
		}

		// Token: 0x17000CF7 RID: 3319
		// (get) Token: 0x06004FD9 RID: 20441 RVA: 0x00297410 File Offset: 0x00295810
		private float FallTime
		{
			get
			{
				return this.startSpatialPosition.y / MoteLeaf.FallSpeed;
			}
		}

		// Token: 0x17000CF8 RID: 3320
		// (get) Token: 0x06004FDA RID: 20442 RVA: 0x00297438 File Offset: 0x00295838
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

		// Token: 0x06004FDB RID: 20443 RVA: 0x00297562 File Offset: 0x00295962
		public void Initialize(Vector3 position, float spawnDelay, bool front, float treeHeight)
		{
			this.startSpatialPosition = position;
			this.spawnDelay = spawnDelay;
			this.front = front;
			this.treeHeight = treeHeight;
			this.TimeInterval(0f);
		}

		// Token: 0x06004FDC RID: 20444 RVA: 0x00297590 File Offset: 0x00295990
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

		// Token: 0x06004FDD RID: 20445 RVA: 0x00297650 File Offset: 0x00295A50
		public override void Draw()
		{
			base.Draw((!this.front) ? this.def.altitudeLayer.AltitudeFor() : (this.def.altitudeLayer.AltitudeFor() + 0.1f * GenMath.InverseLerp(0f, this.treeHeight, this.currentSpatialPosition.y) * 2f));
		}

		// Token: 0x040034E5 RID: 13541
		private Vector3 startSpatialPosition;

		// Token: 0x040034E6 RID: 13542
		private Vector3 currentSpatialPosition;

		// Token: 0x040034E7 RID: 13543
		private float spawnDelay;

		// Token: 0x040034E8 RID: 13544
		private bool front;

		// Token: 0x040034E9 RID: 13545
		private float treeHeight;

		// Token: 0x040034EA RID: 13546
		[TweakValue("Graphics", 0f, 5f)]
		private static float FallSpeed = 0.5f;
	}
}
