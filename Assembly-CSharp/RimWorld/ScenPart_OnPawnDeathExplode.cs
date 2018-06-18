using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000637 RID: 1591
	public class ScenPart_OnPawnDeathExplode : ScenPart
	{
		// Token: 0x060020CE RID: 8398 RVA: 0x001188EF File Offset: 0x00116CEF
		public override void Randomize()
		{
			this.radius = (float)Rand.RangeInclusive(3, 8) - 0.1f;
			this.damage = this.PossibleDamageDefs().RandomElement<DamageDef>();
		}

		// Token: 0x060020CF RID: 8399 RVA: 0x00118917 File Offset: 0x00116D17
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.radius, "radius", 0f, false);
			Scribe_Defs.Look<DamageDef>(ref this.damage, "damage");
		}

		// Token: 0x060020D0 RID: 8400 RVA: 0x00118948 File Offset: 0x00116D48
		public override string Summary(Scenario scen)
		{
			return "ScenPart_OnPawnDeathExplode".Translate(new object[]
			{
				this.damage.label,
				this.radius.ToString()
			});
		}

		// Token: 0x060020D1 RID: 8401 RVA: 0x00118990 File Offset: 0x00116D90
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 2f);
			Widgets.TextFieldNumericLabeled<float>(scenPartRect.TopHalf(), "radius".Translate(), ref this.radius, ref this.radiusBuf, 0f, 1E+09f);
			if (Widgets.ButtonText(scenPartRect.BottomHalf(), this.damage.LabelCap, true, false, true))
			{
				FloatMenuUtility.MakeMenu<DamageDef>(this.PossibleDamageDefs(), (DamageDef d) => d.LabelCap, (DamageDef d) => delegate()
				{
					this.damage = d;
				});
			}
		}

		// Token: 0x060020D2 RID: 8402 RVA: 0x00118A30 File Offset: 0x00116E30
		public override void Notify_PawnDied(Corpse corpse)
		{
			if (corpse.Spawned)
			{
				GenExplosion.DoExplosion(corpse.Position, corpse.Map, this.radius, this.damage, null, -1, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false);
			}
		}

		// Token: 0x060020D3 RID: 8403 RVA: 0x00118A84 File Offset: 0x00116E84
		private IEnumerable<DamageDef> PossibleDamageDefs()
		{
			yield return DamageDefOf.Bomb;
			yield return DamageDefOf.Flame;
			yield break;
		}

		// Token: 0x040012C7 RID: 4807
		private float radius = 5.9f;

		// Token: 0x040012C8 RID: 4808
		private DamageDef damage;

		// Token: 0x040012C9 RID: 4809
		private string radiusBuf;
	}
}
