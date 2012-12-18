using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using Common.Logging;

namespace AppUpdater
{
    public class VersionFile
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(VersionFile));

        public String NewVersion { get; set; }
        // bool RollbackVersion { get; set; }
        public String StartupFile { get; set; }

        private List<ItemInfo> _filesItem = new List<ItemInfo>();
        public List<ItemInfo> Files
        {
            get { return _filesItem; }
            set { _filesItem = value; }
        }

        public VersionFile() { }

        #region Methods
        /// <summary>
        /// If use xml serializer with 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static VersionFile LoadFile(String file)
        {
            VersionFile ver = null;

            try
            {
                XmlSerializer formatter = new XmlSerializer(typeof(VersionFile));
                using (StreamReader reader = new StreamReader(file))
                {
                    ver = formatter.Deserialize(reader) as VersionFile;
                }
            }
            catch(Exception ex)
            {
                Log.Debug(ex.Message);
            }

            return ver;
        }

        public static VersionFile LoadFileFromStream(Stream data)
        {
            VersionFile ver = null;

            try
            {
                XmlSerializer formater = new XmlSerializer(typeof(VersionFile));
                using (StreamReader reader = new StreamReader(data))
                {
                    ver = formater.Deserialize(reader) as VersionFile;
                }
            }
            catch (Exception ex)
            {
                Log.Debug(ex.Message);
            }

            return ver;
        }

        public VersionFile GetVersionFileFromStream(Stream stream)
        {
            return VersionFile.LoadFileFromStream(stream);
        }

        public void Save(String filename)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(VersionFile));

                using (StreamWriter sw = new StreamWriter(filename))
                {
                    xs.Serialize(sw, this);
                    sw.Close();
                }
            }
            catch { throw; }
        }
        #endregion
    }
}
