using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000FD8 RID: 4056
	public static class VerbUtility
	{
		// Token: 0x0400402E RID: 16430
		public const float InterceptDist_Possible = 4f;

		// Token: 0x0400402F RID: 16431
		private const float InterceptDist_Short = 7f;

		// Token: 0x04004030 RID: 16432
		private const float InterceptDist_Normal = 10f;

		// Token: 0x04004031 RID: 16433
		private const float InterceptChanceFactor_VeryShort = 0.5f;

		// Token: 0x04004032 RID: 16434
		private const float InterceptChanceFactor_Short = 0.75f;

		// Token: 0x0600622D RID: 25133 RVA: 0x003184D4 File Offset: 0x003168D4
		public static ThingDef GetProjectile(this Verb verb)
		{
			Verb_LaunchProjectile verb_LaunchProjectile = verb as Verb_LaunchProjectile;
			return (verb_LaunchProjectile == null) ? null : verb_LaunchProjectile.Projectile;
		}

		// Token: 0x0600622E RID: 25134 RVA: 0x00318504 File Offset: 0x00316904
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

		// Token: 0x0600622F RID: 25135 RVA: 0x0031855C File Offset: 0x0031695C
		public static bool IsIncendiary(this Verb verb)
		{
			ThingDef projectile = verb.GetProjectile();
			return projectile != null && projectile.projectile.ai_IsIncendiary;
		}

		// Token: 0x06006230 RID: 25136 RVA: 0x0031858C File Offset: 0x0031698C
		public static bool ProjectileFliesOverhead(this Verb verb)
		{
			ThingDef projectile = verb.GetProjectile();
			return projectile != null && projectile.projectile.flyOverhead;
		}

		// Token: 0x06006231 RID: 25137 RVA: 0x003185BC File Offset: 0x003169BC
		public static bool HarmsHealth(this Verb verb)
		{
			DamageDef damageDef = verb.GetDamageDef();
			return damageDef != null && damageDef.harmsHealth;
		}

		// Token: 0x06006232 RID: 25138 RVA: 0x003185E8 File Offset: 0x003169E8
		public static bool IsEMP(this Verb verb)
		{
			return verb.GetDamageDef() == DamageDefOf.EMP;
		}

		// Token: 0x06006233 RID: 25139 RVA: 0x0031860C File Offset: 0x00316A0C
		public static bool UsesExplosiveProjectiles(this Verb verb)
		{
			ThingDef projectile = verb.GetProjectile();
			return projectile != null && projectile.projectile.explosionRadius > 0f;
		}

		// Token: 0x06006234 RID: 25140 RVA: 0x00318644 File Offset: 0x00316A44
		public static string GenerateBeatFireLoadId(Pawn pawn)
		{
			return string.Format("{0}_BeatFire", pawn.ThingID);
		}

		// Token: 0x06006235 RID: 25141 RVA: 0x0031866C File Offset: 0x00316A6C
		public static string GenerateIgniteLoadId(Pawn pawn)
		{
			return string.Format("{0}_Ignite", pawn.ThingID);
		}

		// Token: 0x06006236 RID: 25142 RVA: 0x00318694 File Offset: 0x00316A94
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

		// Token: 0x06006237 RID: 25143 RVA: 0x00318738 File Offset: 0x00316B38
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

		// Token: 0x06006238 RID: 25144 RVA: 0x003187A0 File Offset: 0x00316BA0
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
	}
}
