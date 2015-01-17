using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communicator.Protocol;
using Communicator.Protocol.Model;
using Communicator.Protocol.Requests;
using Communicator.Untils.Archivizers.Message;
using Communicator.Protocol.Notifications;
namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
           var at = new Attachment();
           at.MimeType = "jakibubuus";
           byte[] scores = { 0111 };
            
            at.Data = scores;
            at.Name = "nafewfzwa";
            
            var mr = new MessageReq();
            mr.Attachment = at;
            mr.Message = "Wiadomoewfweść";
            mr.Recipient = "lfewffewfuj";
            mr.Login = "jafewfkis2";

            var xma = new XmlMessageArchivizer();
            
          //  xma.Save(mr, @"c:\MyTest.xml");

            var us = new User();
            us.Login = "jafewfkis2";
            List<MessageNotification> mn = xma.Read(us, @"c:\MyTest.xml");

            foreach (var mess in mn)
            {
                Console.WriteLine(Encoding.UTF8.GetString(mess.Attachment.Data));
                Console.WriteLine(mess.Message);
                Console.WriteLine(mess.Attachment.MimeType);
                Console.WriteLine(mess.Attachment.Name);
                Console.WriteLine(mess.SendTime.ToString());
                Console.WriteLine(mess.Sender);
            }
            
           
            
            
            Console.ReadLine();

            
        }
    }
}
