using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DE9 RID: 3561
	public abstract class Mote : Thing
	{
		// Token: 0x040034DE RID: 13534
		public Vector3 exactPosition;

		// Token: 0x040034DF RID: 13535
		public float exactRotation = 0f;

		// Token: 0x040034E0 RID: 13536
		public Vector3 exactScale = new Vector3(1f, 1f, 1f);

		// Token: 0x040034E1 RID: 13537
		public float rotationRate = 0f;

		// Token: 0x040034E2 RID: 13538
		public Color instanceColor = Color.white;

		// Token: 0x040034E3 RID: 13539
		private int lastMaintainTick;

		// Token: 0x040034E4 RID: 13540
		public int spawnTick;

		// Token: 0x040034E5 RID: 13541
		public float spawnRealTime;

		// Token: 0x040034E6 RID: 13542
		public MoteAttachLink link1 = MoteAttachLink.Invalid;

		// Token: 0x040034E7 RID: 13543
		protected float skidSpeedMultiplierPerTick = Rand.Range(0.3f, 0.95f);

		// Token: 0x040034E8 RID: 13544
		protected const float MinSpeed = 0.02f;

		// Token: 0x17000CEC RID: 3308
		// (set) Token: 0x06004FC2 RID: 20418 RVA: 0x001432DE File Offset: 0x001416DE
		public float Scale
		{
			set
			{
				this.exactScale = new Vector3(value, 1f, value);
			}
		}

		// Token: 0x17000CED RID: 3309
		// (get) Token: 0x06004FC3 RID: 20419 RVA: 0x001432F4 File Offset: 0x001416F4
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

		// Token: 0x17000CEE RID: 3310
		// (get) Token: 0x06004FC4 RID: 20420 RVA: 0x00143348 File Offset: 0x00141748
		public override Vector3 DrawPos
		{
			get
			{
				return this.exactPosition;
			}
		}

		// Token: 0x17000CEF RID: 3311
		// (get) Token: 0x06004FC5 RID: 20421 RVA: 0x00143364 File Offset: 0x00141764
		protected virtual bool EndOfLife
		{
			get
			{
				return this.AgeSecs >= this.def.mote.Lifespan;
			}
		}

		// Token: 0x17000CF0 RID: 3312
		// (get) Token: 0x06004FC6 RID: 20422 RVA: 0x00143394 File Offset: 0x00141794
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

		// Token: 0x06004FC7 RID: 20423 RVA: 0x001434BC File Offset: 0x001418BC
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.spawnTick = Find.TickManager.TicksGame;
			this.spawnRealTime = Time.realtimeSinceStartup;
			RealTime.moteList.MoteSpawned(this);
			base.Map.moteCounter.Notify_MoteSpawned();
			this.exactPosition.y = this.def.altitudeLayer.AltitudeFor();
		}

		// Token: 0x06004FC8 RID: 20424 RVA: 0x00143524 File Offset: 0x00141924
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.DeSpawn(mode);
			RealTime.moteList.MoteDespawned(this);
			map.moteCounter.Notify_MoteDespawned();
		}

		// Token: 0x06004FC9 RID: 20425 RVA: 0x00143556 File Offset: 0x00141956
		public override void Tick()
		{
			if (!this.def.mote.realTime)
			{
				this.TimeInterval(0.0166666675f);
			}
		}

		// Token: 0x06004FCA RID: 20426 RVA: 0x00143579 File Offset: 0x00141979
		public void RealtimeUpdate()
		{
			if (this.def.mote.realTime)
			{
				this.TimeInterval(Time.deltaTime);
			}
		}

		// Token: 0x06004FCB RID: 20427 RVA: 0x0014359C File Offset: 0x0014199C
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

		// Token: 0x06004FCC RID: 20428 RVA: 0x001436B4 File Offset: 0x00141AB4
		public override void Draw()
		{
			this.Draw(this.def.altitudeLayer.AltitudeFor());
		}

		// Token: 0x06004FCD RID: 20429 RVA: 0x001436CD File Offset: 0x00141ACD
		public void Draw(float altitude)
		{
			this.exactPosition.y = altitude;
			base.Draw();
		}

		// Token: 0x06004FCE RID: 20430 RVA: 0x001436E2 File Offset: 0x00141AE2
		public void Maintain()
		{
			this.lastMaintainTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06004FCF RID: 20431 RVA: 0x001436F5 File Offset: 0x00141AF5
		public void Attach(TargetInfo a)
		{
			this.link1 = new MoteAttachLink(a);
		}

		// Token: 0x06004FD0 RID: 20432 RVA: 0x00143704 File Offset: 0x00141B04
		public override void Notify_MyMapRemoved()
		{
			base.Notify_MyMapRemoved();
			RealTime.moteList.MoteDespawned(this);
		}
	}
}
