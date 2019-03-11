using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NoteAPI.Models;
using Microsoft.AspNetCore.Http;

namespace NoteAPI.Services
{
	public class TokenService
	{
		private readonly Dictionary<string, Token> currentTokens = new Dictionary<string, Token>();

		public TokenService() { }

		public bool IsExpired(Guid token)
		{
			if (this.currentTokens.ContainsKey(token.ToString()))
				return this.currentTokens[token.ToString()].ExpireTime > DateTime.UtcNow;
			return false;
		}

		public bool TryGetToken(HttpContext context, out Guid token)
		{
			Guid tempToken = Guid.Empty;
			if (context.Request.Query.ContainsKey("token"))
			{
				tempToken = new Guid(context.Request.Query["token"]);
				if (this.IsExpired(tempToken))
				{
					token = tempToken;
					return true;
				}
			}

			token = tempToken;
			return false;
		}

		public User GetUserByToken(Guid token)
		{
			if (token != Guid.Empty)
				return this.currentTokens[token.ToString()].UserData;
			return null;
		}

		public Guid AddToken(User user)
		{
			if (user != null)
			{
				Guid tokenID = Guid.NewGuid();
				this.currentTokens.Add(tokenID.ToString(), new Token()
				{
					ID = tokenID,
					ExpireTime = DateTime.UtcNow.AddMinutes(60),
					UserData = user
				});
				return tokenID;
			}
			else
				return Guid.Empty;
		}

		public class Token
		{
			public Guid ID { get; set; }
			public DateTime ExpireTime { get; set; }
			public User UserData { get; set; }
		}
	}
}
