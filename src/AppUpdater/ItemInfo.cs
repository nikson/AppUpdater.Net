using System;
using System.Xml.Serialization;

namespace AppUpdater
{
    [Serializable]
    public class ItemInfo
    {
        #region Properties , Fields

        private String fileName;
        private long fileSize;
        private String downloadUrl;
        private String contentType;
        private String path;

        [XmlAttribute("Name")]
        public String FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        [XmlAttribute("Size")]
        public long FileSize
        {
            get { return fileSize; }
            set { fileSize = value; }
        }

        public String DownloadUrl
        {
            get { return downloadUrl; }
            set { downloadUrl = value; }
        }

        public String ContentType
        {
            get { return contentType; }
            set { contentType = value; }
        }

        public String FullPath
        {
            get { return path; }
            set { path = value; }
        }

        #endregion
        
        #region Constructor 

        public ItemInfo()
        {
        }

        public ItemInfo(String filename, long size, String dllink, String contentype)
        {
            this.fileName = filename;
            this.fileSize = size;
            this.downloadUrl = dllink;
            this.contentType = contentype;
        }

        #endregion

    }
}
