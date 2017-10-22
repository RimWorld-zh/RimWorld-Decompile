using System;
using System.Collections.Generic;
using System.Text;

namespace Verse
{
	public class LoadIDsWantedBank
	{
		private struct IdRecord
		{
			public string targetLoadID;

			public Type targetType;

			public string pathRelToParent;

			public IExposable parent;

			public IdRecord(string targetLoadID, Type targetType, string pathRelToParent, IExposable parent)
			{
				this.targetLoadID = targetLoadID;
				this.targetType = targetType;
				this.pathRelToParent = pathRelToParent;
				this.parent = parent;
			}
		}

		private struct IdListRecord
		{
			public List<string> targetLoadIDs;

			public string pathRelToParent;

			public IExposable parent;

			public IdListRecord(List<string> targetLoadIDs, string pathRelToParent, IExposable parent)
			{
				this.targetLoadIDs = targetLoadIDs;
				this.pathRelToParent = pathRelToParent;
				this.parent = parent;
			}
		}

		private List<IdRecord> idsRead = new List<IdRecord>();

		private List<IdListRecord> idListsRead = new List<IdListRecord>();

		public void ConfirmClear()
		{
			if (this.idsRead.Count > 0 || this.idListsRead.Count > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("Not all loadIDs which were read were consumed.");
				if (this.idsRead.Count > 0)
				{
					stringBuilder.AppendLine("Singles:");
					for (int i = 0; i < this.idsRead.Count; i++)
					{
						StringBuilder obj = stringBuilder;
						object[] obj2 = new object[8]
						{
							"  ",
							null,
							null,
							null,
							null,
							null,
							null,
							null
						};
						IdRecord idRecord = this.idsRead[i];
						obj2[1] = idRecord.targetLoadID.ToStringSafe();
						obj2[2] = " of type ";
						IdRecord idRecord2 = this.idsRead[i];
						obj2[3] = idRecord2.targetType;
						obj2[4] = ". pathRelToParent=";
						IdRecord idRecord3 = this.idsRead[i];
						obj2[5] = idRecord3.pathRelToParent;
						obj2[6] = ", parent=";
						IdRecord idRecord4 = this.idsRead[i];
						obj2[7] = idRecord4.parent.ToStringSafe();
						obj.AppendLine(string.Concat(obj2));
					}
				}
				if (this.idListsRead.Count > 0)
				{
					stringBuilder.AppendLine("Lists:");
					for (int j = 0; j < this.idListsRead.Count; j++)
					{
						StringBuilder obj3 = stringBuilder;
						object[] obj4 = new object[6]
						{
							"  List with ",
							null,
							null,
							null,
							null,
							null
						};
						IdListRecord idListRecord = this.idListsRead[j];
						int num;
						if (idListRecord.targetLoadIDs != null)
						{
							IdListRecord idListRecord2 = this.idListsRead[j];
							num = idListRecord2.targetLoadIDs.Count;
						}
						else
						{
							num = 0;
						}
						obj4[1] = num;
						obj4[2] = " elements. pathRelToParent=";
						IdListRecord idListRecord3 = this.idListsRead[j];
						obj4[3] = idListRecord3.pathRelToParent;
						obj4[4] = ", parent=";
						IdListRecord idListRecord4 = this.idListsRead[j];
						obj4[5] = idListRecord4.parent.ToStringSafe();
						obj3.AppendLine(string.Concat(obj4));
					}
				}
				Log.Warning(stringBuilder.ToString().TrimEndNewlines());
			}
			this.Clear();
		}

		public void Clear()
		{
			this.idsRead.Clear();
			this.idListsRead.Clear();
		}

		public void RegisterLoadIDReadFromXml(string targetLoadID, Type targetType, string pathRelToParent, IExposable parent)
		{
			for (int i = 0; i < this.idsRead.Count; i++)
			{
				IdRecord idRecord = this.idsRead[i];
				if (idRecord.parent == parent)
				{
					IdRecord idRecord2 = this.idsRead[i];
					if (idRecord2.pathRelToParent == pathRelToParent)
					{
						Log.Error("Tried to register the same load ID twice: " + targetLoadID + ", pathRelToParent=" + pathRelToParent + ", parent=" + parent.ToStringSafe());
						return;
					}
				}
			}
			this.idsRead.Add(new IdRecord(targetLoadID, targetType, pathRelToParent, parent));
		}

		public void RegisterLoadIDReadFromXml(string targetLoadID, Type targetType, string toAppendToPathRelToParent)
		{
			string text = Scribe.loader.curPathRelToParent;
			if (!toAppendToPathRelToParent.NullOrEmpty())
			{
				text = text + '/' + toAppendToPathRelToParent;
			}
			this.RegisterLoadIDReadFromXml(targetLoadID, targetType, text, Scribe.loader.curParent);
		}

		public void RegisterLoadIDListReadFromXml(List<string> targetLoadIDList, string pathRelToParent, IExposable parent)
		{
			for (int i = 0; i < this.idListsRead.Count; i++)
			{
				IdListRecord idListRecord = this.idListsRead[i];
				if (idListRecord.parent == parent)
				{
					IdListRecord idListRecord2 = this.idListsRead[i];
					if (idListRecord2.pathRelToParent == pathRelToParent)
					{
						Log.Error("Tried to register the same list of load IDs twice. pathRelToParent=" + pathRelToParent + ", parent=" + parent.ToStringSafe());
						return;
					}
				}
			}
			this.idListsRead.Add(new IdListRecord(targetLoadIDList, pathRelToParent, parent));
		}

		public void RegisterLoadIDListReadFromXml(List<string> targetLoadIDList, string toAppendToPathRelToParent)
		{
			string text = Scribe.loader.curPathRelToParent;
			if (!toAppendToPathRelToParent.NullOrEmpty())
			{
				text = text + '/' + toAppendToPathRelToParent;
			}
			this.RegisterLoadIDListReadFromXml(targetLoadIDList, text, Scribe.loader.curParent);
		}

		public string Take<T>(string pathRelToParent, IExposable parent)
		{
			int num = 0;
			string result;
			while (true)
			{
				if (num < this.idsRead.Count)
				{
					IdRecord idRecord = this.idsRead[num];
					if (idRecord.parent == parent)
					{
						IdRecord idRecord2 = this.idsRead[num];
						if (idRecord2.pathRelToParent == pathRelToParent)
						{
							IdRecord idRecord3 = this.idsRead[num];
							string targetLoadID = idRecord3.targetLoadID;
							Type typeFromHandle = typeof(T);
							IdRecord idRecord4 = this.idsRead[num];
							if (typeFromHandle != idRecord4.targetType)
							{
								object[] obj = new object[8]
								{
									"Trying to get load ID of object of type ",
									typeof(T),
									", but it was registered as ",
									null,
									null,
									null,
									null,
									null
								};
								IdRecord idRecord5 = this.idsRead[num];
								obj[3] = idRecord5.targetType;
								obj[4] = ". pathRelToParent=";
								obj[5] = pathRelToParent;
								obj[6] = ", parent=";
								obj[7] = parent.ToStringSafe<IExposable>();
								Log.Error(string.Concat(obj));
							}
							this.idsRead.RemoveAt(num);
							result = targetLoadID;
							break;
						}
					}
					num++;
					continue;
				}
				Log.Error("Could not get load ID. We're asking for something which was never added during LoadingVars. pathRelToParent=" + pathRelToParent + ", parent=" + parent.ToStringSafe<IExposable>());
				result = (string)null;
				break;
			}
			return result;
		}

		public List<string> TakeList(string pathRelToParent, IExposable parent)
		{
			int num = 0;
			List<string> result;
			while (true)
			{
				if (num < this.idListsRead.Count)
				{
					IdListRecord idListRecord = this.idListsRead[num];
					if (idListRecord.parent == parent)
					{
						IdListRecord idListRecord2 = this.idListsRead[num];
						if (idListRecord2.pathRelToParent == pathRelToParent)
						{
							IdListRecord idListRecord3 = this.idListsRead[num];
							List<string> targetLoadIDs = idListRecord3.targetLoadIDs;
							this.idListsRead.RemoveAt(num);
							result = targetLoadIDs;
							break;
						}
					}
					num++;
					continue;
				}
				Log.Error("Could not get load IDs list. We're asking for something which was never added during LoadingVars. pathRelToParent=" + pathRelToParent + ", parent=" + parent.ToStringSafe());
				result = new List<string>();
				break;
			}
			return result;
		}
	}
}
