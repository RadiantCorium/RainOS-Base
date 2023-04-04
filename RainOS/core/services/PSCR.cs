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

        internal void Load()
        {
            try
            {
                if (!File.Exists(path))
                    throw new FileNotFoundException("PSCR: " + path);

                data = new Dictionary<string, string>();
                string[] content = File.ReadAllLines(path);

                foreach (string line in content)
                {
                    var splitData = line.Split(':');
                    data[splitData[0]] = splitData[1];
                }
            }
            catch (Exception e)
            {
                BSOD.Trigger(e);
            }
        }
    }
}
