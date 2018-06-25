using System;
using UnityEngine;

namespace Verse
{
	public class MoteLeaf : Mote
	{
		private Vector3 startSpatialPosition;

		private Vector3 currentSpatialPosition;

		private float spawnDelay;

		private bool front;

		private float treeHeight;

		[TweakValue("Graphics", 0f, 5f)]
		private static float FallSpeed = 0.5f;

		public MoteLeaf()
		{
		}

		protected override bool EndOfLife
		{
			get
			{
				return base.AgeSecs >= this.spawnDelay + this.FallTime + this.def.mote.solidTime + this.def.mote.fadeOutTime;
			}
		}

		private float FallTime
		{
			get
			{
				return this.startSpatialPosition.y / MoteLeaf.FallSpeed;
			}
		}

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

		public void Initialize(Vector3 position, float spawnDelay, bool front, float treeHeight)
		{
			this.startSpatialPosition = position;
			this.spawnDelay = spawnDelay;
			this.front = front;
			this.treeHeight = treeHeight;
			this.TimeInterval(0f);
		}

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

		public override void Draw()
		{
			base.Draw((!this.front) ? this.def.altitudeLayer.AltitudeFor() : (this.def.altitudeLayer.AltitudeFor() + 0.1f * GenMath.InverseLerp(0f, this.treeHeight, this.currentSpatialPosition.y) * 2f));
		}

		// Note: this type is marked as 'beforefieldinit'.
		static MoteLeaf()
		{
		}
	}
}
