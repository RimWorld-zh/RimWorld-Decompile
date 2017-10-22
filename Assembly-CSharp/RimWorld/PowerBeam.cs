using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class PowerBeam : OrbitalStrike
	{
		public const float Radius = 15f;

		private const int FiresStartedPerTick = 3;

		private static readonly IntRange FlameDamageAmountRange = new IntRange(43, 95);

		private static readonly IntRange CorpseFlameDamageAmountRange = new IntRange(5, 10);

		private static List<Thing> tmpThings = new List<Thing>();

		public override void StartStrike()
		{
			base.StartStrike();
			MoteMaker.MakePowerBeamMote(base.Position, base.Map);
		}

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

		private void StartRandomFireAndDoFlameDamage()
		{
			IntVec3 c = (from x in GenRadial.RadialCellsAround(base.Position, 15f, true)
			where x.InBounds(base.Map)
			select x).RandomElementByWeight((Func<IntVec3, float>)((IntVec3 x) => (float)(1.0 - Mathf.Min((float)(x.DistanceTo(base.Position) / 15.0), 1f) + 0.05000000074505806)));
			FireUtility.TryStartFireIn(c, base.Map, Rand.Range(0.1f, 0.925f));
			PowerBeam.tmpThings.Clear();
			PowerBeam.tmpThings.AddRange(c.GetThingList(base.Map));
			for (int i = 0; i < PowerBeam.tmpThings.Count; i++)
			{
				int amount = (!(PowerBeam.tmpThings[i] is Corpse)) ? PowerBeam.FlameDamageAmountRange.RandomInRange : PowerBeam.CorpseFlameDamageAmountRange.RandomInRange;
				PowerBeam.tmpThings[i].TakeDamage(new DamageInfo(DamageDefOf.Flame, amount, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown));
			}
			PowerBeam.tmpThings.Clear();
		}
	}
}
