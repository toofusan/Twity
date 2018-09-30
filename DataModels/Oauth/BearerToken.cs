using System;

namespace Twity.DataModels.Oauth
{
	[Serializable]
	public class BearerToken
	{
		public string token_type;
		public string access_token;
	}
}
