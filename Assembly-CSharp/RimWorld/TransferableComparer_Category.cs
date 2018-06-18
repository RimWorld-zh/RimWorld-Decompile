using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020008AE RID: 2222
	public class TransferableComparer_Category : TransferableComparer
	{
		// Token: 0x060032D8 RID: 13016 RVA: 0x001B6364 File Offset: 0x001B4764
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return TransferableComparer_Category.Compare(lhs.ThingDef, rhs.ThingDef);
		}

		// Token: 0x060032D9 RID: 13017 RVA: 0x001B638C File Offset: 0x001B478C
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
