using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class CompProperties_Explosive : CompProperties
	{
		public float explosiveRadius = 1.9f;

		public DamageDef explosiveDamageType = DamageDefOf.Bomb;

		public int damageAmountBase = -1;

		public ThingDef postExplosionSpawnThingDef;

		public float postExplosionSpawnChance;

		public int postExplosionSpawnThingCount = 1;

		public bool applyDamageToExplosionCellsNeighbors;

		public ThingDef preExplosionSpawnThingDef;

		public float preExplosionSpawnChance;

		public int preExplosionSpawnThingCount = 1;

		public float chanceToStartFire;

		public bool dealMoreDamageAtCenter;

		public float explosiveExpandPerStackcount;

		public float explosiveExpandPerFuel;

		public EffecterDef explosionEffect;

		public SoundDef explosionSound;

		public DamageDef startWickOnDamageTaken;

		public float startWickHitPointsPercent = 0.2f;

		public IntRange wickTicks = new IntRange(140, 150);

		public float wickScale = 1f;

		public float chanceNeverExplodeFromDamage;

		public float destroyThingOnExplosionSize;

		public CompProperties_Explosive()
		{
			base.compClass = typeof(CompExplosive);
		}

		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			using (IEnumerator<string> enumerator = base.ConfigErrors(parentDef).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					string e = enumerator.Current;
					yield return e;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (parentDef.tickerType == TickerType.Normal)
				yield break;
			yield return "CompExplosive requires Normal ticker type";
			/*Error: Unable to find new state assignment for yield return*/;
			IL_00f3:
			/*Error near IL_00f4: Unexpected return in MoveNext()*/;
		}
	}
}
