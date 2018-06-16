using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000026 RID: 38
	public class BillStack : IExposable
	{
		// Token: 0x0600015B RID: 347 RVA: 0x0000DC8B File Offset: 0x0000C08B
		public BillStack(IBillGiver giver)
		{
			this.billGiver = giver;
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600015C RID: 348 RVA: 0x0000DCB0 File Offset: 0x0000C0B0
		public List<Bill> Bills
		{
			get
			{
				return this.bills;
			}
		}

		// Token: 0x0600015D RID: 349 RVA: 0x0000DCCC File Offset: 0x0000C0CC
		public IEnumerator<Bill> GetEnumerator()
		{
			return this.bills.GetEnumerator();
		}

		// Token: 0x1700004A RID: 74
		public Bill this[int index]
		{
			get
			{
				return this.bills[index];
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x0600015F RID: 351 RVA: 0x0000DD18 File Offset: 0x0000C118
		public int Count
		{
			get
			{
				return this.bills.Count;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000160 RID: 352 RVA: 0x0000DD38 File Offset: 0x0000C138
		public Bill FirstShouldDoNow
		{
			get
			{
				for (int i = 0; i < this.Count; i++)
				{
					if (this.bills[i].ShouldDoNow())
					{
						return this.bills[i];
					}
				}
				return null;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000161 RID: 353 RVA: 0x0000DD90 File Offset: 0x0000C190
		public bool AnyShouldDoNow
		{
			get
			{
				for (int i = 0; i < this.Count; i++)
				{
					if (this.bills[i].ShouldDoNow())
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06000162 RID: 354 RVA: 0x0000DDDC File Offset: 0x0000C1DC
		public void AddBill(Bill bill)
		{
			bill.billStack = this;
			this.bills.Add(bill);
		}

		// Token: 0x06000163 RID: 355 RVA: 0x0000DDF2 File Offset: 0x0000C1F2
		public void Delete(Bill bill)
		{
			bill.deleted = true;
			this.bills.Remove(bill);
		}

		// Token: 0x06000164 RID: 356 RVA: 0x0000DE09 File Offset: 0x0000C209
		public void Clear()
		{
			this.bills.Clear();
		}

		// Token: 0x06000165 RID: 357 RVA: 0x0000DE18 File Offset: 0x0000C218
		public void Reorder(Bill bill, int offset)
		{
			int num = this.bills.IndexOf(bill);
			num += offset;
			if (num >= 0)
			{
				this.bills.Remove(bill);
				this.bills.Insert(num, bill);
			}
		}

		// Token: 0x06000166 RID: 358 RVA: 0x0000DE5C File Offset: 0x0000C25C
		public void RemoveIncompletableBills()
		{
			for (int i = this.bills.Count - 1; i >= 0; i--)
			{
				if (!this.bills[i].CompletableEver)
				{
					this.bills.Remove(this.bills[i]);
				}
			}
		}

		// Token: 0x06000167 RID: 359 RVA: 0x0000DEB8 File Offset: 0x0000C2B8
		public int IndexOf(Bill bill)
		{
			return this.bills.IndexOf(bill);
		}

		// Token: 0x06000168 RID: 360 RVA: 0x0000DEDC File Offset: 0x0000C2DC
		public void ExposeData()
		{
			Scribe_Collections.Look<Bill>(ref this.bills, "bills", LookMode.Deep, new object[0]);
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				for (int i = 0; i < this.bills.Count; i++)
				{
					this.bills[i].billStack = this;
				}
			}
		}

		// Token: 0x06000169 RID: 361 RVA: 0x0000DF40 File Offset: 0x0000C340
		public Bill DoListing(Rect rect, Func<List<FloatMenuOption>> recipeOptionsMaker, ref Vector2 scrollPosition, ref float viewHeight)
		{
			Bill result = null;
			GUI.BeginGroup(rect);
			Text.Font = GameFont.Small;
			if (this.Count < 15)
			{
				Rect rect2 = new Rect(0f, 0f, 150f, 29f);
				if (Widgets.ButtonText(rect2, "AddBill".Translate(), true, false, true))
				{
					Find.WindowStack.Add(new FloatMenu(recipeOptionsMaker()));
				}
				UIHighlighter.HighlightOpportunity(rect2, "AddBill");
			}
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
			Rect outRect = new Rect(0f, 35f, rect.width, rect.height - 35f);
			Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, viewHeight);
			Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect, true);
			float num = 0f;
			for (int i = 0; i < this.Count; i++)
			{
				Bill bill = this.bills[i];
				Rect rect3 = bill.DoInterface(0f, num, viewRect.width, i);
				if (!bill.DeletedOrDereferenced && Mouse.IsOver(rect3))
				{
					result = bill;
				}
				num += rect3.height + 6f;
			}
			if (Event.current.type == EventType.Layout)
			{
				viewHeight = num + 60f;
			}
			Widgets.EndScrollView();
			GUI.EndGroup();
			return result;
		}

		// Token: 0x04000199 RID: 409
		[Unsaved]
		public IBillGiver billGiver = null;

		// Token: 0x0400019A RID: 410
		private List<Bill> bills = new List<Bill>();

		// Token: 0x0400019B RID: 411
		public const int MaxCount = 15;

		// Token: 0x0400019C RID: 412
		private const float TopAreaHeight = 35f;

		// Token: 0x0400019D RID: 413
		private const float BillInterfaceSpacing = 6f;

		// Token: 0x0400019E RID: 414
		private const float ExtraViewHeight = 60f;
	}
}
