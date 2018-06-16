using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DEA RID: 3562
	public abstract class Mote : Thing
	{
		// Token: 0x17000CEC RID: 3308
		// (set) Token: 0x06004FAB RID: 20395 RVA: 0x00142D6A File Offset: 0x0014116A
		public float Scale
		{
			set
			{
				this.exactScale = new Vector3(value, 1f, value);
			}
		}

		// Token: 0x17000CED RID: 3309
		// (get) Token: 0x06004FAC RID: 20396 RVA: 0x00142D80 File Offset: 0x00141180
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
		// (get) Token: 0x06004FAD RID: 20397 RVA: 0x00142DD4 File Offset: 0x001411D4
		public override Vector3 DrawPos
		{
			get
			{
				return this.exactPosition;
			}
		}

		// Token: 0x17000CEF RID: 3311
		// (get) Token: 0x06004FAE RID: 20398 RVA: 0x00142DF0 File Offset: 0x001411F0
		protected virtual bool EndOfLife
		{
			get
			{
				return this.AgeSecs >= this.def.mote.Lifespan;
			}
		}

		// Token: 0x17000CF0 RID: 3312
		// (get) Token: 0x06004FAF RID: 20399 RVA: 0x00142E20 File Offset: 0x00141220
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

		// Token: 0x06004FB0 RID: 20400 RVA: 0x00142F48 File Offset: 0x00141348
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.spawnTick = Find.TickManager.TicksGame;
			this.spawnRealTime = Time.realtimeSinceStartup;
			RealTime.moteList.MoteSpawned(this);
			base.Map.moteCounter.Notify_MoteSpawned();
			this.exactPosition.y = this.def.altitudeLayer.AltitudeFor();
		}

		// Token: 0x06004FB1 RID: 20401 RVA: 0x00142FB0 File Offset: 0x001413B0
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.DeSpawn(mode);
			RealTime.moteList.MoteDespawned(this);
			map.moteCounter.Notify_MoteDespawned();
		}

		// Token: 0x06004FB2 RID: 20402 RVA: 0x00142FE2 File Offset: 0x001413E2
		public override void Tick()
		{
			if (!this.def.mote.realTime)
			{
				this.TimeInterval(0.0166666675f);
			}
		}

		// Token: 0x06004FB3 RID: 20403 RVA: 0x00143005 File Offset: 0x00141405
		public void RealtimeUpdate()
		{
			if (this.def.mote.realTime)
			{
				this.TimeInterval(Time.deltaTime);
			}
		}

		// Token: 0x06004FB4 RID: 20404 RVA: 0x00143028 File Offset: 0x00141428
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

		// Token: 0x06004FB5 RID: 20405 RVA: 0x00143140 File Offset: 0x00141540
		public override void Draw()
		{
			this.Draw(this.def.altitudeLayer.AltitudeFor());
		}

		// Token: 0x06004FB6 RID: 20406 RVA: 0x00143159 File Offset: 0x00141559
		public void Draw(float altitude)
		{
			this.exactPosition.y = altitude;
			base.Draw();
		}

		// Token: 0x06004FB7 RID: 20407 RVA: 0x0014316E File Offset: 0x0014156E
		public void Maintain()
		{
			this.lastMaintainTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06004FB8 RID: 20408 RVA: 0x00143181 File Offset: 0x00141581
		public void Attach(TargetInfo a)
		{
			this.link1 = new MoteAttachLink(a);
		}

		// Token: 0x06004FB9 RID: 20409 RVA: 0x00143190 File Offset: 0x00141590
		public override void Notify_MyMapRemoved()
		{
			base.Notify_MyMapRemoved();
			RealTime.moteList.MoteDespawned(this);
		}

		// Token: 0x040034CE RID: 13518
		public Vector3 exactPosition;

		// Token: 0x040034CF RID: 13519
		public float exactRotation = 0f;

		// Token: 0x040034D0 RID: 13520
		public Vector3 exactScale = new Vector3(1f, 1f, 1f);

		// Token: 0x040034D1 RID: 13521
		public float rotationRate = 0f;

		// Token: 0x040034D2 RID: 13522
		public Color instanceColor = Color.white;

		// Token: 0x040034D3 RID: 13523
		private int lastMaintainTick;

		// Token: 0x040034D4 RID: 13524
		public int spawnTick;

		// Token: 0x040034D5 RID: 13525
		public float spawnRealTime;

		// Token: 0x040034D6 RID: 13526
		public MoteAttachLink link1 = MoteAttachLink.Invalid;

		// Token: 0x040034D7 RID: 13527
		protected float skidSpeedMultiplierPerTick = Rand.Range(0.3f, 0.95f);

		// Token: 0x040034D8 RID: 13528
		protected const float MinSpeed = 0.02f;
	}
}
