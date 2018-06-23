using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DE6 RID: 3558
	public abstract class Mote : Thing
	{
		// Token: 0x040034D7 RID: 13527
		public Vector3 exactPosition;

		// Token: 0x040034D8 RID: 13528
		public float exactRotation = 0f;

		// Token: 0x040034D9 RID: 13529
		public Vector3 exactScale = new Vector3(1f, 1f, 1f);

		// Token: 0x040034DA RID: 13530
		public float rotationRate = 0f;

		// Token: 0x040034DB RID: 13531
		public Color instanceColor = Color.white;

		// Token: 0x040034DC RID: 13532
		private int lastMaintainTick;

		// Token: 0x040034DD RID: 13533
		public int spawnTick;

		// Token: 0x040034DE RID: 13534
		public float spawnRealTime;

		// Token: 0x040034DF RID: 13535
		public MoteAttachLink link1 = MoteAttachLink.Invalid;

		// Token: 0x040034E0 RID: 13536
		protected float skidSpeedMultiplierPerTick = Rand.Range(0.3f, 0.95f);

		// Token: 0x040034E1 RID: 13537
		protected const float MinSpeed = 0.02f;

		// Token: 0x17000CED RID: 3309
		// (set) Token: 0x06004FBE RID: 20414 RVA: 0x00142F2E File Offset: 0x0014132E
		public float Scale
		{
			set
			{
				this.exactScale = new Vector3(value, 1f, value);
			}
		}

		// Token: 0x17000CEE RID: 3310
		// (get) Token: 0x06004FBF RID: 20415 RVA: 0x00142F44 File Offset: 0x00141344
		public float AgeSecs
		{
			get
			{
				float result;
				if (this.def.mote.realTime)
				{
					result = Time.realtimeSinceStartup - this.spawnRealTime;
				}
				else
				{
					result = (float)(Find.TickManager.TicksGame - this.spawnTick) / 60f;
				}
				return result;
			}
		}

		// Token: 0x17000CEF RID: 3311
		// (get) Token: 0x06004FC0 RID: 20416 RVA: 0x00142F98 File Offset: 0x00141398
		public override Vector3 DrawPos
		{
			get
			{
				return this.exactPosition;
			}
		}

		// Token: 0x17000CF0 RID: 3312
		// (get) Token: 0x06004FC1 RID: 20417 RVA: 0x00142FB4 File Offset: 0x001413B4
		protected virtual bool EndOfLife
		{
			get
			{
				return this.AgeSecs >= this.def.mote.Lifespan;
			}
		}

		// Token: 0x17000CF1 RID: 3313
		// (get) Token: 0x06004FC2 RID: 20418 RVA: 0x00142FE4 File Offset: 0x001413E4
		public virtual float Alpha
		{
			get
			{
				float ageSecs = this.AgeSecs;
				float result;
				if (ageSecs <= this.def.mote.fadeInTime)
				{
					if (this.def.mote.fadeInTime > 0f)
					{
						result = ageSecs / this.def.mote.fadeInTime;
					}
					else
					{
						result = 1f;
					}
				}
				else if (ageSecs <= this.def.mote.fadeInTime + this.def.mote.solidTime)
				{
					result = 1f;
				}
				else if (this.def.mote.fadeOutTime > 0f)
				{
					result = 1f - Mathf.InverseLerp(this.def.mote.fadeInTime + this.def.mote.solidTime, this.def.mote.fadeInTime + this.def.mote.solidTime + this.def.mote.fadeOutTime, ageSecs);
				}
				else
				{
					result = 1f;
				}
				return result;
			}
		}

		// Token: 0x06004FC3 RID: 20419 RVA: 0x0014310C File Offset: 0x0014150C
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.spawnTick = Find.TickManager.TicksGame;
			this.spawnRealTime = Time.realtimeSinceStartup;
			RealTime.moteList.MoteSpawned(this);
			base.Map.moteCounter.Notify_MoteSpawned();
			this.exactPosition.y = this.def.altitudeLayer.AltitudeFor();
		}

		// Token: 0x06004FC4 RID: 20420 RVA: 0x00143174 File Offset: 0x00141574
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.DeSpawn(mode);
			RealTime.moteList.MoteDespawned(this);
			map.moteCounter.Notify_MoteDespawned();
		}

		// Token: 0x06004FC5 RID: 20421 RVA: 0x001431A6 File Offset: 0x001415A6
		public override void Tick()
		{
			if (!this.def.mote.realTime)
			{
				this.TimeInterval(0.0166666675f);
			}
		}

		// Token: 0x06004FC6 RID: 20422 RVA: 0x001431C9 File Offset: 0x001415C9
		public void RealtimeUpdate()
		{
			if (this.def.mote.realTime)
			{
				this.TimeInterval(Time.deltaTime);
			}
		}

		// Token: 0x06004FC7 RID: 20423 RVA: 0x001431EC File Offset: 0x001415EC
		protected virtual void TimeInterval(float deltaTime)
		{
			if (this.EndOfLife && !base.Destroyed)
			{
				this.Destroy(DestroyMode.Vanish);
			}
			else if (this.def.mote.needsMaintenance && Find.TickManager.TicksGame - 1 > this.lastMaintainTick)
			{
				this.Destroy(DestroyMode.Vanish);
			}
			else if (this.def.mote.growthRate != 0f)
			{
				this.exactScale = new Vector3(this.exactScale.x + this.def.mote.growthRate * deltaTime, this.exactScale.y, this.exactScale.z + this.def.mote.growthRate * deltaTime);
				this.exactScale.x = Mathf.Max(this.exactScale.x, 0.0001f);
				this.exactScale.z = Mathf.Max(this.exactScale.z, 0.0001f);
			}
		}

		// Token: 0x06004FC8 RID: 20424 RVA: 0x00143304 File Offset: 0x00141704
		public override void Draw()
		{
			this.Draw(this.def.altitudeLayer.AltitudeFor());
		}

		// Token: 0x06004FC9 RID: 20425 RVA: 0x0014331D File Offset: 0x0014171D
		public void Draw(float altitude)
		{
			this.exactPosition.y = altitude;
			base.Draw();
		}

		// Token: 0x06004FCA RID: 20426 RVA: 0x00143332 File Offset: 0x00141732
		public void Maintain()
		{
			this.lastMaintainTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06004FCB RID: 20427 RVA: 0x00143345 File Offset: 0x00141745
		public void Attach(TargetInfo a)
		{
			this.link1 = new MoteAttachLink(a);
		}

		// Token: 0x06004FCC RID: 20428 RVA: 0x00143354 File Offset: 0x00141754
		public override void Notify_MyMapRemoved()
		{
			base.Notify_MyMapRemoved();
			RealTime.moteList.MoteDespawned(this);
		}
	}
}
