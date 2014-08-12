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
using System.IO;
using DustInTheWind.ProjArchiver.Properties;
using NLog;

namespace DustInTheWind.ProjArchiver
{
    public class Restorer
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly IFileCompressor fileCompressor;
        private readonly IStorage storage;
        private string archiveFileFullPath;

        public string ArchivesDirectoryFullPath { get; set; }
        public string WorkDirectoryFullPath { get; set; }
        public string ProjectName { get; set; }

        public Restorer(IStorage storage, IFileCompressor fileCompressor)
        {
            if (storage == null)
                throw new ArgumentNullException("storage");

            if (fileCompressor == null)
                throw new ArgumentNullException("fileCompressor");

            this.storage = storage;
            this.fileCompressor = fileCompressor;
        }

        public void Restore()
        {
            if (string.IsNullOrEmpty(ProjectName))
                throw new ProjArchiveException(Resources.Err_ProjectNameNotSpecified);

            if (string.IsNullOrEmpty(ArchivesDirectoryFullPath))
                throw new ProjArchiveException(Resources.Err_ArchivesDirectoryNotSpecified);

            if (string.IsNullOrEmpty(WorkDirectoryFullPath))
                throw new ProjArchiveException(Resources.Err_WorkDirectoryNotSpecified);

            logger.Info("Restoring project '{0}'.", ProjectName);

            archiveFileFullPath = Path.Combine(ArchivesDirectoryFullPath, ProjectName + Path.DirectorySeparatorChar + ProjectName + fileCompressor.DefaultExtension);

            DecompressProject(WorkDirectoryFullPath);

            DeleteCompressedFile();
        }

        private void DecompressProject(string workDirectoryFullPath)
        {
            string projectDirectoryFullPath = Path.Combine(workDirectoryFullPath, ProjectName);

            if (storage.ExistsDirectory(projectDirectoryFullPath))
                throw new ProjArchiveException(Resources.Err_ProjectAlreadyExists);

            logger.Info("Decompressing project: '{0}' into directory '{1}'", archiveFileFullPath, workDirectoryFullPath);
            fileCompressor.Decompress(archiveFileFullPath, workDirectoryFullPath);
        }

        private void DeleteCompressedFile()
        {
            logger.Info("Deleting the compressed file '{0}'.", archiveFileFullPath);
            storage.DeleteFile(archiveFileFullPath);
        }
    }
}