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

using System.IO.Compression;

namespace DustInTheWind.ProjArchiver
{
    public class FileCompressor : IFileCompressor
    {
        public string DefaultExtension
        {
            get { return ".zip"; }
        }

        public void Compress(string sourceDirectoryFullPath, string destinationArchiveFileFullPath)
        {
            ZipFile.CreateFromDirectory(sourceDirectoryFullPath, destinationArchiveFileFullPath, CompressionLevel.Optimal, true);
        }

        public void Decompress(string sourceArchiveFileFullPath, string destinationDirectoryFullPath)
        {
            ZipFile.ExtractToDirectory(sourceArchiveFileFullPath, destinationDirectoryFullPath);
        }
    }
}