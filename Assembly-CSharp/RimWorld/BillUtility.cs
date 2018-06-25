using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	public static class BillUtility
	{
		public static Bill Clipboard = null;

		public static void TryDrawIngredientSearchRadiusOnMap(this Bill bill, IntVec3 center)
		{
			if (bill.ingredientSearchRadius < GenRadial.MaxRadialPatternRadius)
			{
				GenDraw.DrawRadiusRing(center, bill.ingredientSearchRadius);
			}
		}

		public static Bill MakeNewBill(this RecipeDef recipe)
		{
			Bill result;
			if (recipe.UsesUnfinishedThing)
			{
				result = new Bill_ProductionWithUft(recipe);
			}
			else
			{
				result = new Bill_Production(recipe);
			}
			return result;
		}

		public static IEnumerable<IBillGiver> GlobalBillGivers()
		{
			foreach (Map map in Find.Maps)
			{
				foreach (Thing thing in map.listerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.PotentialBillGiver)))
				{
					IBillGiver billgiver = thing as IBillGiver;
					if (billgiver == null)
					{
						Log.ErrorOnce("Found non-bill-giver tagged as PotentialBillGiver", 13389774, false);
					}
					else
					{
						yield return billgiver;
					}
				}
				foreach (Thing thing2 in map.listerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.MinifiedThing)))
				{
					IBillGiver billgiver2 = thing2.GetInnerIfMinified() as IBillGiver;
					if (billgiver2 != null)
					{
						yield return billgiver2;
					}
				}
			}
			foreach (Caravan caravan in Find.WorldObjects.Caravans)
			{
				foreach (Thing thing3 in caravan.AllThings)
				{
					IBillGiver billgiver3 = thing3.GetInnerIfMinified() as IBillGiver;
					if (billgiver3 != null)
					{
						yield return billgiver3;
					}
				}
			}
			yield break;
		}

		public static IEnumerable<Bill> GlobalBills()
		{
			foreach (IBillGiver billgiver in BillUtility.GlobalBillGivers())
			{
				foreach (Bill bill in billgiver.BillStack)
				{
					yield return bill;
				}
			}
			if (BillUtility.Clipboard != null)
			{
				yield return BillUtility.Clipboard;
			}
			yield break;
		}

		public static void Notify_ZoneStockpileRemoved(Zone_Stockpile stockpile)
		{
			foreach (Bill bill in BillUtility.GlobalBills())
			{
				bill.ValidateSettings();
			}
		}

		public static void Notify_ColonistUnavailable(Pawn pawn)
		{
			foreach (Bill bill in BillUtility.GlobalBills())
			{
				bill.ValidateSettings();
			}
		}

		public static WorkGiverDef GetWorkgiver(this IBillGiver billGiver)
		{
			Thing thing = billGiver as Thing;
			WorkGiverDef result;
			if (thing == null)
			{
				Log.ErrorOnce(string.Format("Attempting to get the workgiver for a non-Thing IBillGiver {0}", billGiver.ToString()), 96810282, false);
				result = null;
			}
			else
			{
				List<WorkGiverDef> allDefsListForReading = DefDatabase<WorkGiverDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					WorkGiverDef workGiverDef = allDefsListForReading[i];
					WorkGiver_DoBill workGiver_DoBill = workGiverDef.Worker as WorkGiver_DoBill;
					if (workGiver_DoBill != null)
					{
						if (workGiver_DoBill.ThingIsUsableBillGiver(thing))
						{
							return workGiverDef;
						}
					}
				}
				Log.ErrorOnce(string.Format("Can't find a WorkGiver for a BillGiver {0}", thing.ToString()), 57348705, false);
				result = null;
			}
			return result;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static BillUtility()
		{
		}

		[CompilerGenerated]
		private sealed class <GlobalBillGivers>c__Iterator0 : IEnumerable, IEnumerable<IBillGiver>, IEnumerator, IDisposable, IEnumerator<IBillGiver>
		{
			internal List<Map>.Enumerator $locvar0;

			internal Map <map>__1;

			internal List<Thing>.Enumerator $locvar1;

			internal Thing <thing>__2;

			internal IBillGiver <billgiver>__3;

			internal List<Thing>.Enumerator $locvar2;

			internal Thing <thing>__4;

			internal IBillGiver <billgiver>__5;

			internal List<Caravan>.Enumerator $locvar3;

			internal Caravan <caravan>__6;

			internal IEnumerator<Thing> $locvar4;

			internal Thing <thing>__7;

			internal IBillGiver <billgiver>__8;

			internal IBillGiver $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GlobalBillGivers>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					enumerator = Find.Maps.GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
				case 2u:
					break;
				case 3u:
					goto IL_22D;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					case 1u:
						Block_5:
						try
						{
							switch (num)
							{
							}
							while (enumerator2.MoveNext())
							{
								thing = enumerator2.Current;
								billgiver = (thing as IBillGiver);
								if (billgiver != null)
								{
									this.$current = billgiver;
									if (!this.$disposing)
									{
										this.$PC = 1;
									}
									flag = true;
									return true;
								}
								Log.ErrorOnce("Found non-bill-giver tagged as PotentialBillGiver", 13389774, false);
							}
						}
						finally
						{
							if (!flag)
							{
								((IDisposable)enumerator2).Dispose();
							}
						}
						enumerator3 = map.listerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.MinifiedThing)).GetEnumerator();
						num = 4294967293u;
						break;
					case 2u:
						break;
					default:
						goto IL_1E9;
					}
					try
					{
						switch (num)
						{
						case 2u:
							IL_1BC:
							break;
						}
						if (enumerator3.MoveNext())
						{
							thing2 = enumerator3.Current;
							billgiver2 = (thing2.GetInnerIfMinified() as IBillGiver);
							if (billgiver2 != null)
							{
								this.$current = billgiver2;
								if (!this.$disposing)
								{
									this.$PC = 2;
								}
								flag = true;
								return true;
							}
							goto IL_1BC;
						}
					}
					finally
					{
						if (!flag)
						{
							((IDisposable)enumerator3).Dispose();
						}
					}
					IL_1E9:
					if (enumerator.MoveNext())
					{
						map = enumerator.Current;
						enumerator2 = map.listerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.PotentialBillGiver)).GetEnumerator();
						num = 4294967293u;
						goto Block_5;
					}
				}
				finally
				{
					if (!flag)
					{
						((IDisposable)enumerator).Dispose();
					}
				}
				enumerator4 = Find.WorldObjects.Caravans.GetEnumerator();
				num = 4294967293u;
				try
				{
					IL_22D:
					switch (num)
					{
					case 3u:
						Block_23:
						try
						{
							switch (num)
							{
							case 3u:
								IL_2D0:
								break;
							}
							if (enumerator5.MoveNext())
							{
								thing3 = enumerator5.Current;
								billgiver3 = (thing3.GetInnerIfMinified() as IBillGiver);
								if (billgiver3 != null)
								{
									this.$current = billgiver3;
									if (!this.$disposing)
									{
										this.$PC = 3;
									}
									flag = true;
									return true;
								}
								goto IL_2D0;
							}
						}
						finally
						{
							if (!flag)
							{
								if (enumerator5 != null)
								{
									enumerator5.Dispose();
								}
							}
						}
						break;
					}
					if (enumerator4.MoveNext())
					{
						caravan = enumerator4.Current;
						enumerator5 = caravan.AllThings.GetEnumerator();
						num = 4294967293u;
						goto Block_23;
					}
				}
				finally
				{
					if (!flag)
					{
						((IDisposable)enumerator4).Dispose();
					}
				}
				this.$PC = -1;
				return false;
			}

			IBillGiver IEnumerator<IBillGiver>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
				case 2u:
					try
					{
						switch (num)
						{
						case 1u:
							try
							{
							}
							finally
							{
								((IDisposable)enumerator2).Dispose();
							}
							break;
						case 2u:
							try
							{
							}
							finally
							{
								((IDisposable)enumerator3).Dispose();
							}
							break;
						}
					}
					finally
					{
						((IDisposable)enumerator).Dispose();
					}
					break;
				case 3u:
					try
					{
						try
						{
						}
						finally
						{
							if (enumerator5 != null)
							{
								enumerator5.Dispose();
							}
						}
					}
					finally
					{
						((IDisposable)enumerator4).Dispose();
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<RimWorld.IBillGiver>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<IBillGiver> IEnumerable<IBillGiver>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				return new BillUtility.<GlobalBillGivers>c__Iterator0();
			}
		}

		[CompilerGenerated]
		private sealed class <GlobalBills>c__Iterator1 : IEnumerable, IEnumerable<Bill>, IEnumerator, IDisposable, IEnumerator<Bill>
		{
			internal IEnumerator<IBillGiver> $locvar0;

			internal IBillGiver <billgiver>__1;

			internal IEnumerator<Bill> $locvar1;

			internal Bill <bill>__2;

			internal Bill $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GlobalBills>c__Iterator1()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					enumerator = BillUtility.GlobalBillGivers().GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_149;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					case 1u:
						Block_6:
						try
						{
							switch (num)
							{
							}
							if (enumerator2.MoveNext())
							{
								bill = enumerator2.Current;
								this.$current = bill;
								if (!this.$disposing)
								{
									this.$PC = 1;
								}
								flag = true;
								return true;
							}
						}
						finally
						{
							if (!flag)
							{
								if (enumerator2 != null)
								{
									enumerator2.Dispose();
								}
							}
						}
						break;
					}
					if (enumerator.MoveNext())
					{
						billgiver = enumerator.Current;
						enumerator2 = billgiver.BillStack.GetEnumerator();
						num = 4294967293u;
						goto Block_6;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				if (BillUtility.Clipboard == null)
				{
					goto IL_149;
				}
				this.$current = BillUtility.Clipboard;
				if (!this.$disposing)
				{
					this.$PC = 2;
				}
				return true;
				IL_149:
				this.$PC = -1;
				return false;
			}

			Bill IEnumerator<Bill>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
						try
						{
						}
						finally
						{
							if (enumerator2 != null)
							{
								enumerator2.Dispose();
							}
						}
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<RimWorld.Bill>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Bill> IEnumerable<Bill>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				return new BillUtility.<GlobalBills>c__Iterator1();
			}
		}
	}
}
