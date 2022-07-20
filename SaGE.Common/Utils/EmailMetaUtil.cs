using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;


namespace SaGE.Common.Utils
{
    public class EmailMetaWriter : IDisposable
    {
        private XmlTextWriter _writer;

        private XmlSerializer _metaserializer;

        private XmlSerializerNamespaces _namespaces;

        private bool _wroteMeta = false;
        private bool _disposed = false;

        public bool IsDisposed { get { return _disposed; } }

        public EmailMetaWriter(Stream target, XmlSerializer emailMetaSerializer)
        {
            _namespaces = new XmlSerializerNamespaces();
            _namespaces.Add("", "");

            _metaserializer = emailMetaSerializer;
            
            _writer = new XmlTextWriter(target, new UTF8Encoding());

            _writer.Formatting = Formatting.Indented;
            _writer.IndentChar = ' ';
            _writer.Indentation = 3;
        }

        //public void WriteMeta(EmailMeta genCorrLayoutsGencorr)
        //{
        //    if (_disposed) throw new ObjectDisposedException("EmailMetaWriter");

        //    if (!_wroteMeta)
        //    {
        //        _writer.WriteStartElement("EmailMeta");
        //        _wroteMeta = true;
        //    }

        //    _metaserializer.Serialize(_writer, genCorrLayoutsGencorr, _namespaces);
        //}

        public void Flush()
        {
            if (_disposed) throw new ObjectDisposedException("EmailMetaWriter");
            _writer.Flush();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                if (_wroteMeta)
                {
                    _writer.WriteEndElement();
                    _wroteMeta = true;
                }

                _writer.Close();
                _disposed = true;
            }
        }
    }
}
