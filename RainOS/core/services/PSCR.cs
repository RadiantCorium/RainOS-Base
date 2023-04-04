using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainOS.core.services
{
    /// <summary>
    /// Protected System Config Reader
    /// </summary>
    internal class PSCR
    {
        internal Dictionary<string, string> data = new Dictionary<string, string>();

        internal string path = "";

        /// <summary>
        /// Loads a new PSC manager.
        /// </summary>
        /// <param name="path">the path to load the config from</param>
        /// <param name="fallback">The dictionary to fallback to incase the configuration could not be loaded</param>
        internal PSCR(string path)
        {
            this.path = path;

            Load();
        }

        internal void Save()
        {
            try
            {
                if (data != null)
                {
                    string final = "";

                    foreach (var kv in data)
                    {
                        final += kv.Key + ":" + kv.Value + "\n";
                    }

                    var stream = File.CreateText(path);
                    stream.WriteLine(final.Trim());
                    stream.Close();
                }
            }
            catch (Exception e)
            {
                BSOD.Trigger(e);
            }
        }

        internal bool Load()
        {
            try
            {
                if (!File.Exists(path))
                {
                    throw new FileNotFoundException("PSCR: " + path);
                    return false;
                }

                data = new Dictionary<string, string>();
                string[] content = File.ReadAllLines(path);

                if (content.Length == 0)
                {
                    return false;
                }

                foreach (string line in content)
                {
                    var splitData = line.Split(':');
                    if (splitData.Length < 2)
                    {
                        continue;
                    }
                    data[splitData[0]] = splitData[1];
                }

                return true;
            }
            catch (Exception e)
            {
                BSOD.Trigger(e);
                return false;
            }
        }
    }
}
