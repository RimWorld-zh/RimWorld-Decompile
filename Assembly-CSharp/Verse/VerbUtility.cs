using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000FD3 RID: 4051
	public static class VerbUtility
	{
		// Token: 0x0600621E RID: 25118 RVA: 0x00317830 File Offset: 0x00315C30
		public static ThingDef GetProjectile(this Verb verb)
		{
			Verb_LaunchProjectile verb_LaunchProjectile = verb as Verb_LaunchProjectile;
			return (verb_LaunchProjectile == null) ? null : verb_LaunchProjectile.Projectile;
		}

		// Token: 0x0600621F RID: 25119 RVA: 0x00317860 File Offset: 0x00315C60
		public static DamageDef GetDamageDef(this Verb verb)
		{
			DamageDef result;
			if (verb.verbProps.LaunchesProjectile)
			{
				ThingDef projectile = verb.GetProjectile();
				if (projectile != null)
				{
					result = projectile.projectile.damageDef;
				}
				else
				{
					result = null;
				}
			}
			else
			{
				result = verb.verbProps.meleeDamageDef;
			}
			return result;
		}

		// Token: 0x06006220 RID: 25120 RVA: 0x003178B8 File Offset: 0x00315CB8
		public static bool IsIncendiary(this Verb verb)
		{
			ThingDef projectile = verb.GetProjectile();
			return projectile != null && projectile.projectile.ai_IsIncendiary;
		}

		// Token: 0x06006221 RID: 25121 RVA: 0x003178E8 File Offset: 0x00315CE8
		public static bool ProjectileFliesOverhead(this Verb verb)
		{
			ThingDef projectile = verb.GetProjectile();
			return projectile != null && projectile.projectile.flyOverhead;
		}

		// Token: 0x06006222 RID: 25122 RVA: 0x00317918 File Offset: 0x00315D18
		public static bool HarmsHealth(this Verb verb)
		{
			DamageDef damageDef = verb.GetDamageDef();
			return damageDef != null && damageDef.harmsHealth;
		}

		// Token: 0x06006223 RID: 25123 RVA: 0x00317944 File Offset: 0x00315D44
		public static bool IsEMP(this Verb verb)
		{
			return verb.GetDamageDef() == DamageDefOf.EMP;
		}

		// Token: 0x06006224 RID: 25124 RVA: 0x00317968 File Offset: 0x00315D68
		public static bool UsesExplosiveProjectiles(this Verb verb)
		{
			ThingDef projectile = verb.GetProjectile();
			return projectile != null && projectile.projectile.explosionRadius > 0f;
		}

		// Token: 0x06006225 RID: 25125 RVA: 0x003179A0 File Offset: 0x00315DA0
		public static string GenerateBeatFireLoadId(Pawn pawn)
		{
			return string.Format("{0}_BeatFire", pawn.ThingID);
		}

		// Token: 0x06006226 RID: 25126 RVA: 0x003179C8 File Offset: 0x00315DC8
		public static string GenerateIgniteLoadId(Pawn pawn)
		{
			return string.Format("{0}_Ignite", pawn.ThingID);
		}

		// Token: 0x06006227 RID: 25127 RVA: 0x003179F0 File Offset: 0x00315DF0
		public static List<Verb> GetConcreteExampleVerbs(Def def, out Thing owner, ThingDef stuff = null)
		{
			owner = null;
			List<Verb> result = null;
			ThingDef thingDef = def as ThingDef;
			if (thingDef != null)
			{
				Thing concreteExample = thingDef.GetConcreteExample(stuff);
				if (concreteExample is Pawn)
				{
					result = (concreteExample as Pawn).VerbTracker.AllVerbs;
				}
				else if (concreteExample is ThingWithComps)
				{
					result = (concreteExample as ThingWithComps).GetComp<CompEquippable>().AllVerbs;
				}
				else
				{
					result = null;
				}
				owner = concreteExample;
			}
			HediffDef hediffDef = def as HediffDef;
			if (hediffDef != null)
			{
				Hediff concreteExample2 = hediffDef.ConcreteExample;
				result = concreteExample2.TryGetComp<HediffComp_VerbGiver>().VerbTracker.AllVerbs;
			}
			return result;
		}

		// Token: 0x06006228 RID: 25128 RVA: 0x00317A94 File Offset: 0x00315E94
		public static float CalculateAdjustedForcedMiss(float forcedMiss, IntVec3 vector)
		{
			float num = (float)vector.LengthHorizontalSquared;
			float result;
			if (num < 9f)
			{
				result = 0f;
			}
			else if (num < 25f)
			{
				result = forcedMiss * 0.5f;
			}
			else if (num < 49f)
			{
				result = forcedMiss * 0.8f;
			}
			else
			{
				result = forcedMiss;
			}
			return result;
		}

		// Token: 0x06006229 RID: 25129 RVA: 0x00317AFC File Offset: 0x00315EFC
		public static float DistanceInterceptChance(Vector3 origin, IntVec3 c, IntVec3 intendedTargetCell)
		{
			float num = (c.ToVector3Shifted() - origin).MagnitudeHorizontalSquared();
			float num2 = (intendedTargetCell.ToVector3Shifted() - origin).MagnitudeHorizontalSquared();
			float result;
			if (num < 16f && num < num2 && c != intendedTargetCell)
			{
				result = 0f;
			}
			else if (num < 49f)
			{
				result = 0.5f;
			}
			else if (num < 100f)
			{
				result = 0.75f;
			}
			else
			{
				result = 1f;
			}
			return result;
		}

		// Token: 0x0400401E RID: 16414
		public const float InterceptDist_Possible = 4f;

		// Token: 0x0400401F RID: 16415
		private const float InterceptDist_Short = 7f;

		// Token: 0x04004020 RID: 16416
		private const float InterceptDist_Normal = 10f;

		// Token: 0x04004021 RID: 16417
		private const float InterceptChanceFactor_VeryShort = 0.5f;

		// Token: 0x04004022 RID: 16418
		private const float InterceptChanceFactor_Short = 0.75f;
	}
}
