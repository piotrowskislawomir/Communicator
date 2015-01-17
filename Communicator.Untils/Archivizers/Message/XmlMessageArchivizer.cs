using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Linq;
using Communicator.Protocol.Model;
using Communicator.Protocol.Requests;

namespace Communicator.Untils.Archivizers.Message
{
    public class XmlMessageArchivizer : IMessageArchivizer
    {
       // string pathToArchivize = "Archiwum_Wiadomości.xml";

        private void CreateArchivizeXmlFile(MessageReq msg, string pathToArchivize)
        {
            try
            {
                var newMessage =
                    new XElement("Messages",
                    new XElement("Message",
                                             new XAttribute("Recipient", msg.Recipient),
                                             new XAttribute("Sender", msg.Login),
                                             new XAttribute("Date", DateTime.Now),
                                new XElement("Body", msg.Message),
                                    new XElement("AttachmentData", msg.Attachment.Data),
                                    new XElement("AttachmentMimeType", msg.Attachment.MimeType),
                                    new XElement("AttachmentName", msg.Attachment.Name)
                                        ));

                newMessage.Save(pathToArchivize);
            }
            catch (Exception)
            {
                Console.WriteLine("Błąd w tworzeniu pliku archiwizacji wiadomości");
            }
        }

        private void AddNewElementToXmlFile(MessageReq msg, string pathToArchivize)
        {
            try
            {
                var xmlFile = XDocument.Load(pathToArchivize);

                var newMessage =
                                
                    new XElement("Message",
                                             new XAttribute("Recipient", msg.Recipient),
                                              new XAttribute("Sender", msg.Login),
                                              new XAttribute("Date", DateTime.Now),
                                new XElement("Body", msg.Message),
                                    new XElement("AttachmentData", msg.Attachment.Data),
                                    new XElement("AttachmentMimeType", msg.Attachment.MimeType),
                                    new XElement("AttachmentName", msg.Attachment.Name)
                                        );

                xmlFile.Element("Messages").Add(newMessage);
                xmlFile.Save(pathToArchivize);
            }
            catch (Exception)
            {
                Console.WriteLine("Błąd w załadowaniu pliku " + pathToArchivize + " lub dodaniu kolejnej pozycji do pliku XML");
            }
        }

        public void Save(MessageReq msg, string pathToArchivize)
        {
            if (File.Exists(pathToArchivize))
                AddNewElementToXmlFile(msg, pathToArchivize);
            else
                CreateArchivizeXmlFile(msg, pathToArchivize);
       }

        public List<MessageNotification> Read(User user, string pathToArchivize)
        {
            XDocument doc = XDocument.Load(pathToArchivize);
            var Messages = (from c in doc.Descendants("Message")
                where (string) c.Attribute("Recipient") == user.Login || (string) c.Attribute("Sender") == user.Login
                select new MessageNotification
                {
                    Sender = (string) c.Attribute("Sender"),
                    Recipient = (string) c.Attribute("Recipient"),
                    Message = (string) c.Element("Body"),
                    SendTime = (DateTime) c.Attribute("Date"),
                    
                    Attachment = new Attachment
                    {
                        Data = Encoding.ASCII.GetBytes((string)c.Element("AttachmentData")),
                        MimeType = (string) c.Element("AttachmentMimeType"),
                        Name = (string) c.Element("AttachmentName")
                    }
               }
            ).ToList();
           return Messages;
        }

        // funkcja testowa do zapisuss
        public static void TestWrite()
        {
            var msg = new MessageReq();
            msg.Message = "Siema Sławek!";
            msg.Login = "Michał";
            msg.Recipient = "Sławek";
            msg.Attachment = new Attachment();

            var msg2 = new MessageReq();
            msg.Message = "Siema Monika!";
            msg.Login = "Sławek";
            msg.Recipient = "Monika";
            msg.Attachment = new Attachment();

            var msg3 = new MessageReq();
            msg.Message = "Siema Adrian!";
            msg.Login = "Monika";
            msg.Recipient = "Adrian";
            msg.Attachment = new Attachment();

            var arch = new XmlMessageArchivizer();
            arch.Save(msg, "Archiwum_Wiadomości.xml");
            arch.Save(msg2, "Archiwum_Wiadomości.xml");
            arch.Save(msg3, "Archiwum_Wiadomości.xml");
        }
    }
}
