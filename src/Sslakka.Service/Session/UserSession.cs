// -----------------------------------------------------------------------
// <copyright file="UserSession.cs" company="Sslakka">
//   Copyright (c) 2015 Sslakka and its contributors
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
// -----------------------------------------------------------------------

namespace Sslakka.Service
{
    using System.Collections.Generic;

    using ServiceStack;
    using ServiceStack.Auth;

    /// <summary>
    /// The UserSession interface.
    /// </summary>
    public interface IUserSession
        : IAuthSession
    {
    }

    /// <summary>
    /// The custom user session.
    /// </summary>
    public class UserSession
        : AuthUserSession, IUserSession
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserSession"/> class.
        /// </summary>
        public UserSession()
        {
        }

        /// <summary>
        /// The on-registered event handler.
        /// </summary>
        /// <param name="httpReq">The HTTP request.</param>
        /// <param name="session">The authentication session.</param>
        /// <param name="service">The service.</param>
        public override void OnRegistered(ServiceStack.Web.IRequest httpReq, IAuthSession session, IServiceBase service)
        {
            base.OnRegistered(httpReq, session, service);
        }

        /// <summary>
        /// The on-authenticated event handler.
        /// </summary>
        /// <param name="authService">The authentication service.</param>
        /// <param name="session">The authentication session.</param>
        /// <param name="tokens">The authentication tokens.</param>
        /// <param name="authInfo">The authentication information.</param>
        public override void OnAuthenticated(
            IServiceBase authService,
            IAuthSession session,
            IAuthTokens tokens,
            Dictionary<string, string> authInfo)
        {
            base.OnAuthenticated(authService, session, tokens, authInfo);
        }

        /// <summary>
        /// The on-logout event handler.
        /// </summary>
        /// <param name="authService">The authentication service.</param>
        public override void OnLogout(IServiceBase authService)
        {
            base.OnLogout(authService);
        }
    }
}
