using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	public static class VerbUtility
	{
		public static ThingDef GetProjectile(this Verb verb)
		{
			Verb_LaunchProjectile verb_LaunchProjectile = verb as Verb_LaunchProjectile;
			return (verb_LaunchProjectile == null) ? null : verb_LaunchProjectile.Projectile;
		}

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

		public static bool IsIncendiary(this Verb verb)
		{
			ThingDef projectile = verb.GetProjectile();
			return projectile != null && projectile.projectile.ai_IsIncendiary;
		}

		public static bool ProjectileFliesOverhead(this Verb verb)
		{
			ThingDef projectile = verb.GetProjectile();
			return projectile != null && projectile.projectile.flyOverhead;
		}

		public static bool HarmsHealth(this Verb verb)
		{
			DamageDef damageDef = verb.GetDamageDef();
			return damageDef != null && damageDef.harmsHealth;
		}

		public static bool IsEMP(this Verb verb)
		{
			return verb.GetDamageDef() == DamageDefOf.EMP;
		}

		public static bool UsesExplosiveProjectiles(this Verb verb)
		{
			ThingDef projectile = verb.GetProjectile();
			return projectile != null && projectile.projectile.explosionRadius > 0f;
		}

		public static string GenerateBeatFireLoadId(Pawn pawn)
		{
			return string.Format("{0}_BeatFire", pawn.ThingID);
		}

		public static string GenerateIgniteLoadId(Pawn pawn)
		{
			return string.Format("{0}_Ignite", pawn.ThingID);
		}

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

		public static float InterceptChanceFactorFromDistance(Vector3 origin, IntVec3 c)
		{
			float num = (c.ToVector3Shifted() - origin).MagnitudeHorizontalSquared();
			float result;
			if (num <= 25f)
			{
				result = 0f;
			}
			else if (num >= 144f)
			{
				result = 1f;
			}
			else
			{
				result = Mathf.InverseLerp(25f, 144f, num);
			}
			return result;
		}
	}
}
