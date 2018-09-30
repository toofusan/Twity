using System;

namespace Twity.DataModels.Oauth
{
	[Serializable]
	public class AccessToken
	{
		public string token_type;
		public string access_token;
	}
}
