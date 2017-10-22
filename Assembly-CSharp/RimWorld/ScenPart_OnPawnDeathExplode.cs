using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class ScenPart_OnPawnDeathExplode : ScenPart
	{
		private float radius = 5.9f;

		private DamageDef damage;

		private string radiusBuf;

		public override void Randomize()
		{
			this.radius = (float)((float)Rand.RangeInclusive(3, 8) - 0.10000000149011612);
			this.damage = this.PossibleDamageDefs().RandomElement();
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.radius, "radius", 0f, false);
			Scribe_Defs.Look<DamageDef>(ref this.damage, "damage");
		}

		public override string Summary(Scenario scen)
		{
			return "ScenPart_OnPawnDeathExplode".Translate(this.damage.label, this.radius.ToString());
		}

		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, (float)(ScenPart.RowHeight * 2.0));
			Widgets.TextFieldNumericLabeled<float>(scenPartRect.TopHalf(), "radius".Translate(), ref this.radius, ref this.radiusBuf, 0f, 1E+09f);
			if (Widgets.ButtonText(scenPartRect.BottomHalf(), this.damage.LabelCap, true, false, true))
			{
				FloatMenuUtility.MakeMenu(this.PossibleDamageDefs(), (Func<DamageDef, string>)((DamageDef d) => d.LabelCap), (Func<DamageDef, Action>)((DamageDef d) => (Action)delegate()
				{
					this.damage = d;
				}));
			}
		}

		public override void Notify_PawnDied(Corpse corpse)
		{
			if (corpse.Spawned)
			{
				GenExplosion.DoExplosion(corpse.Position, corpse.Map, this.radius, this.damage, null, null, null, null, null, 0f, 1, false, null, 0f, 1);
			}
		}

		private IEnumerable<DamageDef> PossibleDamageDefs()
		{
			yield return DamageDefOf.Bomb;
			yield return DamageDefOf.Flame;
		}
	}
}
