using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communicator.Protocol.Requests;
using Communicator.Protocol.Responses;
using Communicator.Untils;
using Communicator.Untils.Serializers;
using Communicator.Untils.Archivizers.UsersList;

namespace Communicator.Untils
{
    class Simula
    {
        public static void Simulation()
        {
         //// SYMULACJA REJESTRACJI
         
            //KLIENT
            var cur = new CreateUserReq();
            cur.Password = "hasłtajne";
            cur.Login = "login12";
        
        
            var js = new JSonSerializerService();
            byte[] buff = js.Serialize(cur);

            // WRZUCENIE DO RABBITA

            //SERWER
            var cuRequest = js.Deserialize<CreateUserReq>(buff);
           
            var xmlUserList = new XmlUserList();
            var createUserResponse = new CreateUserResponse();
            //createUserResponse.CreatedSuccessfully = xmlUserList.CreateNewUser(cuRequest, "ListaUserów.xml");

            // serializacja po stronie serwera
            byte[] response = js.Serialize(createUserResponse);

            //Rabbit

            //// KLient
            //deserializacja
            var objResponse = js.Deserialize<CreateUserResponse>(response);
            Console.WriteLine("Czy utowrzono użytkonika: " + objResponse.CreatedSuccessfully);

           

            /// LOGOWANIE/////

            var authReq = new AuthRequest();
            authReq.Login = "Jan";
            authReq.Password = "Mis";

            //serializacja
            byte[] buff2 = js.Serialize(authReq);

            //rabbit

            //SERWER
            var autReqServer = js.Deserialize<AuthRequest>(buff2);
            //Console.WriteLine(autReqServer.Password);
            //Console.WriteLine(autReqServer.Login);

            var ul = new XmlUserList();
            var authResp = new AuthResponse();
           // authResp.IsAuthenticated =  ul.AuthenticationUser(autReqServer, "ListaUserów.xml");

            buff2 = js.Serialize(authResp);
            
            //Rabbit
            
            var AuthResp = js.Deserialize<AuthResponse>(buff2);
            Console.WriteLine("Czy logowanie powiodło się:" + AuthResp.IsAuthenticated);

        }   
    }
}
