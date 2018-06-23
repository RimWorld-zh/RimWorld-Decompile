using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020008E2 RID: 2274
	public class WITab_Caravan_Items : WITab
	{
		// Token: 0x04001C45 RID: 7237
		private Vector2 scrollPosition;

		// Token: 0x04001C46 RID: 7238
		private float scrollViewHeight;

		// Token: 0x04001C47 RID: 7239
		private TransferableSorterDef sorter1;

		// Token: 0x04001C48 RID: 7240
		private TransferableSorterDef sorter2;

		// Token: 0x04001C49 RID: 7241
		private List<TransferableImmutable> cachedItems = new List<TransferableImmutable>();

		// Token: 0x04001C4A RID: 7242
		private int cachedItemsHash;

		// Token: 0x04001C4B RID: 7243
		private int cachedItemsCount;

		// Token: 0x04001C4C RID: 7244
		private const float SortersSpace = 25f;

		// Token: 0x04001C4D RID: 7245
		private const float AssignDrugPoliciesButtonHeight = 27f;

		// Token: 0x0600344C RID: 13388 RVA: 0x001BFB6A File Offset: 0x001BDF6A
		public WITab_Caravan_Items()
		{
			this.labelKey = "TabCaravanItems";
		}

		// Token: 0x0600344D RID: 13389 RVA: 0x001BFB8C File Offset: 0x001BDF8C
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

		// Token: 0x0600344E RID: 13390 RVA: 0x001BFCB9 File Offset: 0x001BE0B9
		protected override void UpdateSize()
		{
			base.UpdateSize();
			this.CheckCacheItems();
			this.size = CaravanItemsTabUtility.GetSize(this.cachedItems, this.PaneTopY, true);
		}

		// Token: 0x0600344F RID: 13391 RVA: 0x001BFCE0 File Offset: 0x001BE0E0
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

		// Token: 0x06003450 RID: 13392 RVA: 0x001BFD58 File Offset: 0x001BE158
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

		// Token: 0x06003451 RID: 13393 RVA: 0x001BFE91 File Offset: 0x001BE291
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
