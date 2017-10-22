using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public static class HostilityResponseModeUtility
	{
		private static readonly Texture2D IgnoreIcon = Resources.Load<Texture2D>("Textures/UI/Icons/HostilityResponse/Ignore");

		private static readonly Texture2D AttackIcon = Resources.Load<Texture2D>("Textures/UI/Icons/HostilityResponse/Attack");

		private static readonly Texture2D FleeIcon = Resources.Load<Texture2D>("Textures/UI/Icons/HostilityResponse/Flee");

		public static Texture2D GetIcon(this HostilityResponseMode response)
		{
			Texture2D result;
			switch (response)
			{
			case HostilityResponseMode.Ignore:
			{
				result = HostilityResponseModeUtility.IgnoreIcon;
				break;
			}
			case HostilityResponseMode.Attack:
			{
				result = HostilityResponseModeUtility.AttackIcon;
				break;
			}
			case HostilityResponseMode.Flee:
			{
				result = HostilityResponseModeUtility.FleeIcon;
				break;
			}
			default:
			{
				result = BaseContent.BadTex;
				break;
			}
			}
			return result;
		}

		public static HostilityResponseMode GetNextResponse(Pawn pawn)
		{
			HostilityResponseMode result;
			switch (pawn.playerSettings.hostilityResponse)
			{
			case HostilityResponseMode.Ignore:
			{
				result = (HostilityResponseMode)((pawn.story == null || !pawn.story.WorkTagIsDisabled(WorkTags.Violent)) ? 1 : 2);
				break;
			}
			case HostilityResponseMode.Attack:
			{
				result = HostilityResponseMode.Flee;
				break;
			}
			case HostilityResponseMode.Flee:
			{
				result = HostilityResponseMode.Ignore;
				break;
			}
			default:
			{
				result = HostilityResponseMode.Ignore;
				break;
			}
			}
			return result;
		}

		public static string GetLabel(this HostilityResponseMode response)
		{
			return ("HostilityResponseMode_" + response).Translate();
		}

		public static void DrawResponseButton(Vector2 pos, Pawn pawn)
		{
			Texture2D icon = pawn.playerSettings.hostilityResponse.GetIcon();
			Rect rect = new Rect(pos.x, pos.y, 24f, 24f);
			if (Widgets.ButtonImage(rect, icon))
			{
				pawn.playerSettings.hostilityResponse = HostilityResponseModeUtility.GetNextResponse(pawn);
				SoundDefOf.TickHigh.PlayOneShotOnCamera(null);
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.HostilityResponse, KnowledgeAmount.SpecificInteraction);
			}
			UIHighlighter.HighlightOpportunity(rect, "HostilityResponse");
			TooltipHandler.TipRegion(rect, "HostilityReponseTip".Translate() + "\n\n" + "HostilityResponseCurrentMode".Translate() + ": " + pawn.playerSettings.hostilityResponse.GetLabel());
		}
	}
}
