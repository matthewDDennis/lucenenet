﻿using System;
using Lucene.Net.Util;
using Lucene.Net.Analysis.Tokenattributes;

namespace Lucene.Net.Analysis.En
{
    /*
	 * Licensed to the Apache Software Foundation (ASF) under one or more
	 * contributor license agreements.  See the NOTICE file distributed with
	 * this work for additional information regarding copyright ownership.
	 * The ASF licenses this file to You under the Apache License, Version 2.0
	 * (the "License"); you may not use this file except in compliance with
	 * the License.  You may obtain a copy of the License at
	 *
	 *     http://www.apache.org/licenses/LICENSE-2.0
	 *
	 * Unless required by applicable law or agreed to in writing, software
	 * distributed under the License is distributed on an "AS IS" BASIS,
	 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
	 * See the License for the specific language governing permissions and
	 * limitations under the License.
	 */

    /// <summary>
    /// TokenFilter that removes possessives (trailing 's) from words.
    /// <a name="version"/>
    /// <para>You must specify the required <seealso cref="Version"/>
    /// compatibility when creating EnglishPossessiveFilter:
    /// <ul>
    ///    <li> As of 3.6, U+2019 RIGHT SINGLE QUOTATION MARK and 
    ///         U+FF07 FULLWIDTH APOSTROPHE are also treated as
    ///         quotation marks.
    /// </ul>
    /// </para>
    /// </summary>
    public sealed class EnglishPossessiveFilter : TokenFilter
    {
        private readonly ICharTermAttribute termAtt;
        private LuceneVersion matchVersion;

        /// @deprecated Use <seealso cref="#EnglishPossessiveFilter(Version, TokenStream)"/> instead. 
        [Obsolete(@"Use <seealso cref=""#EnglishPossessiveFilter(org.apache.lucene.util.Version, org.apache.lucene.analysis.TokenStream)""/> instead.")]
        public EnglishPossessiveFilter(TokenStream input) : this(LuceneVersion.LUCENE_35, input)
        {
        }

        public EnglishPossessiveFilter(LuceneVersion version, TokenStream input) : base(input)
        {
            this.matchVersion = version;
            this.termAtt = AddAttribute<ICharTermAttribute>();
        }

        public override bool IncrementToken()
        {
            if (!input.IncrementToken())
            {
                return false;
            }
            char[] buffer = termAtt.Buffer();
            int bufferLength = termAtt.Length;

            if (bufferLength >= 2 && (buffer[bufferLength - 2] == '\'' || (matchVersion.OnOrAfter(LuceneVersion.LUCENE_36) && (buffer[bufferLength - 2] == '\u2019' || buffer[bufferLength - 2] == '\uFF07'))) && (buffer[bufferLength - 1] == 's' || buffer[bufferLength - 1] == 'S'))
            {
                termAtt.Length = bufferLength - 2; // Strip last 2 characters off
            }

            return true;
        }
    }
}