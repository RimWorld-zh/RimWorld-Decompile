using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DE9 RID: 3561
	public abstract class Mote : Thing
	{
		// Token: 0x17000CEB RID: 3307
		// (set) Token: 0x06004FA9 RID: 20393 RVA: 0x00142DE2 File Offset: 0x001411E2
		public float Scale
		{
			set
			{
				this.exactScale = new Vector3(value, 1f, value);
			}
		}

		// Token: 0x17000CEC RID: 3308
		// (get) Token: 0x06004FAA RID: 20394 RVA: 0x00142DF8 File Offset: 0x001411F8
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

		// Token: 0x17000CED RID: 3309
		// (get) Token: 0x06004FAB RID: 20395 RVA: 0x00142E4C File Offset: 0x0014124C
		public override Vector3 DrawPos
		{
			get
			{
				return this.exactPosition;
			}
		}

		// Token: 0x17000CEE RID: 3310
		// (get) Token: 0x06004FAC RID: 20396 RVA: 0x00142E68 File Offset: 0x00141268
		protected virtual bool EndOfLife
		{
			get
			{
				return this.AgeSecs >= this.def.mote.Lifespan;
			}
		}

		// Token: 0x17000CEF RID: 3311
		// (get) Token: 0x06004FAD RID: 20397 RVA: 0x00142E98 File Offset: 0x00141298
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

		// Token: 0x06004FAE RID: 20398 RVA: 0x00142FC0 File Offset: 0x001413C0
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.spawnTick = Find.TickManager.TicksGame;
			this.spawnRealTime = Time.realtimeSinceStartup;
			RealTime.moteList.MoteSpawned(this);
			base.Map.moteCounter.Notify_MoteSpawned();
			this.exactPosition.y = this.def.altitudeLayer.AltitudeFor();
		}

		// Token: 0x06004FAF RID: 20399 RVA: 0x00143028 File Offset: 0x00141428
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.DeSpawn(mode);
			RealTime.moteList.MoteDespawned(this);
			map.moteCounter.Notify_MoteDespawned();
		}

		// Token: 0x06004FB0 RID: 20400 RVA: 0x0014305A File Offset: 0x0014145A
		public override void Tick()
		{
			if (!this.def.mote.realTime)
			{
				this.TimeInterval(0.0166666675f);
			}
		}

		// Token: 0x06004FB1 RID: 20401 RVA: 0x0014307D File Offset: 0x0014147D
		public void RealtimeUpdate()
		{
			if (this.def.mote.realTime)
			{
				this.TimeInterval(Time.deltaTime);
			}
		}

		// Token: 0x06004FB2 RID: 20402 RVA: 0x001430A0 File Offset: 0x001414A0
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

		// Token: 0x06004FB3 RID: 20403 RVA: 0x001431B8 File Offset: 0x001415B8
		public override void Draw()
		{
			this.Draw(this.def.altitudeLayer.AltitudeFor());
		}

		// Token: 0x06004FB4 RID: 20404 RVA: 0x001431D1 File Offset: 0x001415D1
		public void Draw(float altitude)
		{
			this.exactPosition.y = altitude;
			base.Draw();
		}

		// Token: 0x06004FB5 RID: 20405 RVA: 0x001431E6 File Offset: 0x001415E6
		public void Maintain()
		{
			this.lastMaintainTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06004FB6 RID: 20406 RVA: 0x001431F9 File Offset: 0x001415F9
		public void Attach(TargetInfo a)
		{
			this.link1 = new MoteAttachLink(a);
		}

		// Token: 0x06004FB7 RID: 20407 RVA: 0x00143208 File Offset: 0x00141608
		public override void Notify_MyMapRemoved()
		{
			base.Notify_MyMapRemoved();
			RealTime.moteList.MoteDespawned(this);
		}

		// Token: 0x040034CC RID: 13516
		public Vector3 exactPosition;

		// Token: 0x040034CD RID: 13517
		public float exactRotation = 0f;

		// Token: 0x040034CE RID: 13518
		public Vector3 exactScale = new Vector3(1f, 1f, 1f);

		// Token: 0x040034CF RID: 13519
		public float rotationRate = 0f;

		// Token: 0x040034D0 RID: 13520
		public Color instanceColor = Color.white;

		// Token: 0x040034D1 RID: 13521
		private int lastMaintainTick;

		// Token: 0x040034D2 RID: 13522
		public int spawnTick;

		// Token: 0x040034D3 RID: 13523
		public float spawnRealTime;

		// Token: 0x040034D4 RID: 13524
		public MoteAttachLink link1 = MoteAttachLink.Invalid;

		// Token: 0x040034D5 RID: 13525
		protected float skidSpeedMultiplierPerTick = Rand.Range(0.3f, 0.95f);

		// Token: 0x040034D6 RID: 13526
		protected const float MinSpeed = 0.02f;
	}
}
