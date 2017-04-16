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

using System.IO;
using DustInTheWind.ProjArchiver.Properties;
using Moq;
using Xunit;

namespace DustInTheWind.ProjArchiver.Tests.ArchiverTests
{
    public class InitTests
    {
        private readonly Mock<IStorage> storage;
        private readonly Archiver archiver;

        public InitTests()
        {
            storage = new Mock<IStorage>();
            Mock<IFileCompressor> fileCompressor = new Mock<IFileCompressor>();
            fileCompressor.SetupGet(x => x.DefaultExtension).Returns(".zip");

            archiver = new Archiver(storage.Object, fileCompressor.Object);
        }

        [Fact]
        public void throws_if_ArchivesDirectory_is_null()
        {
            archiver.ArchivesDirectoryFullPath = null;
            archiver.ProjectDirectoryFullPath = @"c:\path\to\projects\MyProject";

            ProjArchiveException exception = Assert.Throws<ProjArchiveException>(() => archiver.Init());

            Assert.Equal(Resources.Err_ArchivesDirectoryNotSpecified, exception.Message);
        }

        [Fact]
        public void throws_if_ArchivesDirectory_is_empty_string()
        {
            archiver.ArchivesDirectoryFullPath = string.Empty;
            archiver.ProjectDirectoryFullPath = @"c:\path\to\projects\MyProject";

            ProjArchiveException exception = Assert.Throws<ProjArchiveException>(() => archiver.Init());

            Assert.Equal(Resources.Err_ArchivesDirectoryNotSpecified, exception.Message);
        }

        [Fact]
        public void throws_if_ProjectDirectory_is_null()
        {
            archiver.ArchivesDirectoryFullPath = @"c:\path\to\archives";
            archiver.ProjectDirectoryFullPath = null;

            ProjArchiveException exception = Assert.Throws<ProjArchiveException>(() => archiver.Init());

            Assert.Equal(Resources.Err_ProjectDirectoryNotSpecified, exception.Message);
        }

        [Fact]
        public void throws_if_ProjectDirectory_is_empty_string()
        {
            archiver.ArchivesDirectoryFullPath = @"c:\path\to\archives";
            archiver.ProjectDirectoryFullPath = string.Empty;

            ProjArchiveException exception = Assert.Throws<ProjArchiveException>(() => archiver.Init());

            Assert.Equal(Resources.Err_ProjectDirectoryNotSpecified, exception.Message);
        }

        [Fact]
        public void does_not_create_archive_directory_if_it_already_exists()
        {
            storage.Setup(x => x.OpenFileToWrite(It.IsAny<string>())).Returns(Stream.Null);
            storage.Setup(x => x.ExistsDirectory(@"c:\path\to\archives\MyProject")).Returns(true);
            archiver.ArchivesDirectoryFullPath = @"c:\path\to\archives";
            archiver.ProjectDirectoryFullPath = @"c:\path\to\projects\MyProject";

            archiver.Init();

            storage.Verify(x => x.CreateDirectory(@"c:\path\to\archives\MyProject"), Times.Never());
        }

        [Fact]
        public void uses_storage_to_create_project_archive_directory_if_it_does_not_already_exist()
        {
            storage.Setup(x => x.OpenFileToWrite(It.IsAny<string>())).Returns(Stream.Null);
            storage.Setup(x => x.ExistsDirectory(@"c:\path\to\archives\MyProject")).Returns(false);
            archiver.ArchivesDirectoryFullPath = @"c:\path\to\archives";
            archiver.ProjectDirectoryFullPath = @"c:\path\to\projects\MyProject";

            archiver.Init();

            storage.Verify(x => x.CreateDirectory(@"c:\path\to\archives\MyProject"), Times.Once());
        }

        [Fact]
        public void creates_an_archive_info_file()
        {
            MemoryStream descriptionFileStream = new MemoryStream();
            storage.Setup(x => x.OpenFileToWrite(@"c:\path\to\archives\MyProject\MyProject.xml")).Returns(descriptionFileStream);
            archiver.ArchivesDirectoryFullPath = @"c:\path\to\archives";
            archiver.ProjectDirectoryFullPath = @"c:\path\to\projects\MyProject";
            archiver.Description = "Description";

            archiver.Init();

            descriptionFileStream.Seek(0, SeekOrigin.Begin);
            XmlAsserter xmlAsserter = new XmlAsserter(descriptionFileStream);
            xmlAsserter.NodeValue("/ArchiveInfo/Description", "Description");
        }
    }
}
