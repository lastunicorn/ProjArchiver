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
using System.Xml.Serialization;
using DustInTheWind.ProjArchiver.Properties;
using NLog;

namespace DustInTheWind.ProjArchiver
{
    public class Archiver
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly IFileCompressor fileCompressor;
        private readonly IStorage storage;

        private string projectDirectoryName;
        private string projectArchiveDirectoryFullPath;

        public string ArchivesDirectoryFullPath { get; set; }
        public string ProjectDirectoryFullPath { get; set; }
        public string Description { get; set; }

        public Archiver(IStorage storage, IFileCompressor fileCompressor)
        {
            if (storage == null)
                throw new ArgumentNullException("storage");

            if (fileCompressor == null)
                throw new ArgumentNullException("fileCompressor");

            this.storage = storage;
            this.fileCompressor = fileCompressor;
        }

        public void Archive()
        {
            if (string.IsNullOrEmpty(ArchivesDirectoryFullPath))
                throw new ProjArchiveException(Resources.Err_ArchivesDirectoryNotSpecified);

            if (string.IsNullOrEmpty(ProjectDirectoryFullPath))
                throw new ProjArchiveException(Resources.Err_ProjectDirectoryNotSpecified);

            logger.Info("Archiving project '{0}'.", ProjectDirectoryFullPath);

            projectDirectoryName = Path.GetFileName(ProjectDirectoryFullPath);
            projectArchiveDirectoryFullPath = Path.Combine(ArchivesDirectoryFullPath, projectDirectoryName);

            CreateArchiveDirectory();
            CreateArchiveInfoFile();
            CreateArchiveFile();
            DeleteProjectDirectory();
        }

        public void Init()
        {
            if (string.IsNullOrEmpty(ArchivesDirectoryFullPath))
                throw new ProjArchiveException(Resources.Err_ArchivesDirectoryNotSpecified);

            if (string.IsNullOrEmpty(ProjectDirectoryFullPath))
                throw new ProjArchiveException(Resources.Err_ProjectDirectoryNotSpecified);

            logger.Info("Initializing archive directory '{0}'.", projectArchiveDirectoryFullPath);

            projectDirectoryName = Path.GetFileName(ProjectDirectoryFullPath);
            projectArchiveDirectoryFullPath = Path.Combine(ArchivesDirectoryFullPath, projectDirectoryName);

            CreateArchiveDirectory();
            CreateArchiveInfoFile();
        }

        private void CreateArchiveDirectory()
        {
            logger.Info("Creating archive directory: '{0}'.", projectArchiveDirectoryFullPath);

            if (!storage.ExistsDirectory(projectArchiveDirectoryFullPath))
                storage.CreateDirectory(projectArchiveDirectoryFullPath);
        }

        private void CreateArchiveFile()
        {
            string archiveFileFullPath = Path.Combine(projectArchiveDirectoryFullPath, projectDirectoryName + fileCompressor.DefaultExtension);

            if (storage.ExistsFile(archiveFileFullPath))
                throw new ProjArchiveException(Resources.Err_ArchiveAlreadyExists);

            logger.Info("Creating compressed file: '{0}'.", archiveFileFullPath);

            fileCompressor.Compress(ProjectDirectoryFullPath, archiveFileFullPath);
        }

        private void CreateArchiveInfoFile()
        {
            string infoFileFullPath = Path.Combine(projectArchiveDirectoryFullPath, projectDirectoryName + ".xml");

            logger.Info("Creating archive info file: '{0}'.", infoFileFullPath);

            Stream infoFileStream = storage.OpenFileToWrite(infoFileFullPath);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ArchiveInfo));

            ArchiveInfo archiveInfo = new ArchiveInfo
            {
                Description = Description
            };

            xmlSerializer.Serialize(infoFileStream, archiveInfo);
            infoFileStream.Flush();
        }

        private void DeleteProjectDirectory()
        {
            logger.Info("Deleting project directory: '{0}'.", ProjectDirectoryFullPath);

            storage.RemoveDirectory(ProjectDirectoryFullPath);
        }
    }
}