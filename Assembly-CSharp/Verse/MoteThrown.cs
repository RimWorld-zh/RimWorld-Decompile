using UnityEngine;
using Verse.Sound;

namespace Verse
{
	public class MoteThrown : Mote
	{
		public float airTimeLeft = 999999f;

		protected Vector3 velocity = Vector3.zero;

		protected bool Flying
		{
			get
			{
				return this.airTimeLeft > 0.0;
			}
		}

		protected bool Skidding
		{
			get
			{
				return !this.Flying && this.Speed > 0.0099999997764825821;
			}
		}

		public Vector3 Velocity
		{
			get
			{
				return this.velocity;
			}
			set
			{
				this.velocity = value;
			}
		}

		public float MoveAngle
		{
			get
			{
				return this.velocity.AngleFlat();
			}
			set
			{
				this.SetVelocity(value, this.Speed);
			}
		}

		public float Speed
		{
			get
			{
				return this.velocity.MagnitudeHorizontal();
			}
			set
			{
				if (value == 0.0)
				{
					this.velocity = Vector3.zero;
				}
				else if (this.velocity == Vector3.zero)
				{
					this.velocity = new Vector3(value, 0f, 0f);
				}
				else
				{
					this.velocity = this.velocity.normalized * value;
				}
			}
		}

		protected override void TimeInterval(float deltaTime)
		{
			base.TimeInterval(deltaTime);
			if (!base.Destroyed && (this.Flying || this.Skidding))
			{
				Vector3 vector = this.NextExactPosition(deltaTime);
				IntVec3 intVec = new IntVec3(vector);
				if (intVec != base.Position)
				{
					if (!intVec.InBounds(base.Map))
					{
						this.Destroy(DestroyMode.Vanish);
						return;
					}
					if (base.def.mote.collide && intVec.Filled(base.Map))
					{
						this.WallHit();
						return;
					}
				}
				base.Position = intVec;
				base.exactPosition = vector;
				base.exactRotation += base.rotationRate * deltaTime;
				this.velocity += base.def.mote.acceleration * deltaTime;
				if (base.def.mote.speedPerTime != 0.0)
				{
					this.Speed = Mathf.Max(this.Speed + base.def.mote.speedPerTime * deltaTime, 0f);
				}
				if (this.airTimeLeft > 0.0)
				{
					this.airTimeLeft -= deltaTime;
					if (this.airTimeLeft < 0.0)
					{
						this.airTimeLeft = 0f;
					}
					if (this.airTimeLeft <= 0.0 && !base.def.mote.landSound.NullOrUndefined())
					{
						base.def.mote.landSound.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
					}
				}
				if (this.Skidding)
				{
					this.Speed *= base.skidSpeedMultiplierPerTick;
					base.rotationRate *= base.skidSpeedMultiplierPerTick;
					if (this.Speed < 0.019999999552965164)
					{
						this.Speed = 0f;
					}
				}
			}
		}

		protected virtual Vector3 NextExactPosition(float deltaTime)
		{
			return base.exactPosition + this.velocity * deltaTime;
		}

		public void SetVelocity(float angle, float speed)
		{
			this.velocity = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward * speed;
		}

		protected virtual void WallHit()
		{
			this.airTimeLeft = 0f;
			this.Speed = 0f;
			base.rotationRate = 0f;
		}
	}
}
