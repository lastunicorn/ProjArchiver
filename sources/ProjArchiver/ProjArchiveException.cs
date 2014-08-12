// ProjArchiver
// Copyright (C) 2011 Dust in the Wind
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Runtime.Serialization;

namespace DustInTheWind.ProjArchiver
{
    [Serializable]
    public class ProjArchiveException : ApplicationException
    {
        private const string DefaultErrorMessage = "Unknown error in ProjArchive application.";

        public ProjArchiveException()
            : base(DefaultErrorMessage)
        {
        }

        public ProjArchiveException(string message)
            : base(message)
        {
        }

        public ProjArchiveException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ProjArchiveException(Exception innerException)
            : base(DefaultErrorMessage, innerException)
        {
        }

        public ProjArchiveException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}