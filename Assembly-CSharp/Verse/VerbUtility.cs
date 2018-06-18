using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000FD2 RID: 4050
	public static class VerbUtility
	{
		// Token: 0x060061F5 RID: 25077 RVA: 0x0031575C File Offset: 0x00313B5C
		public static ThingDef GetProjectile(this Verb verb)
		{
			Verb_LaunchProjectile verb_LaunchProjectile = verb as Verb_LaunchProjectile;
			return (verb_LaunchProjectile == null) ? null : verb_LaunchProjectile.Projectile;
		}

		// Token: 0x060061F6 RID: 25078 RVA: 0x0031578C File Offset: 0x00313B8C
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

		// Token: 0x060061F7 RID: 25079 RVA: 0x003157E4 File Offset: 0x00313BE4
		public static bool IsIncendiary(this Verb verb)
		{
			ThingDef projectile = verb.GetProjectile();
			return projectile != null && projectile.projectile.ai_IsIncendiary;
		}

		// Token: 0x060061F8 RID: 25080 RVA: 0x00315814 File Offset: 0x00313C14
		public static bool ProjectileFliesOverhead(this Verb verb)
		{
			ThingDef projectile = verb.GetProjectile();
			return projectile != null && projectile.projectile.flyOverhead;
		}

		// Token: 0x060061F9 RID: 25081 RVA: 0x00315844 File Offset: 0x00313C44
		public static bool HarmsHealth(this Verb verb)
		{
			DamageDef damageDef = verb.GetDamageDef();
			return damageDef != null && damageDef.harmsHealth;
		}

		// Token: 0x060061FA RID: 25082 RVA: 0x00315870 File Offset: 0x00313C70
		public static bool IsEMP(this Verb verb)
		{
			return verb.GetDamageDef() == DamageDefOf.EMP;
		}

		// Token: 0x060061FB RID: 25083 RVA: 0x00315894 File Offset: 0x00313C94
		public static bool UsesExplosiveProjectiles(this Verb verb)
		{
			ThingDef projectile = verb.GetProjectile();
			return projectile != null && projectile.projectile.explosionRadius > 0f;
		}

		// Token: 0x060061FC RID: 25084 RVA: 0x003158CC File Offset: 0x00313CCC
		public static string GenerateBeatFireLoadId(Pawn pawn)
		{
			return string.Format("{0}_BeatFire", pawn.ThingID);
		}

		// Token: 0x060061FD RID: 25085 RVA: 0x003158F4 File Offset: 0x00313CF4
		public static string GenerateIgniteLoadId(Pawn pawn)
		{
			return string.Format("{0}_Ignite", pawn.ThingID);
		}

		// Token: 0x060061FE RID: 25086 RVA: 0x0031591C File Offset: 0x00313D1C
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

		// Token: 0x060061FF RID: 25087 RVA: 0x003159C0 File Offset: 0x00313DC0
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

		// Token: 0x06006200 RID: 25088 RVA: 0x00315A28 File Offset: 0x00313E28
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

		// Token: 0x04004001 RID: 16385
		public const float InterceptDist_Possible = 4f;

		// Token: 0x04004002 RID: 16386
		private const float InterceptDist_Short = 7f;

		// Token: 0x04004003 RID: 16387
		private const float InterceptDist_Normal = 10f;

		// Token: 0x04004004 RID: 16388
		private const float InterceptChanceFactor_VeryShort = 0.5f;

		// Token: 0x04004005 RID: 16389
		private const float InterceptChanceFactor_Short = 0.75f;
	}
}
