using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Communicator.Untils.Archivizers.Message
{
    class XmlMessageArchivizer : IMessageArchivizer
    {
       // string pathToArchivize = "Archiwum_Wiadomości.xml";

        private void CreateArchivizeXmlFile(Message msg, string pathToArchivize)
        {
            try
            {
                var newMessage =
                    new XElement("Messages",
                    new XElement("Message",
                                             new XAttribute("Recipient", msg.Recipient),
                                              new XAttribute("Sender", msg.Sender),
                                              new XAttribute("Date", DateTime.Now),
                                new XElement("Body", msg.Body)
                                        ));

                newMessage.Save(pathToArchivize);
            }
            catch (Exception)
            {
                Console.WriteLine("Błąd w tworzeniu pliku");
            }
        }

        private void AddNewElementToXmlFile(Message msg, string pathToArchivize)
        {
            try
            {
                var xmlFile = XDocument.Load(pathToArchivize);

                var newMessage =
                                new XElement("Message",
                                             new XAttribute("Recipient", msg.Recipient),
                                              new XAttribute("Sender", msg.Sender),
                                              new XAttribute("Date", DateTime.Now),
                                new XElement("Body", msg.Body)
                                        );

                xmlFile.Element("Messages").Add(newMessage);
                xmlFile.Save(pathToArchivize);
            }
            catch (Exception)
            {
                Console.WriteLine("Błąd w załadowaniu pliku " + pathToArchivize + " lub dodaniu kolejnej pozycji do pliku XML");
            }
        }

        public void Save(Message msg, string pathToArchivize)
        {
            if (File.Exists(pathToArchivize))
                AddNewElementToXmlFile(msg, pathToArchivize);
            else
                CreateArchivizeXmlFile(msg, pathToArchivize);
       }

        public List<Message> Read(string Recipient, string pathToArchivize)
        {
            XDocument doc = XDocument.Load(pathToArchivize);
            var Messages = (from c in doc.Descendants("Message")
                            where (string)c.Attribute("Recipient") == Recipient
                            select new Message
                            {
                                Sender = (string)c.Attribute("Sender"),
                                Recipient = (string)c.Attribute("Recipient"),
                                DateTimeDelivery = (DateTime)c.Attribute("Date"),
                                Body = (string)c.Element("Body")
                            }).ToList();
            return Messages;
        }

        // funkcja testowa do zapisu
        public static void TestWrite()
        {
            Message msg = new Message();
            msg.Body = "Siema Sławek!";
            msg.Sender = "Michał";
            msg.Recipient = "Sławek";

            Message msg2 = new Message();
            msg2.Body = "Siema dubfiusdbgsudg sdgdfshdhsgdsjjgdj sdgdfshdhsgdsjjgdj sdgdfshdhsgdsjjgdjsdgdfshdhsgdsjjgdjsdgdfshdhsgdsjjgdj!";
            msg2.Sender = "Adrian";
            msg2.Recipient = "Michał";

            Message msg3 = new Message();
            msg2.Body = "Siema dsfh gsdibgf sg dsijfg seiuge!";
            msg2.Sender = "Monika";
            msg2.Recipient = "Sławek";

            XmlMessageArchivizer arch = new XmlMessageArchivizer();
            arch.Save(msg, "Archiwum_Wiadomości.xml");
            arch.Save(msg2, "Archiwum_Wiadomości.xml");
            arch.Save(msg3, "Archiwum_Wiadomości.xml");
        }

        //funkcja testowa do odczytu
        public static void TestSearchMessages(string Recipient)
        {
            XmlMessageArchivizer obj = new XmlMessageArchivizer();
            var res = obj.Read(Recipient, "Archiwum_Wiadomości.xml");

            foreach (Message s in res)
            {
                Console.WriteLine(s.Body);
                Console.WriteLine(s.Recipient);
                Console.WriteLine(s.Sender);
                Console.WriteLine(s.DateTimeDelivery);
            }
        }
    }
}
