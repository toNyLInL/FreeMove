// FreeMove -- Move directories without breaking shortcuts or installations 
//    Copyright(C) 2020  Luca De Martini

//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.

//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
//    GNU General Public License for more details.

//    You should have received a copy of the GNU General Public License
//    along with this program.If not, see<http://www.gnu.org/licenses/>.

using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace FreeMove.IO
{
    /// <summary>
    /// Writes a traceability .ini file into the migration target folder root.
    /// </summary>
    static class MigrationTrace
    {
        /// <summary>
        /// Best-effort writer. It must NEVER throw, so it can safely be called
        /// at the end of a successful migration without breaking the flow.
        /// </summary>
        public static void Write(string source, string destination)
        {
            try
            {
                var text = new StringBuilder();
                text.AppendLine("[Migration]");
                text.AppendLine("Source=" + source);
                text.AppendLine("Destination=" + destination);
                text.AppendLine("Operation=Move");
                text.AppendLine("Time=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                text.AppendLine("Version=" + GetVersion());
                text.AppendLine("User=" + Environment.UserName);

                string path = Path.Combine(destination, "FreeMove-Migration.ini");
                // UTF-8 with BOM so Chinese/special characters remain readable.
                File.WriteAllText(path, text.ToString(), new UTF8Encoding(true));
            }
            catch (Exception)
            {
                // Best-effort: ignore write failures entirely.
            }
        }

        private static string GetVersion()
        {
            try
            {
                var info = FileVersionInfo.GetVersionInfo(Environment.ProcessPath);
                return info.FileVersion;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}