using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public class Skyfaller : Thing, IThingHolder
	{
		public ThingOwner innerContainer;

		public int ticksToImpact;

		public float angle;

		public float shrapnelDirection;

		private Material cachedShadowMaterial;

		private bool anticipationSoundPlayed;

		private static MaterialPropertyBlock shadowPropertyBlock = new MaterialPropertyBlock();

		public const float DefaultAngle = -33.7f;

		private const int RoofHitPreDelay = 15;

		private const int LeaveMapAfterTicks = 220;

		public override Graphic Graphic
		{
			get
			{
				Thing thingForGraphic = this.GetThingForGraphic();
				return (thingForGraphic != this) ? thingForGraphic.Graphic.ExtractInnerGraphicFor(thingForGraphic).GetShadowlessGraphic() : base.Graphic;
			}
		}

		public override Vector3 DrawPos
		{
			get
			{
				Vector3 result;
				switch (base.def.skyfaller.movementType)
				{
				case SkyfallerMovementType.Accelerate:
				{
					result = SkyfallerDrawPosUtility.DrawPos_Accelerate(base.DrawPos, this.ticksToImpact, this.angle, base.def.skyfaller.speed);
					break;
				}
				case SkyfallerMovementType.ConstantSpeed:
				{
					result = SkyfallerDrawPosUtility.DrawPos_ConstantSpeed(base.DrawPos, this.ticksToImpact, this.angle, base.def.skyfaller.speed);
					break;
				}
				case SkyfallerMovementType.Decelerate:
				{
					result = SkyfallerDrawPosUtility.DrawPos_Decelerate(base.DrawPos, this.ticksToImpact, this.angle, base.def.skyfaller.speed);
					break;
				}
				default:
				{
					Log.ErrorOnce("SkyfallerMovementType not handled: " + base.def.skyfaller.movementType, base.thingIDNumber ^ 1948576711);
					result = SkyfallerDrawPosUtility.DrawPos_Accelerate(base.DrawPos, this.ticksToImpact, this.angle, base.def.skyfaller.speed);
					break;
				}
				}
				return result;
			}
		}

		private Material ShadowMaterial
		{
			get
			{
				if ((UnityEngine.Object)this.cachedShadowMaterial == (UnityEngine.Object)null && !base.def.skyfaller.shadow.NullOrEmpty())
				{
					this.cachedShadowMaterial = MaterialPool.MatFrom(base.def.skyfaller.shadow, ShaderDatabase.Transparent);
				}
				return this.cachedShadowMaterial;
			}
		}

		public Skyfaller()
		{
			this.innerContainer = new ThingOwner<Thing>(this);
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[1]
			{
				this
			});
			Scribe_Values.Look<int>(ref this.ticksToImpact, "ticksToImpact", 0, false);
			Scribe_Values.Look<float>(ref this.angle, "angle", 0f, false);
			Scribe_Values.Look<float>(ref this.shrapnelDirection, "shrapnelDirection", 0f, false);
		}

		public override void PostMake()
		{
			base.PostMake();
			if (base.def.skyfaller.MakesShrapnel)
			{
				this.shrapnelDirection = Rand.Range(0f, 360f);
			}
		}

		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.ticksToImpact = base.def.skyfaller.ticksToImpactRange.RandomInRange;
				if (base.def.skyfaller.MakesShrapnel)
				{
					float num = GenMath.PositiveMod(this.shrapnelDirection, 360f);
					if (num < 270.0 && num >= 90.0)
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
				if (base.def.rotatable && this.innerContainer.Any)
				{
					base.Rotation = this.innerContainer[0].Rotation;
				}
			}
		}

		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			base.Destroy(mode);
			this.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
		}

		public override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			Thing thingForGraphic = this.GetThingForGraphic();
			float extraRotation = (float)((!base.def.skyfaller.rotateGraphicTowardsDirection) ? 0.0 : this.angle);
			this.Graphic.Draw(drawLoc, (!flip) ? thingForGraphic.Rotation : thingForGraphic.Rotation.Opposite, thingForGraphic, extraRotation);
			this.DrawDropSpotShadow();
		}

		public override void Tick()
		{
			this.innerContainer.ThingOwnerTick(true);
			if (base.def.skyfaller.reversed)
			{
				this.ticksToImpact++;
				if (!this.anticipationSoundPlayed && base.def.skyfaller.anticipationSound != null && this.ticksToImpact > base.def.skyfaller.anticipationSoundTicks)
				{
					this.anticipationSoundPlayed = true;
					base.def.skyfaller.anticipationSound.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
				}
				if (this.ticksToImpact == 220)
				{
					this.LeaveMap();
				}
				else if (this.ticksToImpact > 220)
				{
					Log.Error("ticksToImpact > LeaveMapAfterTicks. Was there an exception? Destroying skyfaller.");
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
				if (!this.anticipationSoundPlayed && base.def.skyfaller.anticipationSound != null && this.ticksToImpact < base.def.skyfaller.anticipationSoundTicks)
				{
					this.anticipationSoundPlayed = true;
					base.def.skyfaller.anticipationSound.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
				}
				if (this.ticksToImpact == 0)
				{
					this.Impact();
				}
				else if (this.ticksToImpact < 0)
				{
					Log.Error("ticksToImpact < 0. Was there an exception? Destroying skyfaller.");
					this.Destroy(DestroyMode.Vanish);
				}
			}
		}

		protected virtual void HitRoof()
		{
			if (base.def.skyfaller.hitRoof)
			{
				CellRect cr = this.OccupiedRect();
				if (cr.Cells.Any((Func<IntVec3, bool>)((IntVec3 x) => x.Roofed(base.Map))))
				{
					RoofDef roof = cr.Cells.First((Func<IntVec3, bool>)((IntVec3 x) => x.Roofed(base.Map))).GetRoof(base.Map);
					if (!roof.soundPunchThrough.NullOrUndefined())
					{
						roof.soundPunchThrough.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
					}
					RoofCollapserImmediate.DropRoofInCells(cr.ExpandedBy(1).ClipInsideMap(base.Map).Cells.Where((Func<IntVec3, bool>)delegate(IntVec3 c)
					{
						bool result;
						if (!c.InBounds(base.Map))
						{
							result = false;
						}
						else if (cr.Contains(c))
						{
							result = true;
						}
						else if (c.GetFirstPawn(base.Map) != null)
						{
							result = false;
						}
						else
						{
							Building edifice = c.GetEdifice(base.Map);
							result = ((byte)((edifice == null || !edifice.def.holdsRoof) ? 1 : 0) != 0);
						}
						return result;
					}), base.Map);
				}
			}
		}

		protected virtual void Impact()
		{
			if (base.def.skyfaller.explosionDamage != null)
			{
				GenExplosion.DoExplosion(base.Position, base.Map, base.def.skyfaller.explosionRadius, base.def.skyfaller.explosionDamage, null, GenMath.RoundRandom((float)base.def.skyfaller.explosionDamage.explosionDamage * base.def.skyfaller.explosionDamageFactor), null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false);
			}
			for (int num = this.innerContainer.Count - 1; num >= 0; num--)
			{
				GenPlace.TryPlaceThing(this.innerContainer[num], base.Position, base.Map, ThingPlaceMode.Near, (Action<Thing, int>)delegate(Thing thing, int count)
				{
					PawnUtility.RecoverFromUnwalkablePositionOrKill(thing.Position, thing.Map);
				});
			}
			this.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
			CellRect cellRect = this.OccupiedRect();
			for (int i = 0; i < cellRect.Area * base.def.skyfaller.motesPerCell; i++)
			{
				MoteMaker.ThrowDustPuff(cellRect.RandomVector3, base.Map, 2f);
			}
			if (base.def.skyfaller.MakesShrapnel)
			{
				SkyfallerShrapnelUtility.MakeShrapnel(base.Position, base.Map, this.shrapnelDirection, base.def.skyfaller.shrapnelDistanceFactor, base.def.skyfaller.metalShrapnelCountRange.RandomInRange, base.def.skyfaller.rubbleShrapnelCountRange.RandomInRange, true);
			}
			if (base.def.skyfaller.cameraShake > 0.0 && base.Map == Find.VisibleMap)
			{
				Find.CameraDriver.shaker.DoShake(base.def.skyfaller.cameraShake);
			}
			if (base.def.skyfaller.impactSound != null)
			{
				base.def.skyfaller.impactSound.PlayOneShot(SoundInfo.InMap(new TargetInfo(base.Position, base.Map, false), MaintenanceType.None));
			}
			this.Destroy(DestroyMode.Vanish);
		}

		protected virtual void LeaveMap()
		{
			this.Destroy(DestroyMode.Vanish);
		}

		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		private Thing GetThingForGraphic()
		{
			return (base.def.graphicData == null && this.innerContainer.Any) ? this.innerContainer[0] : this;
		}

		private void DrawDropSpotShadow()
		{
			Material shadowMaterial = this.ShadowMaterial;
			if (!((UnityEngine.Object)shadowMaterial == (UnityEngine.Object)null))
			{
				Skyfaller.DrawDropSpotShadow(base.DrawPos, base.Rotation, shadowMaterial, base.def.skyfaller.shadowSize, this.ticksToImpact);
			}
		}

		public static void DrawDropSpotShadow(Vector3 center, Rot4 rot, Material material, Vector2 shadowSize, int ticksToImpact)
		{
			if (rot.IsHorizontal)
			{
				Gen.Swap<float>(ref shadowSize.x, ref shadowSize.y);
			}
			ticksToImpact = Mathf.Max(ticksToImpact, 0);
			Vector3 pos = center;
			pos.y = Altitudes.AltitudeFor(AltitudeLayer.Shadows);
			float num = (float)(1.0 + (float)ticksToImpact / 100.0);
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
