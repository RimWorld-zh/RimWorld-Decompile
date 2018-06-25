using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Verse
{
	public static class WorkTypeDefsUtility
	{
		[CompilerGenerated]
		private static Func<WorkTypeDef, int> <>f__am$cache0;

		public static IEnumerable<WorkTypeDef> WorkTypeDefsInPriorityOrder
		{
			get
			{
				return from wt in DefDatabase<WorkTypeDef>.AllDefs
				orderby wt.naturalPriority descending
				select wt;
			}
		}

		public static string LabelTranslated(this WorkTags tags)
		{
			string result;
			switch (tags)
			{
			case WorkTags.None:
				result = "WorkTagNone".Translate();
				break;
			default:
				if (tags != WorkTags.Violent)
				{
					if (tags != WorkTags.Caring)
					{
						if (tags != WorkTags.Social)
						{
							if (tags != WorkTags.Intellectual)
							{
								if (tags != WorkTags.Animals)
								{
									if (tags != WorkTags.Artistic)
									{
										if (tags != WorkTags.Crafting)
										{
											if (tags != WorkTags.Cooking)
											{
												if (tags != WorkTags.Firefighting)
												{
													if (tags != WorkTags.Cleaning)
													{
														if (tags != WorkTags.Hauling)
														{
															if (tags != WorkTags.PlantWork)
															{
																if (tags != WorkTags.Mining)
																{
																	Log.Error("Unknown or mixed worktags for naming: " + (int)tags, false);
																	result = "Worktag";
																}
																else
																{
																	result = "WorkTagMining".Translate();
																}
															}
															else
															{
																result = "WorkTagPlantWork".Translate();
															}
														}
														else
														{
															result = "WorkTagHauling".Translate();
														}
													}
													else
													{
														result = "WorkTagCleaning".Translate();
													}
												}
												else
												{
													result = "WorkTagFirefighting".Translate();
												}
											}
											else
											{
												result = "WorkTagCooking".Translate();
											}
										}
										else
										{
											result = "WorkTagCrafting".Translate();
										}
									}
									else
									{
										result = "WorkTagArtistic".Translate();
									}
								}
								else
								{
									result = "WorkTagAnimals".Translate();
								}
							}
							else
							{
								result = "WorkTagIntellectual".Translate();
							}
						}
						else
						{
							result = "WorkTagSocial".Translate();
						}
					}
					else
					{
						result = "WorkTagCaring".Translate();
					}
				}
				else
				{
					result = "WorkTagViolent".Translate();
				}
				break;
			case WorkTags.ManualDumb:
				result = "WorkTagManualDumb".Translate();
				break;
			case WorkTags.ManualSkilled:
				result = "WorkTagManualSkilled".Translate();
				break;
			}
			return result;
		}

		public static bool OverlapsWithOnAnyWorkType(this WorkTags a, WorkTags b)
		{
			List<WorkTypeDef> allDefsListForReading = DefDatabase<WorkTypeDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				WorkTypeDef workTypeDef = allDefsListForReading[i];
				if ((workTypeDef.workTags & a) != WorkTags.None && (workTypeDef.workTags & b) != WorkTags.None)
				{
					return true;
				}
			}
			return false;
		}

		[CompilerGenerated]
		private static int <get_WorkTypeDefsInPriorityOrder>m__0(WorkTypeDef wt)
		{
			return wt.naturalPriority;
		}
	}
}
