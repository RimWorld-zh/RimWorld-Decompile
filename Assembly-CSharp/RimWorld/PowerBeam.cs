using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020006BB RID: 1723
	public class PowerBeam : OrbitalStrike
	{
		// Token: 0x04001482 RID: 5250
		public const float Radius = 15f;

		// Token: 0x04001483 RID: 5251
		private const int FiresStartedPerTick = 3;

		// Token: 0x04001484 RID: 5252
		private static readonly IntRange FlameDamageAmountRange = new IntRange(43, 95);

		// Token: 0x04001485 RID: 5253
		private static readonly IntRange CorpseFlameDamageAmountRange = new IntRange(5, 10);

		// Token: 0x04001486 RID: 5254
		private static List<Thing> tmpThings = new List<Thing>();

		// Token: 0x06002517 RID: 9495 RVA: 0x0013E7F8 File Offset: 0x0013CBF8
		public override void StartStrike()
		{
			base.StartStrike();
			MoteMaker.MakePowerBeamMote(base.Position, base.Map);
		}

		// Token: 0x06002518 RID: 9496 RVA: 0x0013E814 File Offset: 0x0013CC14
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

		// Token: 0x06002519 RID: 9497 RVA: 0x0013E854 File Offset: 0x0013CC54
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
	}
}
