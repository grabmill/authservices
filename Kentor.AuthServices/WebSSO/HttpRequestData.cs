﻿using Kentor.AuthServices.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Claims;

namespace Kentor.AuthServices.WebSso
{
    /// <summary>
    /// The data of a http request that AuthServices needs to handle. A separate DTO is used
    /// to make the core library totally independent of the hosting environment.
    /// </summary>
    public class HttpRequestData
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="httpMethod">Http method of the request</param>
        /// <param name="url">Full url requested</param>
        /// <param name="applicationPath">Path to the application root</param>
        /// <param name="formData">Form data, if present (only for POST requests)</param>
        /// <param name="queryString">The query string parameters of the request</param>
        /// <param name="nameIdentifier">The claim that contains the nameIdentifier of the logged in user</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public HttpRequestData(
            string httpMethod,
            Uri url,
            string applicationPath,
            IEnumerable<KeyValuePair<string, string[]>> formData,
            NameValueCollection queryString = null,
            Claim nameIdentifier = null)
        {
            Init(httpMethod, url, applicationPath, formData, queryString, nameIdentifier);
        }

        // Used by tests.
        internal HttpRequestData(string httpMethod, Uri url)
        {
            Init(httpMethod, url, "/", null, null, null);
        }

        private void Init(
            string httpMethod,
            Uri url,
            string applicationPath,
            IEnumerable<KeyValuePair<string, string[]>> formData,
            NameValueCollection queryString,
            Claim nameIdentifier)
        {
            HttpMethod = httpMethod;
            Url = url;
            ApplicationUrl = new Uri(url, applicationPath);
            Form = new ReadOnlyDictionary<string, string>(
                (formData ?? Enumerable.Empty<KeyValuePair<string, string[]>>())
                .ToDictionary(kv => kv.Key, kv => kv.Value.Single()));
            QueryString = queryString ?? new NameValueCollection();
            NameIdentifier = nameIdentifier;
        }

        /// <summary>
        /// The http method of the request.
        /// </summary>
        public string HttpMethod { get; private set; }

        /// <summary>
        /// The complete Url of the request.
        /// </summary>
        public Uri Url { get; private set; }

        /// <summary>
        /// The form data associated with the request (if any).
        /// </summary>
        public IReadOnlyDictionary<string, string> Form { get; private set; }

        /// <summary>
        /// The query string parameters of the request.
        /// </summary>
        public NameValueCollection QueryString { get; private set; }

        /// <summary>
        /// The root Url of the application. This includes the virtual directory
        /// that the application is installed in, e.g. http://hosting.example.com/myapp/
        /// </summary>
        public Uri ApplicationUrl { get; private set; }

        /// <summary>
        /// The Claim that contains the nameidentifier of the user that is currently logged in
        /// </summary>
        public Claim NameIdentifier { get; private set; }
    }
}
