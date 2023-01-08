using Client.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Client.Services
{
    public class VotacionesCliente
    {
        HttpClient cliente = new HttpClient();


        public VotacionesCliente()
        {
            cliente.BaseAddress = new Uri("https://8690-189-159-255-77.ngrok.io/votacion/");
        }

        public async Task<Pregunta> GetPregunta()
        {
            HttpResponseMessage response = await cliente.GetAsync("pregunta");
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var json = await response.Content.ReadAsStringAsync();
                Pregunta p = JsonConvert.DeserializeObject<Pregunta>(json);
                return p;
            }
            else
            {
                return null;
            }
        }

        public async Task Votar(int opcion)
        {
            var response = await cliente.GetAsync("responder?voto=" + opcion);
            response.EnsureSuccessStatusCode();
        }

        public async Task VotarPost(Voto v)
        {
            var json = JsonConvert.SerializeObject(v);
            var response = await cliente.PostAsync("responder",  new StringContent(json, Encoding.UTF8, "application/json")
                );
            response.EnsureSuccessStatusCode();
        }

    }
}
