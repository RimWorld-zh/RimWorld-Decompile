using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020006EC RID: 1772
	public class ThingSetMaker_RandomOption : ThingSetMaker
	{
		// Token: 0x0400157F RID: 5503
		public List<ThingSetMaker_RandomOption.Option> options;

		// Token: 0x06002696 RID: 9878 RVA: 0x0014A9B4 File Offset: 0x00148DB4
		protected override bool CanGenerateSub(ThingSetMakerParams parms)
		{
			for (int i = 0; i < this.options.Count; i++)
			{
				if (this.options[i].thingSetMaker.CanGenerate(parms) && this.GetSelectionWeight(this.options[i]) > 0f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002697 RID: 9879 RVA: 0x0014AA28 File Offset: 0x00148E28
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			ThingSetMaker_RandomOption.Option option;
			if ((from x in this.options
			where x.thingSetMaker.CanGenerate(parms)
			select x).TryRandomElementByWeight(new Func<ThingSetMaker_RandomOption.Option, float>(this.GetSelectionWeight), out option))
			{
				outThings.AddRange(option.thingSetMaker.Generate(parms));
			}
		}

		// Token: 0x06002698 RID: 9880 RVA: 0x0014AA90 File Offset: 0x00148E90
		private float GetSelectionWeight(ThingSetMaker_RandomOption.Option option)
		{
			float? weightIfPlayerHasNoSuchItem = option.weightIfPlayerHasNoSuchItem;
			float result;
			if (weightIfPlayerHasNoSuchItem != null && !PlayerItemAccessibilityUtility.PlayerOrItemStashHas(option.thingSetMaker.fixedParams.filter))
			{
				result = option.weightIfPlayerHasNoSuchItem.Value;
			}
			else
			{
				result = option.weight;
			}
			return result;
		}

		// Token: 0x06002699 RID: 9881 RVA: 0x0014AAEC File Offset: 0x00148EEC
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			for (int i = 0; i < this.options.Count; i++)
			{
				this.options[i].thingSetMaker.ResolveReferences();
			}
		}

		// Token: 0x0600269A RID: 9882 RVA: 0x0014AB34 File Offset: 0x00148F34
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			for (int i = 0; i < this.options.Count; i++)
			{
				float weight = this.options[i].weight;
				float? weightIfPlayerHasNoSuchItem = this.options[i].weightIfPlayerHasNoSuchItem;
				if (weightIfPlayerHasNoSuchItem != null)
				{
					weight = Mathf.Max(weight, this.options[i].weightIfPlayerHasNoSuchItem.Value);
				}
				if (weight > 0f)
				{
					foreach (ThingDef t in this.options[i].thingSetMaker.AllGeneratableThingsDebug(parms))
					{
						yield return t;
					}
				}
			}
			yield break;
		}

		// Token: 0x020006ED RID: 1773
		public class Option
		{
			// Token: 0x04001580 RID: 5504
			public ThingSetMaker thingSetMaker;

			// Token: 0x04001581 RID: 5505
			public float weight;

			// Token: 0x04001582 RID: 5506
			public float? weightIfPlayerHasNoSuchItem;
		}
	}
}
