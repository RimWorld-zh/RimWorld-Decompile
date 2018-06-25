using System;
using System.Collections;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x020008AA RID: 2218
	public static class PriceTypeUtlity
	{
		// Token: 0x060032CE RID: 13006 RVA: 0x001B6258 File Offset: 0x001B4658
		public static float PriceMultiplier(this PriceType pType)
		{
			float result;
			switch (pType)
			{
			case PriceType.VeryCheap:
				result = 0.4f;
				break;
			case PriceType.Cheap:
				result = 0.7f;
				break;
			case PriceType.Normal:
				result = 1f;
				break;
			case PriceType.Expensive:
				result = 2f;
				break;
			case PriceType.Exorbitant:
				result = 5f;
				break;
			default:
				result = -1f;
				break;
			}
			return result;
		}

		// Token: 0x060032CF RID: 13007 RVA: 0x001B62CC File Offset: 0x001B46CC
		public static PriceType ClosestPriceType(float priceFactor)
		{
			float num = 99999f;
			PriceType priceType = PriceType.Undefined;
			IEnumerator enumerator = Enum.GetValues(typeof(PriceType)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					PriceType priceType2 = (PriceType)obj;
					float num2 = Mathf.Abs(priceFactor - priceType2.PriceMultiplier());
					if (num2 < num)
					{
						num = num2;
						priceType = priceType2;
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			if (priceType == PriceType.Undefined)
			{
				priceType = PriceType.VeryCheap;
			}
			return priceType;
		}
	}
}
