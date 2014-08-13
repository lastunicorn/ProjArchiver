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

using System.Collections.Generic;

namespace DustInTheWind.ProjArchiver.ConsoleApp
{
    class ConsoleOptions
    {
        public ConsoleAction Action { get; set; }

        public string ProjectDirectory { get; set; }

        public string Description { get; set; }

        public string WorkDirectory { get; set; }

        public string ProjectName { get; set; }

        public ConsoleOptions(IList<string> args)
        {
            switch (args[0])
            {
                case "--archive":
                case "-a":
                    Action = ConsoleAction.Archive;

                    if (args.Count > 1)
                        ProjectDirectory = args[1];

                    if (args.Count > 2)
                        Description = args[2];

                    break;

                case "--init":
                case "-i":
                    Action = ConsoleAction.Init;

                    if (args.Count > 1)
                        ProjectDirectory = args[1];

                    if (args.Count > 2)
                        Description = args[2];

                    break;

                case "--restore":
                case "-r":
                    Action = ConsoleAction.Restore;

                    if (args.Count > 1)
                        ProjectName = args[1];

                    if (args.Count > 2)
                        WorkDirectory = args[2];

                    break;

                default:
                    Action = ConsoleAction.None;
                    break;
            }
        }
    }
}
