using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	[StaticConstructorOnStartup]
	public static class CaravanMergeUtility
	{
		private static readonly Texture2D MergeCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/MergeCaravans", true);

		private static List<Caravan> tmpSelectedPlayerCaravans = new List<Caravan>();

		private static List<Caravan> tmpCaravansOnSameTile = new List<Caravan>();

		public static bool ShouldShowMergeCommand
		{
			get
			{
				return CaravanMergeUtility.CanMergeAnySelectedCaravans || CaravanMergeUtility.AnySelectedCaravanCloseToAnyOtherMergeableCaravan;
			}
		}

		public static bool CanMergeAnySelectedCaravans
		{
			get
			{
				List<WorldObject> selectedObjects = Find.WorldSelector.SelectedObjects;
				int num = 0;
				bool result;
				while (true)
				{
					if (num < selectedObjects.Count)
					{
						Caravan caravan = selectedObjects[num] as Caravan;
						if (caravan != null && caravan.IsPlayerControlled)
						{
							for (int i = num + 1; i < selectedObjects.Count; i++)
							{
								Caravan caravan2 = selectedObjects[i] as Caravan;
								if (caravan2 != null && caravan2.IsPlayerControlled && CaravanMergeUtility.CloseToEachOther(caravan, caravan2))
									goto IL_006b;
							}
						}
						num++;
						continue;
					}
					result = false;
					break;
					IL_006b:
					result = true;
					break;
				}
				return result;
			}
		}

		public static bool AnySelectedCaravanCloseToAnyOtherMergeableCaravan
		{
			get
			{
				List<WorldObject> selectedObjects = Find.WorldSelector.SelectedObjects;
				List<Caravan> caravans = Find.WorldObjects.Caravans;
				int num = 0;
				bool result;
				while (true)
				{
					if (num < selectedObjects.Count)
					{
						Caravan caravan = selectedObjects[num] as Caravan;
						if (caravan != null && caravan.IsPlayerControlled)
						{
							for (int i = 0; i < caravans.Count; i++)
							{
								Caravan caravan2 = caravans[i];
								if (caravan2 != caravan && caravan2.IsPlayerControlled && CaravanMergeUtility.CloseToEachOther(caravan, caravan2))
									goto IL_0077;
							}
						}
						num++;
						continue;
					}
					result = false;
					break;
					IL_0077:
					result = true;
					break;
				}
				return result;
			}
		}

		public static Command MergeCommand(Caravan caravan)
		{
			Command_Action command_Action = new Command_Action();
			command_Action.defaultLabel = "CommandMergeCaravans".Translate();
			command_Action.defaultDesc = "CommandMergeCaravansDesc".Translate();
			command_Action.icon = CaravanMergeUtility.MergeCommandTex;
			command_Action.action = (Action)delegate
			{
				CaravanMergeUtility.TryMergeSelectedCaravans();
				SoundDefOf.TickHigh.PlayOneShotOnCamera(null);
			};
			if (!CaravanMergeUtility.CanMergeAnySelectedCaravans)
			{
				command_Action.Disable("CommandMergeCaravansFailCaravansNotSelected".Translate());
			}
			return command_Action;
		}

		public static void TryMergeSelectedCaravans()
		{
			CaravanMergeUtility.tmpSelectedPlayerCaravans.Clear();
			List<WorldObject> selectedObjects = Find.WorldSelector.SelectedObjects;
			for (int i = 0; i < selectedObjects.Count; i++)
			{
				Caravan caravan = selectedObjects[i] as Caravan;
				if (caravan != null && caravan.IsPlayerControlled)
				{
					CaravanMergeUtility.tmpSelectedPlayerCaravans.Add(caravan);
				}
			}
			while (CaravanMergeUtility.tmpSelectedPlayerCaravans.Any())
			{
				Caravan caravan2 = CaravanMergeUtility.tmpSelectedPlayerCaravans[0];
				CaravanMergeUtility.tmpSelectedPlayerCaravans.RemoveAt(0);
				CaravanMergeUtility.tmpCaravansOnSameTile.Clear();
				CaravanMergeUtility.tmpCaravansOnSameTile.Add(caravan2);
				for (int num = CaravanMergeUtility.tmpSelectedPlayerCaravans.Count - 1; num >= 0; num--)
				{
					if (CaravanMergeUtility.CloseToEachOther(CaravanMergeUtility.tmpSelectedPlayerCaravans[num], caravan2))
					{
						CaravanMergeUtility.tmpCaravansOnSameTile.Add(CaravanMergeUtility.tmpSelectedPlayerCaravans[num]);
						CaravanMergeUtility.tmpSelectedPlayerCaravans.RemoveAt(num);
					}
				}
				if (CaravanMergeUtility.tmpCaravansOnSameTile.Count >= 2)
				{
					CaravanMergeUtility.MergeCaravans(CaravanMergeUtility.tmpCaravansOnSameTile);
				}
			}
		}

		private static bool CloseToEachOther(Caravan c1, Caravan c2)
		{
			bool result;
			if (c1.Tile == c2.Tile)
			{
				result = true;
			}
			else
			{
				Vector3 drawPos = c1.DrawPos;
				Vector3 drawPos2 = c2.DrawPos;
				float num = (float)(Find.WorldGrid.averageTileSize * 0.5);
				result = ((byte)(((drawPos - drawPos2).sqrMagnitude < num * num) ? 1 : 0) != 0);
			}
			return result;
		}

		private static void MergeCaravans(List<Caravan> caravans)
		{
			Caravan caravan = caravans.MaxBy((Func<Caravan, int>)((Caravan x) => x.PawnsListForReading.Count));
			for (int i = 0; i < caravans.Count; i++)
			{
				Caravan caravan2 = caravans[i];
				if (caravan2 != caravan)
				{
					caravan2.pawns.TryTransferAllToContainer(caravan.pawns, true);
					Find.WorldObjects.Remove(caravan2);
				}
			}
		}
	}
}
