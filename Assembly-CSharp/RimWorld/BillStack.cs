using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class BillStack : IExposable
	{
		public const int MaxCount = 15;

		private const float TopAreaHeight = 35f;

		private const float BillInterfaceSpacing = 6f;

		private const float ExtraViewHeight = 60f;

		[Unsaved]
		public IBillGiver billGiver;

		private List<Bill> bills = new List<Bill>();

		public List<Bill> Bills
		{
			get
			{
				return this.bills;
			}
		}

		public Bill this[int index]
		{
			get
			{
				return this.bills[index];
			}
		}

		public int Count
		{
			get
			{
				return this.bills.Count;
			}
		}

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

		public BillStack(IBillGiver giver)
		{
			this.billGiver = giver;
		}

		public IEnumerator<Bill> GetEnumerator()
		{
			return (IEnumerator<Bill>)(object)this.bills.GetEnumerator();
		}

		public void AddBill(Bill bill)
		{
			bill.billStack = this;
			this.bills.Add(bill);
		}

		public void Delete(Bill bill)
		{
			bill.deleted = true;
			this.bills.Remove(bill);
		}

		public void Clear()
		{
			this.bills.Clear();
		}

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

		public void RemoveIncompletableBills()
		{
			for (int num = this.bills.Count - 1; num >= 0; num--)
			{
				if (!this.bills[num].CompletableEver)
				{
					this.bills.Remove(this.bills[num]);
				}
			}
		}

		public int IndexOf(Bill bill)
		{
			return this.bills.IndexOf(bill);
		}

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
			Rect outRect = new Rect(0f, 35f, rect.width, (float)(rect.height - 35.0));
			Rect viewRect = new Rect(0f, 0f, (float)(outRect.width - 16.0), viewHeight);
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
				num = (float)(num + (rect3.height + 6.0));
			}
			if (Event.current.type == EventType.Layout)
			{
				viewHeight = (float)(num + 60.0);
			}
			Widgets.EndScrollView();
			GUI.EndGroup();
			return result;
		}
	}
}
