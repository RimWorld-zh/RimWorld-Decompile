using System;
using System.Collections;
using UnityEngine;

namespace RimWorld
{
	public static class PriceTypeUtlity
	{
		public static float PriceMultiplier(this PriceType pType)
		{
			float result;
			switch (pType)
			{
			case PriceType.VeryCheap:
			{
				result = 0.4f;
				break;
			}
			case PriceType.Cheap:
			{
				result = 0.7f;
				break;
			}
			case PriceType.Normal:
			{
				result = 1f;
				break;
			}
			case PriceType.Expensive:
			{
				result = 2f;
				break;
			}
			case PriceType.Exorbitant:
			{
				result = 5f;
				break;
			}
			default:
			{
				result = -1f;
				break;
			}
			}
			return result;
		}

		public static PriceType ClosestPriceType(float priceFactor)
		{
			float num = 99999f;
			PriceType priceType = PriceType.Undefined;
			IEnumerator enumerator = Enum.GetValues(typeof(PriceType)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					PriceType priceType2 = (PriceType)enumerator.Current;
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
