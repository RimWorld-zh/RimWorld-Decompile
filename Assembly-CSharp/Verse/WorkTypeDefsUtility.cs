using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x02000BB7 RID: 2999
	public static class WorkTypeDefsUtility
	{
		// Token: 0x17000A25 RID: 2597
		// (get) Token: 0x060040FF RID: 16639 RVA: 0x0022547C File Offset: 0x0022387C
		public static IEnumerable<WorkTypeDef> WorkTypeDefsInPriorityOrder
		{
			get
			{
				return from wt in DefDatabase<WorkTypeDef>.AllDefs
				orderby wt.naturalPriority descending
				select wt;
			}
		}

		// Token: 0x06004100 RID: 16640 RVA: 0x002254B8 File Offset: 0x002238B8
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

		// Token: 0x06004101 RID: 16641 RVA: 0x0022568C File Offset: 0x00223A8C
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
	}
}
