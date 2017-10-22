using System;
using System.Collections.Generic;

namespace Verse
{
	public class RoomRoleDef : Def
	{
		public Type workerClass;

		private List<RoomStatDef> relatedStats = null;

		[Unsaved]
		private RoomRoleWorker workerInt = null;

		public RoomRoleWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (RoomRoleWorker)Activator.CreateInstance(this.workerClass);
				}
				return this.workerInt;
			}
		}

		public bool IsStatRelated(RoomStatDef def)
		{
			bool result;
			if (this.relatedStats == null)
			{
				result = false;
			}
			else
			{
				for (int i = 0; i < this.relatedStats.Count; i++)
				{
					if (this.relatedStats[i] == def)
						goto IL_002d;
				}
				result = false;
			}
			goto IL_0051;
			IL_002d:
			result = true;
			goto IL_0051;
			IL_0051:
			return result;
		}
	}
}
