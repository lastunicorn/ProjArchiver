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
    public class ArchiveTests
    {
        private readonly Mock<IStorage> storage;
        private readonly Archiver archiver;
        private readonly Mock<IFileCompressor> fileCompressor;

        public ArchiveTests()
        {
            storage = new Mock<IStorage>();
            fileCompressor = new Mock<IFileCompressor>();
            fileCompressor.SetupGet(x => x.DefaultExtension).Returns(".zip");

            archiver = new Archiver(storage.Object, fileCompressor.Object);
        }

        [Fact]
        public void throws_if_ArchiveDirectory_is_null()
        {
            archiver.ArchivesDirectoryFullPath = null;
            archiver.ProjectDirectoryFullPath = @"d:\Projects\MyProject";

            ProjArchiveException exception = Assert.Throws<ProjArchiveException>(() => archiver.Archive());

            Assert.Equal(Resources.Err_ArchivesDirectoryNotSpecified, exception.Message);
        }

        [Fact]
        public void throws_if_ArchiveDirectory_is_empty_string()
        {
            archiver.ArchivesDirectoryFullPath = string.Empty;
            archiver.ProjectDirectoryFullPath = @"d:\Projects\MyProject";

            ProjArchiveException exception = Assert.Throws<ProjArchiveException>(() => archiver.Archive());

            Assert.Equal(Resources.Err_ArchivesDirectoryNotSpecified, exception.Message);
        }

        [Fact]
        public void throws_if_ProjectDirectory_is_null()
        {
            archiver.ArchivesDirectoryFullPath = @"d:\Archives";
            archiver.ProjectDirectoryFullPath = null;

            ProjArchiveException exception = Assert.Throws<ProjArchiveException>(() => archiver.Archive());

            Assert.Equal(Resources.Err_ProjectDirectoryNotSpecified, exception.Message);
        }

        [Fact]
        public void throws_if_ProjectDirectory_is_empty_string()
        {
            archiver.ArchivesDirectoryFullPath = @"d:\Archives";
            archiver.ProjectDirectoryFullPath = string.Empty;

            ProjArchiveException exception = Assert.Throws<ProjArchiveException>(() => archiver.Archive());

            Assert.Equal(Resources.Err_ProjectDirectoryNotSpecified, exception.Message);
        }

        [Fact]
        public void uses_storage_to_verify_that_archive_does_not_exist()
        {
            storage.Setup(x => x.OpenFileToWrite(It.IsAny<string>())).Returns(Stream.Null);
            storage.Setup(x => x.ExistsDirectory(It.IsAny<string>()))
                .Returns(false);
            archiver.ArchivesDirectoryFullPath = @"d:\Archives";
            archiver.ProjectDirectoryFullPath = @"d:\Projects\MyProject";

            archiver.Archive();

            storage.Verify(x => x.ExistsDirectory(@"d:\Archives\MyProject"), Times.Once());
        }

        [Fact]
        public void throws_if_ProjectArchiveFile_already_exists()
        {
            storage.Setup(x => x.OpenFileToWrite(It.IsAny<string>())).Returns(Stream.Null);
            storage.Setup(x => x.ExistsFile(@"d:\Archives\MyProject\MyProject.zip"))
                .Returns(true);
            archiver.ArchivesDirectoryFullPath = @"d:\Archives";
            archiver.ProjectDirectoryFullPath = @"d:\Projects\MyProject";

            ProjArchiveException exception = Assert.Throws<ProjArchiveException>(() => archiver.Archive());

            Assert.Equal(DustInTheWind.ProjArchiver.Properties.Resources.Err_ArchiveAlreadyExists, exception.Message);
        }

        [Fact]
        public void uses_storage_to_create_project_archive_directory()
        {
            storage.Setup(x => x.OpenFileToWrite(It.IsAny<string>())).Returns(Stream.Null);
            archiver.ArchivesDirectoryFullPath = @"d:\Archives";
            archiver.ProjectDirectoryFullPath = @"d:\Projects\MyProject";

            archiver.Archive();

            storage.Verify(x => x.CreateDirectory(@"d:\Archives\MyProject"), Times.Once());
        }

        [Fact]
        public void uses_fileCompressor_to_compress_the_project_directory()
        {
            storage.Setup(x => x.OpenFileToWrite(It.IsAny<string>())).Returns(Stream.Null);
            fileCompressor.SetupGet(x => x.DefaultExtension).Returns(".zip");
            archiver.ArchivesDirectoryFullPath = @"d:\Archives";
            archiver.ProjectDirectoryFullPath = @"d:\Projects\MyProject";

            archiver.Archive();

            fileCompressor.Verify(x => x.Compress(@"d:\Projects\MyProject", @"d:\Archives\MyProject\MyProject.zip"), Times.Once());
        }

        [Fact]
        public void creates_an_archive_info_file()
        {
            MemoryStream descriptionFileStream = new MemoryStream();
            storage.Setup(x => x.OpenFileToWrite(@"d:\Archives\MyProject\MyProject.xml"))
                .Returns(descriptionFileStream);
            archiver.ArchivesDirectoryFullPath = @"d:\Archives";
            archiver.ProjectDirectoryFullPath = @"d:\Projects\MyProject";
            archiver.Description = "Description";

            archiver.Archive();

            descriptionFileStream.Seek(0, SeekOrigin.Begin);
            XmlAsserter xmlAsserter = new XmlAsserter(descriptionFileStream);
            xmlAsserter.NodeValue("/ArchiveInfo/Description", "Description");
        }

        [Fact]
        public void deletes_project_directory()
        {
            storage.Setup(x => x.OpenFileToWrite(It.IsAny<string>())).Returns(Stream.Null);
            storage.Setup(x => x.ExistsFile(@"d:\Archives\MyProject\MyProject.zip"))
                .Returns(false);
            archiver.ArchivesDirectoryFullPath = @"d:\Archives";
            archiver.ProjectDirectoryFullPath = @"d:\Projects\MyProject";

            archiver.Archive();

            storage.Verify(x => x.RemoveDirectory(@"d:\Projects\MyProject"), Times.Once());
        }
    }
}
