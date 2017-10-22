using System;

namespace Verse
{
	public static class ThingRequestGroupUtility
	{
		public static bool StoreInRegion(this ThingRequestGroup group)
		{
			bool result;
			switch (group)
			{
			case ThingRequestGroup.Undefined:
			{
				result = false;
				break;
			}
			case ThingRequestGroup.Nothing:
			{
				result = false;
				break;
			}
			case ThingRequestGroup.Everything:
			{
				result = true;
				break;
			}
			case ThingRequestGroup.HaulableEver:
			{
				result = true;
				break;
			}
			case ThingRequestGroup.HaulableAlways:
			{
				result = true;
				break;
			}
			case ThingRequestGroup.FoodSource:
			{
				result = true;
				break;
			}
			case ThingRequestGroup.FoodSourceNotPlantOrTree:
			{
				result = true;
				break;
			}
			case ThingRequestGroup.Corpse:
			{
				result = true;
				break;
			}
			case ThingRequestGroup.Blueprint:
			{
				result = true;
				break;
			}
			case ThingRequestGroup.BuildingArtificial:
			{
				result = true;
				break;
			}
			case ThingRequestGroup.BuildingFrame:
			{
				result = true;
				break;
			}
			case ThingRequestGroup.Pawn:
			{
				result = true;
				break;
			}
			case ThingRequestGroup.PotentialBillGiver:
			{
				result = true;
				break;
			}
			case ThingRequestGroup.Medicine:
			{
				result = true;
				break;
			}
			case ThingRequestGroup.Filth:
			{
				result = true;
				break;
			}
			case ThingRequestGroup.AttackTarget:
			{
				result = true;
				break;
			}
			case ThingRequestGroup.Weapon:
			{
				result = true;
				break;
			}
			case ThingRequestGroup.Refuelable:
			{
				result = true;
				break;
			}
			case ThingRequestGroup.HaulableEverOrMinifiable:
			{
				result = true;
				break;
			}
			case ThingRequestGroup.Drug:
			{
				result = true;
				break;
			}
			case ThingRequestGroup.Shell:
			{
				result = true;
				break;
			}
			case ThingRequestGroup.HarvestablePlant:
			{
				result = true;
				break;
			}
			case ThingRequestGroup.Fire:
			{
				result = true;
				break;
			}
			case ThingRequestGroup.Plant:
			{
				result = false;
				break;
			}
			case ThingRequestGroup.Construction:
			{
				result = false;
				break;
			}
			case ThingRequestGroup.HasGUIOverlay:
			{
				result = false;
				break;
			}
			case ThingRequestGroup.Apparel:
			{
				result = false;
				break;
			}
			case ThingRequestGroup.MinifiedThing:
			{
				result = false;
				break;
			}
			case ThingRequestGroup.Grave:
			{
				result = false;
				break;
			}
			case ThingRequestGroup.Art:
			{
				result = false;
				break;
			}
			case ThingRequestGroup.ThingHolder:
			{
				result = false;
				break;
			}
			case ThingRequestGroup.ActiveDropPod:
			{
				result = false;
				break;
			}
			case ThingRequestGroup.Transporter:
			{
				result = false;
				break;
			}
			case ThingRequestGroup.LongRangeMineralScanner:
			{
				result = false;
				break;
			}
			case ThingRequestGroup.AffectsSky:
			{
				result = false;
				break;
			}
			case ThingRequestGroup.PsychicDroneEmanator:
			{
				result = false;
				break;
			}
			case ThingRequestGroup.WindSource:
			{
				result = false;
				break;
			}
			case ThingRequestGroup.AlwaysFlee:
			{
				result = false;
				break;
			}
			case ThingRequestGroup.WildAnimalsFlee:
			{
				result = false;
				break;
			}
			default:
			{
				throw new ArgumentException("group");
			}
			}
			return result;
		}
	}
}
