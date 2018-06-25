using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020006E6 RID: 1766
	[StaticConstructorOnStartup]
	public class Skyfaller : Thing, IThingHolder
	{
		// Token: 0x0400156D RID: 5485
		public ThingOwner innerContainer;

		// Token: 0x0400156E RID: 5486
		public int ticksToImpact;

		// Token: 0x0400156F RID: 5487
		public float angle;

		// Token: 0x04001570 RID: 5488
		public float shrapnelDirection;

		// Token: 0x04001571 RID: 5489
		private Material cachedShadowMaterial;

		// Token: 0x04001572 RID: 5490
		private bool anticipationSoundPlayed;

		// Token: 0x04001573 RID: 5491
		private static MaterialPropertyBlock shadowPropertyBlock = new MaterialPropertyBlock();

		// Token: 0x04001574 RID: 5492
		public const float DefaultAngle = -33.7f;

		// Token: 0x04001575 RID: 5493
		private const int RoofHitPreDelay = 15;

		// Token: 0x04001576 RID: 5494
		private const int LeaveMapAfterTicks = 220;

		// Token: 0x0600266B RID: 9835 RVA: 0x00148D82 File Offset: 0x00147182
		public Skyfaller()
		{
			this.innerContainer = new ThingOwner<Thing>(this);
		}

		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x0600266C RID: 9836 RVA: 0x00148D98 File Offset: 0x00147198
		public override Graphic Graphic
		{
			get
			{
				Thing thingForGraphic = this.GetThingForGraphic();
				Graphic result;
				if (thingForGraphic == this)
				{
					result = base.Graphic;
				}
				else
				{
					result = thingForGraphic.Graphic.ExtractInnerGraphicFor(thingForGraphic).GetShadowlessGraphic();
				}
				return result;
			}
		}

		// Token: 0x170005DA RID: 1498
		// (get) Token: 0x0600266D RID: 9837 RVA: 0x00148DD8 File Offset: 0x001471D8
		public override Vector3 DrawPos
		{
			get
			{
				Vector3 result;
				switch (this.def.skyfaller.movementType)
				{
				case SkyfallerMovementType.Accelerate:
					result = SkyfallerDrawPosUtility.DrawPos_Accelerate(base.DrawPos, this.ticksToImpact, this.angle, this.def.skyfaller.speed);
					break;
				case SkyfallerMovementType.ConstantSpeed:
					result = SkyfallerDrawPosUtility.DrawPos_ConstantSpeed(base.DrawPos, this.ticksToImpact, this.angle, this.def.skyfaller.speed);
					break;
				case SkyfallerMovementType.Decelerate:
					result = SkyfallerDrawPosUtility.DrawPos_Decelerate(base.DrawPos, this.ticksToImpact, this.angle, this.def.skyfaller.speed);
					break;
				default:
					Log.ErrorOnce("SkyfallerMovementType not handled: " + this.def.skyfaller.movementType, this.thingIDNumber ^ 1948576711, false);
					result = SkyfallerDrawPosUtility.DrawPos_Accelerate(base.DrawPos, this.ticksToImpact, this.angle, this.def.skyfaller.speed);
					break;
				}
				return result;
			}
		}

		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x0600266E RID: 9838 RVA: 0x00148EF4 File Offset: 0x001472F4
		private Material ShadowMaterial
		{
			get
			{
				if (this.cachedShadowMaterial == null && !this.def.skyfaller.shadow.NullOrEmpty())
				{
					this.cachedShadowMaterial = MaterialPool.MatFrom(this.def.skyfaller.shadow, ShaderDatabase.Transparent);
				}
				return this.cachedShadowMaterial;
			}
		}

		// Token: 0x0600266F RID: 9839 RVA: 0x00148F5C File Offset: 0x0014735C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Values.Look<int>(ref this.ticksToImpact, "ticksToImpact", 0, false);
			Scribe_Values.Look<float>(ref this.angle, "angle", 0f, false);
			Scribe_Values.Look<float>(ref this.shrapnelDirection, "shrapnelDirection", 0f, false);
		}

		// Token: 0x06002670 RID: 9840 RVA: 0x00148FC8 File Offset: 0x001473C8
		public override void PostMake()
		{
			base.PostMake();
			if (this.def.skyfaller.MakesShrapnel)
			{
				this.shrapnelDirection = Rand.Range(0f, 360f);
			}
		}

		// Token: 0x06002671 RID: 9841 RVA: 0x00148FFC File Offset: 0x001473FC
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.ticksToImpact = this.def.skyfaller.ticksToImpactRange.RandomInRange;
				if (this.def.skyfaller.MakesShrapnel)
				{
					float num = GenMath.PositiveMod(this.shrapnelDirection, 360f);
					if (num < 270f && num >= 90f)
					{
						this.angle = Rand.Range(0f, 33f);
					}
					else
					{
						this.angle = Rand.Range(-33f, 0f);
					}
				}
				else
				{
					this.angle = -33.7f;
				}
				if (this.def.rotatable && this.innerContainer.Any)
				{
					base.Rotation = this.innerContainer[0].Rotation;
				}
			}
		}

		// Token: 0x06002672 RID: 9842 RVA: 0x001490E9 File Offset: 0x001474E9
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			base.Destroy(mode);
			this.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
		}

		// Token: 0x06002673 RID: 9843 RVA: 0x00149100 File Offset: 0x00147500
		public override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			Thing thingForGraphic = this.GetThingForGraphic();
			float extraRotation = (!this.def.skyfaller.rotateGraphicTowardsDirection) ? 0f : this.angle;
			this.Graphic.Draw(drawLoc, (!flip) ? thingForGraphic.Rotation : thingForGraphic.Rotation.Opposite, thingForGraphic, extraRotation);
			this.DrawDropSpotShadow();
		}

		// Token: 0x06002674 RID: 9844 RVA: 0x00149170 File Offset: 0x00147570
		public override void Tick()
		{
			this.innerContainer.ThingOwnerTick(true);
			if (this.def.skyfaller.reversed)
			{
				this.ticksToImpact++;
				if (!this.anticipationSoundPlayed && this.def.skyfaller.anticipationSound != null && this.ticksToImpact > this.def.skyfaller.anticipationSoundTicks)
				{
					this.anticipationSoundPlayed = true;
					this.def.skyfaller.anticipationSound.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
				}
				if (this.ticksToImpact == 220)
				{
					this.LeaveMap();
				}
				else if (this.ticksToImpact > 220)
				{
					Log.Error("ticksToImpact > LeaveMapAfterTicks. Was there an exception? Destroying skyfaller.", false);
					this.Destroy(DestroyMode.Vanish);
				}
			}
			else
			{
				this.ticksToImpact--;
				if (this.ticksToImpact == 15)
				{
					this.HitRoof();
				}
				if (!this.anticipationSoundPlayed && this.def.skyfaller.anticipationSound != null && this.ticksToImpact < this.def.skyfaller.anticipationSoundTicks)
				{
					this.anticipationSoundPlayed = true;
					this.def.skyfaller.anticipationSound.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
				}
				if (this.ticksToImpact == 0)
				{
					this.Impact();
				}
				else if (this.ticksToImpact < 0)
				{
					Log.Error("ticksToImpact < 0. Was there an exception? Destroying skyfaller.", false);
					this.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x06002675 RID: 9845 RVA: 0x0014932C File Offset: 0x0014772C
		protected virtual void HitRoof()
		{
			if (this.def.skyfaller.hitRoof)
			{
				CellRect cr = this.OccupiedRect();
				if (cr.Cells.Any((IntVec3 x) => x.Roofed(this.Map)))
				{
					RoofDef roof = cr.Cells.First((IntVec3 x) => x.Roofed(this.Map)).GetRoof(base.Map);
					if (!roof.soundPunchThrough.NullOrUndefined())
					{
						roof.soundPunchThrough.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
					}
					RoofCollapserImmediate.DropRoofInCells(cr.ExpandedBy(1).ClipInsideMap(base.Map).Cells.Where(delegate(IntVec3 c)
					{
						bool result;
						if (!c.InBounds(this.Map))
						{
							result = false;
						}
						else if (cr.Contains(c))
						{
							result = true;
						}
						else if (c.GetFirstPawn(this.Map) != null)
						{
							result = false;
						}
						else
						{
							Building edifice = c.GetEdifice(this.Map);
							result = (edifice == null || !edifice.def.holdsRoof);
						}
						return result;
					}), base.Map, null);
				}
			}
		}

		// Token: 0x06002676 RID: 9846 RVA: 0x0014942C File Offset: 0x0014782C
		protected virtual void Impact()
		{
			if (this.def.skyfaller.CausesExplosion)
			{
				GenExplosion.DoExplosion(base.Position, base.Map, this.def.skyfaller.explosionRadius, this.def.skyfaller.explosionDamage, null, GenMath.RoundRandom((float)this.def.skyfaller.explosionDamage.defaultDamage * this.def.skyfaller.explosionDamageFactor), null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false);
			}
			for (int i = this.innerContainer.Count - 1; i >= 0; i--)
			{
				GenPlace.TryPlaceThing(this.innerContainer[i], base.Position, base.Map, ThingPlaceMode.Near, delegate(Thing thing, int count)
				{
					PawnUtility.RecoverFromUnwalkablePositionOrKill(thing.Position, thing.Map);
				}, null);
			}
			this.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
			CellRect cellRect = this.OccupiedRect();
			for (int j = 0; j < cellRect.Area * this.def.skyfaller.motesPerCell; j++)
			{
				MoteMaker.ThrowDustPuff(cellRect.RandomVector3, base.Map, 2f);
			}
			if (this.def.skyfaller.MakesShrapnel)
			{
				SkyfallerShrapnelUtility.MakeShrapnel(base.Position, base.Map, this.shrapnelDirection, this.def.skyfaller.shrapnelDistanceFactor, this.def.skyfaller.metalShrapnelCountRange.RandomInRange, this.def.skyfaller.rubbleShrapnelCountRange.RandomInRange, true);
			}
			if (this.def.skyfaller.cameraShake > 0f && base.Map == Find.CurrentMap)
			{
				Find.CameraDriver.shaker.DoShake(this.def.skyfaller.cameraShake);
			}
			if (this.def.skyfaller.impactSound != null)
			{
				this.def.skyfaller.impactSound.PlayOneShot(SoundInfo.InMap(new TargetInfo(base.Position, base.Map, false), MaintenanceType.None));
			}
			this.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x06002677 RID: 9847 RVA: 0x00149675 File Offset: 0x00147A75
		protected virtual void LeaveMap()
		{
			this.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x06002678 RID: 9848 RVA: 0x00149680 File Offset: 0x00147A80
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x06002679 RID: 9849 RVA: 0x0014969B File Offset: 0x00147A9B
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x0600267A RID: 9850 RVA: 0x001496AC File Offset: 0x00147AAC
		private Thing GetThingForGraphic()
		{
			Thing result;
			if (this.def.graphicData != null || !this.innerContainer.Any)
			{
				result = this;
			}
			else
			{
				result = this.innerContainer[0];
			}
			return result;
		}

		// Token: 0x0600267B RID: 9851 RVA: 0x001496F4 File Offset: 0x00147AF4
		private void DrawDropSpotShadow()
		{
			Material shadowMaterial = this.ShadowMaterial;
			if (!(shadowMaterial == null))
			{
				Skyfaller.DrawDropSpotShadow(base.DrawPos, base.Rotation, shadowMaterial, this.def.skyfaller.shadowSize, this.ticksToImpact);
			}
		}

		// Token: 0x0600267C RID: 9852 RVA: 0x00149744 File Offset: 0x00147B44
		public static void DrawDropSpotShadow(Vector3 center, Rot4 rot, Material material, Vector2 shadowSize, int ticksToImpact)
		{
			if (rot.IsHorizontal)
			{
				Gen.Swap<float>(ref shadowSize.x, ref shadowSize.y);
			}
			ticksToImpact = Mathf.Max(ticksToImpact, 0);
			Vector3 pos = center;
			pos.y = AltitudeLayer.Shadows.AltitudeFor();
			float num = 1f + (float)ticksToImpact / 100f;
			Vector3 s = new Vector3(num * shadowSize.x, 1f, num * shadowSize.y);
			Color white = Color.white;
			if (ticksToImpact > 150)
			{
				white.a = Mathf.InverseLerp(200f, 150f, (float)ticksToImpact);
			}
			Skyfaller.shadowPropertyBlock.SetColor(ShaderPropertyIDs.Color, white);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(pos, rot.AsQuat, s);
			Graphics.DrawMesh(MeshPool.plane10Back, matrix, material, 0, null, 0, Skyfaller.shadowPropertyBlock);
		}
	}
}
