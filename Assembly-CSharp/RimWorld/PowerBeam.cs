using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020006BD RID: 1725
	public class PowerBeam : OrbitalStrike
	{
		// Token: 0x0600251A RID: 9498 RVA: 0x0013E280 File Offset: 0x0013C680
		public override void StartStrike()
		{
			base.StartStrike();
			MoteMaker.MakePowerBeamMote(base.Position, base.Map);
		}

		// Token: 0x0600251B RID: 9499 RVA: 0x0013E29C File Offset: 0x0013C69C
		public override void Tick()
		{
			base.Tick();
			if (!base.Destroyed)
			{
				for (int i = 0; i < 3; i++)
				{
					this.StartRandomFireAndDoFlameDamage();
				}
			}
		}

		// Token: 0x0600251C RID: 9500 RVA: 0x0013E2DC File Offset: 0x0013C6DC
		private void StartRandomFireAndDoFlameDamage()
		{
			IntVec3 c = (from x in GenRadial.RadialCellsAround(base.Position, 15f, true)
			where x.InBounds(base.Map)
			select x).RandomElementByWeight((IntVec3 x) => 1f - Mathf.Min(x.DistanceTo(base.Position) / 15f, 1f) + 0.05f);
			FireUtility.TryStartFireIn(c, base.Map, Rand.Range(0.1f, 0.925f));
			PowerBeam.tmpThings.Clear();
			PowerBeam.tmpThings.AddRange(c.GetThingList(base.Map));
			for (int i = 0; i < PowerBeam.tmpThings.Count; i++)
			{
				int num = (!(PowerBeam.tmpThings[i] is Corpse)) ? PowerBeam.FlameDamageAmountRange.RandomInRange : PowerBeam.CorpseFlameDamageAmountRange.RandomInRange;
				Pawn pawn = PowerBeam.tmpThings[i] as Pawn;
				BattleLogEntry_DamageTaken battleLogEntry_DamageTaken = null;
				if (pawn != null)
				{
					battleLogEntry_DamageTaken = new BattleLogEntry_DamageTaken(pawn, RulePackDefOf.DamageEvent_PowerBeam, this.instigator as Pawn);
					Find.BattleLog.Add(battleLogEntry_DamageTaken);
				}
				Thing thing = PowerBeam.tmpThings[i];
				DamageDef flame = DamageDefOf.Flame;
				float amount = (float)num;
				Thing instigator = this.instigator;
				thing.TakeDamage(new DamageInfo(flame, amount, -1f, instigator, null, this.weaponDef, DamageInfo.SourceCategory.ThingOrUnknown, null)).AssociateWithLog(battleLogEntry_DamageTaken);
			}
			PowerBeam.tmpThings.Clear();
		}

		// Token: 0x04001480 RID: 5248
		public const float Radius = 15f;

		// Token: 0x04001481 RID: 5249
		private const int FiresStartedPerTick = 3;

		// Token: 0x04001482 RID: 5250
		private static readonly IntRange FlameDamageAmountRange = new IntRange(43, 95);

		// Token: 0x04001483 RID: 5251
		private static readonly IntRange CorpseFlameDamageAmountRange = new IntRange(5, 10);

		// Token: 0x04001484 RID: 5252
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
