using System;
using System.IO;
using System.Xml.Linq;

namespace Communicator.Untils.Archivisers.Message
{
    class XmlMessageArchivizer : IMessageArchivizer
    {
        string pathToArchivize = "Archiwum_Wiadomości.xml";

        private void CreateArchivizeXmlFile(Message msg)
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

        private void AddNewElementToXmlFile(Message msg)
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

        public void Save(Message msg)
        {
            if (File.Exists(pathToArchivize))
                AddNewElementToXmlFile(msg);
            else
                CreateArchivizeXmlFile(msg);
       }

        // wywołanie działania działania 
        public static void Test()
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
            arch.Save(msg);
            arch.Save(msg2);
            arch.Save(msg3);
        }
    }
}
