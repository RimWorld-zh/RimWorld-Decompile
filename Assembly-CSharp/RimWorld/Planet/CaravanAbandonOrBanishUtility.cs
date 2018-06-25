using System;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld.Planet
{
	public static class CaravanAbandonOrBanishUtility
	{
		public static void TryAbandonOrBanishViaInterface(Thing t, Caravan caravan)
		{
			Pawn p = t as Pawn;
			if (p != null)
			{
				if (!caravan.PawnsListForReading.Any((Pawn x) => x != p && caravan.IsOwner(x)))
				{
					Messages.Message("MessageCantBanishLastColonist".Translate(), caravan, MessageTypeDefOf.RejectInput, false);
				}
				else
				{
					PawnBanishUtility.ShowBanishPawnConfirmationDialog(p);
				}
			}
			else
			{
				Dialog_MessageBox window = Dialog_MessageBox.CreateConfirmation("ConfirmAbandonItemDialog".Translate(new object[]
				{
					t.Label
				}), delegate
				{
					Pawn ownerOf = CaravanInventoryUtility.GetOwnerOf(caravan, t);
					if (ownerOf == null)
					{
						Log.Error("Could not find owner of " + t, false);
					}
					else
					{
						ownerOf.inventory.innerContainer.Remove(t);
						t.Destroy(DestroyMode.Vanish);
						caravan.RecacheImmobilizedNow();
						caravan.RecacheDaysWorthOfFood();
					}
				}, true, null);
				Find.WindowStack.Add(window);
			}
		}

		public static void TryAbandonOrBanishViaInterface(TransferableImmutable t, Caravan caravan)
		{
			Pawn pawn = t.AnyThing as Pawn;
			if (pawn != null)
			{
				CaravanAbandonOrBanishUtility.TryAbandonOrBanishViaInterface(pawn, caravan);
			}
			else
			{
				Dialog_MessageBox window = Dialog_MessageBox.CreateConfirmation("ConfirmAbandonItemDialog".Translate(new object[]
				{
					t.LabelWithTotalStackCount
				}), delegate
				{
					for (int i = 0; i < t.things.Count; i++)
					{
						Thing thing = t.things[i];
						Pawn ownerOf = CaravanInventoryUtility.GetOwnerOf(caravan, thing);
						if (ownerOf == null)
						{
							Log.Error("Could not find owner of " + thing, false);
							return;
						}
						ownerOf.inventory.innerContainer.Remove(thing);
						thing.Destroy(DestroyMode.Vanish);
					}
					caravan.RecacheImmobilizedNow();
					caravan.RecacheDaysWorthOfFood();
				}, true, null);
				Find.WindowStack.Add(window);
			}
		}

		public static void TryAbandonSpecificCountViaInterface(Thing t, Caravan caravan)
		{
			Find.WindowStack.Add(new Dialog_Slider("AbandonSliderText".Translate(new object[]
			{
				t.LabelNoCount
			}), 1, t.stackCount, delegate(int x)
			{
				Pawn ownerOf = CaravanInventoryUtility.GetOwnerOf(caravan, t);
				if (ownerOf == null)
				{
					Log.Error("Could not find owner of " + t, false);
				}
				else
				{
					if (x >= t.stackCount)
					{
						ownerOf.inventory.innerContainer.Remove(t);
						t.Destroy(DestroyMode.Vanish);
					}
					else
					{
						t.SplitOff(x).Destroy(DestroyMode.Vanish);
					}
					caravan.RecacheImmobilizedNow();
					caravan.RecacheDaysWorthOfFood();
				}
			}, int.MinValue));
		}

		public static void TryAbandonSpecificCountViaInterface(TransferableImmutable t, Caravan caravan)
		{
			Find.WindowStack.Add(new Dialog_Slider("AbandonSliderText".Translate(new object[]
			{
				t.Label
			}), 1, t.TotalStackCount, delegate(int x)
			{
				int num = x;
				for (int i = 0; i < t.things.Count; i++)
				{
					if (num <= 0)
					{
						break;
					}
					Thing thing = t.things[i];
					Pawn ownerOf = CaravanInventoryUtility.GetOwnerOf(caravan, thing);
					if (ownerOf == null)
					{
						Log.Error("Could not find owner of " + thing, false);
						return;
					}
					if (num >= thing.stackCount)
					{
						num -= thing.stackCount;
						ownerOf.inventory.innerContainer.Remove(thing);
						thing.Destroy(DestroyMode.Vanish);
					}
					else
					{
						thing.SplitOff(num).Destroy(DestroyMode.Vanish);
						num = 0;
					}
				}
				caravan.RecacheImmobilizedNow();
				caravan.RecacheDaysWorthOfFood();
			}, int.MinValue));
		}

		public static string GetAbandonOrBanishButtonTooltip(Thing t, bool abandonSpecificCount)
		{
			Pawn pawn = t as Pawn;
			string result;
			if (pawn != null)
			{
				result = PawnBanishUtility.GetBanishButtonTip(pawn);
			}
			else
			{
				result = CaravanAbandonOrBanishUtility.GetAbandonItemButtonTooltip(t.stackCount, abandonSpecificCount);
			}
			return result;
		}

		public static string GetAbandonOrBanishButtonTooltip(TransferableImmutable t, bool abandonSpecificCount)
		{
			Pawn pawn = t.AnyThing as Pawn;
			string result;
			if (pawn != null)
			{
				result = PawnBanishUtility.GetBanishButtonTip(pawn);
			}
			else
			{
				result = CaravanAbandonOrBanishUtility.GetAbandonItemButtonTooltip(t.TotalStackCount, abandonSpecificCount);
			}
			return result;
		}

		private static string GetAbandonItemButtonTooltip(int currentStackCount, bool abandonSpecificCount)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (currentStackCount == 1)
			{
				stringBuilder.AppendLine("AbandonTip".Translate());
			}
			else if (abandonSpecificCount)
			{
				stringBuilder.AppendLine("AbandonSpecificCountTip".Translate());
			}
			else
			{
				stringBuilder.AppendLine("AbandonAllTip".Translate());
			}
			stringBuilder.AppendLine();
			stringBuilder.Append("AbandonItemTipExtraText".Translate());
			return stringBuilder.ToString();
		}

		[CompilerGenerated]
		private sealed class <TryAbandonOrBanishViaInterface>c__AnonStorey0
		{
			internal Pawn p;

			internal Caravan caravan;

			internal Thing t;

			public <TryAbandonOrBanishViaInterface>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Pawn x)
			{
				return x != this.p && this.caravan.IsOwner(x);
			}

			internal void <>m__1()
			{
				Pawn ownerOf = CaravanInventoryUtility.GetOwnerOf(this.caravan, this.t);
				if (ownerOf == null)
				{
					Log.Error("Could not find owner of " + this.t, false);
				}
				else
				{
					ownerOf.inventory.innerContainer.Remove(this.t);
					this.t.Destroy(DestroyMode.Vanish);
					this.caravan.RecacheImmobilizedNow();
					this.caravan.RecacheDaysWorthOfFood();
				}
			}
		}

		[CompilerGenerated]
		private sealed class <TryAbandonOrBanishViaInterface>c__AnonStorey1
		{
			internal TransferableImmutable t;

			internal Caravan caravan;

			public <TryAbandonOrBanishViaInterface>c__AnonStorey1()
			{
			}

			internal void <>m__0()
			{
				for (int i = 0; i < this.t.things.Count; i++)
				{
					Thing thing = this.t.things[i];
					Pawn ownerOf = CaravanInventoryUtility.GetOwnerOf(this.caravan, thing);
					if (ownerOf == null)
					{
						Log.Error("Could not find owner of " + thing, false);
						return;
					}
					ownerOf.inventory.innerContainer.Remove(thing);
					thing.Destroy(DestroyMode.Vanish);
				}
				this.caravan.RecacheImmobilizedNow();
				this.caravan.RecacheDaysWorthOfFood();
			}
		}

		[CompilerGenerated]
		private sealed class <TryAbandonSpecificCountViaInterface>c__AnonStorey2
		{
			internal Caravan caravan;

			internal Thing t;

			public <TryAbandonSpecificCountViaInterface>c__AnonStorey2()
			{
			}

			internal void <>m__0(int x)
			{
				Pawn ownerOf = CaravanInventoryUtility.GetOwnerOf(this.caravan, this.t);
				if (ownerOf == null)
				{
					Log.Error("Could not find owner of " + this.t, false);
				}
				else
				{
					if (x >= this.t.stackCount)
					{
						ownerOf.inventory.innerContainer.Remove(this.t);
						this.t.Destroy(DestroyMode.Vanish);
					}
					else
					{
						this.t.SplitOff(x).Destroy(DestroyMode.Vanish);
					}
					this.caravan.RecacheImmobilizedNow();
					this.caravan.RecacheDaysWorthOfFood();
				}
			}
		}

		[CompilerGenerated]
		private sealed class <TryAbandonSpecificCountViaInterface>c__AnonStorey3
		{
			internal TransferableImmutable t;

			internal Caravan caravan;

			public <TryAbandonSpecificCountViaInterface>c__AnonStorey3()
			{
			}

			internal void <>m__0(int x)
			{
				int num = x;
				for (int i = 0; i < this.t.things.Count; i++)
				{
					if (num <= 0)
					{
						break;
					}
					Thing thing = this.t.things[i];
					Pawn ownerOf = CaravanInventoryUtility.GetOwnerOf(this.caravan, thing);
					if (ownerOf == null)
					{
						Log.Error("Could not find owner of " + thing, false);
						return;
					}
					if (num >= thing.stackCount)
					{
						num -= thing.stackCount;
						ownerOf.inventory.innerContainer.Remove(thing);
						thing.Destroy(DestroyMode.Vanish);
					}
					else
					{
						thing.SplitOff(num).Destroy(DestroyMode.Vanish);
						num = 0;
					}
				}
				this.caravan.RecacheImmobilizedNow();
				this.caravan.RecacheDaysWorthOfFood();
			}
		}
	}
}
