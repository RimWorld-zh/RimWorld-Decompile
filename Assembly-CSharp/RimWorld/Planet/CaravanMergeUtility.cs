using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x020005E0 RID: 1504
	[StaticConstructorOnStartup]
	public static class CaravanMergeUtility
	{
		// Token: 0x04001198 RID: 4504
		private static readonly Texture2D MergeCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/MergeCaravans", true);

		// Token: 0x04001199 RID: 4505
		private static List<Caravan> tmpSelectedPlayerCaravans = new List<Caravan>();

		// Token: 0x0400119A RID: 4506
		private static List<Caravan> tmpCaravansOnSameTile = new List<Caravan>();

		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x06001DAF RID: 7599 RVA: 0x001000D4 File Offset: 0x000FE4D4
		public static bool ShouldShowMergeCommand
		{
			get
			{
				return CaravanMergeUtility.CanMergeAnySelectedCaravans || CaravanMergeUtility.AnySelectedCaravanCloseToAnyOtherMergeableCaravan;
			}
		}

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x06001DB0 RID: 7600 RVA: 0x001000FC File Offset: 0x000FE4FC
		public static bool CanMergeAnySelectedCaravans
		{
			get
			{
				List<WorldObject> selectedObjects = Find.WorldSelector.SelectedObjects;
				for (int i = 0; i < selectedObjects.Count; i++)
				{
					Caravan caravan = selectedObjects[i] as Caravan;
					if (caravan != null && caravan.IsPlayerControlled)
					{
						for (int j = i + 1; j < selectedObjects.Count; j++)
						{
							Caravan caravan2 = selectedObjects[j] as Caravan;
							if (caravan2 != null && caravan2.IsPlayerControlled && CaravanMergeUtility.CloseToEachOther(caravan, caravan2))
							{
								return true;
							}
						}
					}
				}
				return false;
			}
		}

		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x06001DB1 RID: 7601 RVA: 0x001001AC File Offset: 0x000FE5AC
		public static bool AnySelectedCaravanCloseToAnyOtherMergeableCaravan
		{
			get
			{
				List<WorldObject> selectedObjects = Find.WorldSelector.SelectedObjects;
				List<Caravan> caravans = Find.WorldObjects.Caravans;
				for (int i = 0; i < selectedObjects.Count; i++)
				{
					Caravan caravan = selectedObjects[i] as Caravan;
					if (caravan != null && caravan.IsPlayerControlled)
					{
						for (int j = 0; j < caravans.Count; j++)
						{
							Caravan caravan2 = caravans[j];
							if (caravan2 != caravan)
							{
								if (caravan2.IsPlayerControlled && CaravanMergeUtility.CloseToEachOther(caravan, caravan2))
								{
									return true;
								}
							}
						}
					}
				}
				return false;
			}
		}

		// Token: 0x06001DB2 RID: 7602 RVA: 0x00100268 File Offset: 0x000FE668
		public static Command MergeCommand(Caravan caravan)
		{
			Command_Action command_Action = new Command_Action();
			command_Action.defaultLabel = "CommandMergeCaravans".Translate();
			command_Action.defaultDesc = "CommandMergeCaravansDesc".Translate();
			command_Action.icon = CaravanMergeUtility.MergeCommandTex;
			command_Action.action = delegate()
			{
				CaravanMergeUtility.TryMergeSelectedCaravans();
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
			};
			if (!CaravanMergeUtility.CanMergeAnySelectedCaravans)
			{
				command_Action.Disable("CommandMergeCaravansFailCaravansNotSelected".Translate());
			}
			return command_Action;
		}

		// Token: 0x06001DB3 RID: 7603 RVA: 0x001002EC File Offset: 0x000FE6EC
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
			while (CaravanMergeUtility.tmpSelectedPlayerCaravans.Any<Caravan>())
			{
				Caravan caravan2 = CaravanMergeUtility.tmpSelectedPlayerCaravans[0];
				CaravanMergeUtility.tmpSelectedPlayerCaravans.RemoveAt(0);
				CaravanMergeUtility.tmpCaravansOnSameTile.Clear();
				CaravanMergeUtility.tmpCaravansOnSameTile.Add(caravan2);
				for (int j = CaravanMergeUtility.tmpSelectedPlayerCaravans.Count - 1; j >= 0; j--)
				{
					if (CaravanMergeUtility.CloseToEachOther(CaravanMergeUtility.tmpSelectedPlayerCaravans[j], caravan2))
					{
						CaravanMergeUtility.tmpCaravansOnSameTile.Add(CaravanMergeUtility.tmpSelectedPlayerCaravans[j]);
						CaravanMergeUtility.tmpSelectedPlayerCaravans.RemoveAt(j);
					}
				}
				if (CaravanMergeUtility.tmpCaravansOnSameTile.Count >= 2)
				{
					CaravanMergeUtility.MergeCaravans(CaravanMergeUtility.tmpCaravansOnSameTile);
				}
			}
		}

		// Token: 0x06001DB4 RID: 7604 RVA: 0x0010040C File Offset: 0x000FE80C
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
				float num = Find.WorldGrid.averageTileSize * 0.5f;
				result = ((drawPos - drawPos2).sqrMagnitude < num * num);
			}
			return result;
		}

		// Token: 0x06001DB5 RID: 7605 RVA: 0x00100478 File Offset: 0x000FE878
		private static void MergeCaravans(List<Caravan> caravans)
		{
			Caravan caravan = caravans.MaxBy((Caravan x) => x.PawnsListForReading.Count);
			for (int i = 0; i < caravans.Count; i++)
			{
				Caravan caravan2 = caravans[i];
				if (caravan2 != caravan)
				{
					caravan2.pawns.TryTransferAllToContainer(caravan.pawns, true);
					Find.WorldObjects.Remove(caravan2);
				}
			}
			caravan.Notify_Merged(caravans);
		}
	}
}
