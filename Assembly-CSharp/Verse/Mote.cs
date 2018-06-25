using System;
using UnityEngine;

namespace Verse
{
	public abstract class Mote : Thing
	{
		public Vector3 exactPosition;

		public float exactRotation = 0f;

		public Vector3 exactScale = new Vector3(1f, 1f, 1f);

		public float rotationRate = 0f;

		public Color instanceColor = Color.white;

		private int lastMaintainTick;

		public int spawnTick;

		public float spawnRealTime;

		public MoteAttachLink link1 = MoteAttachLink.Invalid;

		protected float skidSpeedMultiplierPerTick = Rand.Range(0.3f, 0.95f);

		protected const float MinSpeed = 0.02f;

		protected Mote()
		{
		}

		public float Scale
		{
			set
			{
				this.exactScale = new Vector3(value, 1f, value);
			}
		}

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

		public override Vector3 DrawPos
		{
			get
			{
				return this.exactPosition;
			}
		}

		protected virtual bool EndOfLife
		{
			get
			{
				return this.AgeSecs >= this.def.mote.Lifespan;
			}
		}

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

		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.spawnTick = Find.TickManager.TicksGame;
			this.spawnRealTime = Time.realtimeSinceStartup;
			RealTime.moteList.MoteSpawned(this);
			base.Map.moteCounter.Notify_MoteSpawned();
			this.exactPosition.y = this.def.altitudeLayer.AltitudeFor();
		}

		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.DeSpawn(mode);
			RealTime.moteList.MoteDespawned(this);
			map.moteCounter.Notify_MoteDespawned();
		}

		public override void Tick()
		{
			if (!this.def.mote.realTime)
			{
				this.TimeInterval(0.0166666675f);
			}
		}

		public void RealtimeUpdate()
		{
			if (this.def.mote.realTime)
			{
				this.TimeInterval(Time.deltaTime);
			}
		}

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

		public override void Draw()
		{
			this.Draw(this.def.altitudeLayer.AltitudeFor());
		}

		public void Draw(float altitude)
		{
			this.exactPosition.y = altitude;
			base.Draw();
		}

		public void Maintain()
		{
			this.lastMaintainTick = Find.TickManager.TicksGame;
		}

		public void Attach(TargetInfo a)
		{
			this.link1 = new MoteAttachLink(a);
		}

		public override void Notify_MyMapRemoved()
		{
			base.Notify_MyMapRemoved();
			RealTime.moteList.MoteDespawned(this);
		}
	}
}
