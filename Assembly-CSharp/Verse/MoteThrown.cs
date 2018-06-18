using System;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000DEF RID: 3567
	public class MoteThrown : Mote
	{
		// Token: 0x17000CFB RID: 3323
		// (get) Token: 0x06004FD7 RID: 20439 RVA: 0x00296294 File Offset: 0x00294694
		protected bool Flying
		{
			get
			{
				return this.airTimeLeft > 0f;
			}
		}

		// Token: 0x17000CFC RID: 3324
		// (get) Token: 0x06004FD8 RID: 20440 RVA: 0x002962B8 File Offset: 0x002946B8
		protected bool Skidding
		{
			get
			{
				return !this.Flying && this.Speed > 0.01f;
			}
		}

		// Token: 0x17000CFD RID: 3325
		// (get) Token: 0x06004FD9 RID: 20441 RVA: 0x002962E8 File Offset: 0x002946E8
		// (set) Token: 0x06004FDA RID: 20442 RVA: 0x00296303 File Offset: 0x00294703
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

		// Token: 0x17000CFE RID: 3326
		// (get) Token: 0x06004FDB RID: 20443 RVA: 0x00296310 File Offset: 0x00294710
		// (set) Token: 0x06004FDC RID: 20444 RVA: 0x00296330 File Offset: 0x00294730
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

		// Token: 0x17000CFF RID: 3327
		// (get) Token: 0x06004FDD RID: 20445 RVA: 0x00296340 File Offset: 0x00294740
		// (set) Token: 0x06004FDE RID: 20446 RVA: 0x00296360 File Offset: 0x00294760
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

		// Token: 0x06004FDF RID: 20447 RVA: 0x002963D4 File Offset: 0x002947D4
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

		// Token: 0x06004FE0 RID: 20448 RVA: 0x00296638 File Offset: 0x00294A38
		protected virtual Vector3 NextExactPosition(float deltaTime)
		{
			return this.exactPosition + this.velocity * deltaTime;
		}

		// Token: 0x06004FE1 RID: 20449 RVA: 0x00296664 File Offset: 0x00294A64
		public void SetVelocity(float angle, float speed)
		{
			this.velocity = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward * speed;
		}

		// Token: 0x06004FE2 RID: 20450 RVA: 0x00296688 File Offset: 0x00294A88
		protected virtual void WallHit()
		{
			this.airTimeLeft = 0f;
			this.Speed = 0f;
			this.rotationRate = 0f;
		}

		// Token: 0x040034EB RID: 13547
		public float airTimeLeft = 999999f;

		// Token: 0x040034EC RID: 13548
		protected Vector3 velocity = Vector3.zero;
	}
}
