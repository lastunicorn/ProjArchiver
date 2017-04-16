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

using DustInTheWind.ProjArchiver.Properties;
using Moq;
using Xunit;

namespace DustInTheWind.ProjArchiver.Tests.RestorerTests
{
    public class RestoreTests
    {
        private readonly Mock<IFileCompressor> fileCompressor;
        private readonly Restorer restorer;
        private readonly Mock<IStorage> storage;

        public RestoreTests()
        {
            fileCompressor = new Mock<IFileCompressor>();
            storage = new Mock<IStorage>();
            fileCompressor.SetupGet(x => x.DefaultExtension).Returns(".zip");
            restorer = new Restorer(storage.Object, fileCompressor.Object);
        }

        [Fact]
        public void throws_if_ProjectName_is_null()
        {
            restorer.ArchivesDirectoryFullPath = @"c:\path\to\archives";
            restorer.WorkDirectoryFullPath = @"c:\path\to\projects";
            restorer.ProjectName = null;

            ProjArchiveException exception = Assert.Throws<ProjArchiveException>(() => restorer.Restore());

            Assert.Equal(Resources.Err_ProjectNameNotSpecified, exception.Message);
        }

        [Fact]
        public void throws_if_ProjectName_is_empty_string()
        {
            restorer.ArchivesDirectoryFullPath = @"c:\path\to\archives";
            restorer.WorkDirectoryFullPath = @"c:\path\to\projects";
            restorer.ProjectName = string.Empty;

            ProjArchiveException exception = Assert.Throws<ProjArchiveException>(() => restorer.Restore());

            Assert.Equal(Resources.Err_ProjectNameNotSpecified, exception.Message);
        }

        [Fact]
        public void throws_if_WorkDirectoryFullPath_is_null()
        {
            restorer.ArchivesDirectoryFullPath = @"c:\path\to\archives";
            restorer.WorkDirectoryFullPath = null;
            restorer.ProjectName = "MyProject";

            ProjArchiveException exception = Assert.Throws<ProjArchiveException>(() => restorer.Restore());

            Assert.Equal(Resources.Err_WorkDirectoryNotSpecified, exception.Message);
        }

        [Fact]
        public void throws_if_WorkDirectoryFullPath_is_empty_string()
        {
            restorer.ArchivesDirectoryFullPath = @"c:\path\to\archives";
            restorer.WorkDirectoryFullPath = string.Empty;
            restorer.ProjectName = "MyProject";

            ProjArchiveException exception = Assert.Throws<ProjArchiveException>(() => restorer.Restore());

            Assert.Equal(Resources.Err_WorkDirectoryNotSpecified, exception.Message);
        }

        [Fact]
        public void throws_if_ArchivesDirectoryFullPath_is_null()
        {
            restorer.ArchivesDirectoryFullPath = null;
            restorer.WorkDirectoryFullPath = @"c:\path\to\projects";
            restorer.ProjectName = "MyProject";

            ProjArchiveException exception = Assert.Throws<ProjArchiveException>(() => restorer.Restore());

            Assert.Equal(Resources.Err_ArchivesDirectoryNotSpecified, exception.Message);
        }

        [Fact]
        public void throws_if_ArchivesDirectoryFullPath_is_empty_string()
        {
            restorer.ArchivesDirectoryFullPath = string.Empty;
            restorer.WorkDirectoryFullPath = @"c:\path\to\projects";
            restorer.ProjectName = "MyProject";

            ProjArchiveException exception = Assert.Throws<ProjArchiveException>(() => restorer.Restore());

            Assert.Equal(Resources.Err_ArchivesDirectoryNotSpecified, exception.Message);
        }

        [Fact]
        public void throws_if_project_directory_exists()
        {
            storage.Setup(x => x.ExistsDirectory(@"c:\path\to\projects\MyProject"))
                .Returns(true);
            restorer.ArchivesDirectoryFullPath = @"c:\path\to\archives";
            restorer.WorkDirectoryFullPath = @"c:\path\to\projects";
            restorer.ProjectName = "MyProject";

            ProjArchiveException exception = Assert.Throws<ProjArchiveException>(() => restorer.Restore());

            Assert.Equal(Resources.Err_ProjectAlreadyExists, exception.Message);
        }

        [Fact]
        public void uses_fileCompressor_to_decompress_archive()
        {
            restorer.ArchivesDirectoryFullPath = @"c:\path\to\archives";
            restorer.WorkDirectoryFullPath = @"c:\path\to\projects";
            restorer.ProjectName = "MyProject";

            restorer.Restore();

            fileCompressor.Verify(x => x.Decompress(@"c:\path\to\archives\MyProject\MyProject.zip", @"c:\path\to\projects"), Times.Once());
        }

        [Fact]
        public void uses_storage_to_delete_archive()
        {
            restorer.ArchivesDirectoryFullPath = @"c:\path\to\archives";
            restorer.WorkDirectoryFullPath = @"c:\path\to\projects";
            restorer.ProjectName = "MyProject";

            restorer.Restore();

            storage.Verify(x => x.DeleteFile(@"c:\path\to\archives\MyProject\MyProject.zip"), Times.Once());
        }
    }
}
