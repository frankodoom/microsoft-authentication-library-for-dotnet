﻿//----------------------------------------------------------------------
// Copyright (c) Microsoft Open Technologies, Inc.
// All Rights Reserved
// Apache License 2.0
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//----------------------------------------------------------------------

using System;
using Microsoft.Identity.Client.Internal;

namespace Microsoft.Identity.Client.Handlers
{
    class AcquireTokenByAuthorizationCodeHandler : AcquireTokenHandlerBase
    {
        private readonly string authorizationCode;

        private readonly Uri redirectUri;

        public AcquireTokenByAuthorizationCodeHandler(HandlerData handlerData, string authorizationCode, Uri redirectUri)
            : base(handlerData)
        {
            if (string.IsNullOrWhiteSpace(authorizationCode))
            {
                throw new ArgumentNullException("authorizationCode");
            }

            this.authorizationCode = authorizationCode;

            if (redirectUri == null)
            {
                throw new ArgumentNullException("redirectUri");
            }

            this.redirectUri = redirectUri;

            this.LoadFromCache = false;
            this.SupportADFS = false;
        }

        protected override void AddAditionalRequestParameters(DictionaryRequestParameters requestParameters)
        {
            requestParameters[OAuthParameter.GrantType] = OAuthGrantType.AuthorizationCode;
            requestParameters[OAuthParameter.Code] = this.authorizationCode;
            requestParameters[OAuthParameter.RedirectUri] = this.redirectUri.OriginalString;
        }

        protected override void PostTokenRequest(AuthenticationResultEx resultEx)
        {
            base.PostTokenRequest(resultEx);
            this.User = resultEx.Result.User;
        }
    }
}