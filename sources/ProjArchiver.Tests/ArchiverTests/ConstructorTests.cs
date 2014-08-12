// ProjArchiver
// Copyright (C) 2014 Dust in the Wind
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
using Moq;
using Xunit;

namespace DustInTheWind.ProjArchiver.Tests.ArchiverTests
{
    public class ConstructorTests
    {
        [Fact]
        public void throws_if_storage_is_null()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new Archiver(null, new Mock<IFileCompressor>().Object));

            Assert.Equal("storage", exception.ParamName);
        }

        [Fact]
        public void throws_if_fileCompressor_is_null()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new Archiver(new Mock<IStorage>().Object, null));

            Assert.Equal("fileCompressor", exception.ParamName);
        }
    }
}
