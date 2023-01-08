using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using U2_Act5_HTTPClientVotatoon.Models;

namespace U2_Act5_HTTPClientVotatoon.Services
{
    public class VotacionService
    {
        HttpListener servidor = new();
        public event Action<int>? VotoRecibido;
        private string pregunta = "";

        public VotacionService()
        {
            servidor.Prefixes.Add("http://*:3456/votacion/");

        }
        public void Iniciar()
        {
            if(!servidor.IsListening)
            {
                servidor.Start();
                servidor.BeginGetContext(RecibirContexto, null);
            }

        }
        public void MandarPregunta(Pregunta p)
        {
            pregunta = JsonConvert.SerializeObject(p);
        }
        private void RecibirContexto(IAsyncResult ar)
        {
            var context = servidor.EndGetContext(ar);
            servidor.BeginGetContext(RecibirContexto, null);

            if (context.Request.Url != null)
            {
                if (context.Request.Url.LocalPath == "/votacion/pregunta")
                {
                    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(pregunta);                   
                    context.Response.ContentType = "application/json"; 
                    context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                    context.Response.StatusCode = 200;
                    context.Response.Close();
                }
                else if (context.Request.HttpMethod == "POST" && context.Request.Url.LocalPath == "/votacion/responder")
                {
                    
                    var stream = new StreamReader(context.Request.InputStream);
                    var json = stream.ReadToEnd();
                    Contestar? voto = JsonConvert.DeserializeObject<Contestar>(json);
                    context.Response.StatusCode = 200;
                    if (voto != null)
                    {
                        VotoRecibido?.Invoke(voto.Opcion);
                    }
                    context.Response.Close();
                }
                else if (context.Request.Url.LocalPath == "/votacion/responder")
                {
                    if (context.Request.QueryString["voto"] != null)
                    {
                        int voto = int.Parse(context.Request.QueryString["voto"]);
                        context.Response.StatusCode = 200;
                        VotoRecibido?.Invoke(voto);
                    }
                    else
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest; 
                    }                    context.Response.Close();
                }
                else
                {
                    context.Response.StatusCode = 404;
                    context.Response.Close(); 
                }
            }
            
        }
    }
}
