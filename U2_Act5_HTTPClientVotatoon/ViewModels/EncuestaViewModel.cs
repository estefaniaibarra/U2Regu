using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using U2_Act5_HTTPClientVotatoon.Models;
using U2_Act5_HTTPClientVotatoon.Services;

namespace U2_Act5_HTTPClientVotatoon.ViewModels
{
        //Vistas//
        public enum Vistas { Encuesta, Resultados }
        public class EncuestaViewModel : INotifyPropertyChanged
        {
        //Objeto//
            public Pregunta? LaRespuesta { get; set; }
            public Vistas Vista { get; set; }
        //comandos//
            public ICommand IniciarCommand { get; set; }
        // declarar los votos//
            public int Voto1 { get; set; }
            public int Voto2 { get; set; }
            public int Voto3 { get; set; }
            public int Voto4 { get; set; }
            public int Voto5 { get; set; }
            public int Voto6 { get; set; }
            public int Voto7 { get; set; }
            public int Voto8 { get; set; }
            public int Voto9 { get; set; }
            public int Voto10 { get; set; }
            public int Voto11 { get; set; }
            public int Voto12 { get; set; }
            public int Voto13 { get; set; }
            public int Voto14 { get; set; }
            public int Voto15 { get; set; }
        //dividir los votos por preguntas//
        public int TotalGrupo1 => Voto1 + Voto2 + Voto3 + Voto4 + Voto5;
        public int TotalGrupo2 => Voto6 + Voto7 + Voto8 + Voto9 + Voto10;
        public int TotalGrupo3 => Voto11 + Voto12 + Voto13 + Voto14 + Voto15;


            public string Error { get; set; } = "";


            VotacionService server = new();

            public EncuestaViewModel()
            {
                Vista = Vistas.Encuesta;
                IniciarCommand = new RelayCommand(Iniciar);
                server.VotoRecibido += Server_VotoRecibido;
                CargarPregunta();
            }
        //métodos//
            private void CargarPregunta()
            {
                if (File.Exists("pregunta.json"))
                {
                    var contenido = File.ReadAllText("pregunta.json");
                    LaRespuesta = JsonConvert.DeserializeObject<Pregunta>(contenido);
                }
                else
                {
                    LaRespuesta = new();
                }
                Actualizar(nameof(LaRespuesta));
            }
            private void Server_VotoRecibido(int obj)
            {
                switch (obj)
                {
                    case 1: Voto1++; break;
                    case 2: Voto2++; break;
                    case 3: Voto3++; break;
                    case 4: Voto4++; break;
                    case 5: Voto5++; break;
                    case 6: Voto6++; break;
                    case 7: Voto7++; break;
                    case 8: Voto8++; break;
                    case 9: Voto9++; break;
                    case 10: Voto10++; break;
                    case 11: Voto11++; break;
                    case 12: Voto12++; break;
                    case 13: Voto13++; break;
                    case 14: Voto14++; break;
                    case 15: Voto15++; break;

                }
                Actualizar();
            }

            private void Iniciar()
            {
                if (LaRespuesta != null)
                {
                    Error = "";
                    
                    if (string.IsNullOrWhiteSpace(LaRespuesta.Encuesta1))
                    {
                        Error = "Escribe la pregunta.";
                    }
                    if (string.IsNullOrWhiteSpace(LaRespuesta.Respuesta1)  || string.IsNullOrWhiteSpace(LaRespuesta.Respuesta2) || string.IsNullOrWhiteSpace(LaRespuesta.Respuesta3) 
                    || string.IsNullOrWhiteSpace(LaRespuesta.Respuesta4) || string.IsNullOrWhiteSpace(LaRespuesta.Respuesta5))
                        { 
                        Error = "Escribe las respuestas.";
                    }
                if (string.IsNullOrWhiteSpace(LaRespuesta.Encuesta2))
                {
                    Error = "Escribe la pregunta.";
                }
                if (string.IsNullOrWhiteSpace(LaRespuesta.Respuesta6) || string.IsNullOrWhiteSpace(LaRespuesta.Respuesta7) || string.IsNullOrWhiteSpace(LaRespuesta.Respuesta8)
                || string.IsNullOrWhiteSpace(LaRespuesta.Respuesta9) || string.IsNullOrWhiteSpace(LaRespuesta.Respuesta10))
                {
                    Error = "Escribe las respuestas.";
                }

                if (string.IsNullOrWhiteSpace(LaRespuesta.Encuesta3))
                {
                    Error = "Escribe la pregunta.";
                }
                if (string.IsNullOrWhiteSpace(LaRespuesta.Respuesta11) || string.IsNullOrWhiteSpace(LaRespuesta.Respuesta12) || string.IsNullOrWhiteSpace(LaRespuesta.Respuesta13)
                || string.IsNullOrWhiteSpace(LaRespuesta.Respuesta14) || string.IsNullOrWhiteSpace(LaRespuesta.Respuesta15))
                {
                    Error = "Escribe las respuestas.";
                }
                if (Error == "")
                    {
                        server.MandarPregunta(LaRespuesta);
                        server.Iniciar();
                        var json = JsonConvert.SerializeObject(LaRespuesta);
                        File.WriteAllText("pregunta.json", json);
                        Vista = Vistas.Resultados;
                    }
                    Actualizar();
                }
           
            }

            void Actualizar(string? Property = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Property)); 
            }
            public event PropertyChangedEventHandler? PropertyChanged;
        }
}
