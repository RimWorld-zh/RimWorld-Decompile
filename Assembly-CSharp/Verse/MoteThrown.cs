using System;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000DEF RID: 3567
	public class MoteThrown : Mote
	{
		// Token: 0x040034FD RID: 13565
		public float airTimeLeft = 999999f;

		// Token: 0x040034FE RID: 13566
		protected Vector3 velocity = Vector3.zero;

		// Token: 0x17000CFC RID: 3324
		// (get) Token: 0x06004FF0 RID: 20464 RVA: 0x00297C7C File Offset: 0x0029607C
		protected bool Flying
		{
			get
			{
				return this.airTimeLeft > 0f;
			}
		}

		// Token: 0x17000CFD RID: 3325
		// (get) Token: 0x06004FF1 RID: 20465 RVA: 0x00297CA0 File Offset: 0x002960A0
		protected bool Skidding
		{
			get
			{
				return !this.Flying && this.Speed > 0.01f;
			}
		}

		// Token: 0x17000CFE RID: 3326
		// (get) Token: 0x06004FF2 RID: 20466 RVA: 0x00297CD0 File Offset: 0x002960D0
		// (set) Token: 0x06004FF3 RID: 20467 RVA: 0x00297CEB File Offset: 0x002960EB
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

		// Token: 0x17000CFF RID: 3327
		// (get) Token: 0x06004FF4 RID: 20468 RVA: 0x00297CF8 File Offset: 0x002960F8
		// (set) Token: 0x06004FF5 RID: 20469 RVA: 0x00297D18 File Offset: 0x00296118
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

		// Token: 0x17000D00 RID: 3328
		// (get) Token: 0x06004FF6 RID: 20470 RVA: 0x00297D28 File Offset: 0x00296128
		// (set) Token: 0x06004FF7 RID: 20471 RVA: 0x00297D48 File Offset: 0x00296148
		public float Speed
		{
			get
			{
				return this.velocity.MagnitudeHorizontal();
			}
			set
			{
				if (value == 0f)
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

		// Token: 0x06004FF8 RID: 20472 RVA: 0x00297DBC File Offset: 0x002961BC
		protected override void TimeInterval(float deltaTime)
		{
			base.TimeInterval(deltaTime);
			if (!base.Destroyed)
			{
				if (this.Flying || this.Skidding)
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
						if (this.def.mote.collide && intVec.Filled(base.Map))
						{
							this.WallHit();
							return;
						}
					}
					base.Position = intVec;
					this.exactPosition = vector;
					if (this.def.mote.rotateTowardsMoveDirection && this.velocity != default(Vector3))
					{
						this.exactRotation = this.velocity.AngleFlat();
					}
					else
					{
						this.exactRotation += this.rotationRate * deltaTime;
					}
					this.velocity += this.def.mote.acceleration * deltaTime;
					if (this.def.mote.speedPerTime != 0f)
					{
						this.Speed = Mathf.Max(this.Speed + this.def.mote.speedPerTime * deltaTime, 0f);
					}
					if (this.airTimeLeft > 0f)
					{
						this.airTimeLeft -= deltaTime;
						if (this.airTimeLeft < 0f)
						{
							this.airTimeLeft = 0f;
						}
						if (this.airTimeLeft <= 0f && !this.def.mote.landSound.NullOrUndefined())
						{
							this.def.mote.landSound.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
						}
					}
					if (this.Skidding)
					{
						this.Speed *= this.skidSpeedMultiplierPerTick;
						this.rotationRate *= this.skidSpeedMultiplierPerTick;
						if (this.Speed < 0.02f)
						{
							this.Speed = 0f;
						}
					}
				}
			}
		}

		// Token: 0x06004FF9 RID: 20473 RVA: 0x00298020 File Offset: 0x00296420
		protected virtual Vector3 NextExactPosition(float deltaTime)
		{
			return this.exactPosition + this.velocity * deltaTime;
		}

		// Token: 0x06004FFA RID: 20474 RVA: 0x0029804C File Offset: 0x0029644C
		public void SetVelocity(float angle, float speed)
		{
			this.velocity = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward * speed;
		}

		// Token: 0x06004FFB RID: 20475 RVA: 0x00298070 File Offset: 0x00296470
		protected virtual void WallHit()
		{
			this.airTimeLeft = 0f;
			this.Speed = 0f;
			this.rotationRate = 0f;
		}
	}
}
