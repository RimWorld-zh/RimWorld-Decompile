using RimWorld;
using System.Collections.Generic;

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
				result = ((projectile == null) ? null : projectile.projectile.damageDef);
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
				result = ((!(concreteExample is Pawn)) ? ((!(concreteExample is ThingWithComps)) ? null : (concreteExample as ThingWithComps).GetComp<CompEquippable>().AllVerbs) : (concreteExample as Pawn).VerbTracker.AllVerbs);
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
	}
}
