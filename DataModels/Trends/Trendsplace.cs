using System.Collections;
using Twity.DataModels.Trend;

namespace Twity.DataModels.Trend {

	[Serializable]
	public class TrendsPlace
	{
		public Trend[] trends;
		public string as_of;
		public string created_at;
		public Woeid_location locaions;
	}
}
