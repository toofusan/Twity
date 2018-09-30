using System;

namespace Twity.DataModels.Oauth
{
	[Serializable]
	public class AccessToken
	{
		public string oauth_token;
		public string oauth_token_secret;
		public bool oauth_callback_confirmed;
	}
}