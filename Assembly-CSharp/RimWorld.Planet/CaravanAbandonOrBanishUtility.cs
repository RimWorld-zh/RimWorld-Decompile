using System;
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
				if (!caravan.PawnsListForReading.Any((Predicate<Pawn>)((Pawn x) => x != p && caravan.IsOwner(x))))
				{
					Messages.Message("MessageCantBanishLastColonist".Translate(), (WorldObject)caravan, MessageTypeDefOf.RejectInput);
				}
				else
				{
					PawnBanishUtility.ShowBanishPawnConfirmationDialog(p);
				}
			}
			else
			{
				Dialog_MessageBox window = Dialog_MessageBox.CreateConfirmation("ConfirmAbandonItemDialog".Translate(t.Label), (Action)delegate()
				{
					Pawn ownerOf = CaravanInventoryUtility.GetOwnerOf(caravan, t);
					if (ownerOf == null)
					{
						Log.Error("Could not find owner of " + t);
					}
					else
					{
						ownerOf.inventory.innerContainer.Remove(t);
						t.Destroy(DestroyMode.Vanish);
						caravan.RecacheImmobilizedNow();
						caravan.RecacheDaysWorthOfFood();
					}
				}, true, (string)null);
				Find.WindowStack.Add(window);
			}
		}

		public static void TryAbandonSpecificCountViaInterface(Thing t, Caravan caravan)
		{
			Find.WindowStack.Add(new Dialog_Slider("AbandonSliderText".Translate(t.LabelNoCount), 1, t.stackCount, (Action<int>)delegate(int x)
			{
				Pawn ownerOf = CaravanInventoryUtility.GetOwnerOf(caravan, t);
				if (ownerOf == null)
				{
					Log.Error("Could not find owner of " + t);
				}
				else
				{
					if (x == t.stackCount)
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
			}, -2147483648));
		}

		public static string GetAbandonOrBanishButtonTooltip(Thing t, Caravan caravan, bool abandonSpecificCount)
		{
			Pawn pawn = t as Pawn;
			string result;
			if (pawn != null)
			{
				result = PawnBanishUtility.GetBanishButtonTip(pawn);
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (t.stackCount == 1)
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
				result = stringBuilder.ToString();
			}
			return result;
		}
	}
}
