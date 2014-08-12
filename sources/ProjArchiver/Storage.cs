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

namespace DustInTheWind.ProjArchiver
{
    public class Storage : IStorage
    {
        public void CreateDirectory(string directoryFullPath)
        {
            Directory.CreateDirectory(directoryFullPath);
        }

        public Stream OpenFileToWrite(string fileFullPath)
        {
            return File.Open(fileFullPath, FileMode.Create, FileAccess.Write);
        }

        public bool ExistsDirectory(string directoryFullPath)
        {
            return Directory.Exists(directoryFullPath);
        }

        public void RemoveDirectory(string directoryFullPath)
        {
            Directory.Delete(directoryFullPath, true);
        }

        public void DeleteFile(string fileFullPath)
        {
            File.Delete(fileFullPath);
        }

        public bool ExistsFile(string fileFullPath)
        {
            return File.Exists(fileFullPath);
        }
    }
}