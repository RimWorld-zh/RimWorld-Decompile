using System;
using Verse;

namespace RimWorld
{
	public class TransferableComparer_Category : TransferableComparer
	{
		public TransferableComparer_Category()
		{
		}

		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return TransferableComparer_Category.Compare(lhs.ThingDef, rhs.ThingDef);
		}

		public static int Compare(ThingDef lhsTh, ThingDef rhsTh)
		{
			int result;
			if (lhsTh.category != rhsTh.category)
			{
				result = lhsTh.category.CompareTo(rhsTh.category);
			}
			else
			{
				float num = TransferableUIUtility.DefaultListOrderPriority(lhsTh);
				float num2 = TransferableUIUtility.DefaultListOrderPriority(rhsTh);
				if (num != num2)
				{
					result = num.CompareTo(num2);
				}
				else
				{
					int num3 = 0;
					if (!lhsTh.thingCategories.NullOrEmpty<ThingCategoryDef>())
					{
						num3 = (int)lhsTh.thingCategories[0].index;
					}
					int value = 0;
					if (!rhsTh.thingCategories.NullOrEmpty<ThingCategoryDef>())
					{
						value = (int)rhsTh.thingCategories[0].index;
					}
					result = num3.CompareTo(value);
				}
			}
			return result;
		}
	}
}
