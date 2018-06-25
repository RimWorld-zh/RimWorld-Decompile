using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020008E4 RID: 2276
	public class WITab_Caravan_Items : WITab
	{
		// Token: 0x04001C4B RID: 7243
		private Vector2 scrollPosition;

		// Token: 0x04001C4C RID: 7244
		private float scrollViewHeight;

		// Token: 0x04001C4D RID: 7245
		private TransferableSorterDef sorter1;

		// Token: 0x04001C4E RID: 7246
		private TransferableSorterDef sorter2;

		// Token: 0x04001C4F RID: 7247
		private List<TransferableImmutable> cachedItems = new List<TransferableImmutable>();

		// Token: 0x04001C50 RID: 7248
		private int cachedItemsHash;

		// Token: 0x04001C51 RID: 7249
		private int cachedItemsCount;

		// Token: 0x04001C52 RID: 7250
		private const float SortersSpace = 25f;

		// Token: 0x04001C53 RID: 7251
		private const float AssignDrugPoliciesButtonHeight = 27f;

		// Token: 0x06003450 RID: 13392 RVA: 0x001BFF7E File Offset: 0x001BE37E
		public WITab_Caravan_Items()
		{
			this.labelKey = "TabCaravanItems";
		}

		// Token: 0x06003451 RID: 13393 RVA: 0x001BFFA0 File Offset: 0x001BE3A0
		protected override void FillTab()
		{
			this.CheckCreateSorters();
			Rect rect = new Rect(0f, 0f, this.size.x, this.size.y);
			if (Widgets.ButtonText(new Rect(rect.x + 10f, rect.y + 10f, 200f, 27f), "AssignDrugPolicies".Translate(), true, false, true))
			{
				Find.WindowStack.Add(new Dialog_AssignCaravanDrugPolicies(base.SelCaravan));
			}
			rect.yMin += 37f;
			GUI.BeginGroup(rect.ContractedBy(10f));
			TransferableUIUtility.DoTransferableSorters(this.sorter1, this.sorter2, delegate(TransferableSorterDef x)
			{
				this.sorter1 = x;
				this.CacheItems();
			}, delegate(TransferableSorterDef x)
			{
				this.sorter2 = x;
				this.CacheItems();
			});
			GUI.EndGroup();
			rect.yMin += 25f;
			GUI.BeginGroup(rect);
			this.CheckCacheItems();
			CaravanItemsTabUtility.DoRows(rect.size, this.cachedItems, base.SelCaravan, ref this.scrollPosition, ref this.scrollViewHeight);
			GUI.EndGroup();
		}

		// Token: 0x06003452 RID: 13394 RVA: 0x001C00CD File Offset: 0x001BE4CD
		protected override void UpdateSize()
		{
			base.UpdateSize();
			this.CheckCacheItems();
			this.size = CaravanItemsTabUtility.GetSize(this.cachedItems, this.PaneTopY, true);
		}

		// Token: 0x06003453 RID: 13395 RVA: 0x001C00F4 File Offset: 0x001BE4F4
		private void CheckCacheItems()
		{
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(base.SelCaravan);
			if (list.Count != this.cachedItemsCount)
			{
				this.CacheItems();
			}
			else
			{
				int num = 0;
				for (int i = 0; i < list.Count; i++)
				{
					num = Gen.HashCombineInt(num, list[i].GetHashCode());
				}
				if (num != this.cachedItemsHash)
				{
					this.CacheItems();
				}
			}
		}

		// Token: 0x06003454 RID: 13396 RVA: 0x001C016C File Offset: 0x001BE56C
		private void CacheItems()
		{
			this.CheckCreateSorters();
			this.cachedItems.Clear();
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(base.SelCaravan);
			int seed = 0;
			for (int i = 0; i < list.Count; i++)
			{
				TransferableImmutable transferableImmutable = TransferableUtility.TransferableMatching<TransferableImmutable>(list[i], this.cachedItems, TransferAsOneMode.Normal);
				if (transferableImmutable == null)
				{
					transferableImmutable = new TransferableImmutable();
					this.cachedItems.Add(transferableImmutable);
				}
				transferableImmutable.things.Add(list[i]);
				seed = Gen.HashCombineInt(seed, list[i].GetHashCode());
			}
			this.cachedItems = this.cachedItems.OrderBy((TransferableImmutable tr) => tr, this.sorter1.Comparer).ThenBy((TransferableImmutable tr) => tr, this.sorter2.Comparer).ThenBy((TransferableImmutable tr) => TransferableUIUtility.DefaultListOrderPriority(tr)).ToList<TransferableImmutable>();
			this.cachedItemsCount = list.Count;
			this.cachedItemsHash = seed;
		}

		// Token: 0x06003455 RID: 13397 RVA: 0x001C02A5 File Offset: 0x001BE6A5
		private void CheckCreateSorters()
		{
			if (this.sorter1 == null)
			{
				this.sorter1 = TransferableSorterDefOf.Category;
			}
			if (this.sorter2 == null)
			{
				this.sorter2 = TransferableSorterDefOf.MarketValue;
			}
		}
	}
}
